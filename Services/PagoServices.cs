using AutoMapper;
using FinancieraAPI.DTOs;
using FinancieraAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancieraAPI.Services
{
    public class PagoServices : IPagoServices
    {
        private readonly FinancieraDbContext _context;
        private readonly IMapper _IMapper;

        public PagoServices(FinancieraDbContext context, IMapper iMapper)
        {
            _context = context;
            _IMapper = iMapper;
        }

        public async Task<int> DeletePago(int pagoId)
        {
            var pago = await _context.Pagos.FindAsync(pagoId);

            if (pago == null)
            {
                return -1; // Indica que el pago no fue encontrado
            }

            _context.Pagos.Remove(pago);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagoResponse> GetPago(int pagoId)
        {
            var pago = await _context.Pagos.FindAsync(pagoId);

            var pagoResponse = _IMapper.Map<PagoResponse>(pago);

            return pagoResponse;
        }

        public async Task<List<PagoResponse>> GetPagos()
        {
            var pagos = await _context.Pagos.ToListAsync();

            var pagosList = _IMapper.Map<List<Pago>, List<PagoResponse>>(pagos);

            return pagosList;
        }

        public async Task<List<PagoFuturoResponse>> GetPagosFuturos(int prestamoId)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.Pagos)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);

            if (prestamo == null)
                throw new KeyNotFoundException("Préstamo no encontrado.");

            // Ordenar los pagos existentes por fecha
            var pagosRealizados = prestamo.Pagos
                .Where(p => p.Estado == "Realizado")
                .OrderBy(p => p.FechaPago)
                .ToList();

            var montoTotal = prestamo.MontoAprobado;
            var tasaInteresAnual = prestamo.TasaInteres;
            var meses = (prestamo.FechaVencimiento.Year - prestamo.FechaInicio.Year) * 12
                        + prestamo.FechaVencimiento.Month - prestamo.FechaInicio.Month;

            var tasaInteresMensual = (tasaInteresAnual / 12) / 100;
            var factor = (decimal)Math.Pow(1 + (double)tasaInteresMensual, meses);
            var cuotaMensual = montoTotal * (tasaInteresMensual * factor) / (factor - 1);
            cuotaMensual = Math.Round(cuotaMensual, 2);

            var saldoRestante = montoTotal;
            var pagosFuturos = new List<PagoFuturoResponse>();

            for (int i = 1; i <= meses; i++)
            {
                var fechaPago = prestamo.FechaInicio.AddMonths(i);
                bool puedePagar = false;
                string estado = "Pendiente";

                decimal interesPagado = saldoRestante * tasaInteresMensual;
                interesPagado = Math.Round(interesPagado, 2);

                decimal capitalPagado = cuotaMensual - interesPagado;
                capitalPagado = Math.Round(capitalPagado, 2);

                capitalPagado = Math.Min(capitalPagado, saldoRestante);
                saldoRestante -= capitalPagado;
                saldoRestante = Math.Round(saldoRestante, 2);


                // Verificar si el pago ya fue realizado
                if (pagosRealizados.Any(p => p.PagoId == i))
                {
                    estado = "Realizado";
                }
                else if (fechaPago <= DateOnly.FromDateTime(DateTime.Today))
                {
                    // Pago vencido o para hoy
                    puedePagar = true;
                    estado = fechaPago == DateOnly.FromDateTime(DateTime.Today) ? "Hoy" : "Vencido";
                }

                pagosFuturos.Add(new PagoFuturoResponse
                {
                    PagoId = i,
                    PrestamoId = prestamo.PrestamoId,
                    FechaPago = fechaPago,
                    MontoAPagar = cuotaMensual,
                    InteresPagado = interesPagado,
                    CapitalPagado = capitalPagado,
                    SaldoRestante = saldoRestante,
                    Estado = estado,
                    PuedePagar = puedePagar
                });

                if (saldoRestante <= 0)
                    break;
            }

            return pagosFuturos;
        }

        // Implementación del método para obtener pagos vencidos
        public async Task<List<PagoResponse>> GetPagosVencidos()
        {
            // Obtener la fecha actual
            var fechaActual = DateOnly.FromDateTime(DateTime.Today);

            // Filtrar pagos vencidos (fecha de pago menor a la fecha actual y estado "Pendiente")
            var pagosVencidos = await _context
                .Pagos.Where(p => p.FechaPago < fechaActual && p.Estado == "Pendiente")
                .ToListAsync();

            // Mapear a PagoResponse
            var pagosVencidosResponse = _IMapper.Map<List<Pago>, List<PagoResponse>>(pagosVencidos);

            return pagosVencidosResponse;
        }

        public async Task<int> PostPago(PagoRequest pago)
        {
            var pagoEntity = _IMapper.Map<PagoRequest, Pago>(pago);

            var prestamo = await _context
                .Prestamos.Include(p => p.Pagos)
                .FirstOrDefaultAsync(p => p.PrestamoId == pago.PrestamoId);

            if (prestamo == null)
            {
                throw new KeyNotFoundException("Préstamo no encontrado.");
            }

            // Asignar los valores proporcionados al pago
            pagoEntity.InteresPagado = pago.InteresPagado;
            pagoEntity.CapitalPagado = pago.CapitalPagado;
            pagoEntity.SaldoRestante = pago.SaldoRestante;

            // Agregar el pago a la lista de pagos del préstamo
            prestamo.Pagos.Add(pagoEntity);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return pagoEntity.PagoId;
        }

        public async Task<int> PutPago(int pagoId, PagoRequest pago)
        {
            var entity = await _context.Pagos.FindAsync(pagoId);

            if (entity == null)
            {
                return -1; // Indica que el pago no fue encontrado
            }

            // Actualiza los atributos especificados
            entity.PrestamoId = pago.PrestamoId;
            entity.FechaPago = pago.FechaPago;
            entity.MontoAPagar = pago.MontoAPagar;
            entity.MontoPagado = pago.MontoPagado;
            entity.SaldoRestante = pago.SaldoRestante;
            entity.Estado = pago.Estado;

            _context.Pagos.Update(entity);

            return await _context.SaveChangesAsync();
        }

        public async Task ProcesarPagosAutomaticosAsync()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            // Obtener todos los préstamos activos
            var prestamosActivos = await _context.Prestamos
                .Include(p => p.Pagos) // Incluir los pagos existentes
                .Where(p => p.Estado == "Activo") // Filtrar préstamos activos
                .ToListAsync();

            foreach (var prestamo in prestamosActivos)
            {
                // Calcular los pagos futuros para el préstamo
                var pagosFuturos = await GetPagosFuturos(prestamo.PrestamoId);

                foreach (var pagoFuturo in pagosFuturos)
                {
                    // Verificar si el pago ya existe en la base de datos
                    var pagoExistente = await _context.Pagos
                        .FirstOrDefaultAsync(p => p.PrestamoId == prestamo.PrestamoId &&
                                                  p.FechaPago == pagoFuturo.FechaPago);

                    // Si el pago no existe y la fecha de pago ya pasó, guardarlo
                    if (pagoExistente == null && pagoFuturo.FechaPago < hoy)
                    {
                        var pago = new Pago
                        {
                            PrestamoId = pagoFuturo.PrestamoId,
                            FechaPago = pagoFuturo.FechaPago,
                            MontoAPagar = pagoFuturo.MontoAPagar,
                            MontoPagado = pagoFuturo.MontoAPagar,
                            InteresPagado = pagoFuturo.InteresPagado,
                            CapitalPagado = pagoFuturo.CapitalPagado,
                            SaldoRestante = pagoFuturo.SaldoRestante,
                            Estado = "Pendiente" // Estado inicial
                        };

                        _context.Pagos.Add(pago);
                    }
                }
            }

            // Guardar todos los cambios en la base de datos
            await _context.SaveChangesAsync();
        }
    }
}

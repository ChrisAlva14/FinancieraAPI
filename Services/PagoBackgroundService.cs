using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinancieraAPI.Services
{
    public class PagoBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PagoBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Crear un ámbito para resolver PagoServices
                using (var scope = _serviceProvider.CreateScope())
                {
                    var pagoServices = scope.ServiceProvider.GetRequiredService<PagoServices>();

                    // Lógica para guardar datos y cambiar estados
                    await pagoServices.ProcesarPagosAutomaticosAsync();
                }

                // Esperar 1 minuto antes de la siguiente ejecución
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
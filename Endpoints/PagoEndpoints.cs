using FinancieraAPI.DTOs;
using FinancieraAPI.Services;
using Microsoft.OpenApi.Models;

namespace FinancieraAPI.Endpoints
{
    public static class PagoEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/pagos").WithTags("Pagos");

            // GET - OBTENER TODOS LOS PAGOS
            group
                .MapGet(
                    "/",
                    async (IPagoServices pagoServices) =>
                    {
                        var pagos = await pagoServices.GetPagos();
                        return Results.Ok(pagos);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER PAGOS",
                    Description = "MUESTRA UNA LISTA DE TODOS LOS PAGOS",
                })
                .RequireAuthorization();

            // GET - OBTENER PAGO POR ID
            group
                .MapGet(
                    "/{id}",
                    async (int id, IPagoServices pagoServices) =>
                    {
                        var pago = await pagoServices.GetPago(id);
                        if (pago is null || (pago.Estado == "Realizado" && pago.FechaPago > DateOnly.FromDateTime(DateTime.Today)))
                            return Results.NotFound();
                        return Results.Ok(pago);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER UN PAGO POR ID",
                    Description = "OBTIENE UN PAGO DADO SU ID",
                })
                .RequireAuthorization();

            // POST - CREAR UN NUEVO PAGO
            group
                .MapPost(
                    "/",
                    async (PagoRequest pago, IPagoServices pagoServices) =>
                    {
                        var createdPagoId = await pagoServices.PostPago(pago);
                        return Results.Created($"api/pagos/{createdPagoId}", pago);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "CREAR NUEVO PAGO",
                    Description = "CREA UN NUEVO PAGO CON LOS DATOS PROPORCIONADOS",
                })
                .RequireAuthorization();

            // PUT - MODIFICAR A UN PAGO
            group
                .MapPut(
                    "/{id}",
                    async (int id, PagoRequest pago, IPagoServices pagoServices) =>
                    {
                        var isUpdated = await pagoServices.PutPago(id, pago);

                        if (isUpdated == -1)
                            return Results.NotFound();
                        else
                            return Results.Ok(pago);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "MODIFICAR UN PAGO",
                    Description = "ACTUALIZA UN PAGO DADO SU ID",
                })
                .RequireAuthorization();

            // DELETE - ELIMINAR UN PAGO
            group
                .MapDelete(
                    "/{id}",
                    async (int id, IPagoServices pagoServices) =>
                    {
                        var result = await pagoServices.DeletePago(id);
                        if (result == -1)
                            return Results.NotFound();
                        else
                            return Results.NoContent();
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "ELIMINAR UN PAGO",
                    Description = "ELIMINAR UN PAGO DADO SU ID",
                })
                .RequireAuthorization();

            // GET - OBTENER PAGOS FUTUROS
            group
                .MapGet(
                    "/futuros/{prestamoId}",
                    async (int prestamoId, IPagoServices pagoServices) =>
                    {
                        var pagosFuturos = await pagoServices.GetPagosFuturos(prestamoId);
                        return Results.Ok(pagosFuturos);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER PAGOS FUTUROS",
                    Description = "MUESTRA UNA LISTA DE PAGOS FUTUROS PARA UN PRÉSTAMO",
                })
                .RequireAuthorization();

            // GET - OBTENER PAGOS VENCIDOS
            group
                .MapGet(
                    "/vencidos",
                    async (IPagoServices pagoServices) =>
                    {
                        var pagosVencidos = await pagoServices.GetPagosVencidos();
                        return Results.Ok(pagosVencidos);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER PAGOS VENCIDOS",
                    Description =
                        "MUESTRA UNA LISTA DE PAGOS VENCIDOS (PAGOS CON FECHA DE PAGO PASADA Y ESTADO 'PENDIENTE')",
                })
                .RequireAuthorization();
        }
    }
}

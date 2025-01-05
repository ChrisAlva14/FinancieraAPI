using FinancieraAPI.DTOs;
using FinancieraAPI.Services;
using Microsoft.OpenApi.Models;

namespace FinancieraAPI.Endpoints
{
    public static class PrestamoEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/prestamos").WithTags("Prestamos");

            // GET - OBTENER TODOS LOS PRESTAMOS
            group
                .MapGet(
                    "/",
                    async (IPrestamoServices prestamoServices) =>
                    {
                        var prestamos = await prestamoServices.GetPrestamos();
                        return Results.Ok(prestamos);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER PRESTAMOS",
                    Description = "MUESTRA UNA LISTA DE TODOS LOS PRESTAMOS",
                })
                .RequireAuthorization();

            // GET - OBTENER PRESTAMO POR ID
            group
                .MapGet(
                    "/{id}",
                    async (int id, IPrestamoServices prestamoServices) =>
                    {
                        var prestamo = await prestamoServices.GetPrestamo(id);
                        return prestamo is not null ? Results.Ok(prestamo) : Results.NotFound();
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER UN PRESTAMO POR ID",
                    Description = "OBTIENE UN PRESTAMO DADO SU ID",
                })
                .RequireAuthorization();

            // POST - CREAR UN NUEVO PRESTAMO
            group
                .MapPost(
                    "/",
                    async (PrestamoRequest prestamo, IPrestamoServices prestamoServices) =>
                    {
                        var createdPrestamoId = await prestamoServices.PostPrestamo(prestamo);
                        return Results.Created($"api/prestamos/{createdPrestamoId}", prestamo);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "CREAR NUEVO PRESTAMO",
                    Description = "CREA UN NUEVO PRESTAMO CON LOS DATOS PROPORCIONADOS",
                })
                .RequireAuthorization();

            // PUT - MODIFICAR A UN PRESTAMO
            group
                .MapPut(
                    "/{id}",
                    async (int id, PrestamoRequest prestamo, IPrestamoServices prestamoServices) =>
                    {
                        var isUpdated = await prestamoServices.PutPrestamo(id, prestamo);

                        if (isUpdated == -1)
                            return Results.NotFound();
                        else
                            return Results.Ok(prestamo);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "MODIFICAR UN PRESTAMO",
                    Description = "ACTUALIZA UN PRESTAMO DADO SU ID",
                })
                .RequireAuthorization();

            // DELETE - ELIMINAR UN PRESTAMO
            group
                .MapDelete(
                    "/{id}",
                    async (int id, IPrestamoServices prestamoServices) =>
                    {
                        var result = await prestamoServices.DeletePrestamo(id);
                        if (result == -1)
                            return Results.NotFound();
                        else
                            return Results.NoContent();
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "ELIMINAR UN PRESTAMO",
                    Description = "ELIMINAR UN PRESTAMO DADO SU ID",
                })
                .RequireAuthorization();
        }
    }
}

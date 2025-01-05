using FinancieraAPI.DTOs;
using FinancieraAPI.Services;
using Microsoft.OpenApi.Models;

namespace FinancieraAPI.Endpoints
{
    public static class EmpleoEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/empleos").WithTags("Empleos");

            // GET - OBTENER TODOS LOS EMPLEOS
            group
                .MapGet(
                    "/",
                    async (IEmpleoServices empleoServices) =>
                    {
                        var empleos = await empleoServices.GetEmpleos();
                        return Results.Ok(empleos);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER EMPLEOS",
                    Description = "MUESTRA UNA LISTA DE TODOS LOS EMPLEOS",
                })
                .RequireAuthorization();

            // GET - OBTENER EMPLEO POR ID
            group
                .MapGet(
                    "/{id}",
                    async (int id, IEmpleoServices empleoServices) =>
                    {
                        var empleo = await empleoServices.GetEmpleo(id);
                        return empleo is not null ? Results.Ok(empleo) : Results.NotFound();
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER UN EMPLEO POR ID",
                    Description = "OBTIENE UN EMPLEO DADO SU ID",
                })
                .RequireAuthorization();

            // POST - CREAR UN NUEVO EMPLEO
            group
                .MapPost(
                    "/",
                    async (EmpleoRequest empleo, IEmpleoServices empleoServices) =>
                    {
                        var createdEmpleoId = await empleoServices.PostEmpleo(empleo);
                        return Results.Created($"api/empleos/{createdEmpleoId}", empleo);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "CREAR NUEVO EMPLEO",
                    Description = "CREA UN NUEVO EMPLEO CON LOS DATOS PROPORCIONADOS",
                })
                .RequireAuthorization();

            // PUT - MODIFICAR A UN EMPLEO
            group
                .MapPut(
                    "/{id}",
                    async (int id, EmpleoRequest empleo, IEmpleoServices empleoServices) =>
                    {
                        var isUpdated = await empleoServices.PutEmpleo(id, empleo);

                        if (isUpdated == -1)
                            return Results.NotFound();
                        else
                            return Results.Ok(empleo);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "MODIFICAR UN EMPLEO",
                    Description = "ACTUALIZA UN EMPLEO DADO SU ID",
                })
                .RequireAuthorization();

            // DELETE - ELIMINAR UN EMPLEO
            group
                .MapDelete(
                    "/{id}",
                    async (int id, IEmpleoServices empleoServices) =>
                    {
                        var result = await empleoServices.DeleteEmpleo(id);
                        if (result == -1)
                            return Results.NotFound();
                        else
                            return Results.NoContent();
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "ELIMINAR UN EMPLEO",
                    Description = "ELIMINAR UN EMPLEO DADO SU ID",
                })
                .RequireAuthorization();
        }
    }
}

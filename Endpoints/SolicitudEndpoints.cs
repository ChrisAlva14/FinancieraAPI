using FinancieraAPI.DTOs;
using FinancieraAPI.Services;
using Microsoft.OpenApi.Models;

namespace FinancieraAPI.Endpoints
{
    public static class SolicitudEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/solicitudes").WithTags("Solicitudes");

            // GET - OBTENER TODOS LAS SOLICITUDES
            group
                .MapGet(
                    "/",
                    async (ISolicitudServices solicitudServices) =>
                    {
                        var solicitudes = await solicitudServices.GetSolicitudes();
                        return Results.Ok(solicitudes);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER SOLICITUDES",
                    Description = "MUESTRA UNA LISTA DE TODOS LAS SOLICITUDES",
                }).RequireAuthorization();

            // GET - OBTENER SOLICITUD POR ID
            group
                .MapGet(
                    "/{id}",
                    async (int id, ISolicitudServices solicitudServices) =>
                    {
                        var solicitud = await solicitudServices.GetSolicitud(id);
                        return solicitud is not null ? Results.Ok(solicitud) : Results.NotFound();
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER SOLICITUD POR ID",
                    Description = "OBTIENE UNA SOLICITUD DADO SU ID",
                }).RequireAuthorization();

            // POST - CREAR SOLICITUD
            group
                .MapPost(
                    "/",
                    async (SolicitudRequest solicitud, ISolicitudServices solicitudServices) =>
                    {
                        var createdSolicitudId = await solicitudServices.PostSolicitud(solicitud);
                        return Results.Created($"api/solicitudes/{createdSolicitudId}", solicitud);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "CREAR SOLICITUD",
                    Description = "CREA UNA SOLICITUD CON LOS DATOS PROPORCIONADOS",
                }).RequireAuthorization();

            // PUT - MODIFICAR SOLICITUD
            group
                .MapPut(
                    "/{id}",
                    async (int id, SolicitudRequest solicitud, ISolicitudServices solicitudServices) =>
                    {
                        var isUpdated = await solicitudServices.PutSolicitud(id, solicitud);

                        if (isUpdated == -1)
                            return Results.NotFound();
                        else
                            return Results.Ok(solicitud);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "MODIFICAR SOLICITUD",
                    Description = "ACTUALIZA UNA SOLICITUD DADO SU ID",
                }).RequireAuthorization();

            // DELETE - ELIMINAR SOLICITUD
            group
            .MapDelete(
                "/{id}",
                async (int id, ISolicitudServices solicitudServices) =>
                {
                    var isDeleted = await solicitudServices.DeleteSolicitud(id);
                    if (isDeleted == -1)
                        return Results.NotFound();
                    else
                        return Results.Ok();
                }
            ).WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "ELIMINAR SOLICITUD",
                    Description = "ELIMINAR UNA SOLICITUD DADO SU ID",
                }).RequireAuthorization();.RequireAuthorization();
}
    }
}

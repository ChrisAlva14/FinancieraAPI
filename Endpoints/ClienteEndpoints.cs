using FinancieraAPI.DTOs;
using FinancieraAPI.Services;
using Microsoft.OpenApi.Models;

namespace FinancieraAPI.Endpoints
{
    public static class ClienteEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/clientes").WithTags("Clientes");

            // GET - OBTENER TODOS LOS CLIENTES
            group
                .MapGet(
                    "/",
                    async (IClienteServices clienteServices) =>
                    {
                        var clientes = await clienteServices.GetClientes();
                        return Results.Ok(clientes);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER CLIENTES",
                    Description = "MUESTRA UNA LISTA DE TODOS LOS CLIENTES",
                })
                .RequireAuthorization();

            // GET - OBTENER CLIENTE POR ID
            group
                .MapGet(
                    "/{id}",
                    async (int id, IClienteServices clienteServices) =>
                    {
                        var cliente = await clienteServices.GetCliente(id);
                        return cliente is not null ? Results.Ok(cliente) : Results.NotFound();
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "OBTENER UN CLIENTE POR ID",
                    Description = "OBTIENE UN CLIENTE DADO SU ID",
                })
                .RequireAuthorization();

            // POST - CREAR UN NUEVO CLIENTE
            group
                .MapPost(
                    "/",
                    async (ClienteRequest cliente, IClienteServices clienteServices) =>
                    {
                        var createdClienteId = await clienteServices.PostCliente(cliente);
                        return Results.Created($"api/clientes/{createdClienteId}", cliente);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "CREAR NUEVO CLIENTE",
                    Description = "CREA UN NUEVO CLIENTE CON LOS DATOS PROPORCIONADOS",
                })
                .RequireAuthorization();

            // PUT - MODIFICAR A UN CLIENTE
            group
                .MapPut(
                    "/{id}",
                    async (int id, ClienteRequest cliente, IClienteServices clienteServices) =>
                    {
                        var isUpdated = await clienteServices.PutCliente(id, cliente);

                        if (isUpdated == -1)
                            return Results.NotFound();
                        else
                            return Results.Ok(cliente);
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "MODIFICAR UN CLIENTE",
                    Description = "ACTUALIZA UN CLIENTE DADO SU ID",
                })
                .RequireAuthorization();

            // DELETE - ELIMINAR UN CLIENTE
            group
                .MapDelete(
                    "/{id}",
                    async (int id, IClienteServices clienteServices) =>
                    {
                        var result = await clienteServices.DeleteCliente(id);
                        if (result == -1)
                            return Results.NotFound();
                        else
                            return Results.NoContent();
                    }
                )
                .WithOpenApi(o => new OpenApiOperation(o)
                {
                    Summary = "ELIMINAR UN CLIENTE",
                    Description = "ELIMINAR UN CLIENTE DADO SU ID",
                })
                .RequireAuthorization();
        }
    }
}

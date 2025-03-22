using FinancieraAPI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinancieraAPI.Endpoints
{
    public static class Startup
    {
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddHostedService<PagoBackgroundService>();
        }

        public static void UseEndpoints(this WebApplication app)
        {
            UserEndpoints.Add(app);
            ClienteEndpoints.Add(app);
            SolicitudEndpoints.Add(app);
            EmpleoEndpoints.Add(app);
            PrestamoEndpoints.Add(app);
            PagoEndpoints.Add(app);
        }
    }
}

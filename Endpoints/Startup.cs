namespace FinancieraAPI.Endpoints
{
    public static class Startup
    {
        public static void UseEndpoints(this WebApplication app)
        {
            UserEndpoints.Add(app);
            ClienteEndpoints.Add(app);
        }
    }
}

namespace ConectorAnaliticoApi.Endpoints
{
    public static class HomeEndpoints
    {
        public static void MapHomeEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Hello World");
        }
    }
}

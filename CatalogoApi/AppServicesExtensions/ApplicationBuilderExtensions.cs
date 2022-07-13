namespace CatalogoApi.AppServicesExtensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseEceptionHandling(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        return app;
    }

    public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
    {
        app.UseCors(p =>
        {
            p.AllowAnyOrigin();
            p.WithMethods("GET");
            p.AllowAnyHeader();
        });

        return app;
    }

    public static IApplicationBuilder UseSwaggerEndpoints(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => { });

        return app;
    }
}

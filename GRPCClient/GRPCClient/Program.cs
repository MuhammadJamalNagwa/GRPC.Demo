using GRPCClient.Repos.UserRepo;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddHttpClient();

    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddControllersWithViews();
}

WebApplication app = builder.Build();
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
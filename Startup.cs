using BackendNotas.Services;
using FirebaseAdmin.Auth;

public class Startup
{
    // ...

    public void ConfigureServices(IServiceCollection services)
    {
        // ...

        // Registra el servicio FirebaseAuth
        services.AddSingleton<FirebaseAuth>();

        // Registra el servicio NoteService
        services.AddSingleton<NoteService>();

        // ...
    }

    // ...
}
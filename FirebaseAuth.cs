using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;

public class FirebaseAuth
{
    private static FirebaseApp _app;

    public FirebaseAuth()
    {
        if (_app == null)
        {
            _app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("C:/Users/LENOVO/OneDrive/Documentos/Proyectos Arquitecturas y Modelos de software/Proyecto notas/BackendUsuarios/fb-notas-firebase-adminsdk-zbhby-a7250ab979.json")
            });
        }
    }

    public async Task<string> VerifyIdTokenAsync(string idToken)
    {
        var auth = FirebaseAdmin.Auth.FirebaseAuth.GetAuth(_app);
        var token = await auth.VerifyIdTokenAsync(idToken);
        return token.Uid;
    }
}

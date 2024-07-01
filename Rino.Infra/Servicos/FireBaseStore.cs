using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;

namespace Rino.Infrastructure.Data
{
    public class FireBaseStore
    {
        public FirestoreDb Firestore { get; set; }
        private readonly string projetctId = "rinoceronteintegracao";

        public FireBaseStore(IConfiguration configuration)
        {
            string pathToServiceAccountKey = configuration["FirebaseCredentials:FilePath"]!;

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathToServiceAccountKey);

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", fullPath);

            Firestore = FirestoreDb.Create(projetctId);
        }
    }
}

using Google.Cloud.Firestore;
using Rino.Dominio.Entidades;
using Rino.Dominio.Interfaces.Servicos;
using Rino.Infra.Conversores;
using Rino.Infra.Mapeamentos;
using Rino.Infrastructure.Data;
using System.Security.Claims;

namespace Rino.Infra.ServicosExternos.Firebase
{
    public class FirebaseUsuarioServico : IFirebaseUsuarioServico
    {
        private readonly FireBaseStore _firebaseStorage;
        public FirebaseUsuarioServico(FireBaseStore firebaseStorage)
        {
            _firebaseStorage = firebaseStorage;
        }
        public async Task<Usuario> BuscarPorLogin(string login)
        {
            Usuario usuario = new Usuario();

            {
                try
                {
                    CollectionReference usuariosRef = _firebaseStorage.Firestore.Collection("Usuario");
                    Query query = usuariosRef.WhereEqualTo("Login", login);

                    QuerySnapshot snapshot = await query.GetSnapshotAsync();
                    DocumentSnapshot docSnapshot = snapshot.Documents.FirstOrDefault()!;

                    if (docSnapshot != null && docSnapshot.Exists)
                    {
                        UsuarioFirestore firestoreUsuario = docSnapshot.ConvertTo<UsuarioFirestore>();
                        return firestoreUsuario.ToUsuario();
                    }

                    return null!;
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao buscar usuário.", ex);
                }
            }
        }

        public async Task<List<string>> BuscarClaimsUsuario(string usuarioId)
        {
            try
            {
                var query = _firebaseStorage.Firestore.Collection("UsuarioClaim").WhereEqualTo("UsuarioID", usuarioId);
                var snapshot = await query.GetSnapshotAsync();

                var claims = snapshot.Documents.Select(doc => doc.GetValue<string>("Nome")).ToList();

                return claims;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar claims do usuário.", ex);
            }
        }
    }
}

using Google.Cloud.Firestore;
using Rino.Dominio.Entidades;
using Rino.Dominio.Interfaces.Servicos;
using Rino.Infra.Conversores;
using Rino.Infra.Mapeamentos;
using Rino.Infrastructure.Data;
using System.Text.RegularExpressions;

namespace Rino.Infra.ServicosExternos.Firebase
{
    public class FirebaseArquivosServico : IFirebaseArquivoServico
    {
        private readonly FireBaseStore _firebaseStorage;

        public FirebaseArquivosServico(FireBaseStore firebaseStorage)
        {
            _firebaseStorage = firebaseStorage;
        }

        #region Arquivo
        public async Task<TipoArquivo> BuscarArquivoPorNome(string nomeArquivo)
        {
            try
            {
                string nomeSemExtensao = Path.GetFileNameWithoutExtension(nomeArquivo);
                string nomeFinal = nomeSemExtensao.Substring(0, nomeSemExtensao.IndexOf('_'));
                string nomeSemNumeros = Path.GetFileNameWithoutExtension(Regex.Replace(nomeArquivo, "[0-9]", ""));

                CollectionReference tiposArquivoRef = _firebaseStorage.Firestore.Collection("TipoArquivo");

                // Criar consulta para buscar documentos que contenham o nomeArquivo como parte do Alias
                
                Query query = tiposArquivoRef.WhereGreaterThanOrEqualTo("Alias", nomeSemNumeros)
                                             .WhereLessThanOrEqualTo("Alias", nomeSemNumeros);

                QuerySnapshot snapshot = await query.GetSnapshotAsync();
                DocumentSnapshot docSnapshot = snapshot.Documents.FirstOrDefault()!;

                if (docSnapshot != null && docSnapshot.Exists)
                {
                    TipoArquivoFirestore firestoreTipoArquivo = docSnapshot.ConvertTo<TipoArquivoFirestore>();
                    return firestoreTipoArquivo.ToTipoArquivo();
                }

                return null!;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar arquivo.", ex);
            }
        }

        public async Task<IEnumerable<Colunas>> BuscaColunasPorTipoArquivo(string idTipoArquivo)
        {
            try
            {
                CollectionReference colunasRef = _firebaseStorage.Firestore.Collection("Colunas");

                Query query = colunasRef.WhereEqualTo("TipoArquivoID", idTipoArquivo);

                QuerySnapshot snapshot = await query.GetSnapshotAsync();

                List<Colunas> colunas = new List<Colunas>();

                foreach (DocumentSnapshot docSnapshot in snapshot.Documents)
                {
                    if (docSnapshot.Exists)
                    {
                        ColunasFirestore firestoreColunas = docSnapshot.ConvertTo<ColunasFirestore>();
                        colunas.Add(firestoreColunas.ToColunas());
                    }
                }

                return colunas;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar colunas por tipo de arquivo.", ex);
            }
        }
        #endregion

        #region Cliente
        public async Task<Cliente> BuscarClientePorCodigo(string codigoCliente)
        {
            try
            {
                CollectionReference clientesRef = _firebaseStorage.Firestore.Collection("Cliente");

                QuerySnapshot querySnapshot = await clientesRef.WhereEqualTo("CodigoCliente", codigoCliente).GetSnapshotAsync();

                if (querySnapshot.Count > 0)
                {
                    DocumentSnapshot documentSnapshot = querySnapshot.Documents.First();

                    ClienteFirestore firestoreCliente = documentSnapshot.ConvertTo<ClienteFirestore>();

                    return firestoreCliente.ToCliente();
                }
                else
                {
                    return null!;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar cliente por código.", ex);
            }
        }


        public async Task<bool> CadastrarCliente(Cliente clienteNovo)
        {


            try
            {

                var clienteDTO = ClienteMap.ToFirestoreCliente(clienteNovo);

                CollectionReference clientesRef = _firebaseStorage.Firestore.Collection("Cliente");

                DocumentReference newDocRef = await clientesRef.AddAsync(clienteDTO);

                if (newDocRef != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Produto
        public async Task<bool> BuscarProdutoPorSKU(string skuProduto)
        {
            try
            {
                CollectionReference clientesRef = _firebaseStorage.Firestore.Collection("Produto");

                Query query = clientesRef.WhereEqualTo("Sku", skuProduto);

                QuerySnapshot snapshot = await query.GetSnapshotAsync();

                return snapshot.Documents.Any();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar cliente por código.", ex);
            }
        }
        public async Task<bool> CadastrarProduto(Produto produto)
        {
            try
            {
                var produtoDto = ProdutoMap.ToFirestoreProduto(produto);
                CollectionReference clientesRef = _firebaseStorage.Firestore.Collection("Produto");

                DocumentReference newDocRef = await clientesRef.AddAsync(produto);
                return true; // TO-DO Corrigir aqui ainda.
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Categoria

        public async Task<Categoria> BuscarCategoriaPorCodigo(string codigoCategoria)
        {
            try
            {
                CollectionReference categoriasRef = _firebaseStorage.Firestore.Collection("Categorias");

                QuerySnapshot querySnapshot = await categoriasRef.WhereEqualTo("Codigo", codigoCategoria).GetSnapshotAsync();

                if (querySnapshot.Count > 0)
                {
                    DocumentSnapshot documentSnapshot = querySnapshot.Documents.First();

                    CategoriaFirestore firestoreCategoria = documentSnapshot.ConvertTo<CategoriaFirestore>();

                    return firestoreCategoria.ToCategoria();
                }
                else
                {
                    return null!;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> CadastrarCategoria(Categoria categoria)
        {
            try
            {
                var categoriaDto = CategoriaMap.ToFirestoreCategoria(categoria);

                CollectionReference categoriasRef = _firebaseStorage.Firestore.Collection("Categorias");
                DocumentReference newDocRef = await categoriasRef.AddAsync(categoriaDto);


                if (newDocRef != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
        #endregion
    }
}

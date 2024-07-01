using Rino.Dominio.Entidades;
using Rino.Dominio.Interfaces.Negocio;
using Rino.Dominio.Interfaces.Servicos;
using Rino.Dominio.Negocio.Servicos;

namespace Rino.Dominio.Negocio
{
    public class CategoriaNegocio : ICategoriaNegocio
    {
        private readonly IFirebaseArquivoServico _firebaseServico;
        
        public CategoriaNegocio(IFirebaseArquivoServico firebaseServico)
        {
            _firebaseServico = firebaseServico;
        }

        public async Task<Categoria> Cadastrar(string codigoCategoria)
        {
            bool cadastrado = false;
            Categoria categoria = new Categoria();
            try
            {
                categoria = await _firebaseServico.BuscarCategoriaPorCodigo(codigoCategoria);

                if (categoria is null)
                {
                    var gerador = new GeradorCategoriaAleatorio();

                    var novaCategoria = gerador.GerarCategoriaAleatoria(codigoCategoria);
                    cadastrado = await _firebaseServico.CadastrarCategoria(novaCategoria);
                    categoria = await _firebaseServico.BuscarCategoriaPorCodigo(codigoCategoria);
                }


                return categoria;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

using Rino.Dominio.DTOs.Usuario;
using Rino.Dominio.Entidades;

namespace Rino.Dominio.Interfaces.Servicos
{
    public interface IFirebaseArquivoServico
    {
        Task<TipoArquivo> BuscarArquivoPorNome(string nomeArquivo);
        Task<IEnumerable<Colunas>> BuscaColunasPorTipoArquivo(string idTipoArquivo);

        Task<Categoria> BuscarCategoriaPorCodigo(string codigoCategoria);
        Task<bool> CadastrarCategoria(Categoria categoria);

        Task<Cliente> BuscarClientePorCodigo(string codigoCliente);
        Task<bool> CadastrarCliente(Cliente clienteNovo);

        Task<bool> CadastrarProduto(Produto novoProduto);
        Task<bool> BuscarProdutoPorSKU(string skuProduto);
    }
}

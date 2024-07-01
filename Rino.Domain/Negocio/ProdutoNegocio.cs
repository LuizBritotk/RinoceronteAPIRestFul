
using Rino.Dominio.DTOs.Produto;
using Rino.Dominio.Entidades;
using Rino.Dominio.Interfaces.Negocio;
using Rino.Dominio.Interfaces.Servicos;
using Rino.Dominio.Negocio.Servicos;
using Rino.Dominio.Util;

namespace Rino.Dominio.Negocio
{
    public class ProdutoNegocio : IProdutoNegocio
    {
        private readonly IFirebaseArquivoServico _firebaseServico;
        private readonly GeradorProdutoAleatorio _geradorProdutoAleatorio;

        public ProdutoNegocio(IFirebaseArquivoServico firebaseArquivoServico, GeradorProdutoAleatorio geradorProdutoAleatorio)
        {
            _firebaseServico = firebaseArquivoServico;
            _geradorProdutoAleatorio = geradorProdutoAleatorio;
        }

        public async Task<Produto> CadastrarProduto(ProdutoDTO produtoDTO)
        {

            bool cadastrado = false;
            try
            {
                Categoria categoria = await _firebaseServico.BuscarCategoriaPorCodigo(produtoDTO.Categoria.Codigo);
                Cliente cliente = await _firebaseServico.BuscarClientePorCodigo(produtoDTO.Cliente.CodigoCliente);

                if (categoria is null)
                    throw new InvalidOperationException($"Categoria com ID '{produtoDTO.Categoria.ID}' não encontrada.");

                if (cliente is null)
                    throw new InvalidOperationException($"Cliente com ID '{produtoDTO.Cliente.ID}' não encontrado.");

                var novoProduto = _geradorProdutoAleatorio.GerarProdutoAleatorio(categoria, cliente, produtoDTO);

                if (novoProduto is not null)
                {

                    bool existeProdutoPorSKU = await _firebaseServico.BuscarProdutoPorSKU(produtoDTO.SkuProduto);
               
                    if(existeProdutoPorSKU is false)
                        cadastrado = await _firebaseServico.CadastrarProduto(novoProduto);

                }


                if (cadastrado)
                    return novoProduto!;
                else
                    throw new Exception("Falha ao cadastrar o produto.");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar o produto.", ex);
            }
        }

    }
}

using Rino.Dominio.Entidades;
using Rino.Dominio.Interfaces.Negocio;
using Rino.Dominio.Interfaces.Servicos;
using Rino.Dominio.Negocio.Servicos;
using Rino.Dominio.Util;

namespace Rino.Dominio.Negocio
{
    public class ClienteNegocio : IClienteNegocio
    {
        private readonly IFirebaseArquivoServico _firebaseServico;
        private readonly GeradorClienteAleatorio _geradorClienteAleatorio;

        public ClienteNegocio(IFirebaseArquivoServico firebaseServico, GeradorClienteAleatorio geradorClienteAleatorio)
        {
            _firebaseServico = firebaseServico;
            _geradorClienteAleatorio = geradorClienteAleatorio;
        }

        public async Task<Cliente> CadastrarCliente(string codigoCliente)
        {
            Cliente cliente = new Cliente();
            bool cadastrado = false;
            try
            {
                cliente = await _firebaseServico.BuscarClientePorCodigo(codigoCliente);

                if (cliente is null)
                {
                    var novoCliente = _geradorClienteAleatorio.GerarClienteAleatorio(codigoCliente);
                    cadastrado = await _firebaseServico.CadastrarCliente(novoCliente);

                    cliente = await _firebaseServico.BuscarClientePorCodigo(codigoCliente);
                }


                return cliente;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

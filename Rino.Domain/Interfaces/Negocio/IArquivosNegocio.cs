using Rino.Dominio.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Dominio.Interfaces.Negocio
{
    public interface IArquivosNegocio
    {
        Task<RespostaPadrao> ProcessarArquivo(Stream stream, string nomeArquivo);
    }
}

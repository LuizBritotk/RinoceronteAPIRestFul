using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Infra.Servicos.Autenticacao
{
    public interface IJwtServico
    {
        string GenerateJwtToken(string id, string userEmail);
    }
}


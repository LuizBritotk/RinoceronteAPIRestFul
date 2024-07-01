using Rino.Dominio.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Dominio.Entidades
{
    public class Colunas
    {
        public string Entidade_ID { get; set; }
        public string ID { get; set; }
        public string Nome { get; set; }
        public string Propriedade_ID { get; set; }
        public TipoCampo Tipo { get; set; }
        public int Posicao { get; set; }
    }

}

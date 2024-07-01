using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rino.Dominio.Entidades
{
    public class Cliente
    {
        public string CPF { get; set; } 
        public DateTime Criacao { get; set; }
        public DateTime Data { get; set; } 
        public string ID { get; set; }
        public string Nome { get; set; } 
        public  string CodigoCliente { get; set; }
    }
}

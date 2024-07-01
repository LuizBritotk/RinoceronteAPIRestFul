using Rino.Dominio.Entidades;
using System;
using System.Linq;

namespace Rino.Dominio.Negocio.Servicos
{
    public class GeradorClienteAleatorio
    {

        #region Métodos Privados

        private static Random random = new Random();
        private static string[] primeirosNomes = { "Luana", "Carlos", "Maria", "João", "Ana", "Pedro", "Bruna", "Felipe", "Julia", "Marcos" };
        private static string[] sobrenomes = { "Telis", "Silva", "Oliveira", "Souza", "Pereira", "Costa", "Fernandes", "Gomes", "Lima", "Ribeiro" };


        /// <summary>
        /// Gera um nome aleatório para o cliente.
        /// </summary>
        private static string GerarNome()
        {
            string primeiroNome = primeirosNomes[random.Next(primeirosNomes.Length)];
            string sobrenome = sobrenomes[random.Next(sobrenomes.Length)];
            return $"{primeiroNome} {sobrenome}";
        }

        
        /// <summary>
        /// Gera um CPF válido.
        /// </summary>
        private static string GerarCPF()
        {
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string cpf = "";
            int soma;
            int resto;

            for (int i = 0; i < 9; i++)
                cpf += random.Next(0, 10);

            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            cpf += resto;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            cpf += resto;

            return cpf;
        }

        /// <summary>
        /// Gera uma data de nascimento aleatória entre os anos de 1950 e 2000.
        /// </summary>
        private static DateTime GerarDataNascimento()
        {
            // Define um intervalo para o ano de nascimento
            int year = random.Next(1950, 2000);

            // Gera um mês aleatório
            int month = random.Next(1, 13);

            // Gera um dia aleatório, considerando o número de dias no mês/ano gerado
            int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);

            // Retorna a data gerada ajustada para o fuso horário UTC-3
            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc).AddHours(-3);
        }

        #endregion

        #region Métodos Publicos

        /// <summary>
        /// Cria um novo cliente com base no código fornecido.
        /// </summary>
        public Cliente GerarClienteAleatorio(string codigoCliente)
        {
            return new Cliente
            {
                ID = Guid.NewGuid().ToString(),
                CPF = GerarCPF(),
                Criacao = DateTime.UtcNow.AddHours(-3),
                Data = GerarDataNascimento(),
                Nome = GerarNome(),
                CodigoCliente = codigoCliente
            };
        }
        #endregion
    }
}

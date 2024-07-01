using Rino.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace Rino.Dominio.Negocio.Servicos
{
    public class GeradorCategoriaAleatorio
    {
        private static readonly string[] Adjetivos = { "Bonita", "Elegante", "Confortável", "Moderna", "Clássica", "Estilosa" };
        private static readonly string[] Substantivos = { "Camiseta", "Calça", "Blusa", "Jaqueta", "Vestido", "Saia" };

        private readonly Random _random;

        public GeradorCategoriaAleatorio()
        {
            _random = new Random();
        }

        public Categoria GerarCategoriaAleatoria(string codigo)
        {
            string nomeProduto = GerarNomeAleatorio();
            var categoria = new Categoria
            {
                Codigo = codigo,
                Criacao = DateTime.Now,
                Criador = "0d601cf6-145b-4b9c-a785-711943d4459c",
                ID = Guid.NewGuid().ToString(),
                Nome = nomeProduto
            };

            return categoria;
        }

        private string GerarNomeAleatorio()
        {
            string adjetivo = Adjetivos[_random.Next(Adjetivos.Length)];
            string substantivo = Substantivos[_random.Next(Substantivos.Length)];

            return $"{adjetivo} {substantivo}";
        }
    }
}

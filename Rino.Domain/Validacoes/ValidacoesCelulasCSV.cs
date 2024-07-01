using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Rino.Dominio.Validacoes
{
    public class ValidacoesCelulasCSV
    {
        private readonly Dictionary<Type, Action<object, string>> _validacoes;

        #region Métodos Privados de Validação

        private void ValidacaoString(object valor, string nomeCampo)
        {
            var stringValue = valor as string;
            if (string.IsNullOrWhiteSpace(stringValue))
                throw new ArgumentException($"O campo '{nomeCampo}' não pode ser vazio ou apenas espaços em branco.");
        }

        private void ValidacaoData(object valor, string nomeCampo)
        {
            if (valor is not DateTime)
                throw new ArgumentException($"O valor do campo '{nomeCampo}' não é do tipo DateTime.");

            var dateValue = (DateTime)valor;
            if (dateValue == default || dateValue == DateTime.MinValue)
                throw new ArgumentException($"O campo '{nomeCampo}' não pode ser a data padrão.");
        }

        private void ValidacaoDecimal(object valor, string nomeCampo)
        {
            if (valor is not decimal)
                throw new ArgumentException($"O valor do campo '{nomeCampo}' não é do tipo decimal.");

            var decimalValue = (decimal)valor;
            if (decimalValue <= 0)
                throw new ArgumentException($"O campo '{nomeCampo}' deve ter um valor decimal maior que zero.");

            // Verificar se o formato está correto
            var decimalString = valor.ToString();
            if (!decimalString.Contains("."))
                throw new ArgumentException($"O campo '{nomeCampo}' deve utilizar ponto como separador decimal.");

            // Tentar parsear o valor com o ponto
            if (!decimal.TryParse(decimalString, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                throw new ArgumentException($"O valor do campo '{nomeCampo}' não está no formato correto.");
        }

        private void ValidacaoInteiro(object valor, string nomeCampo)
        {
            if (valor is not int)
                throw new ArgumentException($"O valor do campo '{nomeCampo}' não é do tipo inteiro.");

            var intValue = (int)valor;
            if (intValue <= 0)
                throw new ArgumentException($"O campo '{nomeCampo}' deve ser um valor inteiro maior que zero.");
        }

        #endregion

        public ValidacoesCelulasCSV()
        {
            _validacoes = new Dictionary<Type, Action<object, string>>
            {
                { typeof(string), ValidacaoString },
                { typeof(DateTime), ValidacaoData },
                { typeof(decimal), ValidacaoDecimal },
                { typeof(int), ValidacaoInteiro },
            };
        }

        public void ValidarCampo(string nomeCampo, object valor)
        {
            if (valor == null)
                throw new ArgumentNullException(nameof(valor), $"O valor do campo '{nomeCampo}' não pode ser nulo.");

            var tipo = valor.GetType();
            if (_validacoes.ContainsKey(tipo))
            {
                _validacoes[tipo](valor, nomeCampo);
            }
            else
            {
                throw new InvalidOperationException($"Tipo de valor '{tipo}' não é suportado para validação do campo '{nomeCampo}'.");
            }
        }
    }
}

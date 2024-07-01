
using Microsoft.VisualBasic.FileIO;
using OfficeOpenXml;
using Rino.Dominio.DTOs.Produto;
using Rino.Dominio.Entidades;
using Rino.Dominio.Interfaces.Negocio;
using Rino.Dominio.Interfaces.Servicos;
using Rino.Dominio.Util;
using Rino.Dominio.Validacoes;
using System.Data;
using System.Globalization;

namespace Rino.Dominio.Negocio
{
    public class ArquivosNegocio : IArquivosNegocio
    {
        private readonly ValidacoesCelulasExcel _validacoesCelulasExcel;
        private readonly ValidacoesCelulasCSV _validacoesCelulasCsv;
        private readonly IFirebaseArquivoServico _firebaseArquivoServico;
        private readonly IClienteNegocio _clienteNegocio;
        private readonly ICategoriaNegocio _categoriaNegocio;
        private readonly IProdutoNegocio _produtoNegocio;

        public ArquivosNegocio(
            ValidacoesCelulasExcel validacoesCelulas,
            ValidacoesCelulasCSV validacoesCelulasCsv,
            IFirebaseArquivoServico firebaseArquivoServico,
            IProdutoNegocio produtoNegocio,
            IClienteNegocio clienteNegocio,
            ICategoriaNegocio categoriaNegocio)
        {
            _validacoesCelulasExcel = validacoesCelulas;
            _firebaseArquivoServico = firebaseArquivoServico;
            _produtoNegocio = produtoNegocio;
            _clienteNegocio = clienteNegocio;
            _categoriaNegocio = categoriaNegocio;
            _validacoesCelulasCsv = validacoesCelulasCsv;
        }

        public async Task<RespostaPadrao> ProcessarArquivo(Stream stream, string nomeArquivo)
        {
            try
            {
                TipoArquivo tipoArquivo = await _firebaseArquivoServico.BuscarArquivoPorNome(nomeArquivo);
                if (tipoArquivo is null)
                    return new RespostaPadrao("Não encontrado o processo desse arquivo", true, 400);

                IEnumerable<Colunas> camposArquivo = await _firebaseArquivoServico.BuscaColunasPorTipoArquivo(tipoArquivo.ID);
                if (camposArquivo is null)
                    return new RespostaPadrao("Não encontrado as colunas do arquivo", true, 400);

                string extensaoEsperada = tipoArquivo.Extensao.ToLower();
                string extensaoArquivoImportado = Path.GetExtension(nomeArquivo)?.ToLower()!;
                if (extensaoArquivoImportado != extensaoEsperada)
                    return new RespostaPadrao("Formato de arquivo inválido", true, 400);

                return extensaoArquivoImportado switch
                {
                    ".xlsx" => await ProcessarArquivoExcel(stream, camposArquivo),
                    ".csv" => await LerArquivoCSV(stream, camposArquivo),
                    ".txt" => await ProcessarArquivoTxt(stream, camposArquivo),
                    ".pdf" => await ProcessarArquivoPdf(stream, camposArquivo),
                    _ => new RespostaPadrao("Tipo de arquivo não suportado", true, 400),
                };
            }
            catch (Exception ex)
            {
                return new RespostaPadrao($"Erro interno no servidor: {ex.Message}", true, 500);
            }
        }

        #region Métodos Privados

        private async Task<RespostaPadrao> LerArquivoCSV(Stream stream, IEnumerable<Colunas> camposArquivo)
        {
            List<string[]> linhas = new List<string[]>();

            using (TextFieldParser parser = new TextFieldParser(stream))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    string[] linha = parser.ReadFields();
                    linhas.Add(linha);
                }
            }

            return await ProcessarLinhasCSV(linhas, camposArquivo);
        }

        private async Task<RespostaPadrao> ProcessarLinhasCSV(List<string[]> linhas, IEnumerable<Colunas> camposArquivo)
        {
            var listaDeErros = new List<string>();
            var totalPorSKU = new Dictionary<string, decimal>();
            var listaDeProdutos = new List<ProdutoDTO>();

            foreach (var linha in linhas)
            {
                try
                {

                    string categoriaProduto = linha[0];                     // Assumindo que Categoria do Produto está no primeiro índice
                    string skuProduto = linha[1];                           // Assumindo que Sku/Produto está no segundo índice
                    
                    string codigoCliente = linha[2]; 
                    DateTime data;
                    if (!DateTime.TryParseExact(linha[3], "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out data))
                        throw new ArgumentException($"Formato de data inválido para o campo 'Data'. Esperado formato MM/DD/YYYY.");

                           // Assumindo que Código Cliente está no quarto índice
                    int quantidade = Convert.ToInt32(linha[4]);             // Assumindo que Quantidade está no quinto índice
                    decimal valorFaturamento;                               // Assumindo que Valor de Faturamento está no sexto índice
                    if (!decimal.TryParse(linha[5], NumberStyles.Any, CultureInfo.InvariantCulture, out valorFaturamento))
                        throw new ArgumentException($"Formato de valor de faturamento inválido para o campo 'Valor de Faturamento'. Esperado número decimal.");
                   
                    _validacoesCelulasExcel.ValidarCampo(null, "Categoria do Produto", categoriaProduto);
                    _validacoesCelulasExcel.ValidarCampo(null, "Sku/Produto", skuProduto);
                    _validacoesCelulasExcel.ValidarCampo(null, "Data", data);
                    _validacoesCelulasExcel.ValidarCampo(null, "Código Cliente", codigoCliente);
                    _validacoesCelulasExcel.ValidarCampo(null, "Quantidade", quantidade);
                    _validacoesCelulasExcel.ValidarCampo(null, "Valor de Faturamento", valorFaturamento);


                    Categoria categoria = await _categoriaNegocio.Cadastrar(categoriaProduto);
                    if (categoria is null)
                    {
                        listaDeErros.Add($"Linha {linhas.IndexOf(linha) + 1}: Falha ao cadastrar categoria {categoriaProduto}.");
                        continue;
                    }

                    Cliente cliente = await _clienteNegocio.CadastrarCliente(codigoCliente);
                    if (cliente is null)
                    {
                        listaDeErros.Add($"Linha {linhas.IndexOf(linha) + 1}: Falha ao cadastrar cliente código {codigoCliente}.");
                        continue;
                    }

                    var produto = new ProdutoDTO
                    {
                        Categoria = categoria,
                        Cliente = cliente,
                        SkuProduto = skuProduto,
                        Data = data,
                        Quantidade = quantidade,
                        ValorFaturamento = valorFaturamento
                    };
                    listaDeProdutos.Add(produto);
                }
                catch (Exception ex)
                {
                    listaDeErros.Add($"Linha {linhas.IndexOf(linha) + 1}: Erro ao processar linha do arquivo: {ex.Message}");
                }
            }

            foreach (var produto in listaDeProdutos)
            {
                try
                {
                    if (totalPorSKU.ContainsKey(produto.SkuProduto))
                        totalPorSKU[produto.SkuProduto] += produto.Quantidade * produto.ValorFaturamento;
                    else
                        totalPorSKU.Add(produto.SkuProduto, produto.Quantidade * produto.ValorFaturamento);

                    await _produtoNegocio.CadastrarProduto(produto);
                }
                catch (Exception ex)
                {
                    listaDeErros.Add($"Erro ao cadastrar produto SKU {produto.SkuProduto}: {ex.Message}");
                }
            }

            return listaDeErros.Count > 0
                ? new RespostaPadrao("Erros encontrados ao processar o arquivo.", true, 500, listaDeErros)
                : new RespostaPadrao("Processamento concluído com sucesso.", false, 200);
        }


            
        private int ObterIndiceColuna(IEnumerable<Colunas> camposArquivo, string nomeColuna)
        {
            var coluna = camposArquivo.FirstOrDefault(c => c.Nome.Equals(nomeColuna, StringComparison.OrdinalIgnoreCase));
            if (coluna == null)
                throw new ArgumentException($"Coluna '{nomeColuna}' não encontrada.");

            return coluna.Posicao - 1   ; // Posicao começa de 1, ajustar para index de 0
        }

        private Task<RespostaPadrao> ProcessarArquivoPdf(Stream stream, IEnumerable<Colunas> camposArquivo)
        {
            throw new NotImplementedException();
        }

        private Task<RespostaPadrao> ProcessarArquivoTxt(Stream stream, IEnumerable<Colunas> camposArquivo)
        {
            throw new NotImplementedException();
        }

        private async Task<RespostaPadrao> ProcessarArquivoExcel(Stream stream, IEnumerable<Colunas> camposArquivo)
        {
            DataTable tabela = new DataTable();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                bool hasHeader = true;

                foreach (var coluna in camposArquivo)
                    tabela.Columns.Add(coluna.Nome);

                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                {
                    var colunasArquivo = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];

                    if (colunasArquivo.Count() != camposArquivo.Count())
                        return new RespostaPadrao($"Linha {rowNum}: O número de colunas no arquivo não coincide com o esperado.", true, 400);

                    DataRow row = tabela.NewRow();
                    foreach (var cell in colunasArquivo)
                        row[cell.Start.Column - 1] = cell.Text;

                    tabela.Rows.Add(row);
                }
            }

            return await ProcessarTabela(tabela);
        }

        private async Task<RespostaPadrao> ProcessarTabela(DataTable tabela)
        {
            var listaDeErros = new List<string>();
            var totalPorSKU = new Dictionary<string, decimal>();
            var listaDeProdutos = new List<ProdutoDTO>();

            foreach (DataRow row in tabela.Rows)
            {
                try
                {
                    string categoriaProduto = row.Field<string>("Categoria do Produto")!;
                    string skuProduto = row.Field<string>("Sku/Produto")!;
                    DateTime data = row.Field<DateTime>("Data");
                    string codigoCliente = row.Field<string>("Código Cliente")!;
                    int quantidade = row.Field<int>("Quantidade");
                    decimal valorFaturamento = row.Field<decimal>("Valor de Faturamento");

                    _validacoesCelulasExcel.ValidarCampo(row, "Categoria do Produto", categoriaProduto);
                    _validacoesCelulasExcel.ValidarCampo(row, "Sku/Produto", skuProduto);
                    _validacoesCelulasExcel.ValidarCampo(row, "Data", data);
                    _validacoesCelulasExcel.ValidarCampo(row, "Código Cliente", codigoCliente);
                    _validacoesCelulasExcel.ValidarCampo(row, "Quantidade", quantidade);
                    _validacoesCelulasExcel.ValidarCampo(row, "Valor de Faturamento", valorFaturamento);

                    Categoria categoria = await _categoriaNegocio.Cadastrar(categoriaProduto);
                    if (categoria is null)
                    {
                        listaDeErros.Add($"Linha {tabela.Rows.IndexOf(row) + 1}: Falha ao cadastrar categoria {categoriaProduto}.");
                        continue;
                    }

                    Cliente cliente = await _clienteNegocio.CadastrarCliente(codigoCliente);
                    if (cliente is null)
                    {
                        listaDeErros.Add($"Linha {tabela.Rows.IndexOf(row) + 1}: Falha ao cadastrar cliente código {codigoCliente}.");
                        continue;
                    }

                    var produto = new ProdutoDTO
                    {
                        Categoria = categoria,
                        Cliente = cliente,
                        SkuProduto = skuProduto,
                        Data = data,
                        Quantidade = quantidade,
                        ValorFaturamento = valorFaturamento
                    };
                    listaDeProdutos.Add(produto);
                }
                catch (Exception ex)
                {
                    listaDeErros.Add($"Linha {tabela.Rows.IndexOf(row) + 1}: Erro ao processar linha do arquivo: {ex.Message}");
                }
            }

            foreach (var produto in listaDeProdutos)
            {
                try
                {
                    if (totalPorSKU.ContainsKey(produto.SkuProduto))
                        totalPorSKU[produto.SkuProduto] += produto.Quantidade * produto.ValorFaturamento;
                    else
                        totalPorSKU.Add(produto.SkuProduto, produto.Quantidade * produto.ValorFaturamento);

                    await _produtoNegocio.CadastrarProduto(produto);
                }
                catch (Exception ex)
                {
                    listaDeErros.Add($"Erro ao cadastrar produto SKU {produto.SkuProduto}: {ex.Message}");
                }
            }

            return listaDeErros.Count > 0
                ? new RespostaPadrao("Erros encontrados ao processar o arquivo.", true, 500, listaDeErros)
                : new RespostaPadrao("Processamento concluído com sucesso.", false, 200);
        }

        #endregion
    }
}

# Projeto Rino
Este projeto consiste em uma API RESTful desenvolvida em .NET Core para gerenciar e processar arquivos, integrando-se com serviços Firebase para armazenamento e operações de dados.

## Funcionalidades Principais
### Autenticação
A autenticação na API é gerenciada através de tokens JWT (JSON Web Tokens). Para acessar os endpoints protegidos, o cliente deve incluir o token JWT no cabeçalho da requisição.

### Upload Arquivo
A API suporta o upload de arquivos nos formatos .xlsx, .txt e .pdf utilizando o endpoint /api/arquivos/upload. O arquivo deve ser enviado como parte de um formulário multipart/form-data. A autenticação é necessária para acessar este endpoint, sendo obrigatório incluir o token JWT no cabeçalho da requisição.

***Exemplo de uso utilizando cURL:***
```bash
Copy code
curl -X POST \
  https://localhost:5001/api/arquivos/upload \
  -H 'Authorization: Bearer SEU_TOKEN_JWT' \
  -F 'file=@/caminho/do/seu/arquivo.xlsx'
 ```

### Processamento de Arquivos
A API é capaz de processar arquivos recebidos, validando e persistindo dados no Firebase. Existe uma regra que busca o PREFIXO do arquivo, então é necessário que o arquivo tenha o nome FATUR_TK.

## Integração com Firebase
Utiliza o Firebase para armazenamento de tipos de arquivo, colunas de arquivos, categorias de produtos, clientes e produtos.

## Estrutura do Projeto
O projeto está estruturado da seguinte forma:
```bash
Rinoceronte/
├── Rino.API/
│   ├── Controllers/
│   │   ├── ArquivosController.cs
│   │   └── AutenticacaoController.cs
│   ├── appsettings.json
│   ├── firebase-credentials.json
│   └── Program.cs
├── Rino.Dominio/
│   ├── Entidades/
│   │   ├── Categoria.cs
│   │   ├── Cliente.cs
│   │   ├── Colunas.cs
│   │   ├── Produto.cs
│   │   ├── TipoArquivo.cs
│   │   ├── TipoCampo.cs
│   │   └── Usuario.cs
│   ├── Interfaces/
│   │   ├── Negocio/
│   │   │   ├── ICategoriaNegocio.cs
│   │   │   ├── IClienteNegocio.cs
│   │   │   ├── IProdutoNegocio.cs
│   │   │   └── IUsuarioNegocio.cs
│   │   └── Servicos/
│   │       ├── Autenticacao/
│   │       │   └── IJwtServico.cs
│   │       ├── IFirebaseArquivoServico.cs
│   │       └── IFirebaseUsuarioServico.cs
│   ├── DTOs/
│   │   └── Produto/
│   │       └── ProdutoDTO.cs
│   ├── Negocio/
│   │   ├── CategoriaNegocio.cs
│   │   ├── ClienteNegocio.cs
│   │   ├── ProdutoNegocio.cs
│   │   ├── UsuarioNegocio.cs
│   │   └── Servicos/
│   │       ├── GeradorCategoriaAleatorio.cs
│   │       ├── GeradorProdutoAleatorio.cs
│   │       └── GeradorUsuarioAleatorio.cs
│   ├── Util/
│   │   ├── RespostaPadrao.cs
│   │   └── Validacoes/
│   │       └── ValidacoesCelulas.cs
├── Rino.Infra/
│   ├── Configuracao/
│   │   └── JwtSettings.cs
│   ├── Mapeamentos/
│   │   └── UsuarioMap.cs
│   ├── Conversores/
│   │   ├── FirestoreDateTimeConversor.cs
│   │   └── UsuarioConversor.cs
│   ├── ServicosExternos/
│   │   └── Firebase/
│   │       ├── FirebaseArquivosServico.cs
│   │       └── FirebaseUsuarioServico.cs
│   └── Entidades/
│       └── FireBaseStore.cs
└── README.md
```

 ## Pré-Requisitos
 - .NET 6 SDK
 - Visual Studio 2022

## Configuração
**1. Clone o repositório** 
```bash
Copy code
git clone https://github.com/seu-usuario/rino-api.git
cd rino-api
```

**2. Configure as credenciais do Firebase**
Obtenha as credenciais do Firebase e configure-as no projeto, seguindo a documentação oficial.

**3. Configure o ambiente de desenvolvimento**:

Defina as configurações de ambiente necessárias no arquivo appsettings.json ou através de variáveis de ambiente.

## Instalação
**1. Restaure os pacotes NuGet**
```bash
dotnet restore
```
**2. Inicie a aplicação**
```bash
dotnet run
```
**3. Acesse a API localmente em https://localhost:5001.**

Uso
Faça upload de arquivos utilizando o endpoint POST /api/arquivos/upload.
Consulte a documentação da API para detalhes sobre outros endpoints e operações disponíveis.

## Pacotes Necessários

Para compilar e executar este projeto, é necessário instalar os seguintes pacotes via NuGet:

- **OfficeOpenXml** (versão 5.6.0): Para manipulação de arquivos Excel.
  ```bash
  dotnet add package OfficeOpenXml --version 5.6.0
  
- **Google.Cloud.Firestore** (versão 3.7.0): Para integração com o Firestore da Google.

  ```bash
    dotnet add package Google.Cloud.Firestore --version 3.7.0

- **Microsoft.Extensions.Configuration** (versão 6.0.0): Para gerenciamento de configurações.
  ```bash
   dotnet add package Microsoft.Extensions.Configuration --version 6.0.0

- **Microsoft.IdentityModel.Tokens** (versão 6.15.0): Para manipulação de tokens JWT.
  ```bash
   dotnet add package Microsoft.IdentityModel.Tokens --version 6.15.0

- **Microsoft.AspNetCore.Mvc.NewtonsoftJson** : Para suporte a JSON. 
  ```bash
   dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson

- **Microsoft.Extensions.Http** : Para clientes HTTP configuráveis.
  ```bash
  dotnet add package Microsoft.Extensions.Http

- **FluentValidation** : Para validação dos dados (versão 10.3.3)
  ```bash 
   dotnet add package FluentValidation --version 10.3.3

- **Microsoft.Extensions.Http** : Para clientes HTTP configuráveis.
  ```bash 
   dotnet add package Microsoft.Extensions.Http

- **AutoMapper.Extensions.Microsoft.DependencyInjection** : Para mapear objetos.
  ```bash 
  dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection


## Contribuição
Contribuições são bem-vindas! Sinta-se à vontade para abrir pull requests para correções ou novas funcionalidades.

## Licença
Este projeto está licenciado sob a MIT License.

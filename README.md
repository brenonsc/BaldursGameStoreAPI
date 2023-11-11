# BaldursGameStoreAPI :space_invader::video_game:
![License](https://badgen.net/badge/License/MIT/purple?icon=)
![.NET](https://badgen.net/badge/.NET/v7.0/blue?icon=)
![NuGet](https://badgen.net/badge/icon/Packages/green?icon=nuget&label)
![Docker](https://badgen.net/badge/icon/Available?icon=docker&label)

Esta é a documentação para a API de uma loja de games que permite a manipulação de dados de produtos e categorias relacionadas a jogos. Esta API foi construída seguindo as melhores práticas do ASP.NET e oferece funcionalidades completas para gerenciar produtos e categorias.

<br>

## Tecnologias e Ferramentas utilizadas 💻
<div>
    <img align='center' height='50' width='70' title='.NET Core' alt='dotnet' src='https://github.com/devicons/devicon/blob/master/icons/dotnetcore/dotnetcore-original.svg' />
    <img align='center' height='50' width='50' title='Nuget' alt='nuget' src='https://github.com/devicons/devicon/blob/master/icons/nuget/nuget-original.svg' />
    <img align='center' height='62' width='72' title='Swagger' alt='swagger' src='https://github.com/bush1D3v/tsbank_api/assets/133554156/6739401f-d03b-47f8-b01f-88da2a9075d1' />
    <img align='center' height='53' width='55' title='JsonWebToken' alt='jsonwebtoken' src='https://github.com/bush1D3v/solid_rest_api/assets/133554156/d23ffb9d-aedc-4d68-9209-7268d7f41ce6' /> &nbsp;
    <img align='center' height='48' width='48' title='Bcrypt' alt='bcrypt' src='https://bcrypt.online/images/bcrypt-esse-tools-logo-square.svg' /> 
    <img align='center' height='50' width='65' title='PostgreSQL' alt='postgresql' src='https://github.com/devicons/devicon/blob/master/icons/postgresql/postgresql-original.svg' />
    <img align='center' height='50' width='64' title='Microsoft SQL Server' alt='mssql' src='https://camo.githubusercontent.com/2636f807a9f2c751d54b817ae081a8a348a6d4eeec7c344c36bdb5f5717e1bcb/68747470733a2f2f63646e2e73696d706c6569636f6e732e6f72672f6d6963726f736f667473716c7365727665722f434332393237' />
    <img align='center' height='49' width='49' title='Jetbrains Rider' alt='rider' src='https://upload.wikimedia.org/wikipedia/commons/thumb/6/6e/JetBrains_Rider_Icon.svg/1200px-JetBrains_Rider_Icon.svg.png' /> &nbsp;&nbsp;
    <img align='center' height='48' width='48' title='Postman' alt='postman' src='https://seeklogo.com/images/P/postman-logo-0087CA0D15-seeklogo.com.png' /> &nbsp;
    <img align='center' height='63' width='63' title='Docker' alt='docker' src='https://github.com/devicons/devicon/blob/master/icons/docker/docker-original.svg' />
</div>

<br>

## Testando a API :man_scientist:

### Na nuvem ☁️
Para testar a API, basta acessar o link do <a target="_blank" href="https://baldursgamestore.onrender.com">deploy</a> e começar a utilizar.



### Localmente (utilizando Docker) :whale:

Certo, se você já tem o Dockerfile criado e apenas precisa seguir com os passos de configuração e execução, aqui estão as instruções revisadas:

### 1. Clone o Projeto

```bash
git clone https://github.com/brenonsc/BaldursGameStoreAPI.git
cd BaldursGame
```

### 2. Inicialize o contêiner do Docker

```bash
docker compose up
```

### 3. Configure o appsettings.json

Certifique-se de alterar a variável "Environment":"Start" no arquivo `appsettings.json do projeto que está com a configuração "PROD", e deve ser alterada para "DEV", como representado abaixo:

```json
"Environment": {
    "Start": "DEV"
 },
```

### 4. Execute a aplicação

Volte ao Terminal ou CMD e execute os seguintes comandos:

```bash
cd BaldursGame
dotnet run
```

Outra opção é usar uma IDE .NET de sua preferência, como Visual Studio ou Jetbrains Rider. A aplicação estará disponível em [localhost://5000](http://localhost:5000/swagger/index.html), no seu navegador. 

<br>

A API está documentada no <a target="_blank" href="https://swagger.io/">Swagger</a>. Certifique-se de testar todas as operações CRUD para os recursos "Produto", "Categoria" e "Usuários", bem como os endpoints de busca por intervalo de preço e título ou console.

<br>

## Autenticação com JWT :key:

Para usar o método de login e obter um token JWT válido, faça uma solicitação POST para `/api/usuarios/logar` com as credenciais do usuário no corpo da solicitação. O servidor irá gerar um token JWT que deve ser incluído no cabeçalho das solicitações subsequentes como Bearer Token para autenticar o usuário. O token é válido por 1 hora, após o qual será necessário fazer login novamente.

Exemplo de cabeçalho de autenticação:

```
Authorization: Bearer <seu-token-jwt>
```

<br>

## Segurança de Acesso aos Recursos :lock:

A fim de garantir a segurança dos recursos e dados da API, foram implementadas regras de acesso que restringem determinadas operações com base na autenticação do usuário. Abaixo, estão as restrições de acesso para os recursos "Produto" e "Categoria":

<br>

### Recurso Produto e Categoria

- Todos os métodos CRUD relacionados ao recurso "Produto" requerem autenticação com um token JWT válido no cabeçalho da solicitação. Isso garante que apenas usuários autenticados tenham acesso a essas operações.

<br>

### Recurso Usuário

Para o recurso "Usuário", a seguinte política de segurança foi implementada:

- **Cadastro de Usuário**: O método de cadastro de usuário (`POST /api/usuarios/cadastrar`) pode ser acessado de forma anônima, ou seja, sem a necessidade de autenticação. Isso permite que novos usuários se registrem na plataforma.
- **Login de Usuário**: O método de login (`POST /api/usuarios/logar`) também pode ser acessado de forma anônima. No entanto, ao fazer login com sucesso, um token JWT válido é gerado e fornecido ao usuário. Este token deve ser usado para autenticar todas as outras operações subsequentes que exigem autenticação.

Lembrando que a segurança é fundamental para proteger os dados e garantir a integridade da plataforma. Certifique-se de incluir o token JWT válido no cabeçalho das solicitações sempre que necessário.

<br>

## Licença

Este software está licenciado sob a [Licença MIT](https://github.com/brenonsc/BaldursGameStoreAPI/blob/main/LICENSE).

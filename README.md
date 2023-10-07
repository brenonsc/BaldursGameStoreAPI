# BaldursGameStoreAPI :space_invader::video_game:

Esta é a documentação para a API de uma loja de games que permite a manipulação de dados de produtos e categorias relacionadas a jogos. Esta API foi construída seguindo as melhores práticas do ASP.NET e oferece funcionalidades completas para gerenciar produtos e categorias.

<br>

## Configuração

Antes de começar a usar a API, certifique-se de seguir os passos de configuração necessários:

1. **Projeto ASP.NET**: O projeto foi criado seguindo as melhores práticas do ASP.NET.
2. **String de Conexão**: Configure a string de conexão com o banco de dados no arquivo `settings.json`.
3. **Classe de Contexto**: A classe de contexto para interagir com o banco de dados está localizada na camada de dados.

<br>

## Recurso Produto

A API oferece um conjunto completo de operações CRUD para o recurso "Produto", incluindo os seguintes métodos:

1. **GET `/api/produtos`**: Retorna a lista de todos os produtos disponíveis.
2. **GET `/api/produtos/{id}`**: Retorna detalhes de um produto específico com base no ID.
3. **POST `/api/produtos`**: Cria um novo produto.
4. **PUT `/api/produtos`**: Atualiza os detalhes de um produto existente com base no ID.
5. **DELETE `/api/produtos/{id}`**: Exclui um produto com base no ID.

<br>

## Recurso Categoria

A API também oferece funcionalidades completas para o recurso "Categoria", incluindo um conjunto de métodos CRUD semelhantes aos do recurso "Produto":

1. **GET `/api/categorias`**: Retorna a lista de todas as categorias disponíveis, juntamente com a coleção de produtos pertencentes a mesma.
2. **GET `/api/categorias/{id}`**: Retorna detalhes de uma categoria específica com base no ID.
3. **POST `/api/categorias/`**: Cria uma nova categoria.
4. **PUT `/api/categorias/`**: Atualiza os detalhes de uma categoria existente com base no ID.
5. **DELETE `/api/categorias/{id}`**: Exclui uma categoria com base no ID.

<br>

## Recurso Usuário

A API oferece um conjunto completo de operações CRUD para o recurso "Usuário", incluindo os seguintes métodos:

1. **GET `/api/usuarios/all`**: Retorna a lista de todos os usuários cadastrados.
2. **GET `/api/usuarios/{id}`**: Retorna detalhes de um usuário específico com base no ID.
3. **POST `/api/usuarios/cadastrar`**: Cria um novo usuário.
4. **POST `/api/usuarios/logar`**: Permite que um usuário faça login na plataforma. Este método utiliza JWT (JSON Web Tokens) para autenticação e gera um token válido por 1 hora, que deve ser incluído nas solicitações subsequentes para autenticação do usuário.
5. **PUT `/api/usuarios/atualizar`**: Atualiza os detalhes de um usuário existente com base no ID e outros dados passados no corpo da solicitação.

<br>

### Exemplo de Autenticação com JWT

Para usar o método de login e obter um token JWT válido, faça uma solicitação POST para `/api/usuarios/logar` com as credenciais do usuário no corpo da solicitação. O servidor irá gerar um token JWT que deve ser incluído no cabeçalho das solicitações subsequentes como Bearer Token para autenticar o usuário. O token é válido por 1 hora, após o qual será necessário fazer login novamente.

Exemplo de cabeçalho de autenticação:

```
Authorization: Bearer <seu-token-jwt>
```

<br>

## Segurança de Acesso aos Recursos

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

### Relacionamento entre Categoria e Produto

Existe um relacionamento do tipo OneToMany entre os recursos "Categoria" e "Produto". Cada categoria pode ter vários produtos associados a ela. Este relacionamento é habilitado no recurso "Produto".

<br>

### Busca por Intervalo de Preço e Título/Console

A API oferece dois endpoints adicionais:

1. **GET `/api/produtos/preco/{min}/{max}`**: Retorna todos os produtos dentro de um intervalo de preço especificado.

2. **GET `/api/produtos/titulo/{titulo}/ouconsole/{console}`**: Permite a busca de jogos por título ou console. Os parâmetros de consulta podem ser usados para especificar os critérios de busca.

<br>

## Testando a API

Recomenda-se o uso da ferramenta [Insomnia](https://insomnia.rest/) ou [Postman](https://www.postman.com/) para testar a API. Certifique-se de testar todas as operações CRUD para os recursos "Produto", "Categoria" e "Usuários", bem como os endpoints de busca por intervalo de preço e título ou console.

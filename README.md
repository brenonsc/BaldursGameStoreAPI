# BaldursGameStoreAPI :space_invader::video_game:
![License](https://badgen.net/badge/license/MIT/purple?icon=)
![.NET](https://badgen.net/badge/.NET/v7.0/blue?icon=)
![NuGet](https://badgen.net/badge/icon/Packages/green?icon=nuget&label)
![Docker](https://badgen.net/badge/icon/Available?icon=docker&label)

Esta é a documentação para a API de uma loja de games que permite a manipulação de dados de produtos e categorias relacionadas a jogos. Esta API foi construída seguindo as melhores práticas do ASP.NET e oferece funcionalidades completas para gerenciar produtos e categorias.

<br>

## Testando a API :man_scientist:

Para testar a API, acesse a documentação no [Swagger](https://baldursgamestore.onrender.com). Certifique-se de testar todas as operações CRUD para os recursos "Produto", "Categoria" e "Usuários", bem como os endpoints de busca por intervalo de preço e título ou console.

<br>

## Recurso Produto :space_invader:

A API oferece um conjunto completo de operações CRUD para o recurso "Produto", incluindo os seguintes métodos:

1. **GET `/api/produtos`**: Retorna a lista de todos os produtos disponíveis.
2. **GET `/api/produtos/{id}`**: Retorna detalhes de um produto específico com base no ID.
3. **GET `/api/produtos/titulo/{titulo}`**: Retorna todos os produtos dentro de um intervalo de preço especificado.
4. **GET `/api/produtos/preco/{min}/{max}`**: Retorna todos os produtos dentro de um intervalo de preço especificado.
5. **GET `/api/produtos/titulo/{titulo}/ouconsole/{console}`**: Permite a busca de jogos por título ou console. Os parâmetros de consulta podem ser usados para especificar os critérios de busca.
6. **POST `/api/produtos/{categoriaId}`**: Cria um novo produto.
7. **PUT `/api/produtos/{id}/categoria/{categoriaId}`**: Atualiza os detalhes de um produto existente com base no ID.
8. **DELETE `/api/produtos/{id}`**: Exclui um produto com base no ID.

<br>

## Recurso Categoria :abc:

A API também oferece funcionalidades completas para o recurso "Categoria", incluindo um conjunto de métodos CRUD semelhantes aos do recurso "Produto":

1. **GET `/api/categorias`**: Retorna a lista de todas as categorias disponíveis, juntamente com a coleção de produtos pertencentes a mesma.
2. **GET `/api/categorias/{id}`**: Retorna detalhes de uma categoria específica com base no ID.
3. **GET `/api/categorias/{id}`**: Retorna detalhes de uma categoria específica com base no tipo informado.
4. **POST `/api/categorias/`**: Cria uma nova categoria.
5. **PUT `/api/categorias/{id}`**: Atualiza os detalhes de uma categoria existente com base no ID.
6. **DELETE `/api/categorias/{id}`**: Exclui uma categoria com base no ID.

<br>

## Recurso Usuário :bust_in_silhouette:

A API oferece um conjunto completo de operações CRUD para o recurso "Usuário", incluindo os seguintes métodos:

1. **GET `/api/usuarios/all`**: Retorna a lista de todos os usuários cadastrados.
2. **GET `/api/usuarios/{id}`**: Retorna detalhes de um usuário específico com base no ID.
3. **POST `/api/usuarios/cadastrar`**: Cria um novo usuário.
4. **POST `/api/usuarios/logar`**: Permite que um usuário faça login na plataforma. Este método utiliza JWT (JSON Web Tokens) para autenticação e gera um token válido por 1 hora, que deve ser incluído nas solicitações subsequentes para autenticação do usuário.
5. **PUT `/api/usuarios/atualizar`**: Atualiza os detalhes de um usuário existente com base no ID e outros dados passados no corpo da solicitação.

<br>

### Exemplo de Autenticação com JWT :key:

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

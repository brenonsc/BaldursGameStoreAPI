# BaldursGameAPI :video_game::space_invader:

Esta é a documentação para a API de uma loja de games que permite a manipulação de dados de produtos e categorias relacionadas a jogos. Esta API foi construída seguindo as melhores práticas do ASP.NET e oferece funcionalidades completas para gerenciar produtos e categorias.

<br>

## Configuração

Antes de começar a usar a API, certifique-se de seguir os passos de configuração necessários:

1. **Projeto ASP.NET**: O projeto foi criado seguindo as melhores práticas do ASP.NET.
2. **String de Conexão**: Configure a string de conexão com o banco de dados no arquivo `settings.json`.
3. **Classe de Contexto**: A classe de contexto para interagir com o banco de dados está localizada na camada de dados.

<br>

## Recurso Produto

O recurso "Produto" permite a manipulação dos dados dos produtos da loja, incluindo a classificação por categoria.

### CRUD de Produto

A API oferece um conjunto completo de operações CRUD para o recurso "Produto", incluindo os seguintes métodos:

1. **GET `/api/produtos`**: Retorna a lista de todos os produtos disponíveis.

2. **GET `/api/produtos/{id}`**: Retorna detalhes de um produto específico com base no ID.

3. **POST `/api/produtos`**: Cria um novo produto.

4. **PUT `/api/produtos/{id}`**: Atualiza os detalhes de um produto existente com base no ID.

5. **DELETE `/api/produtos/{id}`**: Exclui um produto com base no ID.

6. **GET `/api/produtos/categoria/{categoriaId}`**: Retorna todos os produtos de uma categoria específica.

### Recurso Categoria

A API também oferece funcionalidades completas para o recurso "Categoria", incluindo um conjunto de métodos CRUD semelhantes aos do recurso "Produto":

1. **GET `/api/categorias`**: Retorna a lista de todas as categorias disponíveis.

2. **GET `/api/categorias/{id}`**: Retorna detalhes de uma categoria específica com base no ID.

3. **POST `/api/categorias`**: Cria uma nova categoria.

4. **PUT `/api/categorias/{id}`**: Atualiza os detalhes de uma categoria existente com base no ID.

5. **DELETE `/api/categorias/{id}`**: Exclui uma categoria com base no ID.

6. **GET `/api/categorias/{id}/produtos`**: Retorna todos os produtos pertencentes a uma categoria específica.

### Relacionamento entre Categoria e Produto

Existe um relacionamento do tipo OneToMany entre os recursos "Categoria" e "Produto". Cada categoria pode ter vários produtos associados a ela. Este relacionamento é habilitado no recurso "Produto".

### Busca por Intervalo de Preço e Título/Console

A API oferece dois endpoints adicionais:

1. **GET `/api/produtos/preco/{min}/{max}`**: Retorna todos os produtos dentro de um intervalo de preço especificado.

2. **GET `/api/produtos/buscar`**: Permite a busca de jogos por título ou console. Os parâmetros de consulta podem ser usados para especificar os critérios de busca.

<br>

## Testando a API

Recomenda-se o uso da ferramenta Insomnia para testar a API. Certifique-se de testar todas as operações CRUD para os recursos "Produto" e "Categoria", bem como os endpoints de busca por intervalo de preço e título/console.

<br>

## Validação na Plataforma

Ao entregar as atividades na plataforma, certifique-se de validar as três classes desenvolvidas:

1. **Produto**: Camada de modelo (Model).
2. **ProdutoService**: Camada de serviço (Service).
3. **ProdutoController**: Camada de controle (Controller).

Certifique-se de que todas as funcionalidades estejam funcionando corretamente antes de concluir a entrega.
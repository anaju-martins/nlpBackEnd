# Backend da Análise de Texto com NLP (.NET 9)

## 📖 Sobre o Projeto

Este repositório contém o backend da aplicação de Análise de Texto, desenvolvido com C# e .NET. Ele atua como o orquestrador central do sistema, gerenciando a lógica de negócio, a persistência de dados e a comunicação com serviços externos de Processamento de Linguagem Natural (NLP)



## 🏛️ Arquitetura e Papel no Ecossistema

Este backend não funciona de forma isolada. Ele é o coração de uma arquitetura de microsserviços distribuída, com as seguintes responsabilidades:

- Orquestrar o fluxo de análise: Recebe requisições do frontend, envia o texto para o serviço de NLP e processa o resultado.

- Gerenciar a persistência de dados: Salva o histórico de análises, os resultados e a biblioteca de palavras-chave do usuário em um banco de dados PostgreSQL.

- Expor uma API RESTful: Fornece endpoints seguros e bem definidos para serem consumidos pelo frontend.

O fluxo geral da aplicação é:

```javascript
[Frontend em Next.js] <--> [Backend .NET (Este Projeto)] <--> [Serviço de NLP em Python/spaCy]
       |                                   |
       '-----------------> [Banco de Dados PostgreSQL]
```

## 🛠️ Tecnologias Utilizadas
- .NET 9 e ASP.NET Core para a construção da Web API.

- Entity Framework Core 9 como ORM para a comunicação com o banco de dados.

- Npgsql como provedor de dados para o PostgreSQL.

- PostgreSQL como sistema de gerenciamento de banco de dados.

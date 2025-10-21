# Developer Test Project

Este projeto tem o intuito de mostrar a comunicação entre serviços atraves de mensageria.

Se voce quer dar uma olhada nas futuras implementações clique [aqui](/.doc/backlog.md)

## Regra de negocio
O serviço é dividido em dois sistemas "Search" e "Persistence", que faz uma busca pelos estados do Brasil em um serviço publico externo, e persiste do nosso lado em Json.

### Regras

* O Primeiro sistema "SEARCH" deve apenas buscar a lista e enviar por mensageria ao segundo sistema "PERSISTENCE"
* O segundo sistema "PERSISTENCE" converte a lista em JSON e salva em arquivo no minIO

## Requirements
tenha certeza que voce tenha as seguintes ferramentas para executar o projeto.
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (with Docker Compose enabled)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (or higher)

## executando o projeto
Esta sessão descreve como executar o projeto.

Veja [Executando o projeto](/.doc/running_project.md)

## Tech Stack
Esta seção lista as principais tecnologias utilizadas no projeto, incluindo os componentes de backend, testes, e armazenamento.

Veja [Tech Stack](/.doc/tech-stack.md)

## Estrutura do projeto
Esta seção descreve a estrutura geral e a organização dos arquivos e diretórios do projeto.

Veja [Estrutura do projeto](/.doc/project-structure.md)

## Backlog
This future implementations for this API. 

Veja [Backlog](/.doc/backlog.md)
[Back to README](../README.md)

### 1: Run Docker Compose
O docker compose ja esta configurado para subir todas as dependencias de ambos os sistemas, execute:
```bash
docker-compose up -d
```

### DEVIDO A PROBLEMA DE COMUNICAÇÃO ENTRE O CONTAINER DO MINIO E O CONTAINER DOS SERVIÇOS, PARA TESTAR O FLUXO COMPLETA UTILIZE DUAS INSTANCIAS DO VISUAL STUDIO, ELAS JA APONTAM PARA OS CONTAINERS CORRETAMENTE

### 2: Rodar com Visual Studio

1. Execute o projeto state_search.sln em uma instancia do visual studio
2. Execute o projeto state_persistence.sln em outra instancia do visual studio

### 3: Tests
Os projetos inclue unit e integration tests.
[Back to README](../README.md)

## Project Structure

Ambos os 
The project should be structured as follows:

### STATE SEARCH
```
states_search/
│
├── src
│   ├── API
│   │   └── Program.cs
│   │   └── appsettings.json
│   │   └── Dockerfile
│   │
│   ├── Application
│   │   └── Interfaces/
│   │   └── Services/
│   │   └── DTOs/
│   │   └── Messaging/
│   │
│   └── Infrastructure
│       └── HttpClient/
│       └── Messaging/
│
├── tests
│   ├── UnitTests
│   │   └── API/
│   │   └── Application/
│   │
│   ├── IntegrationTests
│      └── Infrastructure/
│
├── docker-compose.yml
```

### STATE PERSISTENCE
```
states_persistence/
│
├── src
│   ├── API
│   │   └── Program.cs
│   │   └── appsettings.json
│   │   └── Dockerfile
│   │
│   ├── Application
│   │   └── Interfaces/
│   │   └── Services/
│   │   └── DTOs/
│   │   └── Messaging/
│   │
│   └── Infrastructure
│       └── Repositories/
│       └── Configurations/
│
├── tests
│   ├── UnitTests
│   │   └── API/
│   │   └── Application/
│   │
│   ├── IntegrationTests
│      └── Infrastructure/
│
├── docker-compose.yml
```
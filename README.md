# Fase 1
## Desafio backend *[.NET (C#)](https://meteor-ocelot-f0d.notion.site/NET-C-5281edbec2e4480d98552e5ca0242c5b)*. 
Solução para o desafio permitindo cadastrar projetos e tarefas.

- 1º Clonar codigo fonte
```cmd
    git clone https://github.com/aasf86/eclipseworks
```

- 2º No diretório raiz do projeto, executar a seguinte linha de comando: *"necessário docker"*
```cmd
    docker-compose up -d
```
- 3º Demonstração via Swagger Open API
    ### Uma vez construido o ambiente, na maquina hospedeira é possivel acessar os servicos.

    - Solução eclipseworks
        - eclipseworks.Api: http://localhost:8081/swagger/index.html
            - *Responsável pela gestão de casdastros de projetos e tarefas*    

    - Ferramentas de infraestrutura
        - PostgreSQL: port 5432
            - *Banco de dados relacional responsável pela retenção dos dados.*
            - [Script definição de estrutura de dados (*01_DDL.sql*)](https://github.com/aasf86/eclipseworks/blob/main/src/eclipseworks.Infrastructure/ChangesDB/1.0.0/01_DDL.sql)
            - [Script inserção dados iniciais (*02_DML.sql*)](https://github.com/aasf86/eclipseworks/blob/main/src/eclipseworks.Infrastructure/ChangesDB/1.0.0/02_DML.sql)
---
# [Fase 2](README2.md)
# [Fase 3](README3.md)

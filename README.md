Desafio Carrefour

Projeto responsável por realizar fluxo de caixa com relatório consolidado com banco de dados Mysql

## Pré-requisitos

- [.NETCore 6.0]([https://www.docker.com/](https://dotnet.microsoft.com/pt-br/download/dotnet/6.0))

### Banco de dados
Este projeto realizará consultas e inserts no modelo de dados gerenciado pelo EntityFrameWork no banco de banco Mysql.
Antes de executar em desenvolvimento é necessário que você faça as configurações do Mysql.

### Execução do projeto via Docker

Na pasta do projeto, execute os seguintes comandos para criar a imagem:
```bash
cd docker
./create-dotnet-image.sh
```
Será criado uma imagem com o nome `ubuntu:dotnet-3.20.2`

### Execução do projeto em desenvolvimento com a imagem docker `ubuntu:dotnet-3.20.2` e `mockserver`:    

Subir mockserver para simular api do service now
```bash
docker-compose -f docker/docker-compose.yml up -d
````
### Execução do projeto sem utilizar docker:    

Entrar na pasta do projeto e executar dotnet run. Será executado o migration onde irá criar a base de dados e as tabelas.
No arquivo application.json na tag ConnectionString esta localizado o endereço da base de dados.

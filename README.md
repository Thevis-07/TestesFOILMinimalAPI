# Testes FOIL Minimal API

API REST para gerenciamento de alunos, perguntas, respostas e cálculo de resultados do teste FOIL, incluindo envio de relatórios por e-mail.

## Sumário

- [Visão Geral](#visão-geral)
- [Instalação](#instalação) 
- [Configuração](#configuração)     
- [Endpoints](#endpoints)
  - [Home](#home)
  - [Alunos](#alunos)
  - [Perguntas](#perguntas)
  - [Respostas](#respostas)
  - [Resultados](#resultados)
- [Envio de E-mail](#envio-de-e-mail)
- [Execução via Docker](#execução-via-docker)
- [Variáveis de Ambiente](#variáveis-de-ambiente)
- [Licença](#licença)

---

## Visão Geral

Esta API permite:

- Cadastro e consulta de alunos
- Cadastro e consulta de perguntas e categorias
- Registro de respostas dos alunos
- Cálculo de resultados por aluno
- Envio automático de relatório de resultados por e-mail

## Instalação

```sh
git clone https://github.com/seu-usuario/TestesFOILMinimalAPI.git
cd TestesFOILMinimalAPI/src
dotnet restore
dotnet run
```

## Configuração

Configure o banco PostgreSQL e SMTP no arquivo [`appsettings.json`](src/appsettings.json):

```json
"ConnectionStrings": {
  "Postgres": "{POSTGRES_CS}"
},
"Smtp": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "User": "usuario@gmail.com",
  "Password": "",
  "UseStartTls": true,
  "FromName": "Testes FOIL",
  "FromEmail": "no-reply@gmail.com"
}
```

## Endpoints

### Home

- `GET /`  
  Retorna "Hello World".

---

### Alunos

- `POST /alunos`  
  Cria um novo aluno.  
  **Body:**  
  ```json
  {
    "Nome": "João",
    "Email": "joao@email.com",
    "Idade": 20,
    "Curso": "Ontopsicologia",
    "Semestre": 3
  }
  ```

- `GET /alunos`  
  Lista todos os alunos.

- `GET /alunos/{id}`  
  Busca aluno por ID.

- `PUT /alunos/{id}`  
  Atualiza dados do aluno.  
  **Body:**  
  ```json
  {
    "Email": "novo@email.com",
    "Idade": 21,
    "Curso": "Ontopsicologia",
    "Semestre": 4
  }
  ```

- `DELETE /alunos/{id}`  
  Remove aluno.

---

### Perguntas

- `POST /perguntas`  
  Cria uma pergunta.  
  **Body:**  
  ```json
  {
    "Texto": "Pergunta exemplo?",
    "CategoriaPerguntaId": "guid-da-categoria"
  }
  ```

- `GET /perguntas`  
  Lista todas as perguntas.

- `PUT /perguntas/{id}`  
  Atualiza pergunta.

- `DELETE /perguntas/{id}`  
  Remove pergunta.

---

### Respostas

- `POST /respostas`  
  Cria ou atualiza resposta de um aluno para uma pergunta.  
  **Body:**  
  ```json
  {
    "AlunoId": "guid-do-aluno",
    "PerguntaId": "guid-da-pergunta",
    "ValorMae": 5,
    "ValorPai": 7
  }
  ```

- `POST /respostas/bulk`  
  Cria ou atualiza várias respostas de um aluno.  
  **Body:**  
  ```json
  {
    "AlunoId": "guid-do-aluno",
    "Respostas": [
      { "PerguntaId": "guid1", "ValorMae": 4, "ValorPai": 6 },
      { "PerguntaId": "guid2", "ValorMae": 7, "ValorPai": 8 }
    ]
  }
  ```

- `GET /respostas/{alunoId}`  
  Lista todas as respostas detalhadas de um aluno.

---

### Resultados

- `GET /resultados/calcular/{alunoId}`  
  Calcula o resultado do teste FOIL para o aluno e envia relatório por e-mail.  
  **Retorno:**  
  ```json
  {
    "aluno": { ... },
    "resultado": [
      {
        "CategoriaNome": "Autoritário",
        "CategoriaAbreviacao": "AUT",
        "CategoriaDescricao": "Descrição...",
        "TotalMae": 10,
        "TotalPai": 8
      }
    ]
  }
  ```

---

## Envio de E-mail

Ao calcular o resultado (`/resultados/calcular/{alunoId}`), o relatório é enviado automaticamente para o e-mail do aluno, usando o template [`Files/EmailTemplates/resultado-estilos-parentais.sbnhtml`](src/Files/EmailTemplates/resultado-estilos-parentais.sbnhtml).

## Execução via Docker

```sh
docker build -t testesfoilapi .
docker run -p 80:80 --env-file .env testesfoilapi
```

## Variáveis de Ambiente

- `POSTGRES_CS` — String de conexão PostgreSQL (pode ser montada via variáveis do sistema)
- `SMTP_PASSWORD` — Senha do SMTP (pode ser definida no ambiente para maior segurança)

## Licença

MIT. Veja o arquivo LICENSE.

---

> Para dúvidas, consulte os arquivos de configuração e os endpoints implementados em [src/Endpoints](src/Endpoints)
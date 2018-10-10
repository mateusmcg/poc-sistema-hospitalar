# Estrutura do projeto

## Folders

### api
- Contém a API que irá disponibilizar as informações para os Médicos(as) e/ou Enfermeiras(os) através de uma interface (Web e/ou Mobile).
- Tecnologia: DotNet Core.

### console
- Contém uma aplicação console que irá monitorar de tempos em tempos a fila de mensagens da AWS para conferir os dados dos pacientes do hospital e sinalizar caso exista alguma discrepância (além de persistir os dados em um DynamoDB na AWS).
- Tecnologia: DotNet Core.

### devops
- k8s
    - Todos os arquivos responsáveis pelo deploy das aplicações no cluster do Kubernetes estão nesta pasta.
- docker
    - Responsável por armazenar todos os Dockerfiles que a aplicação necessita.

### evidências
- Contém todas as imagens para evidenciar que a aplicação foi deployada com sucesso na AWS.

### iot
- Contém a API que simula os dados de um paciente de um hospital e envia os dados para uma fila na AWS (IoT).
- Tecnologia: NodeJS.

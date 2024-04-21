
Exemplo de implementação de microsserviços utilizando .NET/C# e Event-Driven Architecture (EDA) com o auxílio de RabbitMQ.

![Diagrama de Eventos](https://ik.imagekit.io/ryeaswait/FluxoEventosv2.png)

### Frameworks/Libs

- .NET/C#
- ASP. NET
- RabbitMQ
- MediatR
- Docker
- SQL Server

### Execução ambiente Local

-  Executar:
docker compose up -d --build

### Pacotes NuGet .Base

No Visual Studio, adicionar um novo 'Package Sources' apontando para a pasta '..\Poc.Microsservicosv2\nupkg'


<br />
<br />

# Projeto

- Contexto é formador por: microserviço (api), mais suas camadas auxiliares. Exemplo, Contexto Pedidos:
```
  .Pedidos.Api
  .Pedidos.Application
  .Pedidos.Domain
  .Pedidos.Ioc
  .Pedidos.MessageBroker
```

- Um Contexto trabalha com Comandos (NovoPedido, RealizarPagamento, PesquisarUsuario, etc.) e Eventos (PedidoRealizado, PagamentoNaoRealizado, etc.).
Comandos estão presentes na camada Application (CONTEXTO.Application); 
Eventos ficam na camada Domain (CONTEXTO.Domain).

<br />

# 1. MessageBroker (Projeto: CONTEXTO.Application)

## AsyncOperationsOnCONTEXT

- Operações assíncronas dentro do próprio contexto.

- Os comandos e eventos são registrados em PublishersCommandsConfig e PublishersEventsConfig.


## IntegrationEventsContexts

- 'Receber' eventos oriundos de outros contextos.

<br />

# 2. Projeto: CONTEXTO.MessageBroker

- Configuração de Conexões, Canais, Exchanges, Publicadores e Consumidores do Contexto.

- Realização de customizações de canais e criação de mais consumidores além dos definidos por padrão.

<br />

# 3. Criação de Exchanges e Queues

- Exchanges e Queues são criadas no startap da aplicação.

- prefetchCount é setado como 10, mais é possível customizar.

- Comandos serão enviados a exchanges do tipo Direct.

- Eventos serão enviados a exchanges do tipo Topic.

- DeadLetter: Exchanges e Queues são criadas para comandos e eventos. 
Estas exchanges são do tipo Fanout.

<br />

# 4. Publisher

- Classes PublisherCommandRabbitMq e PublisherEventRabbitMq.

## Métodos PublishCommandAsync e PublishEventAsync

- Caso não seja uma retentativa de publicação, é criada uma nova mensagem no banco.

- Recupera-se as configurações para a realização da publicação: PublisherSetup. Caso não exista configurações, logs são registrados, e uma exception é disparada.

- Recupera-se a mensagem do banco, em caso de retentativa, para verificar se ela já foi publicada no broker.

- Realiza-se a publicação e a mensagem no banco é atualizada para 'MessageInBroker'.

- Em caso de erro na publicação, a mensagem é mantida no banco de dados como pendente de publicação.

<br />

# 5. Consumer

- Um Serviço para a criação de novos 'consumer' foi criado para Eventos, e outro para Comandos: Classes ConsumerCommandRabbitMq e ConsumerEventRabbitMq

## Métodos ConsumerEventAsync e ConsumerCommandAsync

- Antes de se registrar um novo consumidor, é recuperada as configurações de consumo para aquela fila (ConsumerSetup), caso não a encontre, um log será registrado.

- O canal, recuperado de ConsumerSetup, é vinculado ao Received.


## Received

- Para eventos: Caso a mensagem seja originária de outro contexto, ela é armazenada em banco, referenciando a mensagem original.

- Se ocorrer um erro durante a deserialização, a mensagem será rejeitada, sem 'requeue'; 
um log será registrado; 
a transação para gravar a mensagem no banco será revertida.

- Se a mensagem recuperada constar como processada (banco de dados), o broker será notificado com um 'ack'.

- Ocorrendo sucesso no tratamento da mensagem (handle), ela será marcada como processada, tanto no banco, quanto no broker.

- Se ocorrer um erro durante o processamento, a mensagem será rejeitada, com 'requeue'; um log será registrado; o contador de tentativas de processamento da mensagem será incrementado.

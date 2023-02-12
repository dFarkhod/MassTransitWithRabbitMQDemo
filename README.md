# MassTransitWithRabbitMQDemo

Docker'da RabbitMq ni ko'tarish buyrug'i:
docker run -d --name rabbitmq -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=secretpassword -p 15672:15672 -p 5672:5672 rabbitmq:3.8.12-management

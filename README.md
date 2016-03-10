# CoreMessageBus
Message bus implementation for .NET Core

Parts:

`IMessageBus` => `Send`

`MessageBusConfiguration` => `MessageBusHandlerConfiguration` => `RegisterHandler()`
AddFromAssemblies
`IMessageHandlerResolver` => configuration.Handlers

# Yes Sensu!

I am a client for sending messages to Sensu.

## Usage

### Installation

> Install-Package YesSensu.Core

### Setup a heart beat

The pacemaker allows you to setup a regular heart beat that is sent to the server to signal that you application is up.

```csharp
var pacemaker = SensuNinja.Get("sensu.myhost.com", 3000);
var heartbeat = new Heartbeat("MyApp");
pacemaker.Start(heartbeat);
```

This will start a heart beat ping that will be sent every 60 seconds by default. The period can be set by providing it into the constructor of the heart beat.

> Heartbeat(string appName, int period = 60)

You can stop the heart beat by asking the `SensuNinja` for the `Pacemaker` singleton and calling `Stop()` on it.

```csharp
var pacemaker = SensuNinja.Get("sensu.myhost.com", 3000);
pacemaker.Stop();
```

### Sensu Monitor

The Sensu Monitor allows you to send a set of messages to the server easily.

As an example you can signal a warning from you application:

```csharp
var sensu = new SensuMonitor(new SensuUdpClient("sensu.myhost.com", 3000), "MyApp");
sensu.Warning();
```

> void Ok(string message = "")  
> void Warning(string message = "")  
> void Error(string message = "")  
> void Metric(string key, Status status, string message = "")  
> void Hearbeat(int period = 60, string message = "")  

The `Metric` method allows sending keys that represent sub-systems within or external to your application (that the application depends on).

```csharp
sensu.Metric("customer_database", Status.Error, "Cannot connect to the customer database for more than 30 seconds.");
```

### Sensu client

Two Sensu clients are supplied: `SensuUdpClient` and `SensuTcpClient`. These clients allow you to send custom messages via the given protocol.

> void Connect()  
> void Send<TMessage>(TMessage message)  

```csharp
var sensuClient = new SensuUdpClient("sensu.myhost.com", 3000);
sensuClient.Connect();
sensuClient.Send(obj); // where obj is some JSON serializable object
```
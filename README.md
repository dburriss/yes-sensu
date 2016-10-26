# Yes Sensu!

I am a client for sending messages to Sensu.

| DEV |MASTER|BLEEDING|NUGET|
|-----|------|--------|-----|
|[![CI status][1]][2]|[![Release Build status][3]][4]|[![MyGet CI][5]][6]|[![NuGet CI][7]][8]|

## Usage

### Installation

> Install-Package YesSensu

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

Alternatively, or on application exit you can kill off the pacemaker like so:

```csharp
var pacemaker = SensuNinja.Get("sensu.myhost.com", 3000);
pacemaker.Kill();
```

### Sensu Monitor

The Sensu Monitor allows you to send a set of messages to the server easily.

As an example you can signal a warning from you application:

```csharp
var sensu = new SensuMonitor(new SensuUdpClient("sensu.myhost.com", 3000), "MyApp");
sensu.Warning("customer_database", "Queries are running slow.");
```

> void Ok(string name, string message = "")  
> void Warning(string name, string message = "")  
> void Error(string name, string message = "")  
> void Heartbeat(int period = 60, string message = "")  

```csharp
sensu.Error("customer_database", "Cannot connect to the customer database for more than 30 seconds.");
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

### Enrichers

Enrichers allow you to add custom meta data to a message. So far the current enrichers are contained in the `YesSensu.Enrichers`.

|BLEEDING|NUGET|
|--------|-----|
|[![MyGet CI][9]][10]|[![NuGet CI][11]][12]|

You can install the existing enrichers with the following command:  

> Install-Package YesSensu.Enrichers

- StaticEnricher - adds values from a supplied dictionary to the **meta**.  
- HostInfoEnricher - adds the *host_name* and *machine_name* to the **meta**.

```javascript
"host_name": "DESKTOP-1PI4O6G",
"machine_name": "DESKTOP-1PI4O6G"
```

- AssemblyInfoEnricher - adds all the `Assembly` attributes as properties on the **meta**.

```javascript
"product": "ConsoleApp1",
"informationalversion": "0.2.0+Branch.feature/AssemblyEnricher.Sha.81c27cee5be57f638ad08c53e4ac17ef73f3ea30",
"fileversion": "0.2.0.0"
```

Example message:

```javascript
{
  "name": "some_metric",
  "output": "text",
  "source": "ConsoleApp1",
  "status": 0,
  "meta": {
    "host_name": "DESKTOP-1PI4O6G",
    "machine_name": "DESKTOP-1PI4O6G",
    "configuration": "",
    "company": "",
    "product": "ConsoleApp1",
    "trademark": "",
    "informationalversion": "0.2.0+Branch.feature/AssemblyEnricher.Sha.81c27cee5be57f638ad08c53e4ac17ef73f3ea30",
    "fileversion": "0.2.0.0"
  }
}
```

#### Creating custom enrichers

The interfaces required for custom message enrichement are contained in the `YesSensu.Core` namespace.

|BLEEDING|NUGET|
|--------|-----|
|[![MyGet CI][13]][14]|[![NuGet CI][15]][16]|

> Install-Package YesSensu.Core

#### Your message

Make sure you message implements `IHaveMeta`. This method should be called by you enricher to add data to the dictionary. The easiest way to get this behaviour is to inherit from `SensuBase`.

```csharp
public interface IHaveMeta
{
    IDictionary<string, object> Meta { get; }
    void AddMeta(string name, object data);
}
```

#### Your enricher

Creating custom enrichers is easy. An enricher needs to implement `ISensuEnricher`.

```csharp
public interface ISensuEnricher
{
    void Enrich(IHaveMeta obj);
}
```

Check out the [HostInfoEnricher](https://github.com/dburriss/yes-sensu/blob/master/src/YesSensu.Enrichers/HostInfoEnricher.cs) and [AssemblyInfoEnricher](https://github.com/dburriss/yes-sensu/blob/master/src/YesSensu.Enrichers/AssemblyInfoEnricher.cs) code as an example.

[1]: https://ci.appveyor.com/api/projects/status/sb2eidh6qhnoj4lt?svg=true
[2]: https://ci.appveyor.com/project/dburriss/yes-sensu
[3]: https://ci.appveyor.com/api/projects/status/sb2eidh6qhnoj4lt/branch/master?svg=true
[4]: https://ci.appveyor.com/project/dburriss/yes-sensu/branch/master
[5]: https://img.shields.io/myget/dburriss-ci/vpre/YesSensu.svg
[6]: https://www.myget.org/feed/Packages/dburriss-ci
[7]: https://img.shields.io/nuget/v/YesSensu.svg
[8]: https://www.nuget.org/packages/YesSensu/
[9]: https://img.shields.io/myget/dburriss-ci/vpre/YesSensu.Enrichers.svg
[10]: https://www.myget.org/feed/Packages/dburriss-ci
[11]: https://img.shields.io/nuget/v/YesSensu.Enrichers.svg
[12]: https://www.nuget.org/packages/YesSensu.Enrichers/
[13]: https://img.shields.io/myget/dburriss-ci/vpre/YesSensu.Core.svg
[14]: https://www.myget.org/feed/Packages/dburriss-ci
[15]: https://img.shields.io/nuget/v/YesSensu.Core.svg
[16]: https://www.nuget.org/packages/YesSensu.Core/
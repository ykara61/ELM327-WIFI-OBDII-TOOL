#  1.Objective

The aim of this repository is to communicate with your vehicle using **Elm327 WiFi OBDII Tool** and **.NET Core**. The code published in this repository uses TCP protocol for communicating with **ELM 327 Wifi OBDII Tool**.

<p>
    <img src="/Images/ELM327_NET6_BASE.png" alt>
</p>

# 2. Why .NET/.NET Core for OBDII Interface

At this point in time, The .NET Core are widely adoped by a large community of enterprise developers. It paves way for use programming language like C# for communicating with vehicle OBDII port. The enterprise developers can integrate this functionality into Console Application, Windows Application and Xamarin mobile applications etc.

# 3. .NET ELM 327 PID and Data Collector


## 3a. Development Environment
- Windows 10
- Visual Studio 2022 with latest updates

## 3b. Runtime Requirements
- .NET Runtime [(.NET 6)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Linux, Windows or MacOs Operational System
- ELM 327 WiFi OBDII Tool [(Amazon Link)](https://www.amazon.com/Interface-Scanner-Diagnostic-Original-Support/dp/B07L4926C1/ref=sr_1_1?crid=370UVA85IY1AF&keywords=elm+327+wifi&qid=1671276353&sprefix=elm+327+wif%2Caps%2C189&sr=8-1)
- Suitable OBDII Interface

## Application Setup

Application setup is quite simple. You need to connect WiFi AP that is automatically created by ELM 327 device. You can ping to ELM 327 tool's IP (usually 192.168.0.10) for checking the connection after you connected to device. 

<p>
    <img src="/Images/ELM327_NET6_SETUP.png" alt>
    <em>Application Setup Diagram</em>
</p>


## Manually Creating an AWS IoT Thing

Alternatively, you can manually create an IoT thing using the AWS IoT console.  To start, let's navigate to the console and create an IoT thing called 'dotnetdevice'.

<p>
    <img src="/Images/ELM327_NET6_CONSOLE_MAIN.png" alt>
    <em>Application Main Console</em>
</p>


Let's associate the following policy with the thing.

``` json
{
kodlar falan
    }
  ]
}
``` 
During the Thing creation process you should get the following four security artifacts.  Start by creating a 'certificates' folder at 'dotnetsamples\certificates'.

- **Device certificate** - This file usually ends with ".pem.crt". When you download this it will save as .txt file extension in windows. Save it in your certificates directory as 'certificates\certificate.cert.pem' and make sure that it is of file type '.pem', not 'txt' or '.crt'

- **Device public key** - This file usually ends with ".pem" and is of file type ".key".  Save this file as 'certificates\certificate.public.key'. 

- **Device private key** -  This file usually ends with ".pem" and is of file type ".key".  Save this file as 'certificates\certificate.private.key'. Make sure that this file is referred with suffix ".key" in the code while making MQTT connection to AWS IoT.

- **Root certificate** - Download from https://www.amazontrust.com/repository/AmazonRootCA1.pem.  Save this file to 'certificates\AmazonRootCA1.crt'


###  Converting Device Certificate from .pem to .pfx

In order to establish an MQTT connection with the AWS IoT platform, the root CA certificate, the private key of the thing, and the certificate of the thing/device are needed. The .NET cryptographic APIs can understand root CA (.crt), device private key (.key) out-of-the-box. It expects the device certificate to be in the .pfx format, not the .pem format. Hence we need to convert the device certificate from .pem to .pfx.

We'll leverage the openssl for converting .pem to .pfx. Navigate to the folder where all the security artifacts are present and launch bash for Windows 10.

The syntax for converting .pem to .pfx is below:

openssl pkcs12 -export -in **iotdevicecertificateinpemformat** -inkey **iotdevivceprivatekey** -out **devicecertificateinpfxformat** -certfile **rootcertificatefile**

If you replace with actual file names the syntax will look like below

openssl pkcs12 -export -in certificates\certificate.cert.pem -inkey certificates\certificate.private.key -out certificates\certificate.cert.pfx -certfile certificates\AmazonRootCA1.crt

![](/images/pic3.JPG)


# 4. AWS IoT Device Publisher and Consumer using .NET Core

## 4a. Development Environment

The following constitutes the development environment for developing AWS IoT device publisher and consumer using .NET Core.

- Ubuntu 20.0.4 or higher (or) any other latest Linux distros
- .NET Core 2.0 or higher
- AWS cli
- Openssl latest version


## 4b. Create an AWS IoT Thing 

Navigate to the 'dotnetcoresamples' folder and execute the provision_thing.sh shell script.  This script handles the setup for the .NET Core examples, following the same steps as the PowerShell script in the .NET Framework examples.

Alternatively, you can copy the certificates created in the .NET Framework example to a 'Dotnetcoresamples\certificates' folder or follow the same steps to create a new Thing and certificate.

## 4d. Device Publisher using .NET core 

Let's create the .NET Core console application for the producer by issuing the following commands in the terminal.

``` shell
mkdir Iotdotnetcorepublisher
kodlar falan
dotnet restore
```
Open Program.cs in Visual Studio and import the following namespaces.

``` c#
using System;
kodlar falan
using M2Mqtt;
``` 

Then perform a 'dotnet restore' in the terminal. It will grab the assemblies for System.Security.Cryptography.X509Certificates.

Then create an instance of Mqtt client object with IoT endpoint, broker port for MQTT, X509Certificate object for root certificate, X5092certificate object for device certificate and Mqttsslprotocols enumeration for TLS1.2. 

Once the connection is successful publish to AWS IoT by specifying the topic and payload. The following code snippet covers all of these. Be sure to update the iotEndpoint variable with the name of your account's IoT endpoint if it was not updated when running the provisioning script.

``` c#
string iotEndpoint = "<<your-iot-endpoint>>";
kodlar falan
}
``` 

Run the application using 'dotnet run' and you should see messages published by dotnet core.

![](/images/pic7.png)

## 4e. Device Consumer using .NET Core 

Let's create the .NET Core console application for the consumer by issuing the following commands in the Terminal.

``` shell
mkdir Iotdotnetcoreconsumer
kodlar falan

```
Open the Program.cs in Visual Studio and import the following namespaces.

``` c#
using System;
kodlar falan
using M2Mqtt.Messages;
``` 

Then perform a 'dotnet restore' in the terminal. It will grab the assemblies for System.Security.Cryptography.X509Certificates.

Then create an instance of Mqtt client object with IoT endpoint, broker port for MQTT, X509Certificate object for root certificate, X5092certificate object for device certificate and Mqttsslprotocols enumeration for TLS1.2. 

You can subscribe to the AWS IoT messages by specifying the Topic as string array and Qos level as byte array. Prior to this event callbacks for MqttMsgSubscribed and MqttMsgPublishReceived should be implemented. The following code snippet covers all of that. Make sure to update the iotEndpoint variable with the name of your account's IoT endpoint if it was not updated when running the provisioning script.

``` c#
private static ManualResetEvent manualResetEvent;

kodlar falan
}
``` 

The complete .NET Core project source for the publisher is available under the Dotnetcoresamples folder in this repository.

Run the application using 'dotnet run' and you should see messages consumed by the dotnetcore

![](/images/pic8.png)

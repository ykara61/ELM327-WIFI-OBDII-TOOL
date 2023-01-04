#  1.Objective

The aim of this repository is to communicate with your vehicle using **Elm327 WiFi OBDII Tool** and **.NET Core**. The code published in this repository uses TCP protocol for communicating with **ELM 327 Wifi OBDII Tool**.

<p>
    <img src="/Images/ELM327_NET6_BASE.png" alt>
</p>

# 2. Why .NET/.NET Core for OBDII Interface

At this point in time, The .NET Core are widely adoped by a large community of enterprise developers. It paves way for use programming language like C# for communicating with vehicle OBDII port. The enterprise developers can integrate this functionality into Console Application, Windows Application and Xamarin mobile applications etc.

# 3. .NET ELM327 WIFI OBDII TOOL


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


## Running The APP

When connection between pc and ELM327 has been succeed. You can run the executable file in debug directory. There are several start option in the application.
These options are:

<p>
    <img src="/Images/ELM327_NET6_CONSOLE_MAIN.png" alt>
</p>

- **Get available PIDs** - It scans vehicle OBDII port and gives you all available PID values names(*such as MonitorStatus,FreezeDTC, CalcEngineLoad, EngineCoolantTemp, EngineRPM, VehicleSpeed, IntakeAirTemperature etc.*) that is active in the vehicle CAN-BUS. You can also see all OBDII PID values in [this wikipedia page](https://en.wikipedia.org/wiki/OBD-II_PIDs). All known PID values stored in "Resources" folder as json file in the solution.
- **Get Current Vehicle Data** - This option writes some significant real time vehicle data that contains *Engine rpm, Vehicle Speed* and *Fuel level*. These values will be printed until user terminates the console application.
- **Free Mode (Pre-Configured)** -  This option gives to availability to send any data you want to send to the Elm 327. All possible commands that is available in the web [Link](https://www.sparkfun.com/datasheets/Widgets/ELM327_AT_Commands.pdf)

<p>
    <img src="/Images/ELM327_ALL_MODES.png" alt>
    <em>Options Screenshots</em>
</p>

The complete .NET project source for the publisher is available in this repository.


# Anki Vector - Unofficial .NET SDK 
![Vector](Documentation/images/VectorSdkBanner.png)

This Vector SDK gives you direct access to [Anki Vector](https://www.anki.com/en-us/vector)'s unprecedented set of advanced sensors, AI capabilities, and robotics technologies including computer vision, intelligent mapping and navigation, and a groundbreaking collection of expressive animations.

It’s powerful but easy to use, complex but not complicated, and versatile enough to be used across a wide range of domains including enterprise, research, and entertainment.

This SDK gives full access to all of Vector's hardware and software features available from the most recent (and final) version of Vector's firmware.  This even includes some features currently not available from of the official Python SDK.

## About this SDK

This SDK implements almost the entire Vector gRPC API.  It even includes functions that don't exist in the Python SDK
including face enrollment and adjusting Vector's permanent settings.  The API design follows the design of the Python SDK but deviates in a few places for simplicity or consistency with the gRPC API.

### Asynchronous API

All API calls that talk to Vector in this SDK are asynchronous methods.  This means that methods in the SDK will return *before* the operation is completed.  Each method returns a [Task](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1?view=netframework-4.8) instance that updates when the operation completes or fails.  The intended way to calls these methods is to declare your methods as `async` and use the `await` keyword for each call.  This gives the illusion of using synchronous calls while supporting asynchronous operation.

For more information on asynchronous programming with `async` and `await`, [click here](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/).

Also you can look at all the code samples (see below) for more examples on using `async`/`await` with this SDK.  Any methods that return a `Task` instance should be called with the `await` keyword.  Any methods that use `await` should be declared `async` and return a `Task`.  Event handlers and the `main` method of a project can be declared `async` and start most operations.

## Getting Started

### Prerequisites

* Vector is powered on.
* You have successfully created an Anki account.
* Vector has been set up with the Vector companion app.
* The Vector companion app is not currently connected to Vector.
* Vector is connected to the same network as your computer.
* You can see Vector’s eyes on his screen.

### Download Microsoft development tools

If you working on Windows, download Visual Studio 2019 Community Edition to get started.  This version is free for personal use.

* [Download Visual Studio 2019 Community Edition](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community)

To get started on Mac and Linux, you can download .NET Core 3.0.  

* [Download .NET Core 3.0](https://dotnet.microsoft.com/download/dotnet-core/3.0)


### Install SDK package from nuget
* https://www.nuget.org/packages/Anki.Vector.SDK


Make sure your Vector robot is connected to the same Wifi as your computer using the [Vector companion app](https://play.google.com/store/apps/details?id=com.anki.vector).

### Vector SDK Configuration and Authentication

In order to use the SDK, you need authenticate with the robot and create a configuration file
that is stored in your user profile.  This SDK uses the same configuration file as the Python SDK
and the [Vector Explorer](https://www.weekendrobot.com/vectorexplorer) application.

The easiest way to get setup with Vector on you Windows PC is to install [Vector Explorer](https://www.weekendrobot.com/vectorexplorer) and configure your robot through that application.  However, you can also use the command line [VectorConfigure](https://www.weekendrobot.com/devtools) application on Windows, Linux, and Mac OS.

You will be prompted for your robot’s name, ip address and serial number. You will also be asked for your Anki login and password. Make sure to use the same account that was used to set up your Vector.  These credentials give full access to your robot, including camera stream, audio stream and data. *Do not share these credentials*.

* **Vector Explorer:** https://www.weekendrobot.com/vectorexplorer

* **VectorConfigure:** https://www.weekendrobot.com/devtools

### SDK Example / Tutorial Programs

You can download the samples Visual Studio solution containing tutorial projects and management
applications.

* Download Sample Visual Studio solution

* [Visit Anki.Vector.Samples GitHub project](https://www.github.com/codaris/Anki.Vector.Samples)

## Documentation

View the complete documentation for the Unofficial .NET SDK by clicking the link below:

* http://blah.com

## Troubleshooting

#### Tutorial program does not run

Before running a Python program, be sure you can see Vector’s eyes. If instead you see an image of a mobile device, the Customer Care Info screen, a missing Wifi icon, or something else, please complete setup of your Vector first and then you will be ready set up the SDK.

#### Vector behaves unexpectedly

You may need to reboot your robot when you are finished running programs with the Vector SDK.  To reboot your Vector remove him from the charger and hold down his backpack button until he turns complete off.  To turn him back on again, place him back on the charger.

#### Can't find robot name

Your Vector robot name looks like "Vector-E5S6". Find your robot name by placing Vector on the charger and double-clicking Vector’s backpack button.

#### Can't find serial number

Your Vector’s serial number looks like "00e20142". Find your robot serial number on the underside of Vector. Or, find the serial number from Vector’s debug screen: double-click his backpack, move his arms up and down, then look for “ESN” on his screen.

#### Can’t find Vector’s IP address

Your Vector IP address looks like "192.168.40.134". Find the IP address from Vector’s debug screen: double-click his backpack, move his arms up and down, then look for "IP" on his screen.  Note that
the SDK will attempt to find Vector's IP address automatically.

## Advanced Tips

### Moving Vector between Wifi networks

The SDK will automatically discover your Vector, even on a new WiFi network, using mDNS.  If this doesn't work, you may have to specify the IP address explictly when connecting to Vector with the SDK.

### Using multiple Vectors

If your computer is configured with more than one Vector robot, you can specify which robot you want to use by passing its serial number or robot name as a parameter to the `Robot.NewConnection` method.

```csharp
using (var robot = await Robot.NewConnection("00e20142"))
{
    await robot.Control.RequestControl();
    await robot.Behavior.SayText("Hello World");
}
```

or

```csharp
using (var robot = await Robot.NewConnection("Vector-B2R5"))
{
    await robot.Control.RequestControl();
    await robot.Behavior.SayText("Hello World");
}
```

### Keeping Vector Still Between SDK Scripts

Vector can be controlled so that  he will not move between SDK scripts. There is an example
command-line application in the Anki.Vector.Samples project and a Windows Tray application 
available at https://www.weekendrobot.com/devtools.

While normal robot behaviors are suppressed, Vector may look 'broken'. Closing the SDK scripts, disconnecting from the robot, or restarting the robot will all release behavior control.

## Getting Help

There are numerous places to get help with this SDK.

* **Official Anki developer forums**: https://forums.anki.com/

* **Anki Vector developer subreddit**: https://www.reddit.com/r/ankivectordevelopers

* **Anki robots Discord chat**: https://discord.gg/FT8EYwu


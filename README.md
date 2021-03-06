JustCli
===
That’s just a quick way to create your own command line tool. 

The idea is to have one command in one class. A set of attributes helps you to map command line arguments to the command class properties and provide additional information. JustCli scans your project, generates help and allows you to run the commands.

Download
---
You can install it using [NuGet](https://www.nuget.org/packages/JustCli/).

Example
---
Let’s create a “*sayhello*” command.
First of all we need a console application with JustCli entry point.
```csharp
static int Main(string[] args)
{
    return CommandLineParser.Default.ParseAndExecuteCommand(args);
}
```
Create SayHelloCommand class and implement command logic.
```csharp
[Command(
    "sayhello",
    "Prints a greeting.",
    @"Very useful command that print hello to the output with the word you provide.
This command is greatly described in this help text so you should be able to use it properly after you read it.
More examples at http://hello.world/greetings/examples")]
class SayHelloCommand : ICommand
{
    [CommandArgument("n", "name", Description = "The someone to greet.", DefaultValue = "World")]
    public string Name { get; set; }

    [CommandOutput]
    public IOutput Output { get; set; }

    public int Execute()
    {
        Output.WriteInfo("Hello {0}!", Name);
        return ReturnCode.Success;
    }
}
```
CommandOutput attribute marks property where the common output is injected. Colored console is used by default.

When run the command line tool we can see command list.
```
cmd> TestApp.exe
Command list:
sayhello - Prints a greeting.
```
Also you can get help for a special command.
```
cmd> TestApp.exe sayhello ? 
sayhello - Prints a greeting.
  Very useful command that print hello to the output with the word you provide.
  This command is greatly described in this help text so you should be able to use it properly after you read it.
  More examples at http://hello.world/greetings/examples
Options:
  -n  --name  [string]  The someone to greet. [default: World]  
```
That is what JustCli makes for you.

Let's test "*sayhello*" command.
```
cmd> TestApp.exe sayhello
Hello World!
cmd> TestApp.exe sayhello -n Bob
Hello Bob!
```
Now you know how to create command line tool quickly and easily! :)



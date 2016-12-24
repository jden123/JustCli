JustCli
===
That’s just a quick way to create your own command line tool. 

Getting Started
---
1. To start working add the JustCli entry point to the main method.

  ```csharp
  static int Main(string[] args)
  {
      return CommandLineParser.Default.ParseAndExecuteCommand(args);
  }
  ```

2. To create a command implement JustCli.ICommand interface.

3. Use the following attributes:
  * [Command]
      * CommandName
      * CommandDescription
      * Order
    
  * [CommandArgument]
      * ShortName
      * LongName
      * DefaultValue
      * Description
  
  * [CommandOutput] - marks IOutput property where output is injected.

For more information go to https://github.com/jden123/JustCli
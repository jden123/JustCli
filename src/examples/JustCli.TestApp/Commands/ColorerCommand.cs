using JustCli.Attributes;

namespace JustCli.TestApp.Commands
{
   enum Color
   {
      Red,
      Green,
      Yellow
   }

   [Command("colorer", "Prints a color opinion.")]
   class ColorerCommand : ICommand
   {
      [CommandArgument("c", "color", Description = "The color", DefaultValue = Color.Green)]
      public Color Color { get; set; }

      [CommandOutput]
      public IOutput Output { get; set; }

      public int Execute()
      {
         if (Color == Color.Red)
         {
            Output.WriteInfo("Oh, no! Red color!");
            return ReturnCode.Failure;
         }

         if (Color == Color.Yellow)
         {
            Output.WriteInfo("Yellow color is better, but not enough!");
            return ReturnCode.Failure;
         }

         Output.WriteInfo("Green color is awesome!!");
         return ReturnCode.Success;
      }
   }
}

using System.Collections.Generic;

namespace JustCli
{
   public interface IArgValueSource
   {
      /// <summary>
      /// Returns argument values.
      /// Key is long name. Value is argument value.
      /// </summary>
      /// <returns>The arguments values.</returns>
      Dictionary<string, string> GetArgValues();
   }
}
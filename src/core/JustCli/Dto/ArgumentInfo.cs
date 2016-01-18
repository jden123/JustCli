using System;

namespace JustCli.Dto
{
    public class ArgumentInfo
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public object DefaultValue { get; set; }
        public Type ArgumentType { get; set; }
    }
}
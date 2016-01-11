using System;

namespace JustCli.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandArgumentAttribute : Attribute
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public object DefaultValue { get; set; }
        public string Description { get; set; }

        public CommandArgumentAttribute(string shortName, string longName = null, object defaultValue = null, string description = null)
        {
            ShortName = shortName;
            LongName = longName;
            DefaultValue = defaultValue;
            Description = description;
        }
    }
}
using System;
using System.ComponentModel;

namespace JustCli
{
    // Works with -a or --action params.
    // TODO: indexed command context.
    // TODO: queue instead of []. Command context + parser.
    public class CommandContext
    {
        public string[] Args { get; set; }

        public CommandContext(string[] args)
        {
            Args = args;
        }

        public object GetArgValue(string shortName, string longName, object defaultValue, Type propertyType)
        {
            var stringValue = GetArgumentValue(shortName, longName);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (defaultValue == null)
                {
                    throw new Exception(string.Format("The argument[{0}] is not presented in command line.", longName));
                }
                
                return defaultValue;
            }
            
            var value = ConvertFromString(stringValue, propertyType);

            return value;
        }

        // TODO: try method and write down an error message
        public object GetArgValue(ArgumentInfo argumentInfo, Type propertyType)
        {
            return GetArgValue(argumentInfo.ShortName, argumentInfo.LongName, argumentInfo.DefaultValue, propertyType);
        }

        // TODO: try method and write down an error message
        private static object ConvertFromString(string stringValue, Type toType)
        {
            var typeConverter = TypeDescriptor.GetConverter(toType);
            var value = typeConverter.ConvertFrom(stringValue);
            return value;
        }

        // TODO: parse once in constructor
        private string GetArgumentValue(string shortName, string longName)
        {
            string value = null;
            for (int i = 0; i < Args.Length; i++)
            {
                if (Args[i] == string.Format("-{0}", shortName) ||
                    Args[i] == string.Format("--{0}", longName))
                {
                    value = Args[i + 1];
                    break;
                }
            }

            return value;
        }
    }
}
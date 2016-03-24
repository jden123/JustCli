using System;
using System.Collections.Generic;
using System.ComponentModel;
using JustCli.Dto;

namespace JustCli
{
    // Works with -a or --action params.
    public class CommandContext
    {
        private readonly string CommandName;
        private readonly Dictionary<string, string> ArgValues; 
        
        public CommandContext(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentOutOfRangeException("args");
            }

            CommandName = args[0];
            ArgValues = new Dictionary<string, string>();

            if (args.Length == 1)
            {
                return;
            }

            // if not an argument name then something wrong.
            if (!IsArgumentName(args[1]))
            {
                throw new ArgumentException("Cannot find argument.");
            }

            var argName = string.Empty;
            var argValue = string.Empty;
            for (var i = 1; i < args.Length; i++)
            {
                var element = args[i];
                if (IsArgumentName(element))
                {
                    if (!string.IsNullOrEmpty(argName) && !ArgValues.ContainsKey(argName))
                    {
                        ArgValues.Add(argName, argValue);
                    }

                    argName = element;
                    argValue = string.Empty;
                }
                else
                {
                    argValue = string.IsNullOrEmpty(argValue)
                        ? element
                        : argValue + " " + element;
                }
            }

            // put last pair
            ArgValues.Add(argName, argValue);
        }

        // TODO: try method and write down an error message
        public object GetArgValue(ArgumentInfo argumentInfo)
        {
            return GetArgValue(argumentInfo.ShortName, argumentInfo.LongName, argumentInfo.DefaultValue, argumentInfo.ArgumentType);
        }

        private object GetArgValue(string shortName, string longName, object defaultValue, Type propertyType)
        {
            var stringValue = GetArgumentValue(shortName, longName);
            if (stringValue == null)
            {
                if (defaultValue == null)
                {
                    throw new Exception(string.Format("The argument [{0}] is not presented in command line.", longName));
                }
                
                return defaultValue;
            }
            
            // special case: flag attributes.
            // if attribute is bool and attribute is in the command line (value = string.empty)
            // then flag = true.
            if (propertyType == typeof (bool) && stringValue == string.Empty)
            {
                return true;
            }

            try
            {
                var value = ConvertFromString(stringValue, propertyType);
                return value;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format(
                    "The argument [{0}] is not set up. The value [{1}] cannot be cast to [{2}].", 
                    longName, stringValue, propertyType.Name));
            }
        }

        // TODO: try method and write down an error message
        private static object ConvertFromString(string stringValue, Type toType)
        {
            var typeConverter = TypeDescriptor.GetConverter(toType);
            var value = typeConverter.ConvertFrom(stringValue);
            return value;
        }

        private string GetArgumentValue(string shortName, string longName)
        {
            string value;
            if (ArgValues.TryGetValue(string.Format("-{0}", shortName), out value))
            {
                return value;
            }

            if (ArgValues.TryGetValue(string.Format("--{0}", longName), out value))
            {
                return value;
            }

            return null;
        }

        private static bool IsArgumentName(string arg)
        {
            return arg.StartsWith("-");
        }
    }
}
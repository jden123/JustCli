using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using JustCli.Dto;

namespace JustCli
{
    // Works with -a or --action params.
    public class CommandContext
    {
        private readonly string CommandName;
        private readonly Dictionary<string, string> ArgValues; 
        
        public CommandContext(string[] args, IEnumerable<IArgValueSource> additionalArgValueSources = null)
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

            // Process additional arg sources
            if (additionalArgValueSources != null)
            {
                foreach (var additionalArgValueSource in additionalArgValueSources)
                {
                    foreach (var additionalArgValuePair in additionalArgValueSource.GetArgValues())
                    {
                        ArgValues[$"--{additionalArgValuePair.Key}"] = additionalArgValuePair.Value;
                    }
                }
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
                        ArgValues[argName] = argValue;
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
            ArgValues[argName] = argValue;
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

                if (!(defaultValue is string))
                {
                    return defaultValue;
                }
                
                var defaultValueString = (string) defaultValue;
                
                // special case: we cannot set default value for datetime. Have to use string.
                if (propertyType == typeof(DateTime))
                {
                    if (defaultValueString.ToLower() == "minvalue")
                    {
                        return DateTime.MinValue;
                    }

                    if (defaultValueString.ToLower() == "maxvalue")
                    {
                        return DateTime.MaxValue;
                    }

                    DateTime defaultValueDate;
                    if (DateTime.TryParse(defaultValueString, CultureInfo.InvariantCulture, DateTimeStyles.None, out defaultValueDate))
                    {
                        return defaultValueDate;
                    }

                    throw new Exception(string.Format("Default value for The argument [{0}] is not valid.", longName));
                }

                // special case: we cannot set default value for GUID. Have to use string.
                if (propertyType == typeof(Guid))
                {
                    if (defaultValueString.ToLower() == "empty")
                    {
                       return Guid.Empty;
                    }

                    Guid defaultValueGuid;
                    if (Guid.TryParse(defaultValueString, out defaultValueGuid))
                    {
                        return defaultValueGuid;
                    }

                    throw new Exception(string.Format("Default value for The argument [{0}] is not valid.", longName));
                }

                if (propertyType != typeof(string))
                {
                    try
                    {
                        return ConvertFromString(defaultValueString, propertyType, true);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "The argument [{0}] is not set up. The default value [{1}] cannot be cast to [{2}].", 
                                longName, defaultValueString, propertyType.Name),
                            e);
                    }
                }

                // if property is string
                return defaultValueString;
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
                return ConvertFromString(stringValue, propertyType);
            }
            catch (Exception e)
            {
                throw new Exception(
                    string.Format(
                        "The argument [{0}] is not set up. The value [{1}] cannot be cast to [{2}].", 
                        longName, stringValue, propertyType.Name),
                    e);
            }
        }

        // TODO: try method and write down an error message
        private static object ConvertFromString(string stringValue, Type toType, bool useInvariantCulture = false)
        {
            var typeConverter = TypeDescriptor.GetConverter(toType);
            if (useInvariantCulture)
            {
                var value = typeConverter.ConvertFromString(null, CultureInfo.InvariantCulture, stringValue);
                return value;
            }
            else
            {
                var value = typeConverter.ConvertFromString(stringValue);
                return value;
            }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using teams_console.Components;
using teams_console.Renderer;

namespace teams_console.Commands
{
    public static class CommandFactory
    {
        private static readonly Dictionary<string, Type> Commands = GetAllCommands();

        public static ICommand Create(string message)
        {
            var regex = new Regex(@"\/(?<command>[\S]*)( (?<options>.*))?");
            var matchResult = regex.Match(message);
            var cmd = GetAllCommands();

            if (matchResult.Success)
            {
                if (Commands.TryGetValue(matchResult.Groups["command"].Value, out var type))
                {
                    var ctor = type.GetConstructor(new[] { typeof(string) });
                    if (ctor != null)
                        return (ICommand)ctor.Invoke(new object[] { matchResult.Groups["options"].Value });

                    ctor = type.GetConstructor(new Type[] {});
                    return (ICommand)ctor.Invoke(null);          // NOTE: should crash if not found for now
                }

                return new InvalidCommand();
            }

            return new TextCommand(message);
        }

        public static CommandAttributeAttribute GetCommandAttribute<T>() where T : ICommand
        {
            return GetCommandAttribute(typeof(T));
        }

        public static CommandAttributeAttribute GetCommandAttribute(Type type)
        {
            var attribute = type.GetCustomAttribute<CommandAttributeAttribute>();
            if (attribute == null)
                return new CommandAttributeAttribute { Name = GetName(type) };

            return new CommandAttributeAttribute
            {
                Name = string.IsNullOrEmpty(attribute.Name) ? GetName(type) : attribute.Name,
                Alias = attribute.Alias,
                Description = attribute.Description,
                Parameters = attribute.Parameters
            };
        }

        public static void RenderHelp(ApplicationView application, params Type[] commands)
        {
            application.Server.Write(LogType.Info, "Available commands");

            foreach (var commandType in commands)
            {
                var attribute = GetCommandAttribute(commandType);
                application.Server.Write(LogType.Info, $" /{attribute}");
            }
        }

        private static Dictionary<string, Type> GetAllCommands()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => typeof(ICommand).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();

            var dictionary = new Dictionary<string, Type>();
            foreach (var type in types)
            {
                var attribute = GetCommandAttribute(type);
                dictionary.Add(attribute.Name, type);
                if (!string.IsNullOrEmpty(attribute.Alias))
                    dictionary.Add(attribute.Alias, type);
            }

            return dictionary;
        }

        private static string GetName(Type type)
        {
            var name = type.Name;

            if (name.EndsWith("Command"))
                name = name.Remove(name.Length - "Command".Length).ToLowerInvariant();

            return name;
        }
    }
}

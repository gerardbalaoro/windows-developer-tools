using System.Text.RegularExpressions;

namespace Alias
{
    internal class Alias
    {
        public string Name { get; set; }

        public string Command { get; set; }

        public Alias(string name, string command)
        {
            Name = name.Trim();
            Command = command.Trim();
        }

        public string Expression {
            get {
                string command = Regex.IsMatch(Command, @"\s")
                         ? $"\"{Command.Replace("\"", "\\\"")}\""
                         : Command;

                return $"alias {Name}={command}";
            }
        }

        public static Alias? Parse(string expression)
        {
            var validation = (new Regex("(?<name>[\\w_-]+)=(?<command>.+)")).Match(expression);

            if (!validation.Success) {
                return null;
            }

            var name = validation.Groups["name"].Value;
            var command = validation.Groups["command"].Value;

            if (
                (command.StartsWith("\"") && command.EndsWith("\"")
                || (command.StartsWith("'") && command.EndsWith("'"))
            )) {
                command = command.Substring(1, command.Length - 1);
            }

            return new Alias(name, command);
        }
    }
}

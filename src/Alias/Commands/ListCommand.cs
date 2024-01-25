using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Alias.Commands
{
    public class ListCommand : Command<ListCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandOption("-f|--filter <filter>")]
            [Description("Name of the alias to remove")]
            public string? Filter { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            var repository = new AliasRepository();
            var aliases = repository.List(settings.Filter);

            if (aliases.Count() == 0) {
                AnsiConsole.MarkupLine("No aliases found.");
                return 0;
            }

            foreach (var alias in aliases) {
                AnsiConsole.MarkupLine($"[blue]{alias.Name}[/]=[silver]{alias.Command}[/]");
            }

            return 0;
        }
    }
}

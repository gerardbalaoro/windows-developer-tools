using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Alias.Commands
{
    public class RemoveCommand : Command<RemoveCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<alias>")]
            [Description("Name of the alias to remove")]
            public string Name { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            var repository = new AliasRepository();
            var alias = repository.Get(settings.Name);

            if (alias == null) {
                AnsiConsole.MarkupLine($"No alias named '{settings.Name}' has been found.");
                return 1;
            }

            repository.Delete(alias.Name);
            AnsiConsole.MarkupLine($"Alias '{alias.Name}' has been removed.");

            return 0;
        }
    }
}

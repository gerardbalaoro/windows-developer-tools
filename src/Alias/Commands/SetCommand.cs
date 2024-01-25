using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Alias.Commands
{
    public class SetCommand : Command<SetCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<expression>")]
            [Description("Assignment expression: <name>=<command>")]
            public string Expression { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            var repository = new AliasRepository();
            var alias = Alias.Parse(settings.Expression);

            if (alias == null) {
                AnsiConsole.MarkupLine("[red]Invalid expression[/]");
                return -1;
            }

            repository.Save(alias);
            AnsiConsole.MarkupLine($"Alias '{alias.Name}' has been set.");

            return 0;
        }
    }
}

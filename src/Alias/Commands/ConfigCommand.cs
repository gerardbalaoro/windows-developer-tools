using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Alias.Commands
{
    public class ConfigCommand : Command<ConfigCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandOption("--enable")]
            [Description("Enable aliases for the current user")]
            public bool Enable { get; set; }

            [CommandOption("--disable")]
            [Description("Disable aliases for the current user")]
            public bool Disable { get; set; }

            [CommandOption("--export <file>")]
            [Description("Export aliases to file")]
            public string Export { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            if ((bool)settings.Enable) {
                return Enable();
            }

            if ((bool)settings.Disable) {
                return Disable();
            }

            if (!string.IsNullOrWhiteSpace(settings.Export)) {
                return Export(settings.Export);
            }

            AnsiConsole.WriteLine("No option specified.");

            return 0;
        }

        private int Enable()
        {
            if (Installer.IsEnabled) {
                AnsiConsole.WriteLine("Aliases are already added enabled.");

                return 0;
            }

            Installer.Enable();
            AnsiConsole.Markup("[green]Aliases have been enabled for this user.[/]");
            AnsiConsole.Markup("Please restart your shell session or refresh your environment variables.");

            return 0;
        }

        private int Disable()
        {
            if (!Installer.IsEnabled) {
                AnsiConsole.WriteLine("Aliases are not enabled.");

                return 0;
            }

            Installer.Enable();
            AnsiConsole.Markup("[green]Aliases have been disabled for this user.[/]");
            AnsiConsole.Markup("Please restart your shell session or refresh your environment variables.");

            return 0;
        }

        private int Export(string path)
        {
            var repository = new AliasRepository();
            var aliases = repository.List();

            using (StreamWriter file = new StreamWriter(path)) {
                foreach (var alias in aliases) {
                    file.WriteLine(alias.Expression);
                }
            }

            return 0;
        }
    }
}

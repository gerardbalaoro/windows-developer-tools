using Spectre.Console;
using Spectre.Console.Cli;

namespace Alias.Interceptors
{
    public class CheckInstallationInterceptor : ICommandInterceptor
    {
        public void Intercept(CommandContext context, CommandSettings settings)
        {
            string[] ignoredCommands = new string[] { "enable", "disable" };

            if (Installer.IsEnabled || ignoredCommands.Contains(context.Name)) {
                return;
            }

            AnsiConsole.MarkupLine("[bold yellow]The aliases directory is not added in the PATH variable.[/]");
            AnsiConsole.MarkupLine($"Please run `[blue]{AppDomain.CurrentDomain.FriendlyName} config --enable[/]`.\n");
        }
    }
}

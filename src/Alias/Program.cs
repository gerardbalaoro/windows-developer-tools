using Alias.Commands;
using Alias.Interceptors;
using Spectre.Console.Cli;

namespace Alias
{
    public class Program
    {
        static int Main(string[] args)
        {
            var app = new CommandApp();

            app.Configure(config => {
                config.SetInterceptor(new CheckInstallationInterceptor());
                config.AddCommand<SetCommand>("set")
                    .WithDescription("Set a new alias")
                    .WithAlias("s");
                config.AddCommand<RemoveCommand>("remove")
                    .WithDescription("Remove an existing alias")
                    .WithAlias("rm");
                config.AddCommand<ListCommand>("list")
                    .WithDescription("List all aliases")
                    .WithAlias("ls");
                config.AddCommand<ConfigCommand>("config")
                    .WithDescription("Configure aliases");
            });

            app.SetDefaultCommand<SetCommand>();

            return app.Run(args);
        }
    }
}

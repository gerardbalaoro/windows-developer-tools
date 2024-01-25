using Core.Environment;

namespace Alias
{
    internal class Installer
    {
        public static string InstallDirectory {
            get {
                string? directory = Environment.GetEnvironmentVariable("DEVTOOLS_ALIASES");

                if (directory == null) {
                    directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "aliases");
                }

                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }

                return directory;
            }
        }

        public static bool IsEnabled {
            get {
                string? path = Environment.GetEnvironmentVariable("PATH");
                string[] paths = string.IsNullOrWhiteSpace(path)
                    ? Array.Empty<string>()
                    : path
                        .Split(';')
                        .Select(x => Environment.ExpandEnvironmentVariables(x))
                        .ToArray();

                return paths.Contains(Installer.InstallDirectory);
            }
        }

        public static void Enable(EnvironmentScope scope = EnvironmentScope.User)
        {
            EnvironmentRepository repository = new EnvironmentRepository(scope);
            EnvironmentVariable? path = repository.Get("PATH");

            if (path == null) {
                path = new EnvironmentVariable("PATH", "");
            }

            path.AddValue(InstallDirectory);
            repository.Put(path);
        }

        public static void Disable(EnvironmentScope scope = EnvironmentScope.User)
        {
            EnvironmentRepository repository = new EnvironmentRepository(scope);
            EnvironmentVariable? path = repository.Get("PATH");

            if (path == null) {
                return;
            }

            path.RemoveValue(InstallDirectory);
            repository.Put(path);
        }
    }
}

using Core.Contracts;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Text.RegularExpressions;

namespace Alias
{
    internal class AliasRepository : IRepository<Alias>
    {
        protected static string GetFileName(string alias)
        {
            return Path.Combine(Installer.InstallDirectory, alias + ".bat");
        }

        public List<Alias> List(string? keyword = null)
        {
            Matcher matcher = new();
            string directory = Installer.InstallDirectory;

            matcher.AddIncludePatterns(new[] { "*.bat" });

            IEnumerable<string> files = matcher.GetResultsInFullPath(directory);

            string[] ids = files.Select(x => Path.GetFileNameWithoutExtension(x)).ToArray();

            return ids.Select(id => Get(id)!).Where(x => x != null).ToList();
        }

        public Alias? Get(string id)
        {
            string file = GetFileName(id);

            if (!File.Exists(file)) {
                return null;
            }

            Regex pattern = new Regex("^@ECHO OFF[\r\n]+", RegexOptions.Singleline);
            string contents = pattern.Replace(File.ReadAllText(file), "");

            return new Alias(id, contents);
        }

        public void Save(Alias alias)
        {
            string file = GetFileName(alias.Name);
            string contents =
                $"""
                @ECHO OFF
                {alias.Command}
                """;

            File.WriteAllText(file, contents);
        }

        public void Delete(string id)
        {
            string file = GetFileName(id);

            if (File.Exists(file)) {
                File.Delete(file);
            }
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Core.Environment
{
    public class EnvironmentRepository
    {
        protected EnvironmentScope Scope;

        protected RegistryKey? Key {
            get {
                if (Scope == EnvironmentScope.Machine) {
                    return Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment", true);
                };
                return Registry.CurrentUser.OpenSubKey(@"Environment", true);
            }
        }

        public EnvironmentRepository(EnvironmentScope scope)
        {
            Scope = scope;
        }

        public EnvironmentVariable? Get(string name)
        {
            if (Key == null) {
                return null;
            }

            name = name.Trim();
            string? value = Key.GetValue(name)?.ToString() ?? null;

            if (value == null) {
                return null;
            }

            return new EnvironmentVariable(name, value, Scope);
        }

        public EnvironmentVariable[] List(string? filter)
        {
            if (Key == null) {
                return new EnvironmentVariable[] { };
            }

            IEnumerable<string> names = Key.GetSubKeyNames().ToList();

            if (filter != null) {
                names = names.Where(name => FileSystemName.MatchesSimpleExpression(filter, name));
            }

            return names
                .Select(name => Get(name)!)
                .Where(variable => variable != null)
                .ToArray();
        }

        public void Put(EnvironmentVariable variable)
        {
            if (Key == null) {
                return;
            }

            Key.SetValue(variable.Name, variable.Value);
        }

        public void Remove(string name)
        {
            if (Key == null) {
                return;
            }

            Key.DeleteValue(name);
        }

        public void Remove(EnvironmentVariable variable)
        {
            Remove(variable.Name);
        }
    }
}

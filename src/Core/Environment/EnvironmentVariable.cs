using System.Xml.Linq;

namespace Core.Environment
{
    public class EnvironmentVariable
    {
        public string Name { get; set; }

        public string Value { get; protected set; } = "";

        public EnvironmentScope? Scope { get; protected set; }

        public static string ValueSepatator = ";";

        public EnvironmentVariable(string name, string value, EnvironmentScope? scope = null)
        {
            Name = name;
            Scope = scope;
            SetValue(value);
        }

        public string SetValue(IEnumerable<string> values)
        {
            if (values.Count() == 0) {
                return SetValue("");
            }

            return SetValue(string.Join(ValueSepatator, values));
        }

        public string SetValue(string value)
        {
            this.Value = value;

            return this.Value;
        }

        public string AddValue(IEnumerable<string> values, int postion = -1)
        {
            List<string> list = Value.Split(ValueSepatator).ToList();

            foreach (var value in values) {
                if (list.Contains(value)) {
                    continue;
                }

                if (postion == -1) {
                    list.Add(value);
                } else {
                    list.Insert(postion, value);
                }
            }

            return SetValue(list);
        }

        public string AddValue(string value, int postion = -1)
        {
            return AddValue(new string[] { value }, postion);
        }

        public string RemoveValue(IEnumerable<string> values)
        {
            List<string> list = Value.Split(ValueSepatator).ToList();

            foreach (var item in list) {
                if (values.Contains(item)) {
                    list.Remove(item);
                }
            }

            return SetValue(list);
        }

        public string RemoveValue(string value)
        {
            return RemoveValue(new string[] { value });
        }    
    }
}

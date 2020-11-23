using System;

namespace teams_console.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttributeAttribute : Attribute
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public string Description { get; set; }

        public string[] Parameters { get; set; } = new string[0];

        public override string ToString()
        {
            var str = Name;
            if (!string.IsNullOrEmpty(Alias))
                str += $" /{Alias}";

            foreach (var p in Parameters)
                str += $" <{p}>";

            if (str.Length < 24)
                str += new string(' ', 24 - str.Length);
            else
                str += " ";

            str += Description;

            return str;
        }
    }
}

using System.Text;

namespace NodeMCU_Studio_2015
{
    class Utilities
    {
        public static string Escape(string command)
        {
            const char backSlash = '\\';
            const char slash = '/';
            const char doubleQuote = '"';

            var output = new StringBuilder(command.Length);
            foreach (var c in command)
            {
                switch (c)
                {
                    case backSlash:
                        output.AppendFormat("{0}{0}", backSlash);
                        break;

                    case slash:
                        output.AppendFormat("{0}{1}", backSlash, slash);
                        break;

                    case doubleQuote:
                        output.AppendFormat("{0}{1}", backSlash, doubleQuote);
                        break;

                    default:
                        output.Append(c);
                        break;
                }
            }

            return output.ToString();
        }
    }
}

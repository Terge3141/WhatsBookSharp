using System;
using System.Linq;

namespace Helper
{
    /// <summary>
    /// Parses the old Soft Bank unicode characters to new unicode characters
    /// Table from http://punchdrunker.github.io/iOSEmoji/table_html/
    /// </summary>
    public class SoftBankConverter
    {
        private static readonly string[][] _mapping = new string[][]
        {
                new []{"e415", "1f604"},
                new []{"e056", "1f60a"},
                new []{"e057", "1f603"},
                new []{"e414", "263a"},
                new []{"e405", "1f609"},
                new []{"e106", "1f60d"},
                new []{"e418", "1f618"},
                new []{"e417", "1f61a"},
                new []{"e40d", "1f633"},
                new []{"e40a", "1f60c"},
                new []{"e404", "1f601"},
                new []{"e105", "1f61c"},
                new []{"e409", "1f61d"},
                new []{"e40e", "1f612"},
                new []{"e402", "1f60f"},
                new []{"e108", "1f613"},
                new []{"e403", "1f614"},
                new []{"e058", "1f61e"},
                new []{"e407", "1f616"},
                new []{"e401", "1f625"},
                new []{"e40f", "1f630"},
                new []{"e40b", "1f628"},
                new []{"e406", "1f623"},
                new []{"e413", "1f622"},
                new []{"e411", "1f62d"},
                new []{"e412", "1f602"},
                new []{"e410", "1f632"},
                new []{"e107", "1f631"},
                new []{"e059", "1f620"},
                new []{"e416", "1f621"},
                new []{"e408", "1f62a"},
                new []{"e40c", "1f637"},
                new []{"e11a", "1f47f"},
                new []{"e10c", "1f47d"},
                new []{"e32c", "1f49b"},
                new []{"e32a", "1f499"},
                new []{"e32d", "1f49c"},
                new []{"e328", "1f497"},
                new []{"e32b", "1f49a"},
                new []{"e022", "2764"},
                new []{"e023", "1f494"},
                new []{"e327", "1f493"},
                new []{"e329", "1f498"},
                new []{"e32e", "2728"},
                new []{"e335", "1f31f"},
                new []{"e334", "1f4a2"},
                new []{"e337", "2755 "},
                new []{"e336", "2754 "},
                new []{"e13c", "1f4a4"},
                new []{"e330", "1f4a8"},
                new []{"e331", "1f4a6"},
                new []{"e326", "1f3b6"},
                new []{"e03e", "1f3b5"},
                new []{"e11d", "1f525"},
                new []{"e05a", "1f4a9"},
                new []{"e00e", "1f44d"},
                new []{"e421", "1f44e"},
                new []{"e420", "1f44c"},
                new []{"e00d", "1f44a"},
                new []{"e010", "270a "},
                new []{"e011", "270c "},
                new []{"e41e", "1f44b"},
                new []{"e012", "270b "},
                new []{"e422", "1f450"},
                new []{"e22e", "1f446"},
                new []{"e22f", "1f447"},
                new []{"e231", "1f449"},
                new []{"e230", "1f448"},
                new []{"e427", "1f64c"},
                new []{"e41d", "1f64f"},
                new []{"e00f", "261d "},
                new []{"e41f", "1f44f"},
                new []{"e14c", "1f4aa"},
                new []{"e201", "1f6b6"},
                new []{"e115", "1f3c3"},
                new []{"e428", "1f46b"},
                new []{"e51f", "1f483"},
                new []{"e429", "1f46f"},
                new []{"e424", "1f646"},
                new []{"e423", "1f645"},
                new []{"e253", "1f481"},
                new []{"e426", "1f647"},
                new []{"e111", "1f48f"},
                new []{"e425", "1f491"},
                new []{"e31e", "1f486"},
                new []{"e31f", "1f487"},
                new []{"e31d", "1f485"},
                new []{"e001", "1f466"},
                new []{"e002", "1f467"},
                new []{"e005", "1f469"},
                new []{"e004", "1f468"},
                new []{"e51a", "1f476"},
                new []{"e519", "1f475"},
                new []{"e518", "1f474"},
                new []{"e515", "1f471"},
                new []{"e516", "1f472"},
                new []{"e517", "1f473"},
                new []{"e51b", "1f477"},
                new []{"e152", "1f46e"},
                new []{"e04e", "1f47c"},
                new []{"e51c", "1f478"},
                new []{"e51e", "1f482"},
                new []{"e11c", "1f480"},
                new []{"e536", "1f463"},
                new []{"e003", "1f48b"},
                new []{"e41c", "1f444"},
                new []{"e41b", "1f442"},
                new []{"e419", "1f440"},
                new []{"e41a", "1f443"},
        };

        public static string GetNewUnicode(string str)
        {
            var query = _mapping.Where(x => x[0].Equals(str));
            if (query.Count() > 0)
            {
                return query.First()[1];
            }
            else
            {
                return null;
            }
        }
    }
}


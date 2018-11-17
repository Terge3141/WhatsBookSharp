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
            new []{ "E415", "1F604" },
            new []{ "E056", "1F60A" },
            new []{ "E057", "1F603" },
            new []{ "E414", "263A" },
            new []{ "E405", "1F609" },
            new []{ "E106", "1F60D" },
            new []{ "E418", "1F618" },
            new []{ "E417", "1F61A" },
            new []{ "E40D", "1F633" },
            new []{ "E40A", "1F60C" },
            new []{ "E404", "1F601" },
            new []{ "E105", "1F61C" },
            new []{ "E409", "1F61D" },
            new []{ "E40E", "1F612" },
            new []{ "E402", "1F60F" },
            new []{ "E108", "1F613" },
            new []{ "E403", "1F614" },
            new []{ "E058", "1F61E" },
            new []{ "E407", "1F616" },
            new []{ "E401", "1F625" },
            new []{ "E40F", "1F630" },
            new []{ "E40B", "1F628" },
            new []{ "E406", "1F623" },
            new []{ "E413", "1F622" },
            new []{ "E411", "1F62D" },
            new []{ "E412", "1F602" },
            new []{ "E410", "1F632" },
            new []{ "E107", "1F631" },
            new []{ "E059", "1F620" },
            new []{ "E416", "1F621" },
            new []{ "E408", "1F62A" },
            new []{ "E40C", "1F637" },
            new []{ "E11A", "1F47F" },
            new []{ "E10C", "1F47D" },
            new []{ "E32C", "1F49B" },
            new []{ "E32A", "1F499" },
            new []{ "E32D", "1F49C" },
            new []{ "E328", "1F497" },
            new []{ "E32B", "1F49A" },
            new []{ "E022", "2764" },
            new []{ "E023", "1F494" },
            new []{ "E327", "1F493" },
            new []{ "E329", "1F498" },
            new []{ "E32E", "2728" },
            new []{ "E335", "1F31F" },
            new []{ "E334", "1F4A2" },
            new []{ "E337", "2755 " },
            new []{ "E336", "2754 " },
            new []{ "E13C", "1F4A4" },
            new []{ "E330", "1F4A8" },
            new []{ "E331", "1F4A6" },
            new []{ "E326", "1F3B6" },
            new []{ "E03E", "1F3B5" },
            new []{ "E11D", "1F525" },
            new []{ "E05A", "1F4A9" },
            new []{ "E00E", "1F44D" },
            new []{ "E421", "1F44E" },
            new []{ "E420", "1F44C" },
            new []{ "E00D", "1F44A" },
            new []{ "E010", "270A " },
            new []{ "E011", "270C " },
            new []{ "E41E", "1F44B" },
            new []{ "E012", "270B " },
            new []{ "E422", "1F450" },
            new []{ "E22E", "1F446" },
            new []{ "E22F", "1F447" },
            new []{ "E231", "1F449" },
            new []{ "E230", "1F448" },
            new []{ "E427", "1F64C" },
            new []{ "E41D", "1F64F" },
            new []{ "E00F", "261D " },
            new []{ "E41F", "1F44F" },
            new []{ "E14C", "1F4AA" },
            new []{ "E201", "1F6B6" },
            new []{ "E115", "1F3C3" },
            new []{ "E428", "1F46B" },
            new []{ "E51F", "1F483" },
            new []{ "E429", "1F46F" },
            new []{ "E424", "1F646" },
            new []{ "E423", "1F645" },
            new []{ "E253", "1F481" },
            new []{ "E426", "1F647" },
            new []{ "E111", "1F48F" },
            new []{ "E425", "1F491" },
            new []{ "E31E", "1F486" },
            new []{ "E31F", "1F487" },
            new []{ "E31D", "1F485" },
            new []{ "E001", "1F466" },
            new []{ "E002", "1F467" },
            new []{ "E005", "1F469" },
            new []{ "E004", "1F468" },
            new []{ "E51A", "1F476" },
            new []{ "E519", "1F475" },
            new []{ "E518", "1F474" },
            new []{ "E515", "1F471" },
            new []{ "E516", "1F472" },
            new []{ "E517", "1F473" },
            new []{ "E51B", "1F477" },
            new []{ "E152", "1F46E" },
            new []{ "E04E", "1F47C" },
            new []{ "E51C", "1F478" },
            new []{ "E51E", "1F482" },
            new []{ "E11C", "1F480" },
            new []{ "E536", "1F463" },
            new []{ "E003", "1F48B" },
            new []{ "E41C", "1F444" },
            new []{ "E41B", "1F442" },
            new []{ "E419", "1F440" },
            new []{ "E41A", "1F443" },
        };

        public static string GetNewUnicode(string str)
        {
            var query=_mapping.Where(x => x[0].Equals(str));
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


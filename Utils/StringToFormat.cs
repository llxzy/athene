using athene.Enums;

namespace athene.Utils
{
    public static class StringToFormat
    {
        //probably can be done better but this works, for now
        public static EntryFormat Convert(string input)
        {
            return input switch
            {
                "epub" => EntryFormat.Epub,
                "pdf" => EntryFormat.Pdf,
                "paper" => EntryFormat.Paper,
                _ => EntryFormat.Other
            };
        }
    }
}
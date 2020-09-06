using System.ComponentModel.DataAnnotations;
using athene.Enums;
using GLib;

namespace athene
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public EntryFormat Format { get; set; }
        public int Score { get; set; }
        public int Year { get; set; }

        public Entry(string title, string author)
        {
            Title = title;
            Author = author;
            Score = 0;
            Year = DateTime.NewNowUtc().Year;
        }
    }
}
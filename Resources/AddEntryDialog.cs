using System;
using System.Linq;
using athene.Utils;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace athene.Resources
{
    public class AddEntryDialog : Dialog
    {
        // todo rename glade file
        
        [UI] private readonly Button _addButton = null;
        [UI] private readonly Button _cancelButton = null;
        [UI] private readonly Gtk.Entry _ratingEntry = null;
        [UI] private readonly Gtk.Entry _authorEntry = null;
        [UI] private readonly Gtk.Entry _titleEntry = null;
        public string Rating { get; set; }
        public string Author { get; set; }
        public new string Title { get; set; }
        
        public AddEntryDialog() : this(new Builder("AddEntryDialog.glade"))
        {
        }

        private AddEntryDialog(Builder builder) : base(builder.GetObject("AddEntry").Handle)
        {
            builder.Autoconnect(this);
            _addButton.Clicked += AddEvent;
            _cancelButton.Clicked += CancelEvent;
        }

        private void CancelEvent(object sender, EventArgs e)
        {
            Respond(ResponseType.No);
            Dispose();
        }

        private void AddEvent(object sender, EventArgs e)
        {
            Title = _titleEntry.Text;
            Author = _authorEntry.Text;
            Rating = _ratingEntry.Text;
            var isWrong = !int.TryParse(Rating, out var parsed) || parsed > 10;
            if (Title.Length == 0)
            {
                MessageDialogDisplayer.Show(this, "Title cannot be empty!");
                return;
            }
            if (Author.Length == 0)
            {
                MessageDialogDisplayer.Show(this, "Author cannot be empty!");
                return;
            }
            if (!Rating.All(char.IsDigit) || isWrong)
            {
                MessageDialogDisplayer.Show(this, "Wrong format for score!");
                return;
            }
            Respond(ResponseType.Ok);
            Dispose();
        }
    }
}
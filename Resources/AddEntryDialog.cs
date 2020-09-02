using System;
using System.Linq;
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
        
        public AddEntryDialog() : this(new Builder("AddEntryWindow.glade"))
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
            if (!Rating.All(char.IsDigit))
            {
                var messageDialog = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error,
                    ButtonsType.Ok, "Format must be a digit.");
                messageDialog.Run();
                messageDialog.Dispose();
                //todo check here if larger than 10 etc
                return;
            }
            Respond(ResponseType.Ok);
            Dispose();
        }
    }
}
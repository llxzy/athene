using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace athene.Resources
{
    public class AddEntryWindow : Window
    {
        [UI] private Button _addButton = null;
        [UI] private Button _cancelButton = null;
        [UI] private Entry _ratingEntry = null;
        [UI] private Entry _authorEntry = null;
        [UI] private Entry _titleEntry = null;
        
        public AddEntryWindow() : this(new Builder("AddEntryWindow.glade"))
        {
        }

        private AddEntryWindow(Builder builder) : base(builder.GetObject("AddEntry").Handle)
        {
            
        }
    }
}
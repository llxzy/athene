using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace athene.Resources
{
    public class ApplicationMainWindow : Window
    {
        [UI] private Button _addButton = null;
        [UI] private ComboBox _yearComboBox = null;
        [UI] private TreeView _treeView = null;

        public ApplicationMainWindow() : this(new Builder("ApplicationMainWindow.glade"))
        {
        }

        private ApplicationMainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            
        }

    }
}
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
            builder.Autoconnect(this);
            ColumnSetup();
            ReloadDatabase();
            _addButton.Clicked += AddClickedEvent;

        }

        private static async void DatabaseAdd(Entry e)
        {
            await using (var db = new DatabaseContext())
            {
                await db.Entries.AddAsync(e);
                await db.SaveChangesAsync();
            }
        }

        private void AddClickedEvent(object sender, EventArgs e)
        {
            using (var window = new AddEntryDialog())
            {
                var dialogRun = window.Run();
                if ((ResponseType) dialogRun == ResponseType.No)
                {
                    return;
                }
                var entry = new Entry(window.Title, window.Author)
                {
                    Score = int.Parse(window.Rating)
                }; //todo tryparse
                if (entry.Score > 10 || entry.Score < 0)
                {
                    entry.Score = 0;
                }
                DatabaseAdd(entry);
            }
            ReloadDatabase();
        }

        private void ColumnSetup()
        {
            var titleColumn = new TreeViewColumn { Title ="Title" };
            var authorColumn = new TreeViewColumn { Title ="Author" };
            var ratingColumn = new TreeViewColumn { Title ="Rating" };
            
            var tc = new CellRendererText();
            titleColumn.PackStart(tc, true);
            var ac = new CellRendererText();
            authorColumn.PackStart(ac, true);
            var rc = new CellRendererText();
            ratingColumn.PackStart(rc, true);
            
            titleColumn.AddAttribute(tc, "text", 0);
            authorColumn.AddAttribute(ac, "text", 1);
            ratingColumn.AddAttribute(rc, "text", 2);
            _treeView.AppendColumn(titleColumn);
            _treeView.AppendColumn(authorColumn);
            _treeView.AppendColumn(ratingColumn);
        }

        private async void ReloadDatabase()
        {
            await using (var db = new DatabaseContext())
            {
                var entries = db.Entries;
                var listStore = new ListStore(typeof(string), typeof(string), typeof(string));
                foreach (var entry in entries)
                {
                    listStore.AppendValues(entry.Title, entry.Author, entry.Score + "/10");
                }

                _treeView.Model = listStore;

            }
        }

    }
}
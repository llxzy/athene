using System;
using System.Linq;
using athene.Utils;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace athene.Resources
{
    public class ApplicationMainWindow : Window
    {
        [UI] private readonly Button        _addButton    = null;
        [UI] private readonly ComboBox      _yearComboBox = null;
        [UI] private readonly TreeView      _treeView     = null;
        [UI] private readonly ImageMenuItem _newItem      = null;
        [UI] private readonly ImageMenuItem _quitButton   = null;
        [UI] private readonly ImageMenuItem _helpButton   = null;
        
        public ApplicationMainWindow() : this(new Builder("ApplicationMainWindow.glade"))
        {
        }

        private ApplicationMainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            builder.Autoconnect(this);
            ColumnSetup();
            ReloadDatabase(null);
            LoadYears();
            _yearComboBox.Changed        += YearChangedEvent;
            _addButton.Clicked           += AddClickedEvent;
            _newItem.ButtonPressEvent    += AddClickedEvent;
            _helpButton.ButtonPressEvent += HelpClickedEvent;
            _quitButton.ButtonPressEvent += QuitClickedEvent;
        }

        private void ColumnSetup()
        {
            var titleColumn  = new TreeViewColumn { Title ="Title"   };
            var authorColumn = new TreeViewColumn { Title ="Author"  };
            var ratingColumn = new TreeViewColumn { Title ="Rating"  };
            var formatColumn = new TreeViewColumn { Title = "Format" };
            
            var tc = new CellRendererText();
            titleColumn.PackStart(tc, true);
            var ac = new CellRendererText();
            authorColumn.PackStart(ac, true);
            var rc = new CellRendererText();
            ratingColumn.PackStart(rc, true);
            var fc = new CellRendererText();
            formatColumn.PackStart(fc, true);
            
            titleColumn.AddAttribute( tc, "text", 0);
            authorColumn.AddAttribute(ac, "text", 1);
            ratingColumn.AddAttribute(rc, "text", 2);
            formatColumn.AddAttribute(fc, "text", 3);
            _treeView.AppendColumn(titleColumn);
            _treeView.AppendColumn(authorColumn);
            _treeView.AppendColumn(ratingColumn);
            _treeView.AppendColumn(formatColumn);
        }

        private static async void DatabaseAdd(Entry e)
        {
            await using (var db = new DatabaseContext())
            {
                await db.Entries.AddAsync(e);
                await db.SaveChangesAsync();
            }
        }

        #region Events
        private void AddClickedEvent(object sender, EventArgs e)
        {
            using (var window = new AddEntryDialog())
            {
                var dialogRun = window.Run();
                if ((ResponseType) dialogRun != ResponseType.Ok 
                    || window.Title.Length == 0 
                    || window.Author.Length == 0
                    || window.Rating.Length == 0
                    )
                {
                    return;
                }
                var entry = new Entry(window.Title, window.Author)
                {
                    Score = int.Parse(window.Rating),
                    Format = StringToFormat.Convert(window.Format)
                }; //todo tryparse
                if (entry.Score > 10 || entry.Score < 0)
                {
                    entry.Score = 0;
                }
                DatabaseAdd(entry);
            }
            ReloadDatabase(DateTime.Now.Year);
        }
        
        private void YearChangedEvent(object sender, EventArgs e)
        {
            _yearComboBox.GetActiveIter(out var iterator);
            var sel = (string) _yearComboBox.Model.GetValue(iterator, 0);
            ReloadDatabase(int.Parse(sel));
        }
        
        private static void HelpClickedEvent(object o, ButtonPressEventArgs args)
        {
            //todo Close button not working fix
            var dialog = new AboutDialog
            {
                Authors = new[] { "lxzy" },
                License = "MIT License",
                Website = "https://github.com/llxzy",
                Comments = "Made for fun."
            };
            dialog.Show();
        }

        private static void QuitClickedEvent(object o, ButtonPressEventArgs args)
        {
            Application.Quit();
        }
        #endregion
        
        #region Loading
        private async void ReloadDatabase(int? year)
        {
            await using (var db = new DatabaseContext())
            {
                var entries = year == null ? db.Entries : db.Entries.Where(e => e.Year == year);
                var listStore = new ListStore(
                    typeof(string), 
                    typeof(string), 
                    typeof(string),
                    typeof(string)
                    );
                foreach (var entry in entries)
                {
                    listStore.AppendValues(entry.Title, 
                                           entry.Author, 
                                           entry.Score + "/10", 
                                           entry.Format.ToString());
                }
                _treeView.Model = listStore;
                LoadYears();
            }
        }
      
        
        private void LoadYears()
        {
            var yearListStore = new ListStore(typeof(string));
            using (var db = new DatabaseContext())
            {
                var years = db.Entries
                    .Select(a => a.Year)
                    .Distinct()
                    .Select(a => a.ToString())
                    .ToList();
                foreach (var y in years)
                {
                    yearListStore.AppendValues(y);
                }
            }
            _yearComboBox.Model = yearListStore;
        }
        #endregion
    }
}
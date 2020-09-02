using System;
using System.Linq;
using athene.Resources;
using Gtk;

namespace athene
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.athene.athene", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            //ClearDB();
            //TODO Scrolling in treeview
            
            var win = new ApplicationMainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }

        private static void ClearDB()
        {
            using (var db = new DatabaseContext())
            {
                foreach (var e in db.Entries)
                {
                    db.Entries.Remove(e);
                }
                db.SaveChanges();
            }
        }
    }
}

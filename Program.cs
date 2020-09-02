using System;
using System.Diagnostics.CodeAnalysis;
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

            //ClearDb();
            //TODO Scrolling in treeview
            
            var win = new ApplicationMainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private static async void ClearDb()
        {
            //PURELY DEBUG METHOD
            await using (var db = new DatabaseContext())
            {
                foreach (var e in db.Entries)
                {
                    db.Entries.Remove(e);
                }
                await db.SaveChangesAsync();
            }
        }
    }
}

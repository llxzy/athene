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

            /*using (var db = new DatabaseContext())
            {
                db.Entries.Add(new Entry("e", "x"));
                db.SaveChanges();
            }*/
            //ClearDB();
            //TODO Scrolling in treeview
            
            //var win = new MainWindow();
            var win = new ApplicationMainWindow();
            //var win = new AddEntryWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
            /*var entry = new Entry("aaa", "testauthor");
            Console.WriteLine($"{entry.Title} by {entry.Author}");
            using (var ctx = new DatabaseContext())
            {
                Console.WriteLine(ctx.Entries.Count());
                ctx.Entries.Add(entry);
                ctx.SaveChanges();
            }*/
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

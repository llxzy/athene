using Gtk;

namespace athene.Utils
{
    public static class MessageDialogDisplayer
    {
        public static void Show(Window sender, string msg)
        {
            var messageDialog = new MessageDialog(sender, 
                DialogFlags.DestroyWithParent, 
                MessageType.Error,
                ButtonsType.Ok, 
                msg);
            messageDialog.Run();
            messageDialog.Dispose();
        }
    }
}
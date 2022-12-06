using System;
using System.Threading;
using System.Windows.Forms;

namespace StaticWorkspace
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            bool result;
            var mutex = new Mutex(true, "CSaratakij/StaticWorkspace", out result);

            if (!result)
            {
                MessageBox.Show("Another instance is already running!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            GC.KeepAlive(mutex);
        }
    }
}

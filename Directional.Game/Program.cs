using System;
using System.Windows.Forms;

namespace Directional.Game
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var form = new Form1())
            {
                Application.Run(form);
            }
        }
    }
}
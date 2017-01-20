using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using br.chatp2p.Forms;

namespace br.chatp2p
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormLogin f = new FormLogin();
            f.Show();
            Application.Run();
        }
    }
}

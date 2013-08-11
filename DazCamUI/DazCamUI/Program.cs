using System;
using System.IO;
using System.Windows.Forms;

namespace DazCamUI
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

            InitializeController();

            Application.Run(new Positions());
        }

        private static void InitializeController()
        {
            // Save all Machine/Controller Settings in C:\Users\[User]\Documents\DazCAM - Create if it doesn't exist
            string settingsFolder = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            settingsFolder += "\\DazCAM";
            if (!Directory.Exists(settingsFolder)) Directory.CreateDirectory(settingsFolder);

            string settingsFile = settingsFolder + "\\Controller Settings.xml";
            Controller.Machine.Initialize(settingsFile);
        }
    }
}

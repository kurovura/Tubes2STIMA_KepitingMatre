using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Windows.Forms.AxHost;

namespace TreasureHunt
{
        class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
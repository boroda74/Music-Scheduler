using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TurnMonitorOn
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);

        static void TurnMonitorOn()
        {
            const int MOUSEEVENTF_MOVE = 0x0001;

            mouse_event(MOUSEEVENTF_MOVE, 0, 1, 0, UIntPtr.Zero);
        }

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TurnMonitorOn();
        }
    }
}

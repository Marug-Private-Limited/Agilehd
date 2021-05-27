using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

public class IHelper
{
    private const int SW_RESTORE = 9;
    private static Mutex mutex;

    [DllImport("user32.dll")]
    private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern int SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int IsIconic(IntPtr hWnd);

    private static IntPtr GetCurrentInstanceWindowHandle()
    {
        IntPtr result = IntPtr.Zero;
        Process currentProcess = Process.GetCurrentProcess();
        Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
        Process[] array = processesByName;
        foreach (Process process in array)
        {
            if (process.Id != currentProcess.Id && process.MainModule.FileName == currentProcess.MainModule.FileName && process.MainWindowHandle != IntPtr.Zero)
            {
                result = process.MainWindowHandle;
                break;
            }
        }
        return result;
    }

    public static void SwitchToCurrentInstance()
    {
        IntPtr currentInstanceWindowHandle = GetCurrentInstanceWindowHandle();
        if (currentInstanceWindowHandle != IntPtr.Zero)
        {
            if (IsIconic(currentInstanceWindowHandle) != 0)
            {
                ShowWindow(currentInstanceWindowHandle, 9);
            }
            SetForegroundWindow(currentInstanceWindowHandle);
        }
    }

    public static bool Start(Form frmMain)
    {
        if (IsAlreadyRunning())
        {
            MessageBox.Show("This application is already running",
                        Application.ProductName, MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);

            SwitchToCurrentInstance();
            return false;
        }
        Application.Run(frmMain);
        return true;
    }

    public static bool Run()
    {
        if (IsAlreadyRunning())
        {
            return false;
        }
        return true;
    }

    public static bool IsAlreadyRunning()
    {
        string location = Assembly.GetExecutingAssembly().Location;
        FileSystemInfo fileSystemInfo = new FileInfo(location);
        string name = fileSystemInfo.Name;
        mutex = new Mutex(true, "Global\\" + name, out bool flag);
        if (flag)
        {
            mutex.ReleaseMutex();
        }
        return !flag;
    }
}

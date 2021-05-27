using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Constants
{
    /// Application
    public static string app_title = "Agile HD";

    public static string app_exit = "Do you want to exit?";


    /// Switching confirmation
    public static string switch_to_playlist = "The device configuration will be removed! Do you want to continue?";

    public static string switch_to_live = "The playlist will be cleared! Do you want to continue?";


    /// Live Devices
    public static string[] BlockDeviceList = {
            "Medialooks DXGI/DX11 ScreenCapture",
           "Medialooks WebCapture"
        };


    /// Sense lock cofigurations
    public static byte[] usr_pin = Encoding.ASCII.GetBytes("340D3A6A");

    public static byte[] new_dev_pin = Encoding.ASCII.GetBytes("7E034C6B904B3195CF5C78A6");

    public static string mCode = "MGN=>MEPTRONIX";
}
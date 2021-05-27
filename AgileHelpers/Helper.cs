using Ionic.Zip;
using mep_agh;
using AgileHDWPF;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using MLCHARGENLib;
using MPLATFORMLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

public static class Helper
{
    private static MainWindow _dashboard;
    public static MainWindow Master => _dashboard;

    public static void MasterPage(object obj)
    {
        _dashboard = (MainWindow) obj;
    }

    #region Writer Times
    private static long SecondsInADay = 86400L;

    private static long SecondsInAHour = 3600L;

    private static long SecondsInAMinte = 60L;

    public static TimeSpan SecondsToTimeSpan(long seconds)
    {
        int num = 0;
        if (seconds > SecondsInADay)
        {
            num = (int)Math.Floor((double)seconds / (double)SecondsInADay);
        }
        seconds -= num * SecondsInADay;
        int num2 = 0;
        if (seconds > SecondsInAHour)
        {
            num2 = (int)Math.Floor((double)seconds / (double)SecondsInAHour);
        }
        seconds -= num2 * SecondsInAHour;
        int num3 = 0;
        if (seconds > SecondsInAMinte)
        {
            num3 = (int)Math.Floor((double)seconds / (double)SecondsInAMinte);
        }
        seconds -= num3 * SecondsInAMinte;
        return new TimeSpan(num, num2, num3, (int)seconds);
    }

    public static string FormatTimeSpan(TimeSpan span, bool skipDay)
    {
        if (skipDay)
        {
            return $"{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
        }
        return $"{span.Days:D2}d:{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
    }
    #endregion

    #region Disk Space
    private static double OneKiloBytes = 1024.0;

    private static double OneMegaBytes = OneKiloBytes * OneKiloBytes;

    private static double OneGigaBytes = OneMegaBytes * OneKiloBytes;

    private static double OneTeraBytes = OneGigaBytes * OneKiloBytes;

    public static void GetDiskStrings(string path, out string diskSpaceString, out decimal freeSpacePerc, out long freeSpace)
    {
        DriveInfo driveInfo = new DriveInfo(path.Substring(0, 1));
        freeSpace = driveInfo.AvailableFreeSpace;
        diskSpaceString = FormatDiskSpace(freeSpace) + " of " + FormatDiskSpace(driveInfo.TotalSize);
        freeSpacePerc = 100m * ((decimal)(driveInfo.TotalSize - freeSpace) / (decimal)driveInfo.TotalSize);
    }

    public static string FormatDiskSpace(long bytes)
    {
        double num;
        string str;
        if ((double)bytes > OneTeraBytes)
        {
            num = OneTeraBytes;
            str = "TB";
        }
        else if ((double)bytes > OneGigaBytes)
        {
            num = OneGigaBytes;
            str = "GB";
        }
        else if ((double)bytes > OneMegaBytes)
        {
            num = OneMegaBytes;
            str = "MB";
        }
        else if ((double)bytes > OneKiloBytes)
        {
            num = OneKiloBytes;
            str = "KB";
        }
        else
        {
            num = 1.0;
            str = "B";
        }
        double num2 = Math.Round((double)bytes / num, 2);
        return $"{num2:##.00}" + str;
    }
    #endregion

    #region Color Conversion
    public static Color ColorFromHexString(string eightBytesHexString)
    {
        if (eightBytesHexString.Length != 8)
        {
            return Color.Empty;
        }
        int argb = Convert.ToInt32(eightBytesHexString, 16);
        return Color.FromArgb(argb);
    }
    #endregion

    public static string TitleSwLink
    {
        get
        {
            try { return ConfigurationManager.AppSettings["TSw"].ToString(); }
            catch (Exception ex) { ExceptionLog.Create(ex); }
            return string.Empty;
        }
        set
        {
            try
            {
                Configuration c = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                c.AppSettings.Settings.Add("TSw", value);
                c.Save(ConfigurationSaveMode.Minimal);
            }
            catch (Exception ex) { ExceptionLog.Create(ex); }
        }
    }

    public static string SkeySwLink
    {
        get
        {
            try { return ConfigurationManager.AppSettings["KSK"].ToString(); }
            catch (Exception ex) { ExceptionLog.Create(ex); }
            return string.Empty;
        }
        set
        {
            try
            {
                Configuration c = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                c.AppSettings.Settings.Add("TSw", value);
                c.Save(ConfigurationSaveMode.Minimal);
            }
            catch (Exception ex) { ExceptionLog.Create(ex); }
        }
    }

    public static void Alert(string _msg, MessageBoxIcon boxIcon = MessageBoxIcon.Information)
    {
        MessageBox.Show(_msg, Constants.app_title, MessageBoxButtons.OK, boxIcon);
    }

    #region Dashboard components
    public static CoMLCharGen m_objCharGen;

    public static string[] IsLive = new string[] { "1", "1", "1", "1", "1" };

    public static class ColorCorrection
    {
        public static int ULevel { get; set; }
        public static int UVGain { get; set; }
        public static int VLevel { get; set; }
        public static int YGain { get; set; }
        public static int YLevel { get; set; }

        public static int Black { get; set; }
        public static int Bright { get; set; }
        public static int Color { get; set; }
        public static int Contrast { get; set; }
        public static int White { get; set; }

        public static bool Apply { get; set; }
    }

    public static bool IsSongPlaying = false;

    public static bool IsTrnLoading = false;

    public static bool IsTrnPlaying = false;

    public static short EfxRunning = 0;

    public static bool IsRecording = false;

    //public static string Layer_A_Color_Filter = string.Empty;

    public static DataTable FillMotionFilters()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("FileName");

            DataRow dr = dt.NewRow();
            dr[0] = "-Select-";
            dr[1] = "";
            dt.Rows.Add(dr);

            DirectoryInfo di = new DirectoryInfo(Defines.Asset.motion_filter_path);
            foreach (FileInfo fi in di.GetFiles())
            {
                dr = dt.NewRow();
                dr[0] = fi.Name.Replace(fi.Extension, "");
                dr[1] = fi.FullName;
                dt.Rows.Add(dr);
            }

            return dt;
        }
        catch (Exception ex) { ExceptionLog.Create(ex); }
        return null;
       // if (cbo.Items.Count > 0) cbo.SelectedIndex = 1;
    }

    public static string HexConverter(Color c)
    {
        return c.R.ToString("X2") + c.G.ToString("X2") + c.R.ToString("X2");
    }

    public static void SwapVideoSwitch()
    {
        _dashboard.ResetChannelA();
        int i = Array.IndexOf(Defines.Stream.MediaInput, Defines.strLayerA);
        switch (i)
        {
            case 0: _dashboard.L1.btnA.Background = Defines.Theme.Switch_Selected; break;
            case 1: _dashboard.L2.btnA.Background = Defines.Theme.Switch_Selected; break;
            //case 2: _dashboard.L3.btnA.Background = Defines.Theme.Switch_Selected; break;
            //case 3: _dashboard.L4.btnA.Background = Defines.Theme.Switch_Selected; break;
            //case 4: _dashboard.L5.btnA.Background = Defines.Theme.Switch_Selected; break;
            case 2: _dashboard.M1.btnA.Background = Defines.Theme.Switch_Selected; break;
            case 3: _dashboard.M2.btnA.Background = Defines.Theme.Switch_Selected; break;
            case 4: _dashboard.G1.btnA.Background = Defines.Theme.Switch_Selected; break;
            case 5: _dashboard.G2.btnA.Background = Defines.Theme.Switch_Selected; break;
        }

        _dashboard.ResetChannelB();
        i = Array.IndexOf(Defines.Stream.MediaInput, Defines.strLayerB);
        switch (i)
        {
            case 0: _dashboard.L1.btnB.Background = Defines.Theme.Switch_Selected; break;
            case 1: _dashboard.L2.btnB.Background = Defines.Theme.Switch_Selected; break;
            //case 2: _dashboard.L3.btnB.Background = Defines.Theme.Switch_Selected; break;
            //case 3: _dashboard.L4.btnB.Background = Defines.Theme.Switch_Selected; break;
            //case 4: _dashboard.L5.btnB.Background = Defines.Theme.Switch_Selected; break;
            case 2: _dashboard.M1.btnB.Background = Defines.Theme.Switch_Selected; break;
            case 3: _dashboard.M2.btnB.Background = Defines.Theme.Switch_Selected; break;
            case 4: _dashboard.G1.btnB.Background = Defines.Theme.Switch_Selected; break;
            case 5: _dashboard.G2.btnB.Background = Defines.Theme.Switch_Selected; break;
        }
    }

    public static string isRuning = "0";

    public static string get_run_code = "w10";

    public static string get_stop_code = "0";
    #endregion

    #region Capture Configurations
    //public static void CreateXMLConfig(string config, string[] fields, string[] row, string filepath)
    //{
    //    DataTable dt = new DataTable(config);
    //    foreach (string f in fields)
    //        dt.Columns.Add(f);

    //    DataRow dr = dt.NewRow();
    //    for (int i = 0; i < fields.Length; i++)
    //    {
    //        dr[i] = row[i];
    //    }
    //    dt.Rows.Add(dr);

    //    DataSet ds = new DataSet("ProjectSettings");
    //    ds.Tables.Add(dt);

    //    //if (!File.Exists(filepath)) File.Create(filepath);
    //    ds.WriteXml(filepath);
    //    //File.WriteAllText(filepath, ds.GetXml());
    //}

    //public static DataSet XMLToDataset(string xmlFilePath)
    //{
    //    DataSet dsHelper = new DataSet();
    //    try
    //    {
    //        XmlReader xmlFile;
    //        xmlFile = XmlReader.Create(xmlFilePath, new XmlReaderSettings());
    //        dsHelper.ReadXml(xmlFile);
    //        xmlFile.Close();
    //    }
    //    catch (Exception) { }
    //    return dsHelper;
    //}

    //public static void WriterConfig()
    //{
    //    try
    //    {
    //        if (!File.Exists(Defines.Asset.config_project))
    //        {
    //            string[] fields = new string[] { "MediaFormat", "OutputLocation", "OutputFormat", "FileDuration", "PauseDuration" };
    //            string[] data = new string[] { Defines.Asset.config_writer, Defines.Asset.capture_path, Defines.Asset.config_writer_extension, Defines.Asset.config_writer_max_duration.ToString(), Defines.Asset.config_writer_max_pause_duration.ToString() };

    //            CreateXMLConfig("Capture", fields, data, Defines.Asset.config_project);
    //        }
    //    }
    //    catch { }
    //}

    //public static string WriterMediaFormat
    //{
    //    get
    //    {
    //        string x = Defines.Asset.config_writer;
    //        try
    //        {
    //            DataSet ds = XMLToDataset(Defines.Asset.config_project);
    //            x = ds.Tables["Capture"].Rows[0]["MediaFormat"].ToString();
    //            ds.Dispose();
    //        }
    //        catch { }

    //        return x;
    //    }
    //}

    //public static string WriterOutputLocation
    //{
    //    get
    //    {
    //        string x = Defines.Asset.capture_path;
    //        try
    //        {
    //            DataSet ds = XMLToDataset(Defines.Asset.config_project);
    //            x = ds.Tables["Capture"].Rows[0]["OutputLocation"].ToString();
    //            ds.Dispose();
    //        }
    //        catch { }

    //        return x;
    //    }
    //}

    //public static string WriterOutputFormat
    //{
    //    get
    //    {
    //        string x = Defines.Asset.config_writer_extension;
    //        try
    //        {
    //            DataSet ds = XMLToDataset(Defines.Asset.config_project);
    //            x = ds.Tables["Capture"].Rows[0]["OutputFormat"].ToString();
    //            ds.Dispose();
    //        }
    //        catch { }

    //        return x;
    //    }
    //}

    //public static short WriterFileDuration
    //{
    //    get
    //    {
    //        short x = Defines.Asset.config_writer_max_duration;
    //        try
    //        {
    //            DataSet ds = XMLToDataset(Defines.Asset.config_project);
    //            x = Convert.ToInt16(ds.Tables["Capture"].Rows[0]["FileDuration"].ToString());
    //            ds.Dispose();
    //        }
    //        catch { }

    //        return x;
    //    }
    //}

    //public static short WriterPauseDuration
    //{
    //    get
    //    {
    //        short x = Defines.Asset.config_writer_max_pause_duration;
    //        try
    //        {
    //            DataSet ds = XMLToDataset(Defines.Asset.config_project);
    //            x = Convert.ToInt16(ds.Tables["Capture"].Rows[0]["PauseDuration"].ToString());
    //            ds.Dispose();
    //        }
    //        catch { }

    //        return x;
    //    }
    //}
    #endregion

    public static bool EnableTimeline1 = true;

    public static bool EnableTimeline2 = true;

    public class VideoResolution
    {
        public int Width;

        public int Height;

        public bool bAspectRatio;

        public int DisplayWidth;

        public int DisplayHeight;

        public Size Size;

        public eMVideoFormat VideoFormat;

        public event EventHandler VideoResolutionChanged;

        public VideoResolution()
        {
            DisplayWidth = (Width = 720);
            DisplayHeight = (Height = 576);
            Size = new Size(Width, Height);
            bAspectRatio = false;
        }

        public VideoResolution(eMVideoFormat _MVideoFormat)
        {
            Set(_MVideoFormat);
        }

        public void Set(eMVideoFormat _MVideoFormat)
        {
            int width = Width;
            int height = Height;
            VideoFormat = _MVideoFormat;
            switch (_MVideoFormat)
            {
                case eMVideoFormat.eMVF_PAL:
                case eMVideoFormat.eMVF_PAL_16x9:
                    bAspectRatio = false;
                    DisplayWidth = (Width = 720);
                    DisplayHeight = (Height = 576);
                    if (_MVideoFormat == eMVideoFormat.eMVF_PAL_16x9)
                    {
                        DisplayWidth = 1280;
                        DisplayHeight = 720;
                        bAspectRatio = true;
                    }
                    break;
                case eMVideoFormat.eMVF_NTSC:
                case eMVideoFormat.eMVF_NTSC_16x9:
                    bAspectRatio = false;
                    DisplayWidth = 720;
                    DisplayHeight = 480;
                    Width = 720;
                    Height = 480;
                    if (_MVideoFormat == eMVideoFormat.eMVF_NTSC_16x9)
                    {
                        DisplayWidth = 1280;
                        DisplayHeight = 720;
                        bAspectRatio = true;
                    }
                    break;
                case eMVideoFormat.eMVF_HD720_50p:
                case eMVideoFormat.eMVF_HD720_5994p:
                case eMVideoFormat.eMVF_HD720_60p:
                    bAspectRatio = true;
                    DisplayWidth = (Width = 1280);
                    DisplayHeight = (Height = 720);
                    break;
                default:
                    bAspectRatio = true;
                    DisplayWidth = (Width = 1920);
                    DisplayHeight = (Height = 1080);
                    break;
            }
            Size = new Size(Width, Height);
            if (this.VideoResolutionChanged != null)
            {
                if (width == Width && height == Height)
                {
                    return;
                }
                this.VideoResolutionChanged(this, new EventArgs());
            }
        }
    }

    public static class Crypto
    {
        static string sKey = "Mep-toniX";
        static string p = "mEp-TroniX~2018";
        static string appName = "{CTR-81e2ed5a-cb90-4279-95e6-ad3705bed5af}";
        public const string assets_dirve = @"D:\";
        public const string assets_dir = @"ahd_assets";

        static string destination
        {
            get
            {
                //return Path.Combine(Path.GetTempPath(), appName.Replace("{", "").Replace("}", "").ToUpper());
                return Path.Combine(assets_dirve, appName.Replace("{", "").Replace("}", "").ToUpper());
            }
        }

        static string InstalledPath
        {
            get
            {
                try
                {
                    string Install_Reg_Loc = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";
                    RegistryKey hKey = (Registry.LocalMachine).OpenSubKey(Install_Reg_Loc, false);
                    RegistryKey appKey = hKey.OpenSubKey(appName);

                    if (null != appKey)
                    {
                        return appKey.GetValue("InstallLocation", string.Empty).ToString();
                    }
                }
                catch { }

                return string.Empty;
            }
        }


        static void MoveFiles(string source)
        {
            DirectoryInfo d = new DirectoryInfo(source);
            //string x = source.Replace(destination, Path.Combine(InstalledPath, "Properties"));
            string x = source.Replace(destination, Path.Combine(assets_dirve, assets_dir)).ToLower();
            if (!Directory.Exists(x)) Directory.CreateDirectory(x);

            foreach (FileInfo fi in d.GetFiles())
            {
                try
                {
                    if (!File.Exists(Path.Combine(x, fi.Name)))
                        File.Move(fi.FullName, Path.Combine(x, fi.Name));
                }
                catch { }
            }

            foreach (DirectoryInfo di in d.GetDirectories())
            {
                MoveFiles(di.FullName);
            }
        }

        public static bool Unzip(string sourceFile)
        {
            try
            {
                if (!System.IO.File.Exists(sourceFile)) return false;
                if (Directory.Exists(destination))
                    Directory.Delete(destination, true);
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(destination);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                using (ZipFile z = new ZipFile(sourceFile))
                {
                    z.Password = p;
                    z.ExtractAll(destination);
                    return true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
        }

        public static bool ImportProjects()
        {
            try
            {
                if (!string.IsNullOrEmpty(InstalledPath))
                {
                    MoveFiles(Path.Combine(destination, "projects"));
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static bool ImportVideos()
        {
            try
            {
                MoveFiles(Path.Combine(destination, "videos"));
                return true;
            }
            catch { return false; }
        }

        public static bool ImportImages()
        {
            try
            {
                MoveFiles(Path.Combine(destination, "images"));
                return true;
            }
            catch { return false; }
        }

        public static bool ImportMasks()
        {
            try
            {
                MoveFiles(Path.Combine(destination, "masks"));
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
        }

        public static bool Import3Ds()
        {
            try
            {
                MoveFiles(Path.Combine(destination, "3ds"));
                return true;
            }
            catch { return false; }
        }

        public static bool RemoveTempFiles()
        {
            try
            {
                if (Directory.Exists(destination)) Directory.Delete(destination, true);
                return true;
            }
            catch { }
            return false;
        }


        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        static extern bool ZeroMemory(IntPtr Destination, int Length);


        public static XmlDocument Decrypt(string inputFile)
        {
            XmlDocument xml = new XmlDocument();

            string outputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ahd", "tmp_config.xml");
            FileInfo f = new FileInfo(outputFile);
            if (!Directory.Exists(f.DirectoryName)) Directory.CreateDirectory(f.DirectoryName);
            if (File.Exists(f.FullName)) try { FileSystem.DeleteFile(outputFile, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently); } catch { }

            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(sKey);
            byte[] salt = new byte[32];

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            // if (AES == null) Init_AES();

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);
            FileStream fsOut = new FileStream(outputFile, FileMode.Create);
            try
            {
                int data;
                while ((data = cs.ReadByte()) != -1)
                {
                    fsOut.WriteByte((byte)data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cs.Close();
                fsOut.Close();
                fsCrypt.Close();
            }


            if (File.Exists(f.FullName))
            {
                xml.Load(f.FullName);
                Thread.Sleep(25);
                File.Delete(f.FullName);
            }
            return xml;
        }

        public static XDocument Decrypt2(string inputFile)
        {
            XDocument xd = null;

            string outputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ahd", "tmp_config.xml");
            FileInfo f = new FileInfo(outputFile);
            if (!Directory.Exists(f.DirectoryName)) Directory.CreateDirectory(f.DirectoryName);
            if (File.Exists(f.FullName)) try { FileSystem.DeleteFile(outputFile, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently); } catch { }

            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(sKey);
            byte[] salt = new byte[32];

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);
            FileStream fsOut = new FileStream(outputFile, FileMode.Create);
            try
            {
                int data;
                while ((data = cs.ReadByte()) != -1)
                {
                    fsOut.WriteByte((byte)data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cs.Close();
                fsOut.Close();
                fsCrypt.Close();
            }
            if (File.Exists(f.FullName))
            {
                xd = XDocument.Load(f.FullName);
                Thread.Sleep(50);
                File.Delete(f.FullName);
            }
            return xd;
        }


        public static XmlDocument DecryptFile(string inputFilePath)
        {
            XmlDocument xml = new XmlDocument();

            //string outputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ahd", "p_config.xml");
            string outputFile = Path.Combine(assets_dirve, assets_dir, "_tmp_", "_trs.xml");

            FileInfo f = new FileInfo(outputFile);
            if (!Directory.Exists(f.DirectoryName))
            {
                DirectoryInfo di = Directory.CreateDirectory(f.DirectoryName);
                di.Attributes = FileAttributes.Directory; //| FileAttributes.Hidden;
            }
            if (File.Exists(f.FullName)) try { Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(outputFile, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently); } catch { }

            try
            {
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(sKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                    {
                        using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create))
                            {
                                int data;
                                while ((data = cs.ReadByte()) != -1)
                                {
                                    fsOutput.WriteByte((byte)data);
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            if (File.Exists(f.FullName))
            {
                xml.Load(f.FullName);
                Thread.Sleep(25);
                Directory.Delete(f.Directory.FullName, true);
            }
            return xml;
        }
    }

    #region Capture Settings
    public static string WriterLocation
    {
        get
        {
            try
            {
                XHelper.get_capture_config(out string rt, out string vb, out string vf, out string ab,
                    out string loc, out string extn, out string sps, out string ps);
                return string.IsNullOrEmpty(loc) ? Defines.Asset.capture_path : loc;
            }
            catch (Exception ex) { ExceptionLog.Create(ex); }
            //try { return ConfigurationManager.AppSettings["wLoc"].ToString(); }
            //catch (Exception ex) { ExceptionLog.Create(ex); }
            return Defines.Asset.capture_path;          
        }
        //set
        //{
        //    try
        //    {
        //        Configuration c = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
        //        c.AppSettings.Settings.Add("wLoc", value);
        //        c.Save(ConfigurationSaveMode.Minimal);
        //    }
        //    catch (Exception ex) { ExceptionLog.Create(ex); }
        //}
    }

    public static decimal MaxDuration
    {
        get
        {
            try
            {
                XHelper.get_capture_config(out string rt, out string vb, out string vf, out string ab,
                    out string loc, out string extn, out string sps, out string ps);
                return string.IsNullOrEmpty(loc) ? Defines.Asset.config_writer_max_duration : Convert.ToDecimal(sps)*60; ;
            }
            catch (Exception ex) { ExceptionLog.Create(ex); }

            //try { return ConfigurationManager.AppSettings["wMax"].ToString(); }
            //catch (Exception ex) { ExceptionLog.Create(ex); }
            return Defines.Asset.config_writer_max_duration;
        }
        //set
        //{
        //    try
        //    {
        //        Configuration c = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
        //        c.AppSettings.Settings.Add("wMax", value);
        //        c.Save(ConfigurationSaveMode.Minimal);
        //    }
        //    catch (Exception ex) { ExceptionLog.Create(ex); }
        //}
    }

    public static decimal PauseDuration
    {
        get
        {
            try
            {
                XHelper.get_capture_config(out string rt, out string vb, out string vf, out string ab,
                    out string loc, out string extn, out string sps, out string ps);
                return string.IsNullOrEmpty(loc) ? Defines.Asset.config_writer_max_pause_duration : Convert.ToDecimal(ps);
            }
            catch (Exception ex) { ExceptionLog.Create(ex); }

            //try { return ConfigurationManager.AppSettings["wPause"].ToString(); }
            //catch (Exception ex) { ExceptionLog.Create(ex); }
            return Defines.Asset.config_writer_max_pause_duration;
        }
        //set
        //{
        //    try
        //    {
        //        Configuration c = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
        //        c.AppSettings.Settings.Add("wPause", value);
        //        c.Save(ConfigurationSaveMode.Minimal);
        //    }
        //    catch (Exception ex) { ExceptionLog.Create(ex); }
        //}
    }
    #endregion
}

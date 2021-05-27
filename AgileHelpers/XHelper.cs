using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

public static class XHelper
{
    #region Capture Settings
    public static bool set_media_config(string _recType, string _vBit, string _vFormat)
    {
        if (File.Exists(Defines.Asset.config_media_clips))
        {
            try { File.Delete(Defines.Asset.config_media_clips); } catch { }
        }

        try
        {
            XmlDocument doc = new XmlDocument();

            /// Create new config file.
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode root = doc.CreateElement("media_clips");
            doc.AppendChild(root);

            XmlAttribute attr = doc.CreateAttribute("rec_type");
            attr.Value = _recType;
            root.Attributes.Append(attr);

            attr = doc.CreateAttribute("video_bit");
            attr.Value = _vBit;
            root.Attributes.Append(attr);

            attr = doc.CreateAttribute("video_format");
            attr.Value = _vFormat;
            root.Attributes.Append(attr);

            doc.Save(Defines.Asset.config_media_clips);
            return true;
        }
        catch { }
        return false;
    }

    public static void get_media_config(out string _recType, out string _vBit, out string _vFormat)
    {
        string rt = "1";
        string vb = "10M";
        string vf = "HD-1080i50";

        try
        {
            if (File.Exists(Defines.Asset.config_media_clips))
            {
                XDocument xd = XDocument.Load(Defines.Asset.config_media_clips);
                XElement s = xd.Element("media_clips");
                if (s.Attribute("rec_type") != null) rt = s.Attribute("rec_type").Value;
                if (s.Attribute("video_bit") != null) vb = s.Attribute("video_bit").Value;
                if (s.Attribute("video_format") != null) vf = s.Attribute("video_format").Value;
            }
        }
        catch { }

        _recType = rt;
        _vBit = vb;
        _vFormat = vf;
    }

    public static bool set_capture_config(string _recType, string _vBit, string _vFormat, string _aBit,
        string _loc, string _exten, string _splitSec, string _pauseSec)
    {

        if (File.Exists(Defines.Asset.config_capture))
        {
            try { File.Delete(Defines.Asset.config_capture); } catch { }
        }

        try
        {
            XmlDocument doc = new XmlDocument();

            /// Create new config file.
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode root = doc.CreateElement("capture");
            doc.AppendChild(root);

            XmlAttribute attr = doc.CreateAttribute("rec_type");
            attr.Value = _recType;
            root.Attributes.Append(attr);

            attr = doc.CreateAttribute("video_bit");
            attr.Value = _vBit;
            root.Attributes.Append(attr);

            attr = doc.CreateAttribute("video_format");
            attr.Value = _vFormat;
            root.Attributes.Append(attr);

            attr = doc.CreateAttribute("audio_bit");
            attr.Value = _aBit;
            root.Attributes.Append(attr);

            attr = doc.CreateAttribute("location");
            attr.Value = _loc;
            root.Attributes.Append(attr);

            attr = doc.CreateAttribute("file_format");
            attr.Value = _exten;
            root.Attributes.Append(attr);

            attr = doc.CreateAttribute("split_duration");
            attr.Value = _splitSec;
            root.Attributes.Append(attr);

            attr = doc.CreateAttribute("pause_duration");
            attr.Value = _pauseSec;
            root.Attributes.Append(attr);

            doc.Save(Defines.Asset.config_capture);
            return true;
        }
        catch { }

        return false;
    }

    public static void get_capture_config(out string _recType, out string _vBit, out string _vFormat, out string _aBit,
        out string _loc, out string _exten, out string _splitSec, out string _pauseSec)
    {
        string rt = "1";
        string vb = "10M";
        string vf = "HD-1080i50";
        string ab = "128K";

        string lc = Defines.Asset.capture_path;
        string en = "MPEG4_Part_2";
        string ss = Defines.Asset.config_writer_max_duration.ToString();
        string ps = Defines.Asset.config_writer_max_pause_duration.ToString();

        try
        {
            if (File.Exists(Defines.Asset.config_capture))
            {
                XDocument xd = XDocument.Load(Defines.Asset.config_capture);
                XElement s = xd.Element("capture");
                if (s.Attribute("rec_type") != null) rt = s.Attribute("rec_type").Value;
                if (s.Attribute("video_bit") != null) vb = s.Attribute("video_bit").Value;
                if (s.Attribute("video_format") != null) vf = s.Attribute("video_format").Value;
                if (s.Attribute("audio_bit") != null) ab = s.Attribute("audio_bit").Value;

                if (s.Attribute("location") != null) lc = s.Attribute("location").Value;
                if (s.Attribute("file_format") != null) en = s.Attribute("file_format").Value;
                if (s.Attribute("split_duration") != null) ss = s.Attribute("split_duration").Value;
                if (s.Attribute("pause_duration") != null) ps = s.Attribute("pause_duration").Value;
            }
        }
        catch { }

        _recType = rt;
        _vBit = vb;
        _vFormat = vf;
        _aBit = ab;

        _loc = lc;
        _exten = en;
        _splitSec = ss;
        _pauseSec = ps;
    }
    #endregion

    #region Camera Settings
    static XmlDocument GetDocument()
    {
        XmlDocument doc = new XmlDocument();

        if (!File.Exists(Defines.Asset.config_input_media))
        {
            /// Create new config file.
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            doc.AppendChild(docNode);

            XmlNode root = doc.CreateElement("Devices");
            doc.AppendChild(root);

            XmlNode dev;
            XmlAttribute attr;
            int i = 0;
            while (i < 2)
            {
                dev = doc.CreateElement("Device");
                root.AppendChild(dev);

                attr = doc.CreateAttribute("Id");
                attr.Value = (i == 0) ? Defines.Stream.strLive1 : Defines.Stream.strLive2;
                dev.Attributes.Append(attr);

                attr = doc.CreateAttribute("Name");
                attr.Value = "";
                dev.Attributes.Append(attr);

                attr = doc.CreateAttribute("Active");
                attr.Value = (i == 0) ? "1" : "0";
                dev.Attributes.Append(attr);

                attr = doc.CreateAttribute("Line");
                attr.Value = "";
                dev.Attributes.Append(attr);

                attr = doc.CreateAttribute("VFormat");
                attr.Value = "";
                dev.Attributes.Append(attr);

                attr = doc.CreateAttribute("Audio");
                attr.Value = "";
                dev.Attributes.Append(attr);

                attr = doc.CreateAttribute("External");
                attr.Value = "";
                dev.Attributes.Append(attr);

                attr = doc.CreateAttribute("AFormat");
                attr.Value = "";
                dev.Attributes.Append(attr);

                i++;
            }

            doc.Save(Defines.Asset.config_input_media);
        }

        doc.Load(Defines.Asset.config_input_media);
        return doc;
    }

    static string getValidString(string x)
    {
        return x.Replace("&lt", "<").Replace("&gt", ">");
    }

    public static bool set_input_config(string _title, string _mode, string _device, string _line, string _vFormat, string _audio, string _external, string _aformat)
    {
        XmlDocument doc = GetDocument();
        try
        {
            XmlNodeList xnList = doc.SelectNodes(string.Format(@"/Devices/Device[@Id='{0}']", _title));
            foreach (XmlNode xn in xnList)
            {
                xn.Attributes["Name"].Value = getValidString(_device);
                xn.Attributes["Active"].Value = getValidString(_mode);
                xn.Attributes["Line"].Value = getValidString(_line);
                xn.Attributes["VFormat"].Value = getValidString(_vFormat);
                xn.Attributes["Audio"].Value = getValidString(_audio);
                xn.Attributes["External"].Value = getValidString(_external);
                xn.Attributes["AFormat"].Value = getValidString(_aformat);
            }

            doc.Save(Defines.Asset.config_input_media);
            return true;
        }
        catch { }
        return false;
    }

    public static void get_input_config(string _title, out string _mode, out string _device, out string _line, out string _vFormat, out string _audio, out string _external, out string _aformat)
    {
        _device = _line = _vFormat = _audio = _external = _aformat = "";
        _mode = "1";

        XmlDocument doc = GetDocument();
        try
        {
            XmlNodeList xnList = doc.SelectNodes(string.Format(@"/Devices/Device[@Id='{0}']", _title));
            foreach (XmlNode xn in xnList)
            {
                if (xn.Attributes["Active"] != null) _mode = xn.Attributes["Active"].Value;
                if (xn.Attributes["Name"] != null) _device = xn.Attributes["Name"].Value;
                if (xn.Attributes["Line"] != null) _line = xn.Attributes["Line"].Value;
                if (xn.Attributes["VFormat"] != null) _vFormat = xn.Attributes["VFormat"].Value;

                if (xn.Attributes["Audio"] != null) _audio = xn.Attributes["Audio"].Value;
                if (xn.Attributes["External"] != null) _external = xn.Attributes["External"].Value;
                if (xn.Attributes["AFormat"] != null) _aformat = xn.Attributes["AFormat"].Value;
            }
        }
        catch { }
    }

    public static string get_input_mode(string _title)
    {
        XmlDocument doc = GetDocument();
        try
        {
            XmlNodeList xnList = doc.SelectNodes(string.Format(@"/Devices/Device[@Id='{0}']", _title));
            foreach (XmlNode xn in xnList)
            {
                if (xn.Attributes["Active"] != null) return xn.Attributes["Active"].Value;
            }
        }
        catch { }

        return "1";
    }

    public static bool set_input_mode(string _title, string _live = "1")
    {
        XmlDocument doc = GetDocument();
        try
        {
            XmlNodeList xnList = doc.SelectNodes(string.Format(@"/Devices/Device[@Id='{0}']", _title));
            foreach (XmlNode xn in xnList)
            {
                xn.Attributes["Active"].Value = string.IsNullOrEmpty(_live) ? "1" : _live;
            }

            doc.Save(Defines.Asset.config_input_media);
            return true;
        }
        catch { }

        return false;
    }
    #endregion
}

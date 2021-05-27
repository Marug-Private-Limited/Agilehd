using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AgileHDWPF.AgileSng
{
    public class SHelper
    {
        static XmlDocument xd;
        static string sKey = "Mep-toniX";

        public static List<Stream> MixerStreams = new List<Stream>();
        static string _streamId, _path, _cmd;

        public static decimal mTranstionDuration = 0;
        public static short mAutoReverse = 0;
        public static short mTransitionFixDuration = 0;
        public static short mTransitionSwapSource = 0;

        public static string mTransitionObjects;
        public static string mTransitionItems;
        public static bool IsLoaded = false;

        public static void LoadFile(string _file)
        {
            IsLoaded = false;
            mTranstionDuration = 0;
            mTransitionFixDuration = 0;
            mTransitionSwapSource = 0;
            mAutoReverse = 0;
            mTransitionObjects = string.Empty;
            mTransitionItems = string.Empty;

            if (string.IsNullOrEmpty(_file)) return;

            /// Decrypt project file.
            xd = DecryptFile(_file);

            /// Mixer Streams.
            XmlNode xn = xd.GetElementsByTagName(AppData.Tag.mixer)[0];
            if (xn != null)
            {
                foreach (XmlNode x in xn.ChildNodes)
                {
                    if (x != null && x.Name == AppData.Tag.mixer_file)
                    {
                        _streamId = (x.Attributes[AppData.Tag.mixer_stream_id] != null) ? x.Attributes[AppData.Tag.mixer_stream_id].Value.ToLower() : "";
                        _path = (x.Attributes[AppData.Tag.mixer_path] != null) ? ReplaceTo(x.Attributes[AppData.Tag.mixer_path].Value, 0) : "";
                        _cmd = (x.Attributes[AppData.Tag.mixer_cmd] != null) ? x.Attributes[AppData.Tag.mixer_cmd].Value : ((x.Attributes[AppData.Tag.mixer_loop] != null) ? (x.Attributes[AppData.Tag.mixer_loop].Value.ToLower() == "true" ? "1" : "0") : "0");

                        /// Replace stream LA & LB.
                        if (_streamId == "la") _streamId = AppData.Song.Stream.SLA;
                        else if (_streamId == "lb") _streamId = AppData.Song.Stream.SLB;

                        /// Check and replace media gallery videos.
                        switch(_streamId)
                        {
                            case "b1": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_boy1);  break;
                            case "b2": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_boy2); break;
                            case "b3": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_boy3); break;
                            case "b4": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_boy4); break;
                            case "b5": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_boy5); break;

                            case "c1": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_couple1); break;
                            case "c2": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_couple2); break;
                            case "c3": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_couple3); break;
                            case "c4": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_couple4); break;
                            case "c5": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_couple5); break;

                            case "g1": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_girl1); break;
                            case "g2": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_girl2); break;
                            case "g3": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_girl3); break;
                            case "g4": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_girl4); break;
                            case "g5": _path = string.Format(@"{0}\{1}.mp4", AppData.Asset.media_clips_path, AppData.Asset.media_girl5); break;
                            default:
                                FileInfo fi = new FileInfo(_path);
                                if (fi.Extension.ToLower() == ".png" || fi.Extension.ToLower() == ".jpg")
                                {
                                    if (fi.Name != AppData.Stills.B1 && fi.Name != AppData.Stills.B2 && fi.Name != AppData.Stills.B3 && fi.Name != AppData.Stills.B4 && fi.Name != AppData.Stills.B5 &&
                                  fi.Name != AppData.Stills.C1 && fi.Name != AppData.Stills.C2 && fi.Name != AppData.Stills.C3 && fi.Name != AppData.Stills.C4 && fi.Name != AppData.Stills.C5 &&
                                  fi.Name != AppData.Stills.G1 && fi.Name != AppData.Stills.G2 && fi.Name != AppData.Stills.G3 && fi.Name != AppData.Stills.G4 && fi.Name != AppData.Stills.G5)
                                    {

                                    }
                                    else
                                    {
                                        _path = string.Format(@"{0}\{1}", AppData.Asset.media_stills_path, fi.Name);
                                    }
                                }
                                //string y = fi.Name;
                                break;
                        }

                        MixerStreams.Add(new Stream(_streamId, _path, _cmd));
                    }
                }
            }

            /// Transition Header
            xn = xd.GetElementsByTagName(AppData.Tag.scene_3d_group)[0];
            if (xn != null)
            {
                string _value;
                if (xn.Attributes[AppData.Tag.id] != null && xn.Attributes[AppData.Tag.id].Value == AppData.Tag.scene_3d_trn_transition)
                {
                    _value = (xn.Attributes[AppData.Tag.scene_3d_trn_duration] != null) ? xn.Attributes[AppData.Tag.scene_3d_trn_duration].Value : "3";
                    mTranstionDuration = (string.IsNullOrEmpty(_value)) ? 3m : Convert.ToDecimal(_value);

                    _value = (xn.Attributes[AppData.Tag.scene_3d_trn_fixed] != null) ? xn.Attributes[AppData.Tag.scene_3d_trn_fixed].Value : "0";
                    if (!string.IsNullOrEmpty(_value)) { mTransitionFixDuration = Convert.ToInt16(_value); }

                    _value = (xn.Attributes[AppData.Tag.scene_3d_trn_reverse] != null) ? xn.Attributes[AppData.Tag.scene_3d_trn_reverse].Value : "0";
                    if (!string.IsNullOrEmpty(_value)) { mAutoReverse = Convert.ToInt16(_value); }

                    _value = (xn.Attributes[AppData.Tag.scene_3d_trn_swap] != null) ? xn.Attributes[AppData.Tag.scene_3d_trn_swap].Value : "0";
                    if (!string.IsNullOrEmpty(_value)) { mTransitionSwapSource = Convert.ToInt16(_value); }
                }
            }

            /// Transition Items.
            xn = xd.GetElementsByTagName(AppData.Tag.scene_3d_group)[1];
            if (xn != null)
            {
                ReplaceAttribute(xn);
                mTransitionObjects = xn.OuterXml;
            }

            /// Update transition view elements.
            xn = xd.GetElementsByTagName(AppData.Tag.view)[0];
            if (xn != null) { mTransitionItems = xn.OuterXml; }

            MHelper.CreateStreams(MixerStreams);
            MHelper.AppendTransition(mTransitionObjects);
            MHelper.AppendTransitionView(mTransitionItems);
            try { MHelper.mElementTree.UpdateTree(false); } catch { }

            IsLoaded = true;
        }


        static XmlDocument DecryptFile(string inputFilePath)
        {
            XmlDocument xml = new XmlDocument();
            string outputFile = Path.Combine(AppData.Asset.root, "_tmp_", "_trs.xml");

            FileInfo f = new FileInfo(outputFile);
            if (!Directory.Exists(f.DirectoryName))
            {
                DirectoryInfo di = Directory.CreateDirectory(f.DirectoryName);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            if (File.Exists(f.FullName))
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(outputFile, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.DeletePermanently);
                }
                catch { }

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
                Thread.Sleep(20);
                Directory.Delete(f.Directory.FullName, true);
            }
            return xml;
        }

        static string ReplaceTo(string x, int y)
        {
            try { if (File.Exists(x)) return x; } catch { }

            FileInfo fi = new FileInfo(x);

            //if (y == 1 && fi.Name != AppData.Stills.B1 && fi.Name != AppData.Stills.B2 && fi.Name != AppData.Stills.B3 && fi.Name != AppData.Stills.B4 && fi.Name != AppData.Stills.B5 &&
            //    fi.Name != AppData.Stills.C1 && fi.Name != AppData.Stills.C2 && fi.Name != AppData.Stills.C3 && fi.Name != AppData.Stills.C4 && fi.Name != AppData.Stills.C5 &&
            //    fi.Name != AppData.Stills.G1 && fi.Name != AppData.Stills.G2 && fi.Name != AppData.Stills.G3 && fi.Name != AppData.Stills.G4 && fi.Name != AppData.Stills.G5)
            //{
            //    return string.Format(@"{0}\{1}", AppData.Asset.media_stills_path, fi.Name);
            //}
            //else
            //{
                DirectoryInfo di = new DirectoryInfo(fi.DirectoryName);
                return string.Format(@"{0}\{1}", (y == 0) ? AppData.Asset.video_path : (y == 1) ? AppData.Asset.image_path : (y == 2) ? AppData.Asset.mask_path : (y == 3) ? (AppData.Asset.object_path + "\\" + di.Name) : "../", fi.Name);
            //}
        }

        //string Replace3DsUrl(string x)
        //{
        //    string[] y = x.Split('\\');
        //    string z = string.Format(@"{0}\{1}\{2}", Constants.PROPERTY_EFFECTS.Replace("projects", "3ds"), y[y.Length - 2], y[y.Length - 1]);
        //    if (!File.Exists(z)) z = string.Format(@"{0}\{1}", Constants.PROPERTY_EFFECTS.Replace("projects", "3ds"), y[y.Length - 1]);
        //    return z;
        //}

        static void ReplaceAttribute(XmlNode xn)
        {
            if (xn.HasChildNodes)
            {
                foreach (XmlNode x in xn.ChildNodes)
                {
                    ReplaceUrl(x);
                    if (x.HasChildNodes)
                        ReplaceAttribute(x);
                }
            }
            else
            {
                ReplaceUrl(xn);
            }
        }

        static void ReplaceUrl(XmlNode x)
        {
            string s = string.Empty;
            //string m = x.Name;
            //try { m = m.ToLower(); } catch { }
            switch (x.Name.ToLower())
            {
                case AppData.Tag.scene_3d_video:
                    if (x.Attributes[AppData.Tag.mixer_stream_id] != null)
                    {
                        s = x.Attributes[AppData.Tag.mixer_stream_id].Value;

                        if (string.Compare(s, "LA") == 0) x.Attributes[AppData.Tag.mixer_stream_id].Value = AppData.Song.Stream.SLA;
                        else if (string.Compare(s, "LB") == 0) x.Attributes[AppData.Tag.mixer_stream_id].Value = AppData.Song.Stream.SLB;
                    }

                    if (x.Attributes[AppData.Tag.mixer_stream_idx] != null) x.Attributes.Remove(x.Attributes[AppData.Tag.mixer_stream_idx]);

                    if (x.Attributes[AppData.Tag.scene_3d_mask] != null)
                    {
                        s = x.Attributes[AppData.Tag.scene_3d_mask].Value;
                        x.Attributes[AppData.Tag.scene_3d_mask].Value = s.IndexOf(@"\") == -1 ? s : ReplaceTo(x.Attributes[AppData.Tag.scene_3d_mask].Value, 2);
                    }
                    break;

                case AppData.Tag.scene_3d_image:
                    if (x.Attributes[AppData.Tag.scene_3d_image_url] != null)
                    {
                        x.Attributes[AppData.Tag.scene_3d_image_url].Value = ReplaceTo(x.Attributes[AppData.Tag.scene_3d_image_url].Value, 1);
                    }

                    if (x.Attributes[AppData.Tag.scene_3d_mask] != null)
                    {
                        s = x.Attributes[AppData.Tag.scene_3d_mask].Value;
                        x.Attributes[AppData.Tag.scene_3d_mask].Value = s.IndexOf(@"/") == -1 ? s : ReplaceTo(x.Attributes[AppData.Tag.scene_3d_mask].Value, 2);
                    }
                    break;

                case AppData.Tag.scene_3d_object:
                    if (x.Attributes[AppData.Tag.scene_3d_image_url] != null) { x.Attributes[AppData.Tag.scene_3d_image_url].Value = ReplaceTo(x.Attributes[AppData.Tag.scene_3d_image_url].Value, 3); }

                    //if (x.Attributes[AppData.Tag.obj_layer_a] != null) { x.Attributes[AppData.Tag.obj_layer_a].Value = AppData.Song.Stream.SLA; }
                    //if (x.Attributes[AppData.Tag.obj_layer_b] != null) { x.Attributes[AppData.Tag.obj_layer_b].Value = AppData.Song.Stream.SLB; }

                    //if (x.Attributes[AppData.Tag.obj_layer_d1] != null) { x.Attributes[AppData.Tag.obj_layer_d1].Value = AppData.Stream.strDDR1; }
                    //if (x.Attributes[AppData.Tag.obj_layer_d2] != null) { x.Attributes[AppData.Tag.obj_layer_d2].Value = AppData.Stream.strDDR2; }
                    //if (x.Attributes[AppData.Tag.obj_layer_d3] != null) { x.Attributes[AppData.Tag.obj_layer_d3].Value = AppData.Stream.strDDR3; }
                    //if (x.Attributes[AppData.Tag.obj_layer_d4] != null) { x.Attributes[AppData.Tag.obj_layer_d4].Value = AppData.Stream.strDDR4; }
                    //if (x.Attributes[AppData.Tag.obj_layer_d5] != null) { x.Attributes[AppData.Tag.obj_layer_d5].Value = AppData.Stream.strDDR5; }

                    //if (x.Attributes[AppData.Tag.obj_layer_d6] != null) { x.Attributes[AppData.Tag.obj_layer_d6].Value = AppData.Stream.strDDR6; }
                    //if (x.Attributes[AppData.Tag.obj_layer_d7] != null) { x.Attributes[AppData.Tag.obj_layer_d7].Value = AppData.Stream.strDDR7; }
                    //if (x.Attributes[AppData.Tag.obj_layer_d8] != null) { x.Attributes[AppData.Tag.obj_layer_d8].Value = AppData.Stream.strDDR8; }
                    //if (x.Attributes[AppData.Tag.obj_layer_d9] != null) { x.Attributes[AppData.Tag.obj_layer_d9].Value = AppData.Stream.strDDR9; }
                    //if (x.Attributes[AppData.Tag.obj_layer_d10] != null) { x.Attributes[AppData.Tag.obj_layer_d10].Value = AppData.Stream.strDDR10; }
                    break;

                default:
                    if (x.HasChildNodes) ReplaceAttribute(x);
                    break;
            }
        }
        
        public static List<Stream> TrnStreams
        {
            get { return MixerStreams; }
        }

    }
}

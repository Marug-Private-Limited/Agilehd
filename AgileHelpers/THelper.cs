using MPLATFORMLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

public enum EffectType { Mix, DVE }

public class StreamItem
{
    public string StreamId { get; set; }
    public string Path { get; set; }
    public string Cmd { get; set; }

    public StreamItem(string _streamId, string _path, string _cmd)
    {
        StreamId = _streamId;
        Path = _path;
        Cmd = _cmd;
    }
}

public class THelper
{
    private static XmlDocument xd = null;

    public static List<StreamItem> MixerStreams = new List<StreamItem>();

    public static string sceneName = "0000";

    public static bool IsLoaded = false;

    public static decimal mTranstionDuration = 0;

    public static decimal mTotalDuration = 0;

    public static short mAutoReverse = 0;

    public static short mTransitionFixDuration = 0;

    public static short mTransitionSwapSource = 0;

    public static EffectType mEfxType = EffectType.DVE;

    public static string mTransitionObjects;

    public static string mTransitionItems;

    public static List<string> replace_a;
    public static List<string> replace_b;

    public static void Transition_load(XmlDocument _xd)
    {
        MixerStreams = new List<StreamItem>();
        string _streamId, _path, _cmd;

        IsLoaded = false;
        sceneName = "0000";
        mTranstionDuration = 0;
        mTransitionFixDuration = 0;
        mTransitionSwapSource = 0;
        mAutoReverse = 0;

        mTransitionObjects = string.Empty;
        mTransitionItems = string.Empty;

        //xd = new XmlDocument();
        //xd.Load(_filePath);
        xd = _xd;

        /// Mixer Streams.
        XmlNode xn = xd.GetElementsByTagName(Defines.Tag.mixer)[0];
        if (xn != null)
        {
            foreach (XmlNode x in xn.ChildNodes)
            {
                if (x != null && x.Name == Defines.Tag.mixer_file)
                {
                    _streamId = (x.Attributes[Defines.Tag.mixer_stream_id] != null) ? x.Attributes[Defines.Tag.mixer_stream_id].Value.ToLower() : "";
                    _path = (x.Attributes[Defines.Tag.mixer_path] != null) ? x.Attributes[Defines.Tag.mixer_path].Value : "";
                    _cmd = (x.Attributes[Defines.Tag.mixer_cmd] != null) ? x.Attributes[Defines.Tag.mixer_cmd].Value : "2";
                    //((x.Attributes[Defines.Tag.mixer_loop] != null) ? (x.Attributes[Defines.Tag.mixer_loop].Value.ToLower() == "true" ? "1" : "0") : "0");
                    foreach(XmlNode x1 in x.ChildNodes)
                    {
                        if(x1 != null && x1.Name == Defines.Tag.mItem_Props)
                        {
                            _cmd = (x1.Attributes[Defines.Tag.mixer_cmd] != null) ? x1.Attributes[Defines.Tag.mixer_cmd].Value : "2";
                        }
                    }
                    if (_streamId == "la") _streamId = Defines.Stream.strLive1.ToLower();
                    else if (_streamId == "lb") _streamId = Defines.Stream.strLive2.ToLower();
                    else
                    {
                        /// Check and replace media gallery videos.
                        switch (_streamId)
                        {
                            case "b1": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy1); break;
                            case "b2": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy2); break;
                            case "b3": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy3); break;
                            case "b4": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy4); break;
                            case "b5": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy5); break;

                            case "c1": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple1); break;
                            case "c2": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple2); break;
                            case "c3": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple3); break;
                            case "c4": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple4); break;
                            case "c5": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple5); break;

                            case "g1": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl1); break;
                            case "g2": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl2); break;
                            case "g3": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl3); break;
                            case "g4": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl4); break;
                            case "g5": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl5); break;
                            default:
                                FileInfo fi = new FileInfo(_path);
                                if (fi.Extension.ToLower() == ".png" || fi.Extension.ToLower() == ".jpg")
                                {
                                    if (fi.Name != Defines.Stills.B1 && fi.Name != Defines.Stills.B2 && fi.Name != Defines.Stills.B3 && fi.Name != Defines.Stills.B4 && fi.Name != Defines.Stills.B5 &&
                                  fi.Name != Defines.Stills.C1 && fi.Name != Defines.Stills.C2 && fi.Name != Defines.Stills.C3 && fi.Name != Defines.Stills.C4 && fi.Name != Defines.Stills.C5 &&
                                  fi.Name != Defines.Stills.G1 && fi.Name != Defines.Stills.G2 && fi.Name != Defines.Stills.G3 && fi.Name != Defines.Stills.G4 && fi.Name != Defines.Stills.G5)
                                    {
                                        _path = ReplaceImageUrl(_path);
                                    }
                                    else
                                    {
                                        _path = string.Format(@"{0}\{1}", Defines.Asset.media_stills_path, fi.Name);
                                    }
                                }
                                else
                                {
                                    _path = ReplaceVideoUrl(_path);
                                }
                                //string y = fi.Name;
                                break;
                        }
                    }

                    if ((_streamId != "la" && _streamId != "lb") && File.Exists(_path))
                        MixerStreams.Add(new StreamItem(_streamId, _path, _cmd));
                }
            }
        }

        /// Scene Detail
        xn = xd.GetElementsByTagName(Defines.Tag.scenes_scene)[0];
        if (xn != null)
        {
            sceneName = xn.Attributes[Defines.Tag.scenes_scene_name].Value;
        }

        /// Transition Header
        xn = xd.GetElementsByTagName(Defines.Tag.scene_3d_group)[0];
        if (xn != null)
        {
            string _value;
            if (xn.Attributes[Defines.Tag.id] != null && xn.Attributes[Defines.Tag.id].Value == Defines.Tag.scene_3d_trn_transition)
            {
                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_duration] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_duration].Value : "2";
                mTranstionDuration = (string.IsNullOrEmpty(_value)) ? 2m : Convert.ToDecimal(_value);

                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_fixed] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_fixed].Value : "0";
                if (!string.IsNullOrEmpty(_value)) { mTransitionFixDuration = Convert.ToInt16(_value); }

                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_reverse] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_reverse].Value : "0";
                if (!string.IsNullOrEmpty(_value)) { mAutoReverse = Convert.ToInt16(_value); }

                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_swap] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_swap].Value : "0";
                if (!string.IsNullOrEmpty(_value)) { mTransitionSwapSource = Convert.ToInt16(_value); }
            }
        }

        /// Transition Items.
        replace_a = new List<string>();
        replace_b = new List<string>();

        xn = xd.GetElementsByTagName(Defines.Tag.scene_3d_group)[1];
        if (xn != null)
        {
            ReplaceAttribute(xn);
            mTransitionObjects = xn.OuterXml;
        }

        /// Update transition view elements.
        mTotalDuration = 0;
        xn = xd.GetElementsByTagName(Defines.Tag.view)[0];
        if (xn != null)
        {
            mTransitionItems = xn.OuterXml;

            decimal d, h;
            foreach (XmlNode x in xn.ChildNodes)
            {
                if (x != null && x.Name == Defines.Tag.view)
                {
                    d = (x.Attributes[Defines.Tag.scene_3d_trn_duration] != null) ? Convert.ToDecimal(x.Attributes[Defines.Tag.scene_3d_trn_duration].Value) : 0;
                    h = (x.Attributes[Defines.Tag.scene_3d_trn_hold] != null) ? Convert.ToDecimal(x.Attributes[Defines.Tag.scene_3d_trn_hold].Value) : 0;
                    mTotalDuration = mTotalDuration + d + h;
                }
            }
        }

        MPHelper.CreateStreams(MixerStreams);
        MPHelper.AppendTransition(mTransitionObjects);
        MPHelper.AppendTransitionView(mTransitionItems);
        if (MPHelper.mElementTree != null) MPHelper.mElementTree.UpdateTree(false);

        mEfxType = EffectType.DVE;
        IsLoaded = true;
    }    

    static void ReplaceAttribute(XmlNode xn)
    {
        string s;

        if (xn.Name.Contains(Defines.Tag.scene_3d_image))
        {
            if (xn.Attributes[Defines.Tag.scene_3d_image_url] != null) { xn.Attributes[Defines.Tag.scene_3d_image_url].Value = ReplaceImageUrl(xn.Attributes[Defines.Tag.scene_3d_image_url].Value); }

            if (xn.Attributes[Defines.Tag.scene_3d_mask] != null) { if (!IsDefaultMask(xn.Attributes["mask"].Value)) xn.Attributes["mask"].Value = ReplaceImageUrl(xn.Attributes["mask"].Value); }

            //if (xn.HasChildNodes) ReplaceAttribute(xn);
        }
        else if (xn.Name.Contains(Defines.Tag.scene_3d_video))
        {
            if (xn.Attributes[Defines.Tag.mixer_stream_id] != null)
            {
                s = xn.Attributes[Defines.Tag.mixer_stream_id].Value.ToUpper();
                if (string.Compare(s, "LA", true) == 0)
                {
                    replace_a.Add(xn.Attributes["id"].Value);
                    xn.Attributes[Defines.Tag.mixer_stream_id].Value = "~a"; //(!Helper.Master.fPanel1.chkFreeze.Checked) ? Defines.strLayerA : Defines.Asset.freeze_stream;
                    try { xn.Attributes.Remove(xn.Attributes[Defines.Tag.mixer_stream_idx]); } catch { }

                    /// Set Color Filter.
                    if (!string.IsNullOrEmpty(Defines.strLayerA_img_filter))
                    {
                        if (xn.Attributes[Defines.Tag.scene_3d_image_filter] == null)
                        {
                            XmlAttribute xa = xd.CreateAttribute(Defines.Tag.scene_3d_image_filter);
                            xa.Value = Defines.strLayerA_img_filter;
                            xn.Attributes.Append(xa);
                        }
                        else
                            xn.Attributes[Defines.Tag.scene_3d_image_filter].Value = Defines.strLayerA_img_filter;
                    }
                }
                else if (string.Compare(s, "LB", true) == 0)
                {
                    replace_b.Add(xn.Attributes["id"].Value);

                    xn.Attributes[Defines.Tag.mixer_stream_id].Value = "~b"; //!string.IsNullOrEmpty(Defines.strLayerB) ? Defines.strLayerB : Defines.strLayerA;
                    try { xn.Attributes.Remove(xn.Attributes[Defines.Tag.mixer_stream_idx]); } catch { }
                }
            }
            else if (xn.Attributes[Defines.Tag.mixer_stream_idx] != null)
            {
                xn.Attributes.Remove(xn.Attributes[Defines.Tag.mixer_stream_idx]);
            }

            /// Mask
            if (xn.Attributes[Defines.Tag.scene_3d_mask] != null)
            {
                s = xn.Attributes[Defines.Tag.scene_3d_mask].Value.ToLower();
                if (!IsDefaultMask(s))
                {
                    xn.Attributes[Defines.Tag.scene_3d_mask].Value = ReplaceImageUrl(xn.Attributes[Defines.Tag.scene_3d_mask].Value);
                }
            }

            //if (xn.HasChildNodes) ReplaceAttribute(xn);
        }
        else if (xn.Name.Contains(Defines.Tag.scene_3d_object))
        {
            if (xn.Attributes[Defines.Tag.scene_3d_image_url] != null) { xn.Attributes[Defines.Tag.scene_3d_image_url].Value = Replace3DsUrl(xn.Attributes[Defines.Tag.scene_3d_image_url].Value); }
            if (xn.Attributes["LayerA.png"] != null) { replace_a.Add(xn.Attributes["id"].Value); xn.Attributes["LayerA.png"].Value = "~a"; } //GetStream(xn.Attributes["LayerA.png"].Value); }
            if (xn.Attributes["LayerB.png"] != null) { replace_b.Add(xn.Attributes["id"].Value); xn.Attributes["LayerB.png"].Value = "~b"; }//GetStream(xn.Attributes["LayerB.png"].Value); }
            if (xn.Attributes["DDR1.png"] != null) { xn.Attributes["DDR1.png"].Value = GetStream(xn.Attributes["DDR1.png"].Value); }
            if (xn.Attributes["DDR2.png"] != null) { xn.Attributes["DDR2.png"].Value = GetStream(xn.Attributes["DDR2.png"].Value); }
            if (xn.Attributes["DDR3.png"] != null) { xn.Attributes["DDR3.png"].Value = GetStream(xn.Attributes["DDR3.png"].Value); }
            if (xn.Attributes["DDR4.png"] != null) { xn.Attributes["DDR4.png"].Value = GetStream(xn.Attributes["DDR4.png"].Value); }
            if (xn.Attributes["DDR5.png"] != null) { xn.Attributes["DDR5.png"].Value = GetStream(xn.Attributes["DDR5.png"].Value); }
            if (xn.Attributes["DDR6.png"] != null) { xn.Attributes["DDR6.png"].Value = GetStream(xn.Attributes["DDR6.png"].Value); }
            if (xn.Attributes["DDR7.png"] != null) { xn.Attributes["DDR7.png"].Value = GetStream(xn.Attributes["DDR7.png"].Value); }
            if (xn.Attributes["DDR8.png"] != null) { xn.Attributes["DDR8.png"].Value = GetStream(xn.Attributes["DDR8.png"].Value); }
            if (xn.Attributes["DDR9.png"] != null) { xn.Attributes["DDR9.png"].Value = GetStream(xn.Attributes["DDR9.png"].Value); }
            if (xn.Attributes["DDR10.png"] != null) { xn.Attributes["DDR10.png"].Value = GetStream(xn.Attributes["DDR10.png"].Value); }

            //if (xn.HasChildNodes) ReplaceAttribute(xn);
        }

        if (xn.HasChildNodes)
        {
            foreach (XmlNode x in xn.ChildNodes)
            {
                ReplaceAttribute(x);
            }
        }

        //if (xn.HasChildNodes)
        //{
        //    foreach (XmlNode x in xn.ChildNodes)
        //    {
        //        ReplaceUrl(x);
        //        if (x.HasChildNodes)
        //            ReplaceAttribute(x);
        //    }
        //}
        //else
        //{
        //    ReplaceUrl(xn);
        //}
    }

    static void ReplaceUrl(XmlNode x)
    {
        try
        {
            string s = string.Empty;

            switch (x.Name.ToLower())
            {
                case Defines.Tag.scene_3d_image:
                    if (x.Attributes[Defines.Tag.scene_3d_image_url] != null) { x.Attributes[Defines.Tag.scene_3d_image_url].Value = ReplaceImageUrl(x.Attributes[Defines.Tag.scene_3d_image_url].Value); }

                    if (x.Attributes[Defines.Tag.scene_3d_mask] != null) { if (!IsDefaultMask(x.Attributes["mask"].Value)) x.Attributes["mask"].Value = ReplaceImageUrl(x.Attributes["mask"].Value); }

                    if (x.HasChildNodes) ReplaceAttribute(x);
                    break;

                case Defines.Tag.scene_3d_video:
                    if (x.Attributes[Defines.Tag.mixer_stream_id] != null)
                    {
                        s = x.Attributes[Defines.Tag.mixer_stream_id].Value.ToUpper();
                        if (string.Compare(s, "LA", true) == 0)
                        {
                            x.Attributes[Defines.Tag.mixer_stream_id].Value = Defines.strLayerA;// (!Helper.Master.fPanel1.chkFreeze.Checked) ? Defines.strLayerA : Defines.Asset.freeze_stream;
                            try { x.Attributes.Remove(x.Attributes[Defines.Tag.mixer_stream_idx]); } catch { }

                            /// Set Color Filter.
                            if (!string.IsNullOrEmpty(Defines.strLayerA_img_filter))
                            {
                                if (x.Attributes[Defines.Tag.scene_3d_image_filter] == null)
                                {
                                    XmlAttribute xa = xd.CreateAttribute(Defines.Tag.scene_3d_image_filter);
                                    xa.Value = Defines.strLayerA_img_filter;
                                    x.Attributes.Append(xa);
                                }
                                else
                                    x.Attributes[Defines.Tag.scene_3d_image_filter].Value = Defines.strLayerA_img_filter;
                            }
                        }
                        else if (string.Compare(s, "LB", true) == 0)
                        {
                            x.Attributes[Defines.Tag.mixer_stream_id].Value = !string.IsNullOrEmpty(Defines.strLayerB) ? Defines.strLayerB : Defines.strLayerA;
                            try { x.Attributes.Remove(x.Attributes[Defines.Tag.mixer_stream_idx]); } catch { }
                        }
                    }

                    else if (x.Attributes[Defines.Tag.mixer_stream_idx] != null)
                    {
                        x.Attributes.Remove(x.Attributes[Defines.Tag.mixer_stream_idx]);
                    }

                    if (x.Attributes[Defines.Tag.scene_3d_mask] != null)
                    {
                        s = x.Attributes[Defines.Tag.scene_3d_mask].Value.ToLower();
                        if (!IsDefaultMask(s))
                        {
                            x.Attributes[Defines.Tag.scene_3d_mask].Value = ReplaceImageUrl(x.Attributes[Defines.Tag.scene_3d_mask].Value);
                        }
                    }

                    if (x.HasChildNodes) ReplaceAttribute(x);
                    break;

                case Defines.Tag.scene_3d_object:
                    if (x.Attributes[Defines.Tag.scene_3d_image_url] != null) { x.Attributes[Defines.Tag.scene_3d_image_url].Value = Replace3DsUrl(x.Attributes[Defines.Tag.scene_3d_image_url].Value); }
                    if (x.Attributes["LayerA.png"] != null) { x.Attributes["LayerA.png"].Value = GetStream(x.Attributes["LayerA.png"].Value); }
                    if (x.Attributes["LayerB.png"] != null) { x.Attributes["LayerB.png"].Value = GetStream(x.Attributes["LayerB.png"].Value); }
                    if (x.Attributes["DDR1.png"] != null) { x.Attributes["DDR1.png"].Value = GetStream(x.Attributes["DDR1.png"].Value); }
                    if (x.Attributes["DDR2.png"] != null) { x.Attributes["DDR2.png"].Value = GetStream(x.Attributes["DDR2.png"].Value); }
                    if (x.Attributes["DDR3.png"] != null) { x.Attributes["DDR3.png"].Value = GetStream(x.Attributes["DDR3.png"].Value); }
                    if (x.Attributes["DDR4.png"] != null) { x.Attributes["DDR4.png"].Value = GetStream(x.Attributes["DDR4.png"].Value); }
                    if (x.Attributes["DDR5.png"] != null) { x.Attributes["DDR5.png"].Value = GetStream(x.Attributes["DDR5.png"].Value); }
                    if (x.Attributes["DDR6.png"] != null) { x.Attributes["DDR6.png"].Value = GetStream(x.Attributes["DDR6.png"].Value); }
                    if (x.Attributes["DDR7.png"] != null) { x.Attributes["DDR7.png"].Value = GetStream(x.Attributes["DDR7.png"].Value); }
                    if (x.Attributes["DDR8.png"] != null) { x.Attributes["DDR8.png"].Value = GetStream(x.Attributes["DDR8.png"].Value); }
                    if (x.Attributes["DDR9.png"] != null) { x.Attributes["DDR9.png"].Value = GetStream(x.Attributes["DDR9.png"].Value); }
                    if (x.Attributes["DDR10.png"] != null) { x.Attributes["DDR10.png"].Value = GetStream(x.Attributes["DDR10.png"].Value); }

                    if (x.HasChildNodes) ReplaceAttribute(x);
                    break;

                default:
                    if (x.HasChildNodes) ReplaceAttribute(x);
                    break;
            }
        }
        catch (Exception ex) { ExceptionLog.Create(ex); }
    }



    static string GetStream(string _streamId)
    {
        switch (_streamId.ToLower())
        {
            case "la":
            case "l1":
                return Defines.strLayerA;

            case "lb":
            case "l2":
                return Defines.strLayerB;

            default:
                return _streamId;
        }
    }

    static string ReplaceVideoUrl(string x)
    {
        try { if (File.Exists(x)) return x; } catch { }

        string[] y = x.Split('\\');
        return string.Format(@"{0}\{1}", Defines.Asset.trans_project_path.Replace("projects", "videos"), y[y.Length - 1]);
    }

    static string ReplaceImageUrl(string x)
    {
        try { if (File.Exists(x)) return x; } catch { }

        string[] y = x.Split('\\');
        string z = string.Empty;

        switch (y[y.Length - 1].ToLower())
        {
            case "b1.jpg": z = string.Format(@"{0}\boy1.jpg", Defines.Asset.media_stills_path); break;
            case "b2.jpg": z = string.Format(@"{0}\boy2.jpg", Defines.Asset.media_stills_path); break;
            case "b3.jpg": z = string.Format(@"{0}\boy3.jpg", Defines.Asset.media_stills_path); break;
            case "b4.jpg": z = string.Format(@"{0}\boy4.jpg", Defines.Asset.media_stills_path); break;
            case "b5.jpg": z = string.Format(@"{0}\boy5.jpg", Defines.Asset.media_stills_path); break;

            case "g1.jpg": z = string.Format(@"{0}\girl1.jpg", Defines.Asset.media_stills_path); break;
            case "g2.jpg": z = string.Format(@"{0}\girl2.jpg", Defines.Asset.media_stills_path); break;
            case "g3.jpg": z = string.Format(@"{0}\girl3.jpg", Defines.Asset.media_stills_path); break;
            case "g4.jpg": z = string.Format(@"{0}\girl4.jpg", Defines.Asset.media_stills_path); break;
            case "g5.jpg": z = string.Format(@"{0}\girl5.jpg", Defines.Asset.media_stills_path); break;

            case "c1.jpg": z = string.Format(@"{0}\couple1.jpg", Defines.Asset.media_stills_path); break;
            case "c2.jpg": z = string.Format(@"{0}\couple2.jpg", Defines.Asset.media_stills_path); break;
            case "c3.jpg": z = string.Format(@"{0}\couple3.jpg", Defines.Asset.media_stills_path); break;
            case "c4.jpg": z = string.Format(@"{0}\couple4.jpg", Defines.Asset.media_stills_path); break;
            case "c5.jpg": z = string.Format(@"{0}\couple5.jpg", Defines.Asset.media_stills_path); break;

            default:
                z = string.Format(@"{0}\{1}", Defines.Asset.trans_project_path.Replace("projects", "images"), y[y.Length - 1]);
                if (!File.Exists(z)) z = string.Format(@"{0}\{1}", Defines.Asset.trans_project_path.Replace("projects", "masks"), y[y.Length - 1]);
                break;
        }
        return z;
    }

    static string Replace3DsUrl(string x)
    {
        try { if (File.Exists(x)) return x; } catch { }

        string[] y = x.Split('\\');
        string z = string.Format(@"{0}\{1}\{2}", Defines.Asset.trans_project_path.Replace("projects", "3ds"), y[y.Length - 2], y[y.Length - 1]);
        if (!File.Exists(z)) z = string.Format(@"{0}\{1}", Defines.Asset.trans_project_path.Replace("projects", "3ds"), y[y.Length - 1]);
        return z;
    }

    static string ReplaceTo(string x, int y)
    {
        FileInfo fi = new FileInfo(x);
        return string.Format(@"{0}\{1}", (y == 0) ? Defines.Asset.video_path : (y == 1) ? Defines.Asset.image_path : (y == 2) ? Defines.Asset.mask_path : (y == 3) ? Defines.Asset.object_path : "../", fi.Name);
    }

    static bool IsDefaultMask(string x)
    {
        x = x.ToLower();
        return (x == "none") ? true : (x == "circle") ? true : (x == "rect") ? true : (x == "round_rect") ? true : false;
    }
         

    public static void Transition_image_save(string _filePath)
    {
        string _path = Path.ChangeExtension(_filePath, Defines.Asset.trans_image_extension);
        MPHelper.StillGrab(_path, Defines.Asset.trans_image_format, null, Defines.Asset.trans_image_width, Defines.Asset.trans_image_height, true);
    }

    public static void Transition_remove()
    {
        MPHelper.ResetTransition();
        MPHelper.ResetTransitionView();
        if (MixerStreams.Count > 0) MPHelper.RemoveStreams(MixerStreams);
        if (MPHelper.mElementTree != null) MPHelper.mElementTree.UpdateTree(true);
    }
    
    public static void Transition_update_xml()
    {
        MPHelper.UpdateTransitionXML();
    }



    public static string UpdateTransition(XmlDocument doc)
    {
        XmlNode xn = doc.GetElementsByTagName(Defines.Tag.scene_3d_group)[1];
        if (xn != null)
        {
            ReplaceAttribute(xn);
            //MPHelper.AppendTransition(xn.OuterXml);
            return xn.OuterXml.ToString();
        }
        return "";
    }

    public static void ResetStreamsToPaly()
    {
        //MPHelper.RemoveStreams(MixerStreams);
        //MPHelper.CreateStreams(MixerStreams);
        MPHelper.ResetStreamsToPlay(MixerStreams);
    }
}

//public static class TrnsHelper
//{
//    private static XmlDocument xd = null;

//    public static List<StreamItem> MixerStreams = new List<StreamItem>();

//    public static string sceneName = "0000";
//    public static bool IsLoaded = false;
//    public static decimal mTranstionDuration = 0;
//    public static decimal mTotalDuration = 0;
//    public static short mAutoReverse = 0;
//    public static short mTransitionFixDuration = 0;
//    public static short mTransitionSwapSource = 0;
//    public static EffectType mEfxType = EffectType.DVE;

//    public static string mTransitionObjects;
//    public static string mTransitionItems;

//    public static int mViewCount = 0;

//    static XmlNode _sc;

//    public static void FillProjectDetail(XmlDocument _xd)
//    {
//        sceneName = "0000";
//        mTranstionDuration = 0;
//        mTransitionFixDuration = 0;
//        mTransitionSwapSource = 0;
//        mAutoReverse = 0;

//        /// Scene Detail
//        XmlNode xn = _xd.GetElementsByTagName(Defines.Tag.scenes_scene)[0];
//        if (xn != null)
//        {
//            sceneName = xn.Attributes[Defines.Tag.scenes_scene_name].Value;
//        }

//        /// Transition Header
//        xn = _xd.GetElementsByTagName(Defines.Tag.scene_3d_group)[0];
//        if (xn != null)
//        {
//            string _value;
//            if (xn.Attributes[Defines.Tag.id] != null && xn.Attributes[Defines.Tag.id].Value == Defines.Tag.scene_3d_trn_transition)
//            {
//                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_duration] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_duration].Value : "2";
//                mTranstionDuration = (string.IsNullOrEmpty(_value)) ? 2m : Convert.ToDecimal(_value);

//                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_fixed] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_fixed].Value : "0";
//                if (!string.IsNullOrEmpty(_value)) { mTransitionFixDuration = Convert.ToInt16(_value); }

//                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_reverse] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_reverse].Value : "0";
//                if (!string.IsNullOrEmpty(_value)) { mAutoReverse = Convert.ToInt16(_value); }

//                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_swap] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_swap].Value : "0";
//                if (!string.IsNullOrEmpty(_value)) { mTransitionSwapSource = Convert.ToInt16(_value); }
//            }
//        }
//    }

//    public static void FillStreams(XmlDocument _xd)
//    {
//        MixerStreams = new List<StreamItem>();
//        string _streamId, _path, _cmd;

//        /// Mixer Streams.
//        XmlNode xn = _xd.GetElementsByTagName(Defines.Tag.mixer)[0];
//        if (xn != null)
//        {
//            foreach (XmlNode x in xn.ChildNodes)
//            {
//                if (x != null && x.Name == Defines.Tag.mixer_file)
//                {
//                    _streamId = (x.Attributes[Defines.Tag.mixer_stream_id] != null) ? x.Attributes[Defines.Tag.mixer_stream_id].Value.ToLower() : "";
//                    _path = (x.Attributes[Defines.Tag.mixer_path] != null) ? x.Attributes[Defines.Tag.mixer_path].Value : "";
//                    _cmd = (x.Attributes[Defines.Tag.mixer_cmd] != null) ? x.Attributes[Defines.Tag.mixer_cmd].Value : "0";
//                    //((x.Attributes[Defines.Tag.mixer_loop] != null) ? (x.Attributes[Defines.Tag.mixer_loop].Value.ToLower() == "true" ? "1" : "0") : "0");

//                    if (_streamId == "la") _streamId = Defines.Stream.strLive1;
//                    else if (_streamId == "lb") _streamId = Defines.Stream.strLive2;
//                    else
//                    {
//                        /// Check and replace media gallery videos.
//                        switch (_streamId)
//                        {
//                            case "b1": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy1); break;
//                            case "b2": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy2); break;
//                            case "b3": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy3); break;
//                            case "b4": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy4); break;
//                            case "b5": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy5); break;

//                            case "c1": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple1); break;
//                            case "c2": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple2); break;
//                            case "c3": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple3); break;
//                            case "c4": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple4); break;
//                            case "c5": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple5); break;

//                            case "g1": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl1); break;
//                            case "g2": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl2); break;
//                            case "g3": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl3); break;
//                            case "g4": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl4); break;
//                            case "g5": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl5); break;
//                            default:
//                                FileInfo fi = new FileInfo(_path);
//                                if (fi.Extension.ToLower() == ".png" || fi.Extension.ToLower() == ".jpg")
//                                {
//                                    if (fi.Name != Defines.Stills.B1 && fi.Name != Defines.Stills.B2 && fi.Name != Defines.Stills.B3 && fi.Name != Defines.Stills.B4 && fi.Name != Defines.Stills.B5 &&
//                                  fi.Name != Defines.Stills.C1 && fi.Name != Defines.Stills.C2 && fi.Name != Defines.Stills.C3 && fi.Name != Defines.Stills.C4 && fi.Name != Defines.Stills.C5 &&
//                                  fi.Name != Defines.Stills.G1 && fi.Name != Defines.Stills.G2 && fi.Name != Defines.Stills.G3 && fi.Name != Defines.Stills.G4 && fi.Name != Defines.Stills.G5)
//                                    {
//                                        _path = ReplaceImageUrl(_path);
//                                    }
//                                    else
//                                    {
//                                        _path = string.Format(@"{0}\{1}", Defines.Asset.media_stills_path, fi.Name);
//                                    }
//                                }
//                                else
//                                {
//                                    _path = ReplaceVideoUrl(_path);
//                                }
//                                //string y = fi.Name;
//                                break;
//                        }
//                    }

//                    if ((_streamId != "la" && _streamId != "lb") && File.Exists(_path))
//                        MixerStreams.Add(new StreamItem(_streamId, _path, _cmd));
//                }
//            }

//            MPHelper.CreateStreams(MixerStreams);
//        }
//    }

//    public static void FillElements(XmlDocument _xd)
//    {
//        mTransitionObjects = string.Empty;

//        XmlNode xn = _xd.GetElementsByTagName(Defines.Tag.scene_3d_group)[1];
//        if (xn != null)
//        {
//            ReplaceAttribute(xn);
//            mTransitionObjects = xn.OuterXml;

//            MPHelper.AppendTransition(mTransitionObjects);
//        }
//    }

//    public static void FillViews(XmlDocument _xd)
//    {
//        /// Update transition view elements.
//        mTotalDuration = 0; mViewCount = 0;
//        XmlNode xn = _xd.GetElementsByTagName(Defines.Tag.view)[0];
//        if (xn != null)
//        {
//            mTransitionItems = xn.OuterXml;
            
//            decimal d, h;
//            foreach (XmlNode x in xn.ChildNodes)
//            {
//                if (x != null && x.Name == Defines.Tag.view)
//                {
//                    mViewCount++;
//                    d = (x.Attributes[Defines.Tag.scene_3d_trn_duration] != null) ? Convert.ToDecimal(x.Attributes[Defines.Tag.scene_3d_trn_duration].Value) : 0;
//                    h = (x.Attributes[Defines.Tag.scene_3d_trn_hold] != null) ? Convert.ToDecimal(x.Attributes[Defines.Tag.scene_3d_trn_hold].Value) : 0;
//                    mTotalDuration = mTotalDuration + d + h;
//                }
//            }

//            MPHelper.AppendTransitionView(mTransitionItems);
//        }       
//    }

//    //public static void Transition_load(string _filePath)
//    public static void Transition_load(XmlDocument _xd)
//    {
//        MixerStreams = new List<StreamItem>();
//        string _streamId, _path, _cmd;

//        IsLoaded = false;
//        sceneName = "0000";
//        mTranstionDuration = 0;
//        mTransitionFixDuration = 0;
//        mTransitionSwapSource = 0;
//        mAutoReverse = 0;

//        mTransitionObjects = string.Empty;
//        mTransitionItems = string.Empty;

//        //xd = new XmlDocument();
//        //xd.Load(_filePath);
//        xd = _xd;

//        /// Mixer Streams.
//        XmlNode xn = xd.GetElementsByTagName(Defines.Tag.mixer)[0];
//        if (xn != null)
//        {
//            foreach (XmlNode x in xn.ChildNodes)
//            {
//                if (x != null && x.Name == Defines.Tag.mixer_file)
//                {
//                    _streamId = (x.Attributes[Defines.Tag.mixer_stream_id] != null) ? x.Attributes[Defines.Tag.mixer_stream_id].Value.ToLower() : "";
//                    _path = (x.Attributes[Defines.Tag.mixer_path] != null) ? x.Attributes[Defines.Tag.mixer_path].Value : "";
//                    _cmd = (x.Attributes[Defines.Tag.mixer_cmd] != null) ? x.Attributes[Defines.Tag.mixer_cmd].Value : "0";
//                    //((x.Attributes[Defines.Tag.mixer_loop] != null) ? (x.Attributes[Defines.Tag.mixer_loop].Value.ToLower() == "true" ? "1" : "0") : "0");

//                    if (_streamId == "la") _streamId = Defines.Stream.strLive1;
//                    else if (_streamId == "lb") _streamId = Defines.Stream.strLive2;
//                    else
//                    {
//                        /// Check and replace media gallery videos.
//                        switch (_streamId)
//                        {
//                            case "b1": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy1); break;
//                            case "b2": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy2); break;
//                            case "b3": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy3); break;
//                            case "b4": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy4); break;
//                            case "b5": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_boy5); break;

//                            case "c1": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple1); break;
//                            case "c2": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple2); break;
//                            case "c3": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple3); break;
//                            case "c4": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple4); break;
//                            case "c5": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_couple5); break;

//                            case "g1": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl1); break;
//                            case "g2": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl2); break;
//                            case "g3": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl3); break;
//                            case "g4": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl4); break;
//                            case "g5": _path = string.Format(@"{0}\{1}.mp4", Defines.Asset.media_clips_path, Defines.Asset.media_girl5); break;
//                            default:
//                                FileInfo fi = new FileInfo(_path);
//                                if (fi.Extension.ToLower() == ".png" || fi.Extension.ToLower() == ".jpg")
//                                {
//                                    if (fi.Name != Defines.Stills.B1 && fi.Name != Defines.Stills.B2 && fi.Name != Defines.Stills.B3 && fi.Name != Defines.Stills.B4 && fi.Name != Defines.Stills.B5 &&
//                                  fi.Name != Defines.Stills.C1 && fi.Name != Defines.Stills.C2 && fi.Name != Defines.Stills.C3 && fi.Name != Defines.Stills.C4 && fi.Name != Defines.Stills.C5 &&
//                                  fi.Name != Defines.Stills.G1 && fi.Name != Defines.Stills.G2 && fi.Name != Defines.Stills.G3 && fi.Name != Defines.Stills.G4 && fi.Name != Defines.Stills.G5)
//                                    {
//                                        _path = ReplaceImageUrl(_path);
//                                    }
//                                    else
//                                    {
//                                        _path = string.Format(@"{0}\{1}", Defines.Asset.media_stills_path, fi.Name);
//                                    }
//                                }
//                                else
//                                {
//                                    _path = ReplaceVideoUrl(_path);
//                                }
//                                //string y = fi.Name;
//                                break;
//                        }
//                    }

//                    if ((_streamId != "la" && _streamId != "lb") && File.Exists(_path))
//                        MixerStreams.Add(new StreamItem(_streamId, _path, _cmd));
//                }
//            }
//        }

//        /// Scene Detail
//        xn = xd.GetElementsByTagName(Defines.Tag.scenes_scene)[0];
//        if (xn != null)
//        {
//            sceneName = xn.Attributes[Defines.Tag.scenes_scene_name].Value;
//        }

//        /// Transition Header
//        xn = xd.GetElementsByTagName(Defines.Tag.scene_3d_group)[0];
//        if (xn != null)
//        {
//            string _value;
//            if (xn.Attributes[Defines.Tag.id] != null && xn.Attributes[Defines.Tag.id].Value == Defines.Tag.scene_3d_trn_transition)
//            {
//                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_duration] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_duration].Value : "2";
//                mTranstionDuration = (string.IsNullOrEmpty(_value)) ? 2m : Convert.ToDecimal(_value);

//                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_fixed] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_fixed].Value : "0";
//                if (!string.IsNullOrEmpty(_value)) { mTransitionFixDuration = Convert.ToInt16(_value); }

//                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_reverse] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_reverse].Value : "0";
//                if (!string.IsNullOrEmpty(_value)) { mAutoReverse = Convert.ToInt16(_value); }

//                _value = (xn.Attributes[Defines.Tag.scene_3d_trn_swap] != null) ? xn.Attributes[Defines.Tag.scene_3d_trn_swap].Value : "0";
//                if (!string.IsNullOrEmpty(_value)) { mTransitionSwapSource = Convert.ToInt16(_value); }
//            }
//        }

//        /// Transition Items.
//        xn = xd.GetElementsByTagName(Defines.Tag.scene_3d_group)[1];
//        if (xn != null)
//        {
//            _sc = xn;
//            //mTransitionObjects = xn.OuterXml;
//            //ReplaceAttribute(xn);
//        }
//        else
//            _sc = null;

//        /// Update transition view elements.
//        mTotalDuration = 0;
//        xn = xd.GetElementsByTagName(Defines.Tag.view)[0];
//        if (xn != null)
//        {
//            mTransitionItems = xn.OuterXml;

//            decimal d, h;
//            foreach (XmlNode x in xn.ChildNodes)
//            {
//                if (x != null && x.Name == Defines.Tag.view)
//                {
//                    d = (x.Attributes[Defines.Tag.scene_3d_trn_duration] != null) ? Convert.ToDecimal(x.Attributes[Defines.Tag.scene_3d_trn_duration].Value) : 0;
//                    h = (x.Attributes[Defines.Tag.scene_3d_trn_hold] != null) ? Convert.ToDecimal(x.Attributes[Defines.Tag.scene_3d_trn_hold].Value) : 0;
//                    mTotalDuration = mTotalDuration + d + h;
//                }
//            }
//        }

//        MPHelper.CreateStreams(MixerStreams);
//        MPHelper.AppendTransition(mTransitionObjects);
//        MPHelper.AppendTransitionView(mTransitionItems);
//        if (MPHelper.mElementTree != null) MPHelper.mElementTree.UpdateTree(false);

//        mEfxType = EffectType.DVE;
//        IsLoaded = true;
//    }

//    public static void SetEffectScript()
//    {
//        if (_sc == null) return;
//        /// Transition Items.
//        XmlNode xn = _sc;
//        if (xn != null)
//        {
//            ReplaceAttribute(xn);
//            mTransitionObjects = xn.OuterXml;
//            MPHelper.AppendTransition(mTransitionObjects);
//            if (MPHelper.mElementTree != null) MPHelper.mElementTree.UpdateTree(false);
//        }

//        MPHelper.AppendTransitionView(mTransitionItems);
//    }

//    static void ReplaceAttribute(XmlNode xn)
//    {
//        string s;

//        if (xn.Name.Contains(Defines.Tag.scene_3d_image))
//        {
//            if (xn.Attributes[Defines.Tag.scene_3d_image_url] != null) { xn.Attributes[Defines.Tag.scene_3d_image_url].Value = ReplaceImageUrl(xn.Attributes[Defines.Tag.scene_3d_image_url].Value); }

//            if (xn.Attributes[Defines.Tag.scene_3d_mask] != null) { if (!IsDefaultMask(xn.Attributes["mask"].Value)) xn.Attributes["mask"].Value = ReplaceImageUrl(xn.Attributes["mask"].Value); }

//            //if (xn.HasChildNodes) ReplaceAttribute(xn);
//        }
//        else if (xn.Name.Contains(Defines.Tag.scene_3d_video))
//        {
//            if (xn.Attributes[Defines.Tag.mixer_stream_id] != null)
//            {
//                s = xn.Attributes[Defines.Tag.mixer_stream_id].Value.ToUpper();
//                if (string.Compare(s, "LA", true) == 0)
//                {
//                    xn.Attributes[Defines.Tag.mixer_stream_id].Value = (!Helper.Master.fPanel1.chkFreeze.Checked) ? Defines.strLayerA : Defines.Asset.freeze_stream;
//                    try { xn.Attributes.Remove(xn.Attributes[Defines.Tag.mixer_stream_idx]); } catch { }

//                    /// Set Color Filter.
//                    if (!string.IsNullOrEmpty(Defines.strLayerA_img_filter))
//                    {
//                        if (xn.Attributes[Defines.Tag.scene_3d_image_filter] == null)
//                        {
//                            XmlAttribute xa = xd.CreateAttribute(Defines.Tag.scene_3d_image_filter);
//                            xa.Value = Defines.strLayerA_img_filter;
//                            xn.Attributes.Append(xa);
//                        }
//                        else
//                            xn.Attributes[Defines.Tag.scene_3d_image_filter].Value = Defines.strLayerA_img_filter;
//                    }
//                }
//                else if (string.Compare(s, "LB", true) == 0)
//                {
//                    xn.Attributes[Defines.Tag.mixer_stream_id].Value = !string.IsNullOrEmpty(Defines.strLayerB) ? Defines.strLayerB : Defines.strLayerA;
//                    try { xn.Attributes.Remove(xn.Attributes[Defines.Tag.mixer_stream_idx]); } catch { }
//                }
//            }
//            else if (xn.Attributes[Defines.Tag.mixer_stream_idx] != null)
//            {
//                xn.Attributes.Remove(xn.Attributes[Defines.Tag.mixer_stream_idx]);
//            }

//            /// Mask
//            if (xn.Attributes[Defines.Tag.scene_3d_mask] != null)
//            {
//                s = xn.Attributes[Defines.Tag.scene_3d_mask].Value.ToLower();
//                if (!IsDefaultMask(s))
//                {
//                    xn.Attributes[Defines.Tag.scene_3d_mask].Value = ReplaceImageUrl(xn.Attributes[Defines.Tag.scene_3d_mask].Value);
//                }
//            }

//            //if (xn.HasChildNodes) ReplaceAttribute(xn);
//        }
//        else if (xn.Name.Contains(Defines.Tag.scene_3d_image))
//        {
//            if (xn.Attributes[Defines.Tag.scene_3d_image_url] != null) { xn.Attributes[Defines.Tag.scene_3d_image_url].Value = Replace3DsUrl(xn.Attributes[Defines.Tag.scene_3d_image_url].Value); }
//            if (xn.Attributes["LayerA.png"] != null) { xn.Attributes["LayerA.png"].Value = GetStream(xn.Attributes["LayerA.png"].Value); }
//            if (xn.Attributes["LayerB.png"] != null) { xn.Attributes["LayerB.png"].Value = GetStream(xn.Attributes["LayerB.png"].Value); }
//            if (xn.Attributes["DDR1.png"] != null) { xn.Attributes["DDR1.png"].Value = GetStream(xn.Attributes["DDR1.png"].Value); }
//            if (xn.Attributes["DDR2.png"] != null) { xn.Attributes["DDR2.png"].Value = GetStream(xn.Attributes["DDR2.png"].Value); }
//            if (xn.Attributes["DDR3.png"] != null) { xn.Attributes["DDR3.png"].Value = GetStream(xn.Attributes["DDR3.png"].Value); }
//            if (xn.Attributes["DDR4.png"] != null) { xn.Attributes["DDR4.png"].Value = GetStream(xn.Attributes["DDR4.png"].Value); }
//            if (xn.Attributes["DDR5.png"] != null) { xn.Attributes["DDR5.png"].Value = GetStream(xn.Attributes["DDR5.png"].Value); }
//            if (xn.Attributes["DDR6.png"] != null) { xn.Attributes["DDR6.png"].Value = GetStream(xn.Attributes["DDR6.png"].Value); }
//            if (xn.Attributes["DDR7.png"] != null) { xn.Attributes["DDR7.png"].Value = GetStream(xn.Attributes["DDR7.png"].Value); }
//            if (xn.Attributes["DDR8.png"] != null) { xn.Attributes["DDR8.png"].Value = GetStream(xn.Attributes["DDR8.png"].Value); }
//            if (xn.Attributes["DDR9.png"] != null) { xn.Attributes["DDR9.png"].Value = GetStream(xn.Attributes["DDR9.png"].Value); }
//            if (xn.Attributes["DDR10.png"] != null) { xn.Attributes["DDR10.png"].Value = GetStream(xn.Attributes["DDR10.png"].Value); }

//            //if (xn.HasChildNodes) ReplaceAttribute(xn);
//        }

//        if (xn.HasChildNodes)
//        {
//            foreach(XmlNode x in xn.ChildNodes)
//            {
//                ReplaceAttribute(x);
//            }
//        }

//        //if (xn.HasChildNodes)
//        //{
//        //    foreach (XmlNode x in xn.ChildNodes)
//        //    {
//        //        ReplaceUrl(x);
//        //        if (x.HasChildNodes)
//        //            ReplaceAttribute(x);
//        //    }
//        //}
//        //else
//        //{
//        //    ReplaceUrl(xn);
//        //}
//    }

//    static void ReplaceUrl(XmlNode x)
//    {
//        try
//        {
//            string s = string.Empty;

//            switch (x.Name.ToLower())
//            {
//                case Defines.Tag.scene_3d_image:
//                    if (x.Attributes[Defines.Tag.scene_3d_image_url] != null) { x.Attributes[Defines.Tag.scene_3d_image_url].Value = ReplaceImageUrl(x.Attributes[Defines.Tag.scene_3d_image_url].Value); }

//                    if (x.Attributes[Defines.Tag.scene_3d_mask] != null){ if (!IsDefaultMask(x.Attributes["mask"].Value)) x.Attributes["mask"].Value = ReplaceImageUrl(x.Attributes["mask"].Value); }

//                    if (x.HasChildNodes) ReplaceAttribute(x);
//                    break;

//                case Defines.Tag.scene_3d_video:
//                    if (x.Attributes[Defines.Tag.mixer_stream_id] != null)
//                    {
//                        s = x.Attributes[Defines.Tag.mixer_stream_id].Value.ToUpper();
//                        if (string.Compare(s, "LA", true) == 0)
//                        {
//                            x.Attributes[Defines.Tag.mixer_stream_id].Value = (!Helper.Master.fPanel1.chkFreeze.Checked) ? Defines.strLayerA : Defines.Asset.freeze_stream;
//                            try { x.Attributes.Remove(x.Attributes[Defines.Tag.mixer_stream_idx]); } catch { }

//                            /// Set Color Filter.
//                            if (!string.IsNullOrEmpty(Defines.strLayerA_img_filter))
//                            {
//                                if (x.Attributes[Defines.Tag.scene_3d_image_filter] == null)
//                                {
//                                    XmlAttribute xa = xd.CreateAttribute(Defines.Tag.scene_3d_image_filter);
//                                    xa.Value = Defines.strLayerA_img_filter;
//                                    x.Attributes.Append(xa);
//                                }
//                                else
//                                    x.Attributes[Defines.Tag.scene_3d_image_filter].Value = Defines.strLayerA_img_filter;
//                            }
//                        }
//                        else if (string.Compare(s, "LB", true) == 0)
//                        {
//                            x.Attributes[Defines.Tag.mixer_stream_id].Value = !string.IsNullOrEmpty(Defines.strLayerB) ? Defines.strLayerB : Defines.strLayerA;
//                            try { x.Attributes.Remove(x.Attributes[Defines.Tag.mixer_stream_idx]); } catch { }
//                        }
//                    }

//                    else if (x.Attributes[Defines.Tag.mixer_stream_idx] != null)
//                    {
//                        x.Attributes.Remove(x.Attributes[Defines.Tag.mixer_stream_idx]);
//                    }

//                    if (x.Attributes[Defines.Tag.scene_3d_mask] != null)
//                    {
//                        s = x.Attributes[Defines.Tag.scene_3d_mask].Value.ToLower();
//                        if (!IsDefaultMask(s))
//                        {
//                            x.Attributes[Defines.Tag.scene_3d_mask].Value = ReplaceImageUrl(x.Attributes[Defines.Tag.scene_3d_mask].Value);
//                        }
//                    }

//                    if (x.HasChildNodes) ReplaceAttribute(x);
//                    break;               

//                case Defines.Tag.scene_3d_object:
//                    if (x.Attributes[Defines.Tag.scene_3d_image_url] != null) { x.Attributes[Defines.Tag.scene_3d_image_url].Value = Replace3DsUrl(x.Attributes[Defines.Tag.scene_3d_image_url].Value); }
//                    if (x.Attributes["LayerA.png"] != null) { x.Attributes["LayerA.png"].Value = GetStream(x.Attributes["LayerA.png"].Value); }
//                    if (x.Attributes["LayerB.png"] != null) { x.Attributes["LayerB.png"].Value = GetStream(x.Attributes["LayerB.png"].Value); }
//                    if (x.Attributes["DDR1.png"] != null) { x.Attributes["DDR1.png"].Value = GetStream(x.Attributes["DDR1.png"].Value); }
//                    if (x.Attributes["DDR2.png"] != null) { x.Attributes["DDR2.png"].Value = GetStream(x.Attributes["DDR2.png"].Value); }
//                    if (x.Attributes["DDR3.png"] != null) { x.Attributes["DDR3.png"].Value = GetStream(x.Attributes["DDR3.png"].Value); }
//                    if (x.Attributes["DDR4.png"] != null) { x.Attributes["DDR4.png"].Value = GetStream(x.Attributes["DDR4.png"].Value); }
//                    if (x.Attributes["DDR5.png"] != null) { x.Attributes["DDR5.png"].Value = GetStream(x.Attributes["DDR5.png"].Value); }
//                    if (x.Attributes["DDR6.png"] != null) { x.Attributes["DDR6.png"].Value = GetStream(x.Attributes["DDR6.png"].Value); }
//                    if (x.Attributes["DDR7.png"] != null) { x.Attributes["DDR7.png"].Value = GetStream(x.Attributes["DDR7.png"].Value); }
//                    if (x.Attributes["DDR8.png"] != null) { x.Attributes["DDR8.png"].Value = GetStream(x.Attributes["DDR8.png"].Value); }
//                    if (x.Attributes["DDR9.png"] != null) { x.Attributes["DDR9.png"].Value = GetStream(x.Attributes["DDR9.png"].Value); }
//                    if (x.Attributes["DDR10.png"] != null) { x.Attributes["DDR10.png"].Value = GetStream(x.Attributes["DDR10.png"].Value); }

//                    if (x.HasChildNodes) ReplaceAttribute(x);
//                    break;

//                default:
//                    if (x.HasChildNodes) ReplaceAttribute(x);
//                    break;
//            }
//        }
//        catch (Exception ex) { ExceptionLog.Create(ex); }
//    }



//    static string GetStream(string _streamId)
//    {
//        switch (_streamId.ToLower())
//        {
//            case "la":
//            case "l1":
//                return Defines.strLayerA;

//            case "lb":
//            case "l2":
//                return Defines.strLayerB;

//            default:
//                return _streamId;
//        }
//    }

//    static string ReplaceVideoUrl(string x)
//    {
//        try { if (File.Exists(x)) return x; } catch { }

//        string[] y = x.Split('\\');
//        return string.Format(@"{0}\{1}", Defines.Asset.trans_project_path.Replace("projects", "videos"), y[y.Length - 1]);
//    }

//    static string ReplaceImageUrl(string x)
//    {
//        try { if (File.Exists(x)) return x; } catch { }

//        string[] y = x.Split('\\');
//        string z = string.Empty;

//        switch (y[y.Length - 1].ToLower())
//        {
//            case "b1.jpg": z = string.Format(@"{0}\boy1.jpg", Defines.Asset.media_stills_path); break;
//            case "b2.jpg": z = string.Format(@"{0}\boy2.jpg", Defines.Asset.media_stills_path); break;
//            case "b3.jpg": z = string.Format(@"{0}\boy3.jpg", Defines.Asset.media_stills_path); break;
//            case "b4.jpg": z = string.Format(@"{0}\boy4.jpg", Defines.Asset.media_stills_path); break;
//            case "b5.jpg": z = string.Format(@"{0}\boy5.jpg", Defines.Asset.media_stills_path); break;

//            case "g1.jpg": z = string.Format(@"{0}\girl1.jpg", Defines.Asset.media_stills_path); break;
//            case "g2.jpg": z = string.Format(@"{0}\girl2.jpg", Defines.Asset.media_stills_path); break;
//            case "g3.jpg": z = string.Format(@"{0}\girl3.jpg", Defines.Asset.media_stills_path); break;
//            case "g4.jpg": z = string.Format(@"{0}\girl4.jpg", Defines.Asset.media_stills_path); break;
//            case "g5.jpg": z = string.Format(@"{0}\girl5.jpg", Defines.Asset.media_stills_path); break;

//            case "c1.jpg": z = string.Format(@"{0}\couple1.jpg", Defines.Asset.media_stills_path); break;
//            case "c2.jpg": z = string.Format(@"{0}\couple2.jpg", Defines.Asset.media_stills_path); break;
//            case "c3.jpg": z = string.Format(@"{0}\couple3.jpg", Defines.Asset.media_stills_path); break;
//            case "c4.jpg": z = string.Format(@"{0}\couple4.jpg", Defines.Asset.media_stills_path); break;
//            case "c5.jpg": z = string.Format(@"{0}\couple5.jpg", Defines.Asset.media_stills_path); break;

//            default:
//                z = string.Format(@"{0}\{1}", Defines.Asset.trans_project_path.Replace("projects", "images"), y[y.Length - 1]);
//                if (!File.Exists(z)) z = string.Format(@"{0}\{1}", Defines.Asset.trans_project_path.Replace("projects", "masks"), y[y.Length - 1]);
//                break;
//        }
//        return z;
//    }

//    static string Replace3DsUrl(string x)
//    {
//        try { if (File.Exists(x)) return x; } catch { }

//        string[] y = x.Split('\\');
//        string z = string.Format(@"{0}\{1}\{2}", Defines.Asset.trans_project_path.Replace("projects", "3ds"), y[y.Length - 2], y[y.Length - 1]);
//        if (!File.Exists(z)) z = string.Format(@"{0}\{1}", Defines.Asset.trans_project_path.Replace("projects", "3ds"), y[y.Length - 1]);
//        return z;
//    }

//    static string ReplaceTo(string x, int y)
//    {
//        FileInfo fi = new FileInfo(x);
//        return string.Format(@"{0}\{1}", (y == 0) ? Defines.Asset.video_path : (y == 1) ? Defines.Asset.image_path : (y == 2) ? Defines.Asset.mask_path : (y == 3) ? Defines.Asset.object_path : "../", fi.Name);
//    }

//    static bool IsDefaultMask(string x)
//    {
//        x = x.ToLower();
//        return (x == "none") ? true : (x == "circle") ? true : (x == "rect") ? true : (x == "round_rect") ? true : false;
//    }





//    public static void Transition_image_save(string _filePath)
//    {
//        string _path = Path.ChangeExtension(_filePath, Defines.Asset.trans_image_extension);
//        MPHelper.StillGrab(_path, Defines.Asset.trans_image_format, null, Defines.Asset.trans_image_width, Defines.Asset.trans_image_height, true);
//    }

//    public static void Transition_remove()
//    {
//        MPHelper.ResetTransition();
//        MPHelper.ResetTransitionView();
//        if (MixerStreams.Count > 0) MPHelper.RemoveStreams(MixerStreams);
//        if (MPHelper.mElementTree!= null) MPHelper.mElementTree.UpdateTree(true);
//    }


//    public static void Transition_update_xml()
//    {
//        MPHelper.UpdateTransitionXML();
//    }
//}

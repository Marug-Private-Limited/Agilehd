using MPLATFORMLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace AgileHDWPF.AgileSng
{
    public class TrnsHelper
    {
        #region Previous version
        private static void GetElement(out MElement mElement, string[] arr)
        {
            mElement = null;

            MPHelper.Control.ScenesActiveGet(out string _, out int _, out IMElements iMElements);
            for (int i = 0; i < arr.Length; i++)
            {
                iMElements.ElementsGetByID(arr[i], out mElement);
                if (mElement == null) { return; }
                iMElements = (IMElements)mElement;
            }

            //ReleaseComObject(iMElements);
        }

        public static void UpdateInScene(string _old, string _new)
        {
            if (_old == _new) return;
            try
            {
                GetElement(out MElement element, Defines.strTransitionVirtual);
                if (element != null)
                {
                    ReplaceLayerStream((IMElements)element, _old, _new);
                }
            }
            catch { }
        }

        static void ReplaceLayerStream(IMElements els, string _old, string _new)
        {
            if (els == null) return;
            MElement el;
            els.ElementsGetCount(out int n);
            for (int i = 0; i < n; i++)
            {
                els.ElementsGetByIndex(i, out el);
                el.ElementTypeGet(out string t);
                el.AttributesHave("stream_id", out int m, out string v);
                if (t == "video" && v == _old) el.AttributesStringSet("stream_id", _new);

                ReplaceLayerStream((IMElements)el, _old, _new);
            }
        }
        #endregion

        #region New version
        private static XmlDocument xd = null;

        public static List<Stream> MixerStreams = new List<Stream>();

        public static string sceneName = "0000";
        public static bool IsLoaded = false;
        public static decimal mTranstionDuration = 0;
        public static short mAutoReverse = 0;
        public static short mTransitionFixDuration = 0;
        public static short mTransitionSwapSource = 0;
        public static EffectType mEfxType = EffectType.DVE;

        public static string mTransitionObjects;
        public static string mTransitionItems;

        public static void Transition_load(string _filePath)
        {
            MixerStreams = new List<Stream>();
            string _streamId, _path, _cmd;

            IsLoaded = false;
            sceneName = "0000";
            mTranstionDuration = 0;
            mTransitionFixDuration = 0;
            mTransitionSwapSource = 0;
            mAutoReverse = 0;

            mTransitionObjects = string.Empty;
            mTransitionItems = string.Empty;

            xd = new XmlDocument();
            xd.Load(_filePath);

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
                        _cmd = (x.Attributes[AppData.Tag.mixer_cmd] != null) ? x.Attributes[AppData.Tag.mixer_cmd].Value :
                            ((x.Attributes[AppData.Tag.mixer_loop] != null) ? (x.Attributes[AppData.Tag.mixer_loop].Value.ToLower() == "true" ? "1" : "0") : "0");

                        if (_streamId != "la" && _streamId != "lb")
                            MixerStreams.Add(new Stream(_streamId, _path, _cmd));
                    }
                }
            }

            /// Scene Detail
            xn = xd.GetElementsByTagName(AppData.Tag.scenes_scene)[0];
            if (xn != null)
            {
                sceneName = xn.Attributes[AppData.Tag.scenes_scene_name].Value;
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
            MHelper.mElementTree.UpdateTree(false);

            mEfxType = EffectType.DVE;
            IsLoaded = true;
        }

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

            switch (x.Name.ToLower())
            {
                case AppData.Tag.scene_3d_video:
                    if (x.Attributes[AppData.Tag.mixer_stream_id] != null)
                    {
                        s = x.Attributes[AppData.Tag.mixer_stream_id].Value;

                        if (string.Compare(s, "LA") == 0)
                        {
                            x.Attributes[AppData.Tag.mixer_stream_id].Value = AppData.strLayerA;
                        }
                        else if (string.Compare(s, "LB") == 0)
                        {
                            x.Attributes[AppData.Tag.mixer_stream_id].Value = !string.IsNullOrEmpty(AppData.strLayerB) ? AppData.strLayerB : AppData.strLayerA;
                        }

                        /// Set Color Filter.
                        if (!string.IsNullOrEmpty(AppData.strLayerA_img_filter))
                        {
                            if (x.Attributes[AppData.Tag.scene_3d_image_filter] == null)
                            {
                                XmlAttribute xa = xd.CreateAttribute(AppData.Tag.scene_3d_image_filter);
                                xa.Value = AppData.strLayerA_img_filter;
                                x.Attributes.Append(xa);
                            }
                            else
                            {
                                x.Attributes[AppData.Tag.scene_3d_image_filter].Value = AppData.strLayerA_img_filter;
                            }
                        }

                        //x.Attributes[AppData.Tag.mixer_stream_id].Value = (string.Compare(s, "LA") == 0) ? AppData.strLayerA : (string.Compare(s, "LB") == 0) ? AppData.strLayerB : s;
                    }

                    if (x.Attributes[AppData.Tag.mixer_stream_idx] != null)
                    {
                        x.Attributes.Remove(x.Attributes[AppData.Tag.mixer_stream_idx]);
                    }

                    if (x.Attributes[AppData.Tag.scene_3d_mask] != null)
                    {
                        s = x.Attributes[AppData.Tag.scene_3d_mask].Value;
                        x.Attributes[AppData.Tag.scene_3d_mask].Value = s.IndexOf(@"/") == -1 ? s : ReplaceTo(x.Attributes[AppData.Tag.scene_3d_mask].Value, 2);
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

                    //if (x.Attributes[AppData.Tag.obj_layer_a] != null) { x.Attributes[AppData.Tag.obj_layer_a].Value = AppData.strLayerA; }
                    //if (x.Attributes[AppData.Tag.obj_layer_b] != null) { x.Attributes[AppData.Tag.obj_layer_b].Value = AppData.strLayerB; }
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
                    ReplaceAttribute(x);
                    break;
            }
        }

        static string ReplaceTo(string x, int y)
        {
            FileInfo fi = new FileInfo(x);
            return string.Format(@"{0}\{1}", (y == 0) ? AppData.Asset.video_path : (y == 1) ? AppData.Asset.image_path : (y == 2) ? AppData.Asset.mask_path : (y == 3) ? AppData.Asset.object_path : "../", fi.Name);
        }


        public static void Transition_image_save(string _filePath)
        {
            string _path = Path.ChangeExtension(_filePath, AppData.Asset.trans_image_extension);
            MHelper.StillGrab(_path, AppData.Asset.trans_image_format, null, AppData.Asset.trans_image_width, AppData.Asset.trans_image_height, true);
        }

        public static void Transition_remove()
        {
            if (SHelper.MixerStreams.Count > 0) MHelper.RemoveStreams(SHelper.MixerStreams);
            MHelper.ResetTransition();
            MHelper.ResetTransitionView();
            if (MHelper.mElementTree != null) MHelper.mElementTree.UpdateTree(false);
            if (MHelper.mMixerList != null) MHelper.mMixerList.UpdateList(false);
        }


        public static void Transition_update_xml()
        {
            MHelper.UpdateTransitionXML();
        }
        #endregion
    }
}

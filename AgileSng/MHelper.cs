using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using MPLATFORMLib;
using MControls;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;

namespace AgileHDWPF.AgileSng
{
    public class MHelper
    {
        private static MWriterClass m_objWriter;
        private static MComposerClass _composer;
        public static MComposerClass Control => _composer;
        private static MStreamsList _mixerList;
        private static MComposerElementsTree _tree;
        private static MAttributesList _attrList;
        private static TextBox _txtXml;

        public static void Init(MComposerClass obj)
        {
            if (obj == null) _composer = new MComposerClass();
            else _composer = obj;
        }


        public static void FindMFile(IMStreams _streams, string _streamId, out MItem _mFile)
        {
            _mFile = null;
            int num = 0;

            try
            {
                _streams.StreamsGet(_streamId, out num, out _mFile);
            }
            catch(Exception){}
        }

        public static void ComposerStreamsGet(string _streamId, out int _pIndex, out MItem _pStream)
        {
            _pIndex = 0;
            _pStream = null;

            try
            {
                Control.StreamsGet(_streamId, out _pIndex, out _pStream);
            }
            catch(Exception){}
        }

        public static void ComposerStreamsAdd(string _bsStreamID, object _pExternOrRef, string _bsPath, string _bsParam, out MItem _ppItem, double _dblTimeForChange)
        {
            _ppItem = null;
            try
            {
                if (_dblTimeForChange == 0.0) _dblTimeForChange = AppData.VirtualZero;
                Control.StreamsAdd(_bsStreamID, _pExternOrRef, _bsPath, _bsParam, out _ppItem, _dblTimeForChange);
            }
            catch {}
        }

        public static void StreamsRemoveAdd(string _bsStreamID, object _pExternOrRef, string _bsPath, string _bsParam, out MItem _ppItem, double _dblTimeForChange)
        {
            _ppItem = null;
            int num = 0;

            try
            {                
                Control.StreamsGet(_bsStreamID, out num, out _ppItem);
                if (_ppItem != null)
                {
                    Control.StreamsRemove(_ppItem, 0.0);
                }
            }
            catch (Exception) { }
            Control.StreamsAdd(_bsStreamID, null, _bsPath, "", out _ppItem, 0.0);
        }


        public static Bitmap StillGrab(string _FilePath, ImageFormat _ImageFormat, MItemClass _Item = null, int _width = 0, int _height = 0, bool xFlip = false)
        {
            Bitmap bitmap = null;
            MFrame mFrame;
            if (_Item == null)
            {
                Control.ObjectFrameGet(out mFrame, "");
            }
            else
            {
                _Item.ObjectFrameGet(out mFrame, "field=2");
            }
            mFrame.FrameClone(out MFrame mFrame2, eMFrameClone.eMFC_Full, eMFCC.eMFCC_ARGB32);
            mFrame2.FrameAVPropsGet(out M_AV_PROPS m_AV_PROPS);
            int num = 0;
            long value = 0L;
            mFrame2.FrameVideoGetBytes(out num, out value);
            try
            {
                int num2 = 32;
                int num3 = (num2 + 7) / 8;
                int stride = 4 * ((m_AV_PROPS.vidProps.nWidth * num3 + 3) / 4);
                int nWidth = m_AV_PROPS.vidProps.nWidth;
                int num4 = (m_AV_PROPS.vidProps.nHeight > 0) ? m_AV_PROPS.vidProps.nHeight : (m_AV_PROPS.vidProps.nHeight * -1);
                Bitmap bitmap2 = new Bitmap(nWidth, num4, stride, PixelFormat.Format32bppPArgb, (IntPtr)value);
                Rectangle rect;
                //if (nWidth == 720 || nWidth == 1920)
                //{
                    int width = nWidth;
                    int height = num4 / 2;
                    Bitmap image = new Bitmap(width, height);
                    using (Graphics graphics = Graphics.FromImage(image))
                    {
                        graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                        graphics.DrawImage(bitmap2, 0, 0, width, height);
                    }
                    height = num4;
                    if (_width > 0 && _height > 0)
                    {
                        width = _width;
                        height = _height;
                    }
                    using (Graphics graphics = Graphics.FromImage(bitmap2))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(image, 0, 0, width, height);
                    }
                    rect = new Rectangle(0, 0, width, height);
                //}
                //else
                //{
                //    rect = ((_width <= 0 || _height <= 0) ? new Rectangle(0, 0, bitmap2.Width, bitmap2.Height) : new Rectangle(0, 0, _width, _height));
                //}
                bitmap = bitmap2.Clone(rect, PixelFormat.Format24bppRgb);
                if (File.Exists(_FilePath))
                {
                    File.SetAttributes(_FilePath, FileAttributes.Normal);
                }
                if (!string.IsNullOrWhiteSpace(_FilePath))
                {
                    bitmap.Save(_FilePath, _ImageFormat);
                }
                return bitmap;
            }
            catch (Exception) {}

            mFrame2.FrameRelease();
            return bitmap;
        }

        public static void StillGrab(MItemClass _Item, string _FilePath, ImageFormat _ImageFormat, int _width = 0, int _height = 0, bool xFlip = false)
        {
            Bitmap bitmap = null;
            if (_Item != null)
            {
                try
                {
                    _Item.ObjectFrameGet(out MFrame mFrame, "field=2");
                    mFrame.FrameClone(out MFrame mFrame2, eMFrameClone.eMFC_Full, eMFCC.eMFCC_ARGB32);
                    if (mFrame2 != null)
                    {
                        mFrame2.FrameAVPropsGet(out M_AV_PROPS m_AV_PROPS);
                        int num = 0;
                        long value = 0L;
                        mFrame2.FrameVideoGetBytes(out num, out value);
                        try
                        {
                            int num2 = 32;
                            int num3 = (num2 + 7) / 8;
                            int stride = 4 * ((m_AV_PROPS.vidProps.nWidth * num3 + 3) / 4);
                            int nWidth = m_AV_PROPS.vidProps.nWidth;
                            int num4 = (m_AV_PROPS.vidProps.nHeight > 0) ? m_AV_PROPS.vidProps.nHeight : (m_AV_PROPS.vidProps.nHeight * -1);
                            Bitmap bitmap2 = new Bitmap(nWidth, num4, stride, PixelFormat.Format32bppPArgb, (IntPtr)value);
                            Rectangle rect;
                            if (nWidth == 720 || nWidth == 1920)
                            {
                                int width = nWidth;
                                int height = num4 / 2;
                                Bitmap image = new Bitmap(width, height);
                                using (Graphics graphics = Graphics.FromImage(image))
                                {
                                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                                    graphics.DrawImage(bitmap2, 0, 0, width, height);
                                }
                                height = num4;
                                using (Graphics graphics = Graphics.FromImage(bitmap2))
                                {
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    graphics.DrawImage(image, 0, 0, width, height);
                                }
                                if (_width > 0 && _height > 0)
                                {
                                    width = _width;
                                    height = _height;
                                }
                                rect = new Rectangle(0, 0, width, height);
                            }
                            else
                            {
                                rect = ((_width <= 0 || _height <= 0) ? new Rectangle(0, 0, bitmap2.Width, bitmap2.Height) : new Rectangle(0, 0, _width, _height));
                            }
                            bitmap = bitmap2.Clone(rect, PixelFormat.Format24bppRgb);
                            if (File.Exists(_FilePath))
                            {
                                File.SetAttributes(_FilePath, FileAttributes.Normal);
                            }
                            if (!string.IsNullOrWhiteSpace(_FilePath))
                            {
                                bitmap.Save(_FilePath, _ImageFormat);
                            }
                        }
                        catch (Exception){}

                        mFrame2.FrameRelease();
                        Marshal.ReleaseComObject(mFrame2);
                    }
                }
                catch (Exception){}
            }
        }

        public static bool Capture(object _objMixer, string _FilePath, decimal _durationSec = default(decimal), string _Prop = "format='mpeg' video::b='256K' audio::codec='null' video::codec='mpeg2video' video::video_size='200x150' ")
        {
            if (_objMixer != null && !string.IsNullOrWhiteSpace(_FilePath))
            {
                m_objWriter = new MWriterClass();
                m_objWriter.PropsSet("max_duration", _durationSec.ToString());
                m_objWriter.WriterNameSet(_FilePath, _Prop);
                m_objWriter.ObjectStart(_objMixer);
                return true;
            }
            return false;
        }

        public static void StopCapture()
        {
            if (m_objWriter != null)
            {
                m_objWriter.ObjectClose();
                m_objWriter = null;
            }
        }


        public static void ComposerSceneSet(string _sceneId, string _script)
        {
            if (Control == null) return;
            IMElements mElements = null;
            try { Control.ScenesRemove(_sceneId); } catch { }
            Control.ScenesAdd(!string.IsNullOrEmpty(_script)?_script:AppData.Scene.switcher, _sceneId, out mElements);
            Control.ScenesActiveSet(_sceneId, "");
        }

        private static void GetElement(out MElement mElement, string[] arr)
        {
            mElement = null;
            try
            {
                Control.ScenesActiveGet(out string _, out int _, out IMElements iMElements);
                for (int i = 0; i < arr.Length; i++)
                {
                    iMElements.ElementsGetByID(arr[i], out mElement);
                    if (mElement == null) { return; }
                    iMElements = (IMElements)mElement;
                }
            }
            catch { }
            //ReleaseComObject(iMElements);
        }


        public static void AppendTransition(string _script)
        {
            GetElement(out MElement mElement, AppData.strScene_3d);
            if (mElement != null)
            {
                ((IMElements)mElement).ElementsAdd(AppData.Tag.scene_3d_trn_transition, AppData.Tag.scene_3d_group, AppData.Props_Transition, out mElement, 0.0);
                ((IMElements)mElement).ElementsAdd(AppData.Tag.scene_3d_trn_virtual, AppData.Tag.scene_3d_group, AppData.Props_Transition, out mElement, 0.0);
                mElement.ElementSet("", _script, 0.0);

                try { ((IMElements)mElement).ElementsGetByID("LayerA_A", out MElement e1); if (e1 != null) { e1.AttributesBoolSet("show", 0); ReleaseComObject(e1); } } catch { }
                try { ((IMElements)mElement).ElementsGetByID("LayerB_B", out MElement e2); if (e2 != null) { e2.AttributesBoolSet("show", 0); ReleaseComObject(e2); } } catch { }
            }
            ReleaseComObject(mElement);
        }

        public static void ResetTransition()
        {
            GetElement(out MElement mElement, AppData.strTransitionTrans);
            if (mElement != null)
            {
                mElement.ElementDetach(0.0);
            }
            ReleaseComObject(mElement);
        }


        public static void AppendTransitionView(string _script)
        {
            GetElement(out MElement mElement, AppData.strTransitionView);
            if (mElement != null)
            {
                mElement.ElementSet("", _script, 0.0);
            }
            ReleaseComObject(mElement);
        }

        public static void ResetTransitionView()
        {
            MElement element = null;
            GetElement(out MElement mElement, AppData.strTransitionView);
            if (mElement != null)
            {
                ((IMElements)mElement).ElementsGetCount(out int count);
                for(int i = count; i > 0; i--)
                {
                    ((IMElements)mElement).ElementsGetByIndex(i-1, out element);
                    if (element != null) { element.ElementDetach(0.0); }
                }
            }
            ReleaseComObject(element);
            ReleaseComObject(mElement);
        }


        public static void CreateStreams(List<Stream> mixerStreams)
        {
            try
            {
                MItem mItem;
                if (mixerStreams.Count < 1) return;
                foreach (Stream s in mixerStreams)
                {
                    if (File.Exists(s.Path))
                    {
                        MHelper.Control.StreamsAdd(s.StreamId, null, s.Path, "", out mItem, 0.0);
                        if (mItem != null)
                        {
                            ((IMProps)mItem).PropsSet("object::audio_gain", "-60");
                            ((IMProps)mItem).PropsSet("object::internal.convert_frame", "true");
                            mItem.FileRateSet(1.0);
                            if (s.Cmd == "0") { mItem.FilePlayPause(0.0); } else if (s.Cmd == "1") { ((IMProps)mItem).PropsSet("loop", "true"); }
                            Thread.Sleep(20);
                            Marshal.ReleaseComObject(mItem);
                        }
                    }
                }
                if (_mixerList != null) _mixerList.UpdateList(false);
            }
            catch { }
        }

        public static void RemoveStreams(List<Stream> mixerStreams)
        {
            try
            {
                int num;
                MItem mItem = null;
                Stream s;
                while (mixerStreams.Count > 0)
                {
                    s = mixerStreams[0];
                    Control.StreamsGet(s.StreamId, out num, out mItem);
                    if (mItem != null)
                    {
                        try { Control.StreamsRemove(mItem, 0.0); mixerStreams.Remove(s); } catch { }
                    }
                }

                if (mItem != null) Marshal.ReleaseComObject(mItem);
                _mixerList.UpdateList(false);
            }
            catch { }
        }

        public static void RemoveAllStreams()
        {
            int num;
            MItem mItem = null;
            string str;
            Control.StreamsGetCount(out num);
            while (num > 0)
            {
                Control.StreamsGetByIndex(0, out str, out mItem);
                if (!string.IsNullOrEmpty(str))
                {
                    Control.StreamsGet(str, out num, out mItem);
                    if (mItem != null)
                    {
                        Control.StreamsRemove(mItem, 0.0);
                    }
                }
                Control.StreamsGetCount(out num);
            }

            if (mItem != null) Marshal.ReleaseComObject(mItem);
            _mixerList.UpdateList(false);
        }


        public static void ReleaseComObject(Object obj)
        {
            if (obj != null)
            {
                Marshal.ReleaseComObject(obj);
            }
        }


        public static MComposerElementsTree mElementTree
        {
            get { return _tree; }
            set { _tree = value; _tree.SetControlledObject(Control); }
        }

        public static MStreamsList mMixerList
        {
            get { return _mixerList; }
            set { _mixerList = value; _mixerList.SetControlledObject(Control); }
        }

        public static MAttributesList mAttributesList
        {
            get { return _attrList; }
            set { _attrList = value; _tree.AfterSelect += new TreeViewEventHandler(AfterSelect);
                //_attrList.ElementDescriptors = MHelpers.MComposerElementDescriptors;
                _tree.ElementDescriptors = MHelpers.MComposerElementDescriptors;
            }
        }

        public static TextBox mElementText
        {
            get { return _txtXml; }
            set { _txtXml = value; }
        }

        private static void AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                MElement pElement = (MElement)e.Node.Tag;
                _attrList.SetControlledObject(pElement);
                string strType, strXML;
                pElement.ElementGet(out strType, out strXML);
                if (_txtXml != null) _txtXml.Text = strXML;
            }
            catch (Exception) { }
        }

        public static void UpdateTransitionXML()
        {
            MElement pSelElement = _tree.SelectedElement;
            if (pSelElement != null)
            {
                pSelElement.ElementSet("", _txtXml.Text, 2.0);
            }
            _tree.UpdateTree(true);
        }


        public static void Apply_Transition(int _index, double _efxDuration, out double _duration, out double _hold)
        {
            _hold = 0.0;
            _duration = 0.0;

            GetElement(out MElement mElement, AppData.strTransitionView);
            if (mElement != null)
            {
                MElement element = null;
                try { ((IMElements)mElement).ElementsGetByIndex(_index, out element); } catch { }
                if (element != null)
                {
                    element.AttributesDoubleGet("duration", out _duration);
                    element.AttributesDoubleGet("hold", out _hold);
                    if (_duration < 0.0) _duration = (0.0 - _duration) * _efxDuration;
                    element.ElementInvoke("select", "", _duration);
                }
            }
        }
        

        public static int mTransitionViewCount
        {
            get
            {
                GetElement(out MElement mElement, AppData.strTransitionView);
                if (mElement != null)
                {
                    ((IMElements)mElement).ElementsGetCount(out int count);
                    return count;
                }
                return 0;
            }
        }

        public static bool HideLayerA
        {
            set
            {
                GetElement(out MElement element, AppData.strTransitionA);
                if (element != null)
                {
                    AppData.LayerA_State = !value;
                    element.ElementBoolSet("show", value ? 1 : 0, 0.0);
                    if (!string.IsNullOrEmpty(AppData.strLayerA_img_filter))
                        element.ElementStringSet("img_filter", AppData.strLayerA_img_filter, 0.0);
                }
            }
        }

        public static bool HideLayerB
        {
            set
            {
                GetElement(out MElement element, AppData.strTransitionB);
                if (element != null)
                {
                    AppData.LayerB_State = !value;
                    element.ElementBoolSet("show", value ? 1 : 0, 0.0);
                }
            }
        }

        public static void Swap()
        {
            string a = AppData.strLayerA;
            //string b = AppData.strLayerB;
            AppData.strLayerA = AppData.strLayerB;
            AppData.strLayerB = a;

            GetElement(out MElement element, AppData.strTransitionA);
            if (element != null) { element.ElementStringSet("stream_id", AppData.strLayerA, 0.0); }

            GetElement(out element, AppData.strTransitionB);
            if (element != null) { element.ElementStringSet("stream_id", AppData.strLayerB, 0.0); }

            //UpdateInScene(a, "_x");
            //UpdateInScene(b, a);
            //UpdateInScene("_x", b);
        }

        public static void SetLayerA(string _streamId)
        {
            GetElement(out MElement element, AppData.strTransitionA);
            if (element != null)
            {
                element.ElementStringSet("stream_id", _streamId, 0.0);
                AppData.strLayerA = _streamId;
            }
        }

        public static void SetLayerB(string _streamId)
        {
            GetElement(out MElement element, AppData.strTransitionB);
            if (element != null)
            {
                element.ElementStringSet("stream_id", _streamId, 0.0);
                AppData.strLayerB = _streamId;
            }
        }

        public static void AddLicense()
        {
            MPlatformSDKLic.IntializeProtection();
            DecoderlibLic.IntializeProtection();
            EncoderlibLic.IntializeProtection();
            MComposerlibLic.IntializeProtection();
        }

        public static void UpdateInScene(string _old, string _new)
        {
            if (_old == _new) return;
            try
            {
                GetElement(out MElement element, AppData.strTransitionVirtual);
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
            for(int i=0; i < n; i++)
            {
                els.ElementsGetByIndex(i, out el);
                el.ElementTypeGet(out string t);
                el.AttributesHave("stream_id", out int m, out string v);
                if (t == "video" && v == _old) el.AttributesStringSet("stream_id", _new);

                ReplaceLayerStream((IMElements)el, _old, _new);
            }
        }

        public static string GetEffectScript
        {
            get
            {
                GetElement(out MElement element, AppData.strTransitionVirtual);
                if (element != null) { element.ElementGet(out string _, out string xml); return xml; } else return string.Empty;
            }
            set
            {
                GetElement(out MElement element, AppData.strTransitionVirtual);
                if (element != null) { element.ElementSet("", value, 0.0); }
            }
        }
    }
}

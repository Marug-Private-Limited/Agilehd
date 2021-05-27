using MControls;
using MPLATFORMLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class MPHelper
{
    #region MComposer
    private static MComposerClass _composer;

    private static MComposerClass _composer1;

    public static MComposerClass Control => _composer;

    public static MComposerClass Control1 => _composer1;//For CG-Editor Updated on 20/02/19 by Hari

    public static void Init()
    {
        _composer = new MComposerClass();
        ComposerSceneSet("AgileHD", Defines.Scenes.switcher);

       AgileHDWPF.AgileSng.MHelper.Init(_composer);

        _composer1 = new MComposerClass();
        ComposerSceneSet("AgileHD", Defines.Scenes.switcher);

        //Mep_Agh.AuraSng.MHelper.Init(_composer1);
    }

    private static MPreviewClass _preview;
    public static MPreviewClass _Preview => _preview;

    public static void InitPreview(Panel pnl)
    {
        if (_preview == null)
        {
            _preview = new MPreviewClass();
            _preview.PreviewWindowSet("", pnl.Handle.ToInt32());
            _preview.PreviewEnable("", 0, 1);

            ((IMProps)_preview).PropsSet("maintain_ar", "letter-box");
            ((IMProps)_preview).PropsSet("deinterlace", "true");
        }
    }

    private static MPreviewClass _cgpreview;
    public static MPreviewClass _CGPreview => _cgpreview;

    public static void InitCGPreview(Panel pnl)
    {
        if (_cgpreview == null)
        {
            _cgpreview = new MPreviewClass();
            _cgpreview.PreviewWindowSet("", pnl.Handle.ToInt32());
            _cgpreview.PreviewEnable("", 0, 1);

            ((IMProps)_cgpreview).PropsSet("maintain_ar", "letter-box");
            ((IMProps)_cgpreview).PropsSet("deinterlace", "true");
        }
    }

    public static int s;
    public static void ScreenShot()
    {

        try
        {
            string folder = "D:\\ahd_assets\\capture\\";
            int fileNumber = s++;

            string filename = folder + "snap" + "_" + fileNumber + ".jpg";
            fileNumber = fileNumber + 1;
            // MessageBox.Show(filename);
            CaptureFrame(filename);

        }
        catch { }
        //SaveFileDialog sfd = new SaveFileDialog();
        //sfd.Filter = Defines.Filters.ScreenShot;

        //if (sfd.ShowDialog() == DialogResult.OK)
        //{

        //    try
        //    {
        //        MFrame pFrame;
        //        ((IMObject)_preview).ObjectFrameGet(out pFrame, "");
        //        pFrame.FrameVideoSaveToFile(sfd.FileName);
        //        MPHelper.ReleaseObject(pFrame);
        //    }
        //    catch { }
        //}
    }

    public static bool EnableFullScreen
    {
        get
        {
            string x;
            ((IMProps)_preview).PropsGet("full_screen", out x);
            return Convert.ToBoolean(x);
        }
        set
        {
            try
            {
                ((IMProps)_preview).PropsSet("full_screen", value ? "true" : "false");
            }
            catch (Exception ex)
            {
                ExceptionLog.Create(ex);
            }
        }
    }

    public static bool EnableDeinterlace
    {
        get
        {
            string x;
            ((IMProps)_preview).PropsGet("deinterlace", out x);
            return Convert.ToBoolean(x);
        }
        set { ((IMProps)_preview).PropsSet("deinterlace", value ? "true" : "false"); }
    }

    public static bool EnableAspectRatio
    {
        get
        {
            string x;
            ((IMProps)_preview).PropsGet("maintain_ar", out x);
            return (x.ToLower() == "letter-box");
        }
        set
        {
            ((IMProps)_preview).PropsSet("maintain_ar", value ? "letter-box" : "none");
        }
    }

    public static bool EnableAudio
    {
        set { _preview.PreviewEnable("", value ? 1 : 0, 1); }
        //set { ((IMProps)_preview).PropsSet("audio_gain", value ? "0" : "-100"); }
    }

    public static void EnableAV(bool audio, bool video)
    {
        _preview.PreviewEnable("", Convert.ToInt16(audio), Convert.ToInt16(video));
    }
    #endregion

    #region MControl
    public static void FindMFile(IMStreams _streams, string _streamId, out MItem _MFile)
    {
        _MFile = null;
        try
        {
            int num = 0;
            MItem mItem = null;
            _streams.StreamsGet(_streamId, out num, out mItem);
            _MFile = mItem;
        }
        catch (Exception ex)
        {
            ExceptionLog.Create(ex);
        }
    }

    public static void ComposerStreamsGet(string _bsStreamID, out int _pnIndex, out MItem _ppStream)
    {
        _ppStream = null;
        _pnIndex = 0;
        try
        {
            Control.StreamsGet(_bsStreamID, out _pnIndex, out _ppStream);
        }
        catch //(Exception ex)
        {
            //ExceptionLog.Create(ex);
        }
    }

    public static void ComposerStreamsAdd(string _bsStreamID, object _pExternOrRef, string _bsPath, string _bsParam, out MItem _ppItem, double _dblTimeForChange)
    {
        _ppItem = null;
        try
        {
            Control.StreamsAdd(_bsStreamID, _pExternOrRef, _bsPath, _bsParam, out _ppItem, _dblTimeForChange);
        }
        catch (COMException ex)
        {
            ExceptionLog.Create(ex);
        }
        if (_mixerList != null) _mixerList.UpdateList(false);
    }

    public static void ComposerStreamsRemove(string _bsStreamID, double _dblTimeForChange)
    {
        MItem _ppItem = null;
        int num = 0;
        try
        {
            Control.StreamsGet(_bsStreamID, out num, out _ppItem);
            if (_ppItem != null) { Control.StreamsRemove(_ppItem, 0.0); }
        }
        catch (Exception ex) { ExceptionLog.Create(ex); }
        if (_mixerList != null) _mixerList.UpdateList(false);
    }
    #endregion

    #region Stills
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
        catch (Exception) { }

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
                    catch (Exception) { }

                    mFrame2.FrameRelease();
                    Marshal.ReleaseComObject(mFrame2);
                }
            }
            catch (Exception) { }
        }
    }

    public static void CaptureFrame(string strPath)
    {
        MFrame pMFrame, frame;
        _composer.ObjectFrameGet(out pMFrame, "");
        ((MFrame)pMFrame).FrameClone(out frame, eMFrameClone.eMFC_Full, eMFCC.eMFCC_ARGB32);

        M_AV_PROPS props;
        frame.FrameAVPropsGet(out props);

        int cbVideo = 0;
        long pbVideo = 0;
        frame.FrameVideoGetBytes(out cbVideo, out pbVideo);

        try
        {
            int bitsPerPixel = ((int)PixelFormat.Format32bppArgb & 0xff00) >> 8;
            int bytesPerPixel = (bitsPerPixel + 7) / 8;
            int stride = 4 * ((props.vidProps.nWidth * bytesPerPixel + 3) / 4);
            int width = props.vidProps.nWidth;
            int height = props.vidProps.nHeight > 0 ? props.vidProps.nHeight : props.vidProps.nHeight * -1;

            Bitmap bmp = new Bitmap(width, height, stride, PixelFormat.Format32bppArgb, (IntPtr)pbVideo);
            bmp.Save(strPath, ImageFormat.Png);
        }
        catch (Exception) { }

        frame.FrameRelease();
        pMFrame.FrameRelease();
        ReleaseObject(frame);
        ReleaseObject(pMFrame);
    }
    #endregion

    #region Writer
    private static MWriterClass m_objWriter;

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
    #endregion

    #region Release Object & Memories
    public static void ReleaseObject(object obj)
    {
        try
        {
            if (obj != null)
            {
                Marshal.ReleaseComObject(obj);
            }
            GC.Collect();
        }
        catch { }
    }
    #endregion

    #region Secene Configurations
    private static MStreamsList _mixerList;
    private static MComposerElementsTree _tree;
    private static MAttributesList _attrList;
    private static TextBox _txtXml;

    public static void ComposerSceneSet(string _sceneId, string _script)
    {
        if (Control == null) return;
        IMElements mElements = null;
        try { Control.ScenesRemove(_sceneId); } catch { }

        Control.ScenesAdd(!string.IsNullOrEmpty(_script) ? _script : Defines.Scenes.switcher, _sceneId, out mElements);
        Control.ScenesActiveSet(_sceneId, string.Empty);
        ReleaseObject(mElements);
    }

    public static void GetElement(out MElement mElement, string[] arr)
    {
        mElement = null;

        _composer.ScenesActiveGet(out string s, out int _, out IMElements iMElements);
        for (int i = 0; i < arr.Length; i++)
        {
            try { iMElements.ElementsGetByID(arr[i], out mElement); } catch { }
            if (mElement == null) { return; }
            iMElements = (IMElements)mElement;
        }

        //ReleaseComObject(iMElements);
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
        set
        {
            _attrList = value; _tree.AfterSelect += new TreeViewEventHandler(AfterSelect);
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
    #endregion

    #region Video Switch
    public static void SetLayerA(string _streamId)
    {
        //string x = Defines.strLayerA;

        GetElement(out MElement element, Defines.strTransitionA);
        if (element != null)
        {
            //SetFreeze(false);
            element.ElementStringSet("stream_id", _streamId, 0.0);
            Defines.strLayerA = _streamId;
            //if (Helper.Master.fPanel1.chkFreeze.Checked)
            //{
            //    _composer.FilePlayPause(0.0);
            //    //Thread.Sleep(50);
            //    SetFreeze(true);
            //}

            if (Helper.EfxRunning != 0)
            {
                UpdateInScene("a"); //x, _streamId, 
            }
        }
    }

    public static void SetLayerB(string _streamId)
    {
        //string x = Defines.strLayerB;

        GetElement(out MElement element, Defines.strTransitionB);
        if (element != null)
        {
            element.ElementStringSet("stream_id", _streamId, 0.0);
            Defines.strLayerB = _streamId;

            if (Helper.EfxRunning != 0) //x, _streamId
            {
                UpdateInScene("b");
            }
        }
    }

    public static bool HideLayerA
    {
        set
        {
            GetElement(out MElement element, Defines.strTransitionA);
            if (element != null)
            {
                Defines.LayerA_State = !value;
                element.ElementBoolSet("show", value ? 0 : 1, 0.0);
                if (!string.IsNullOrEmpty(Defines.strLayerA_img_filter))
                    element.ElementStringSet("img_filter", Defines.strLayerA_img_filter, 0.0);
            }
        }
    }

    public static bool HideLayerB
    {
        set
        {
            GetElement(out MElement element, Defines.strTransitionB);
            if (element != null)
            {
                Defines.LayerB_State = !value;
                element.ElementBoolSet("show", value ? 0 : 1, 0.0);
            }
        }
    }

    public static void Swap()
    {
        string a = Defines.strLayerA;
        Defines.strLayerA = Defines.strLayerB;
        Defines.strLayerB = a;

        GetElement(out MElement element, Defines.strTransitionA);
        if (element != null) { element.ElementStringSet("stream_id", Defines.strLayerA, 0.0); }

        GetElement(out element, Defines.strTransitionB);
        if (element != null) { element.ElementStringSet("stream_id", Defines.strLayerB, 0.0); }
    }
    #endregion

    #region Audio Switch
    public static void SetAudioLayerA(string _streamId, bool _enable = true, double _delay = 0.0)
    {
        try
        {
            GetElement(out MElement element, Defines.strAudioA);
            if (element != null)
            {
                string x = string.Format(@"stream_id='{0}' audio_gain={1}", _streamId, _enable ? 0 : -80);
                element.ElementMultipleSet(x, _delay);
                Defines.strAudioLayerA = _streamId;
            }
        }
        catch { }
    }

    public static void SetAudioLayerA(bool _enable = false, double _delay = 0.0)
    {
        try
        {
            GetElement(out MElement element, Defines.strAudioA);
            if (element != null)
            {
                element.ElementStringSet("audio_gain", _enable ? "0" : "-80", _delay);
            }
        }
        catch { }
    }

    public static void SetAudioLayerB(string _streamId, bool _enable = true, double _delay = 0.0)
    {
        try
        {
            GetElement(out MElement element, Defines.strAudioB);
            if (element != null)
            {
                string x = string.Format(@"stream_id='{0}' audio_gain={1}", _streamId, _enable ? 0 : -80);
                element.ElementMultipleSet(x, _delay);
                Defines.strAudioLayerB = _streamId;
            }
        }
        catch { }
    }

    public static void SetAudioLayerB(bool _enable = false, double _delay = 0.0)
    {
        try
        {
            GetElement(out MElement element, Defines.strAudioB);
            if (element != null)
            {
                element.ElementStringSet("audio_gain", _enable ? "0" : "-80", _delay);
            }
        }
        catch { }
    }
    #endregion

    #region External Audio  //edited by ramesh
    public static void SetExternalAudio(bool _enable = true, double _delay = 0.0)
    {
        try
        {
            GetElement(out MElement element, Defines.strExtAudio);
            if (element != null)
            {
                string x = string.Format(@"stream_id='{0}' audio_gain={1}", Defines.Stream.strExtAudio, _enable ? 0 : -80);
                element.ElementMultipleSet(x, _delay);
            }
        }
        catch { }
    }
    #endregion

    #region Color Mixer
    private static CoMColorsClass _color = new CoMColorsClass();
    public static CoMColorsClass mColors => _color;

    #endregion

    #region Properties
    public static List<string> lstStream = null;
    public static bool MuteAllStreams
    {
        set
        {
            MItem mItem;
            string _;
            if (value) { lstStream = new List<string>(); }

            _composer.StreamsGetCount(out int n);
            for (int i = 0; i < n; i++)
            {
                _composer.StreamsGetByIndex(i, out _, out mItem);
                if (value) { mItem.FilePlayStop(0.0); lstStream.Add(_); }
                else { mItem.FilePlayStart(); }
            }
        }
    }
    #endregion

    #region Freeze
    public static void SetFreeze(bool _enable)
    {
        MElement element = null;
        MItem pItem = null;
        if (_enable)
        {
            _composer.FilePlayPause(0.0);
            if (File.Exists(Defines.Asset.freeze_file))
                File.Delete(Defines.Asset.freeze_file);
            _composer.FilePlayStart();
            Thread.Sleep(100);
            CaptureFrame(Defines.Asset.freeze_file);

            _composer.StreamsAdd(Defines.Asset.freeze_stream, null, Defines.Asset.freeze_file, "", out pItem, 0.0);

            ////GetElement(out element, Defines.strAfxMedia);
            GetElement(out element, Defines.strTransitionA);
            if (element != null)
            {
                string x = string.Format(@"stream_id='{0}' show='{1}'", Defines.Asset.freeze_stream, "true");
                element.ElementMultipleSet(x, 0.0);
            }

            //_composer.FilePlayStart();
        }
        else
        {
            //GetElement(out element, Defines.strAfxMedia);
            GetElement(out element, Defines.strTransitionA);
            if (element != null)
            {
                //element.ElementBoolSet("show", 0, 0.0);
                string x = string.Format(@"stream_id='{0}'", Defines.strLayerA);
                element.ElementMultipleSet(x, 0.0);
            }

            //_composer.StreamsGet(Defines.Asset.freeze_stream, out int i, out pItem); commented on 27 Nov 18 by Hari
            //if (pItem != null)
            //{
            //    _composer.StreamsRemove(pItem, 0.0);
            //}
        }

        ReleaseObject(element);
        ReleaseObject(pItem);
    }
    #endregion

    #region Filters
    public static void SetMotionFilter(bool _enable, string _file)
    {
        MElement element = null;
        MItem pItem = null;
        if (_enable)
        {
            if (string.IsNullOrEmpty(_file) && !File.Exists(_file)) return;

            _composer.StreamsAdd(Defines.Asset.filter_stream, null, _file, "", out pItem, 0.0);

            GetElement(out element, Defines.strMotionFilter);
            if (element != null)
            {
                string x = string.Format(@"stream_id='{0}' show='{1}'", Defines.Asset.filter_stream, "true");
                element.ElementMultipleSet(x, 0.5);
            }
        }
        else
        {
            GetElement(out element, Defines.strMotionFilter);
            if (element != null)
            {
                element.ElementBoolSet("show", 0, 0.5);
            }
            Thread.Sleep(1500);
            _composer.StreamsGet(Defines.Asset.filter_stream, out int i, out pItem);
            if (pItem != null)
            {
                _composer.StreamsRemove(pItem, 0.0);
            }
        }

        ReleaseObject(element);
        ReleaseObject(pItem);
    }

    public static void SetColorFilter(bool _enable, string _color)
    {
        try
        {
            MElement element = null;
            if (_enable)
            {
                GetElement(out element, Defines.strTransitionA);
                if (element != null)
                {
                    element.ElementMultipleSet("img_filter = '" + _color + "'", 0.0);
                    Defines.strLayerA_img_filter = _color;
                }
            }
            else
            {
                GetElement(out element, Defines.strTransitionA);
                if (element != null)
                {
                    element.AttributesRemove("img_filter");
                    Defines.strLayerA_img_filter = string.Empty;
                    //element.ElementMultipleSet("img_filter = 'FFFFFF' ", 0.0);
                    //Defines.strLayerA_img_filter = "FFFFFF";
                }
            }

            ReleaseObject(element);

            if (mElementTree != null) { mElementTree.UpdateTree(false); }
        }
        catch (Exception ex) { ExceptionLog.Create(ex); }
    }

    static CoMColorsClass m_colorsBW = new CoMColorsClass();
    public static void SetBlockAndWhite(bool _enable)
    {
        if (_enable)
        {
            //if(m_colorsBW == null) { m_colorsBW = new CoMColorsClass(); }
            stream_plugin(Defines.strLayerA).PluginsAdd(m_colorsBW, 0);
            Thread.Sleep(50);
            stream_plugin(Defines.strLayerA).PluginsRemove(m_colorsBW);

            MG_COLOR_PARAM cParam = new MG_COLOR_PARAM
            {
                dblUlevel = 0,
                dblUVGain = 0,
                dblVlevel = 0,
                dblYGain = 1,
                dblYlevel = 0
            };
            MG_BRIGHT_CONT_PARAM bcParam = new MG_BRIGHT_CONT_PARAM
            {
                dblWhiteLevel = 1,
                dblContrast = 0,
                dblColorGain = 1,
                dblBrightness = 0,
                dblBlackLevel = 0
            };

            m_colorsBW.SetColorParam(cParam, 1, 0);
            m_colorsBW.SetBrightContParam(bcParam, 1, 0);

            stream_plugin(Defines.strLayerA).PluginsAdd(m_colorsBW, 0);
        }
        else
        {
            stream_plugin(Defines.strLayerA).PluginsRemove(m_colorsBW);
            //stream_plugin(Defines.strLayerA).PluginsRemove(m_colorsBW);
            Thread.Sleep(50);
        }
    }

    static IMPlugins stream_plugin(string _streamId)
    {
        FindMFile(Control, _streamId, out MItem mItem);
        if (mItem != null)
        {
            return (mItem as IMPlugins);
        }
        return null;
    }

    //public static void ApplyColorFilterToTransition()
    //{
    //    GetElement(out MElement mElement, Defines.strTransitionVirtual);
    //    if (mElement != null)
    //    {

    //    }
    //    ReleaseObject(mElement);

    //    if (mElementTree != null) { mElementTree.UpdateTree(false); }

    //}

    public static void ApplyColorFilter()
    {
        GetElement(out MElement mElement, Defines.strTransitionVirtual);
        IMElements els = (IMElements)mElement;
        //if (els == null) return;
        MElement el;
        els.ElementsGetCount(out int n);
        for (int i = 0; i < n; i++)
        {
            els.ElementsGetByIndex(i, out el);
            el.ElementTypeGet(out string t);
            el.AttributesHave("stream_id", out int m, out string v);
            //if (t == "video" && (v == "LA" || v == Defines.strLayerA)) el.AttributesStringSet("img_filter", Defines.strLayerA_img_filter);
            if (t == "video" && (v == Defines.strLayerA)) el.AttributesStringSet("img_filter", Defines.strLayerA_img_filter);
        }
    }

    public static void RemoveColorFilter()
    {
        GetElement(out MElement mElement, Defines.strTransitionVirtual);
        IMElements els = (IMElements)mElement;
        //if (els == null) return;
        MElement el;
        els.ElementsGetCount(out int n);
        for (int i = 0; i < n; i++)
        {
            els.ElementsGetByIndex(i, out el);
            el.ElementTypeGet(out string t);
            el.AttributesHave("stream_id", out int m, out string v);
            if (t == "video" && (v == "LA" || v == Defines.strLayerA))
            {
                el.AttributesHave("img_filter", out int j, out string x);
                if (!string.IsNullOrEmpty(x)) el.AttributesRemove("img_filter");
                //el.ElementMultipleSet("img_filter='FFFFFF' ", 0.0);
            }
        }
    }
    #endregion

    #region FTB
    public static void SetFTB(bool _enable)
    {
        MElement element = null;
        MItem pItem = null;

        if (_enable)
        {
            HideLayerB = true;
            _composer.StreamsAdd(Defines.Asset.ftb_stream, null, "black", "", out pItem, 0.0);
            GetElement(out element, Defines.strAfxMedia);
            if (element != null)
            {
                string x = string.Format(@"stream_id='{0}' show='{1}'", Defines.Asset.ftb_stream, "true");
                element.ElementMultipleSet(x, 2.0);
            }

            //Helper.Master.aMeter1.mAudioMeter1.FadeInOut(2.0, true, true);
        }
        else
        {
            GetElement(out element, Defines.strAfxMedia);
            if (element != null)
            {
                element.ElementBoolSet("show", 0, 2.0);
            }
            Thread.Sleep(2 * 1000);
            _composer.StreamsGet(Defines.Asset.ftb_stream, out int i, out pItem);
            if (pItem != null)
            {
                _composer.StreamsRemove(pItem, 0.0);
            }
            HideLayerB = false;
            //Helper.Master.aMeter1.mAudioMeter1.FadeInOut(2.0, false, false);
        }

        ReleaseObject(element);
        ReleaseObject(pItem);
    }
    #endregion

    #region Mixer
    public static void SetMixerMode(bool _enable)
    {
        MElement element = null;
        if (_enable)
        {
            GetElement(out element, Defines.strTransitionB);
            if (element != null)
            {
                element.ElementBoolSet("show", 0, 0.0);
            }

            GetElement(out element, Defines.strTransitionA);
            if (element != null)
            {
                element.ElementStringSet("alpha", "0.0", 1.0);
            }
        }
        else
        {
            GetElement(out element, Defines.strTransitionA);
            if (element != null)
            {
                element.ElementStringSet("alpha", "1.0", 1.0);
            }

            GetElement(out element, Defines.strTransitionB);
            if (element != null)
            {
                element.ElementBoolSet("show", 1, 2.0);
            }
        }

        if (mElementTree != null) mElementTree.UpdateTree(false);
    }
    #endregion

    #region Effects
    public static void AppendTransition(string _script)
    {
        GetElement(out MElement mElement, Defines.strScene_3d);
        if (mElement != null)
        {
            mElement.ElementSet("", string.Empty, 0.0);
            ((IMElements)mElement).ElementsAdd(Defines.Tag.scene_3d_trn_transition, Defines.Tag.scene_3d_group, Defines.Props_Transition, out mElement, 0.0);
            ((IMElements)mElement).ElementsAdd(Defines.Tag.scene_3d_trn_virtual, Defines.Tag.scene_3d_group, Defines.Props_Transition, out mElement, 0.0);
            mElement.ElementSet("", _script, 0.0);
        }
        ReleaseObject(mElement);

        if (mElementTree != null) { mElementTree.UpdateTree(false); }
    }

    public static void ResetTransition()
    {
        GetElement(out MElement mElement, Defines.strTransitionTrans);
        if (mElement != null)
        {
            mElement.ElementDetach(0.0);
        }
        ReleaseObject(mElement);

        if (mElementTree != null) { mElementTree.UpdateTree(false); }
    }


    public static void AppendTransitionView(string _script)
    {
        GetElement(out MElement mElement, Defines.strTransitionView);
        if (mElement != null)
        {
            mElement.ElementSet("", _script, 0.0);
        }
        ReleaseObject(mElement);

        if (mElementTree != null) { mElementTree.UpdateTree(false); }
    }

    public static void ResetTransitionView()
    {
        MElement element = null;
        GetElement(out MElement mElement, Defines.strTransitionView);
        if (mElement != null)
        {
            ((IMElements)mElement).ElementsGetCount(out int count);
            for (int i = count; i > 0; i--)
            {
                ((IMElements)mElement).ElementsGetByIndex(i - 1, out element);
                if (element != null) { element.ElementDetach(0.0); }
            }
        }
        ReleaseObject(element);
        ReleaseObject(mElement);

        if (mElementTree != null) { mElementTree.UpdateTree(false); }
    }


    public static void CreateStreams(List<StreamItem> mixerStreams)
    {
        MItem mItem;
        foreach (StreamItem s in mixerStreams)
        {
            if (File.Exists(s.Path))
            {
                _composer.StreamsAdd(s.StreamId, null, s.Path, "", out mItem, 0.0);
                if (mItem != null)
                {
                    if (s.Cmd == "0") { mItem.FilePlayPause(0.0); }
                    Thread.Sleep(10);
                    Marshal.ReleaseComObject(mItem);
                }
            }
        }
        if (_mixerList != null) _mixerList.UpdateList(false);
    }

    //public static void ResetStreamsToPlay(List<StreamItem> mixerStreams)
    //{
    //    MItem mItem;
    //    foreach (StreamItem s in mixerStreams)
    //    {
    //        if (File.Exists(s.Path))
    //        {
    //            //_composer.StreamsAdd(s.StreamId, null, s.Path, "", out mItem, 0.0);
    //            _composer.StreamsGet(s.StreamId, out int _, out mItem);
    //            if (mItem != null)
    //            {
    //                if (s.Cmd == "0") { mItem.FilePlayStart(); ((IMProps)mItem).PropsSet("loop", "true"); }
    //                Thread.Sleep(10);
    //                Marshal.ReleaseComObject(mItem);
    //            }
    //        }
    //    }
    //    if (_mixerList != null) _mixerList.UpdateList(false);
    //}
    public static void ResetStreamsToPlay(List<StreamItem> mixerStreams)
    {
        MPlaylistClass mPlaylist;
        MItem mItem, pItem = null;
        double num4 = 0.0;
        double _sp = 1.0;

        M_DATETIME mStartTime = new M_DATETIME(); mPlaylist = new MPlaylistClass();
        foreach (StreamItem s in mixerStreams)
        {
            if (File.Exists(s.Path))
            {
                //_composer.StreamsAdd(s.StreamId, null, s.Path, "", out mItem, 0.0);
                _composer.StreamsGet(s.StreamId, out int _, out mItem);
                if (mItem != null)
                {
                    //mItem.FilePlayStop(0.0);
                    mItem.FilePosSet(0.0, 0.0);
                    //((IMProps)mItem).PropsSet("loop", "true");

                    ///////////////
                    num4 = Convert.ToDouble(s.Cmd);
                    //num4 += 2.3;
                    _sp = 0;

                    if (num4.ToString().IndexOf(".") > 0)
                    {
                        string[] c_arr = num4.ToString().Split('.');
                        //if (c_arr.Length > 0)
                        //{
                        //    _sp = num4 * 1000.0 % 1000.0 / 100.0;
                        //}

                        num4 = Convert.ToDouble(c_arr[0]);
                        _sp = Convert.ToDouble(c_arr[1]);
                    }

                    if (num4 == 0)
                    {
                        mItem.FilePlayPause(0.0);
                    }
                    else
                    {
                        ((IMProps)mItem).PropsSet("loop", (num4 == 2) ? "true" : "false");

                        if (_sp != 0)
                        {
                            //Thread.Sleep((int)_sp * 1000);

                            DateTime date = DateTime.Now.AddSeconds(_sp);
                            mStartTime.nHour = date.Hour;
                            mStartTime.nMinute = date.Minute;
                            mStartTime.nSecond = mStartTime.nSecond + date.Second;// + Convert.ToInt32(_sp);

                            int nIndex = -1;
                            mPlaylist.PlaylistAdd(mItem, "", "", ref nIndex, out mItem);

                            mPlaylist.PlaylistCommandAdd("stop", "", null, 0, out mItem);
                            mItem.ItemCommandSet("stop", _sp.ToString(), null); // to stop for n seconds
                            //mPlaylist.FilePlayStart();

                            //mItem.ItemStartTimeSet(ref mStartTime, eMStartType.eMST_Off);
                            mItem.FilePosSet(0.0, 0.0);

                            //Thread.Sleep(20);
                            //Marshal.ReleaseComObject(pItem);
                            mItem.FilePlayStart();
                        }
                        else
                            mItem.FilePlayStart();
                    }
                    ///////////////
                    // -- old >> if (s.Cmd == "0") { mItem.FilePlayStart(); ((IMProps)mItem).PropsSet("loop", "true"); }
                    Thread.Sleep(10);
                    Marshal.ReleaseComObject(mItem);
                }
            }
        }

        if (pItem != null)
            Marshal.ReleaseComObject(pItem);
        if (_mixerList != null) _mixerList.UpdateList(false);
    }

    public static void RemoveStreams(List<StreamItem> mixerStreams)
    {
        int num;
        MItem mItem;
        foreach (StreamItem s in mixerStreams)
        {
            Control.StreamsGet(s.StreamId, out num, out mItem);
            if (mItem != null)
            {
                Control.StreamsRemove(mItem, 0.0);
                Marshal.ReleaseComObject(mItem);
            }
        }
        if (_mixerList != null) _mixerList.UpdateList(false);
    }


    public static void Apply_Transition(int _index, double _efxDuration, out double _duration, out double _hold)
    {
        _hold = 0.0;
        _duration = 0.0;

        GetElement(out MElement mElement, Defines.strTransitionView);
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
            GetElement(out MElement mElement, Defines.strTransitionView);
            if (mElement != null)
            {
                ((IMElements)mElement).ElementsGetCount(out int count);
                return count;
            }
            return 0;
        }
    }


    public static string GetEffectScript
    {
        get
        {
            GetElement(out MElement element, Defines.strTransitionVirtual);
            if (element != null) { element.ElementGet(out string _, out string xml); return xml; } else return string.Empty;
        }
        set
        {
            GetElement(out MElement element, Defines.strTransitionVirtual);
            if (element != null) { element.ElementSet(string.Empty, value, 0.0); }
            if (mElementTree != null) mElementTree.UpdateTree(true);
        }
    }

    public static string GetViewScript
    {
        get
        {
            GetElement(out MElement element, Defines.strTransitionView);
            if (element != null) { element.ElementGet(out string _, out string xml); return xml; } else return string.Empty;
        }
        set
        {
            GetElement(out MElement element, Defines.strTransitionView);
            if (element != null) { element.ElementSet(string.Empty, value, 0.0); }
            if (mElementTree != null) mElementTree.UpdateTree(true);
        }
    }

    public static void UpdateInScene(string _channel) //string _old, string _new,
    {
        //if (_old == _new) return;
        try
        {
            GetElement(out MElement element, Defines.strTransitionVirtual);
            if (element != null)
            {
                ReplaceLayerStream((IMElements)element, _channel); //_old, _new,
            }
        }
        catch { }
    }

    static void ReplaceLayerStream(IMElements els, string _channel) //string _old, string _new, 
    {
        if (els == null) return;
        MElement el;
        els.ElementsGetCount(out int n);
        for (int i = 0; i < n; i++)
        {
            els.ElementsGetByIndex(i, out el);
            el.ElementTypeGet(out string t);
            el.AttributesHave("id", out int _, out string _i);

            switch (t)
            {
                case "video":
                    if (_channel == "a") { if (THelper.replace_a.Contains(_i)) { el.AttributesStringSet("stream_id", Defines.strLayerA); } }
                    else if (_channel == "b") { if (THelper.replace_b.Contains(_i)) { el.AttributesStringSet("stream_id", Defines.strLayerB); } }
                    break;

                case "object":
                    if (_channel == "a") { if (THelper.replace_a.Contains(_i)) { el.AttributesStringSet("LayerA.png", Defines.strLayerA); } }
                    else if (_channel == "b") { if (THelper.replace_b.Contains(_i)) { el.AttributesStringSet("LayerB.png", Defines.strLayerB); } }
                    break;
            }
            //el.AttributesHave("stream_id", out int m, out string v);
            //if (t == "video" && v == _old) el.AttributesStringSet("stream_id", _new);
            ReplaceLayerStream((IMElements)el, _channel);
        }
    }
    #endregion
}

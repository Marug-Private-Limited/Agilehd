using MPLATFORMLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace AgileHDWPF.AgileSng
{
    /// <summary>
    /// Interaction logic for SngPlayer.xaml
    /// </summary>
    public partial class TitlePlayer1 : System.Windows.Controls.UserControl
    {
        System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();

        string _parent = AgileSng.AppData.Asset.title_project_path;
        string _sngItemPath = "";
        string _sngItemName = "";
        string _video = "", _vLoop = "0", _vMute = "0";
        string _audio = "", _aLoop = "0", _aMute = "0";

        int _pIndex = -1;
        int _cIndex = -1;

        public TitlePlayer1()
        {
            InitializeComponent();
            /// Initialize background file control.
            v = new AgileHDWPF.AgileSng.vFile();
            v.btnVb.Click += new EventHandler(bgBrowse_Click);
            v.btnVb.Click += new EventHandler(bgBrowse_Click);
            od = new OpenFileDialog();
            od.Multiselect = false;
            //timer1.Tick += new EventHandler(timer1_Tick);
            //timer1.Interval = 100;
        }

        private void GalaryScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToHorizontalOffset(scv.HorizontalOffset - e.Delta);
            e.Handled = true;
        }

        #region Song Load
        public void LoadParent()
        {
            DirectoryInfo di = new DirectoryInfo(_parent);

            foreach (DirectoryInfo d in di.GetDirectories())
            {
                flpParent.Controls.Add(CreateTitle(d.Name, d.FullName));
                //ComboBoxItem item = new ComboBoxItem();
                //item.Content = d.Name;
                //item.Tag = d.FullName;
                //cmbSongDirectories.Items.Add(item);
            }
        }
        System.Windows.Forms.Button CreateTitle(string _title, string _path)
        {
            try
            {
                System.Windows.Forms.Button bt = new System.Windows.Forms.Button();

                bt.Text = _title.ToUpper();
                bt.Tag = _path;
                bt.Width = 100;
                bt.Height = 30;
                bt.BackColor = Defines.Theme.TG_Parent_H;
                bt.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                bt.FlatStyle = FlatStyle.Flat;
                bt.FlatAppearance.BorderSize = 0;
                bt.Click += new EventHandler(FolderClick);

                return bt;
            }
            catch { }
            return null;
        }
        void FolderClick(object sender, EventArgs e)
        {
            System.Windows.Forms.Button bt = sender as System.Windows.Forms.Button;
            GItem gi;

            /// Highlight color.
            foreach (System.Windows.Forms.Control c in flpParent.Controls)
            {
                ((System.Windows.Forms.Button)c).BackColor = Defines.Theme.TG_Parent_H;
            }
            bt.BackColor = Defines.Theme.TG_Parent_D;

            _pIndex = flpParent.Controls.GetChildIndex(bt);
            _cIndex = -1;

            /// Reset songs and lists.
           // flpItems.Controls.Clear();
            flpChild.Children.Clear();
            _sngItemPath = bt.Tag.ToString();

            _sngItemName = bt.Text.ToString();

            FillSongGallery();
        }
        private void CmbSongDirectories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)cmbSongDirectories.SelectedItem;
            //_sngItemPath = item.Tag.ToString();
            //_sngItemName = item.Content.ToString();
            FillSongGallery();
        }

        private void FillSongGallery()
        {
            string tempDir = @"D:\ahd_assets\temp\" + _sngItemName;
            string tempfile = tempDir;
            foreach (AgileSng.GItem1 gItem in flpChild.Children)
            {
                gItem.pic1.Source = null;
            }
            flpChild.Children.Clear();

            AgileSng.GItem1 g;
            System.IO.Stream s = null;
            DirectoryInfo di = new DirectoryInfo(_sngItemPath);
            if (!Directory.Exists(tempDir))
            {
                //var dir = new DirectoryInfo(tempDir);
                //dir.Delete(true);
                Directory.CreateDirectory(tempDir);
            }

            foreach (FileInfo f in di.GetFiles("*.ahs"))
            {
                g = new AgileSng.GItem1();
                g.pic1.Tag = f.FullName.Replace(f.Extension, ".ahs");
                g.btn1.Content = f.Name.Replace(f.Extension, "");
                g.AllowToDrage = false;
                //g.onClick += new EventHandler(LoadSongItems);

                /// Show first item picture.
                XmlDocument xd = new XmlDocument();
                string _file;
                try { xd.Load(f.FullName); } catch { }
                if (xd != null)
                {
                    try
                    {
                        XmlNode xn = xd.GetElementsByTagName("items")[0];
                        if (xn != null && xn.HasChildNodes && xn.ChildNodes[0].Name == "item")
                        {
                            _file = (xn.ChildNodes[0].Attributes["file"] != null) ? xn.ChildNodes[0].Attributes["file"].Value.ToLower() : "";

                            s = File.Open(_file.Replace(".ahp", ".ahi"), FileMode.Open);
                            BitmapFrame source = BitmapFrame.Create(s, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                            //tempfile = tempDir;

                            //string fname = _file.Substring(_file.LastIndexOf(@"\") + 1);

                            //tempfile = string.Format("{0}\\{1}", tempfile, fname.Replace(".ahp", ".ahi"));
                            //tempfile = tempfile.Replace(".ahi", ".jpg");
                            //_file = _file.Replace(".ahp", ".ahi");
                            //if (!File.Exists(tempfile))
                            //{
                            //    File.Copy(_file, tempfile);
                            //}

                            g.pic1.Source = source;//new BitmapImage(new Uri(tempfile)); // (ImageSource)converter.ConvertFrom(s); 
                            g.pic1.Stretch = Stretch.Uniform;
                            g.Height = 110;
                            g.Width = 100;
                            g.Margin = new Thickness(5, 2, 5, 2);
                            g.MouseDoubleClick += new MouseButtonEventHandler(LoadSongItems);
                            //g.btn1.Click += new RoutedEventHandler(LoadSongItems);
                            s.Close(); s.Dispose();
                        }
                    }
                    catch (Exception ex) { }
                }

                flpChild.Children.Add(g);
            }
        }

        private void LoadSongItems(object sender, EventArgs e)
        {
            AgileSng.GItem1 gi = sender as AgileSng.GItem1;

            _cIndex = flpChild.Children.IndexOf(gi);

            LoadSongDetail(gi.pic1.Tag.ToString());
        }

        void SelectedItem(object sender, EventArgs e)
        {
            AgileSng.GItem1 g = sender as AgileSng.GItem1;

            /// Highlight selected item.
            foreach (AgileSng.GItem1 c in flpTransChild.Children)
            {
                ((AgileSng.GItem1)c).btn1.Background = Defines.Theme.SG_Item_D; //Defines.Theme.TG_Child_D;
            }
            g.btn1.Background = Defines.Theme.SG_Item_H; //Defines.Theme.TG_Child_H;
            f_index = flpTransChild.Children.IndexOf(g);
            //f_index++;
            btnRemove.Visibility = Visibility.Visible;
            //MessageBox.Show(g.btn1.Tag.ToString());
        }

        void LoadSongDetail(string fn)
        {
            //string tempDir = @"D:\ahd_assets\temp\" + _sngItemName;
            //string tempfile = tempDir;
            /// Reset list.
            flpTransChild.Children.Clear();

            //btnVideo.Visible = btnAudio.Visible = false;
            //btnRemove.Visible = false;
            _video = _audio = "";
            _vLoop = _vMute = _aLoop = _aMute = "0";


            /// Show all items.
            XmlDocument xd = new XmlDocument();
            string _file;
            System.IO.Stream fs = null;
            AgileSng.GItem1 g;
            try
            {
                //xd.Load(gi.pic1.Tag.ToString());
                xd.Load(fn);
                if (xd != null)
                {
                    //btnVideo.Visible = btnAudio.Visible = true;

                    /// Background file details.
                    XmlNode xn = xd.GetElementsByTagName("video")[0];
                    if (xn != null)
                    {
                        _video = (xn.Attributes["path"] != null) ? xn.Attributes["path"].Value.ToLower() : "";
                        _vLoop = (xn.Attributes["loop"] != null) ? xn.Attributes["loop"].Value : "0";
                        _vMute = (xn.Attributes["mute"] != null) ? xn.Attributes["mute"].Value : "0";
                    }
                    xn = xd.GetElementsByTagName("audio")[0];
                    if (xn != null)
                    {
                        _audio = (xn.Attributes["path"] != null) ? xn.Attributes["path"].Value.ToLower() : "";
                        _aLoop = (xn.Attributes["loop"] != null) ? xn.Attributes["loop"].Value : "0";
                        _aMute = (xn.Attributes["mute"] != null) ? xn.Attributes["mute"].Value : "0";
                    }

                    /// Song sub items.
                    xn = xd.GetElementsByTagName("items")[0];
                    foreach (XmlNode x in xn.ChildNodes)
                    {
                        g = new AgileSng.GItem1();
                        if (x != null && x.Name == "item")
                        {
                            _file = (x.Attributes["file"] != null) ? x.Attributes["file"].Value.ToLower() : "";

                            if (!string.IsNullOrEmpty(_file))
                            {
                                FileInfo f = new FileInfo(_file);
                                //arrSng.Add(f.FullName);

                                g.btn1.Tag = f.FullName.Replace(f.Extension, ".ahp");
                                g.btn1.Content = f.Name.Replace(f.Extension, "");

                                fs = File.Open(_file.Replace(".ahp", ".ahi"), FileMode.Open);

                                BitmapFrame source = BitmapFrame.Create(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                                //tempfile = tempDir;

                                //string fname = _file.Substring(_file.LastIndexOf(@"\") + 1);

                                //tempfile = string.Format("{0}\\{1}", tempfile, fname.Replace(".ahp", ".ahi"));
                                //tempfile = tempfile.Replace(".ahi", ".jpg");
                                //_file = _file.Replace(".ahp", ".ahi");
                                //if (!File.Exists(tempfile))
                                //{
                                //    File.Copy(_file, tempfile);
                                //}

                                g.pic1.Source = source;//new BitmapImage(new Uri(tempfile)); // (ImageSource)converter.ConvertFrom(s); 
                                g.pic1.Stretch = Stretch.Uniform;
                                g.Height = 110;
                                g.Width = 100;
                                g.Margin = new Thickness(5, 2, 5, 2);
                                g.MouseDoubleClick += new MouseButtonEventHandler(SelectedItem);
                                fs.Close();
                                fs.Dispose();
                            }
                        }

                        flpTransChild.Children.Add(g);
                    }
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region Player
        short status = 0;
        int f_index = 0;
        int t_index = 0;
        string efx_script;

        Thread thrd_1, thrd_2, thrd_3;
        AutoResetEvent are_1 = new AutoResetEvent(false);
        //AutoResetEvent are_2 = new AutoResetEvent(false);

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                if (MPHelper.Control == null || flpTransChild.Children.Count < 1 || status == 1) return;
                btnAudio.IsEnabled = btnVideo.IsEnabled = btnInsert.IsEnabled = btnAppend.IsEnabled = btnSave.IsEnabled = btnRemove.IsEnabled = false;
                if (status == 0)
                {
                    f_index = 0;
                    status = 1;
                    //timer1.Start();
                    PlaySong();
                    PlayEffect();
                }
                else
                {
                    status = 1;
                    MPHelper.Control.PropsSet("pause_changes", "1");
                    MPHelper.Control.FilePlayStart();

                    MItem pItem = null;
                    int n;
                    foreach (string s in MPHelper.lstStream)
                    {
                        MPHelper.Control.StreamsGet(s, out n, out pItem);
                        if (pItem != null) { pItem.FilePlayPause(0.0); }
                    }
                    MHelper.ReleaseComObject(pItem);

                    /// Resume threads.
                    if (thrd_1 != null) { thrd_1.Resume(); }
                    if (thrd_2 != null) { thrd_2.Resume(); }
                }

                btnPause.Visibility = Visibility.Visible;
                btnPlay.Visibility = Visibility.Hidden;
            }
            catch { }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                if (status != 1) return;

                status = -1;

                /// Pause transtions.
                MPHelper.Control.PropsSet("pause_changes", "2");
                MPHelper.Control.FilePlayPause(0.0);

#pragma warning disable CS0618 // 'Thread.Suspend()' is obsolete: 'Thread.Suspend has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202'
                /// Pause threads.
                if (thrd_1 != null) { thrd_1.Suspend(); }
#pragma warning restore CS0618 // 'Thread.Suspend()' is obsolete: 'Thread.Suspend has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202'
#pragma warning disable CS0618 // 'Thread.Suspend()' is obsolete: 'Thread.Suspend has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202'
                if (thrd_2 != null) { thrd_2.Suspend(); }
#pragma warning restore CS0618 // 'Thread.Suspend()' is obsolete: 'Thread.Suspend has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202'


                btnPause.Visibility = Visibility.Hidden;
                btnPlay.Visibility = Visibility.Visible;
            }
            catch { }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (status == 0) return;

                if (thrd_3 != null) try { thrd_3.Abort(); } catch { }
                thrd_3 = new Thread(StopSong);
                thrd_3.Start();

                //f_index = 0;
                //t_index = 0;
                //status = 0;
                //btnPause.Visible = false;
                //btnPlay.Visible = true;
                //timer1.Stop();

                ////Thread.Sleep(50);
                ////thrdReset.Reset();
                ////thrdReset.Close();
                //new Thread(ChangeToSwitcherMode).Start();
                ////ChangeToSwitcherMode();              
                btnAudio.IsEnabled = btnVideo.IsEnabled = btnInsert.IsEnabled = btnAppend.IsEnabled = btnSave.IsEnabled = btnRemove.IsEnabled = true;
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (f_index >= flpTransChild.Children.Count) { btnStop_Click(sender, e); return; }

            status = 1;
            //if (f_index == 0) new Thread(ChangeToSongMode).Start();                
            if (f_index == 0) ChangeToSongMode();
            new Thread(ReadEffect).Start();
        }

        void ReadEffect()
        {
            AgileSng.GItem1 g = (AgileSng.GItem1)flpTransChild.Children[f_index];
            SelectedItem(flpTransChild.Children[f_index], null);
            AgileSng.SHelper.LoadFile(g.btn1.Tag.ToString());
            efx_script = AgileSng.MHelper.GetEffectScript;

            //new Thread(PlayEffect).Start();
            PlayEffect();
        }

        void PlayEffect()
        {
            double _hold = 0.0;
            double _duration = 0.0;

            t_index = 0;
            try
            {
                AgileSng.MHelper.GetEffectScript = efx_script;

                while (t_index < AgileSng.MHelper.mTransitionViewCount)
                {
                    if (t_index == 0)
                    {
                        if (v_pItem != null) v_pItem.FilePlayStart();
                        if (a_pItem != null) a_pItem.FilePlayStart();
                    }

                    _hold = 0.0;
                    _duration = 0.0;
                    AgileSng.MHelper.Apply_Transition(t_index, (double)AgileSng.SHelper.mTranstionDuration, out _duration, out _hold);
                    if (_hold < 0.0) _hold = (0.0 - _hold) * _duration;
                    Thread.Sleep((Int32)(_duration + _hold) * 1000);

                    while (true)
                    {
                        if (status < 0) Thread.Sleep(40); else break;
                    }
                    t_index++;
                }
            }
            catch { }

            try
            {
                Dispatcher.BeginInvoke((MethodInvoker)delegate
                {
                    Thread.Sleep(50);
                    AgileSng.TrnsHelper.Transition_remove();
                    f_index++;
                    Thread.Sleep(20);
                    if (status == 1) timer1.Start();
                });
            }
            catch { }
        }
        #endregion

        #region Switcher Controls
        IMElements spro_scene, active_scene = null;
        MItem v_pItem = null, a_pItem = null;
        //Form1 Frm;

        string active_scene_id = "";
        string scene_id = "sPro";


        public void SetControlledObject(object obj)
        {
            // Frm = (Form1) obj;
            AgileSng.MHelper.Init(MPHelper.Control);
            //Sng.MHelper.mMixerList = Form1.sConfig.mStreamsList1;
            //Sng.MHelper.mElementTree = Form1.sConfig.mComposerElementsTree1;
            //Sng.MHelper.mAttributesList = Form1.sConfig.mAttributesList1;
        }

        void ChangeToSongMode()
        {
            Dispatcher.BeginInvoke((MethodInvoker)delegate
            {
                int nIndex;
                if (active_scene_id == "") { MPHelper.Control.ScenesActiveGet(out active_scene_id, out nIndex, out active_scene); }

                /// Mute all streams
                //if (Helper.Master != null)
                //{
                //Frm.MuteAll(true);
                //Frm.MFC1.Enabled = Frm.MFC2.Enabled = Frm.MFC3.Enabled = Frm.MFC4.Enabled = Frm.MFC5.Enabled = Frm.MFC6.Enabled = false;
                //Frm.sequencer1.Enabled = false;
                //Frm.mPlaylistTimeline1.Enabled = Frm.mPlaylistTimeline2.Enabled = false;
                //Frm.FPanel1.Enabled = Frm.EPanel1.Enabled = false;
                //Frm.TGallery1.Enabled = false;
                //Frm.btnTitle.Enabled = Frm.btnRemoveCG.Enabled = Frm.btnCg.Enabled = Frm.btnSCapture.Enabled = Frm.btnCMix.Enabled = Frm.btnAMix.Enabled = false;

                MPHelper.MuteAllStreams = true;
                Helper.Master.L2.IsEnabled = false;
                //Helper.Master.mPlaylistTimeline1.Enabled = Helper.Master.mPlaylistTimeline2.Enabled = false;
                //}

                MPHelper.Control.ScenesAdd(Defines.Scenes.song, scene_id, out spro_scene);
                MPHelper.Control.ScenesActiveSet(scene_id, "");
                //Form1.sConfig.mScenesCombo1.UpdateCombo();

                v_pItem = null;
                a_pItem = null;
                if (!string.IsNullOrEmpty(_video)) StreamsAdd(AgileSng.AppData.Song.Stream.SBV, _video, _vLoop, _vMute, out v_pItem);
                if (!string.IsNullOrEmpty(_audio)) StreamsAdd(AgileSng.AppData.Song.Stream.SBA, _audio, _aLoop, _aMute, out a_pItem);

                //Form1.sConfig.mStreamsList1.UpdateList(false);
                Helper.IsSongPlaying = true;
            });
        }

        void ChangeToSwitcherMode()
        {
            if (string.IsNullOrEmpty(active_scene_id)) return;

            try
            {
                AgileSng.MHelper.Control.ScenesActiveSet(active_scene_id, "");
                AgileSng.MHelper.Control.ScenesRemove(scene_id);
            }
            catch { };

            /// Remove all song streams.
            if (!string.IsNullOrEmpty(_video)) { MPHelper.ComposerStreamsRemove(AgileSng.AppData.Song.Stream.SBV, 0.0); AgileSng.MHelper.ReleaseComObject(v_pItem); }
            if (!string.IsNullOrEmpty(_audio)) { MPHelper.ComposerStreamsRemove(AgileSng.AppData.Song.Stream.SBA, 0.0); AgileSng.MHelper.ReleaseComObject(a_pItem); }

            //if (Frm != null)
            //{
            //Frm.MuteAll(false);
            MPHelper.MuteAllStreams = false;
            Helper.Master.L2.IsEnabled = true;
            //Helper.Master.mPlaylistTimeline1.Enabled = Helper.Master.mPlaylistTimeline2.Enabled = true;

            ///// Enable all other options.
            //Frm.MFC1.Enabled = Frm.MFC2.Enabled = Frm.MFC3.Enabled = Frm.MFC4.Enabled = Frm.MFC5.Enabled = Frm.MFC6.Enabled = true;
            //Frm.sequencer1.Enabled = true;
            //Frm.mPlaylistTimeline1.Enabled = Frm.mPlaylistTimeline2.Enabled = true;
            //Frm.FPanel1.Enabled = Frm.EPanel1.Enabled = true;
            //Frm.TGallery1.Enabled = true;
            //Frm.btnTitle.Enabled = Frm.btnRemoveCG.Enabled = Frm.btnCg.Enabled = Frm.btnSCapture.Enabled = Frm.btnCMix.Enabled = Frm.btnAMix.Enabled = true;
            //}
            //Form1.sConfig.mScenesCombo1.UpdateCombo();
            //Form1.sConfig.mStreamsList1.UpdateList(false);
            Helper.IsSongPlaying = false;

            status = 0;
            v_pItem = a_pItem = null;
        }

        void StreamsAdd(string _stream, string _path, string _loop, string _mute, out MItem _pItem)
        {
            //MItem 
            _pItem = null;
            try
            {
                MPHelper.Control.StreamsAdd(_stream, null, _path, "", out _pItem, 0.0);
                if (_pItem != null)
                {
                    _pItem.FilePlayPause(0);
                    ((IMProps)_pItem).PropsSet("loop", (_loop == "1") ? "true" : "false");
                    ((IMProps)_pItem).PropsSet("object::audio_gain", (_mute == "1") ? "-80" : "0");
                }
            }
            catch { }
        }
        #endregion

        #region Play Song Transitions
        int _TransCount = 0;
        WrapPanel _TransChild;
        void PlaySong()
        {
            /// Mute all streams
            //if (Frm != null)
            //{
            //Frm.MuteAll(true);
            Dispatcher.BeginInvoke((MethodInvoker)delegate
            {
                MPHelper.MuteAllStreams = true;
                //Helper.Master.btnCGEditor.IsEnabled = false;
                //Helper.Master.btnAudioChanel.IsEnabled = false;
                //Helper.Master.btnVideoMixer.IsEnabled = false;
                Helper.Master.mPlaylistTimeline1.Enabled = false;
                Helper.Master.mPlaylistTimeline2.Enabled = false;
                //Helper.Master.btnTitlesequencer.IsEnabled = false;
                Helper.Master.btnSongProject.IsEnabled = true;
            });

            //Frm.MFC1.Enabled = Frm.MFC2.Enabled = Frm.MFC3.Enabled = Frm.MFC4.Enabled = Frm.MFC5.Enabled = Frm.MFC6.Enabled = false;
            //Frm.sequencer1.Enabled = false;
            //Frm.mPlaylistTimeline1.Enabled = Frm.mPlaylistTimeline2.Enabled = false;
            //Frm.FPanel1.Enabled = Frm.EPanel1.Enabled = false;
            //Frm.TGallery1.Enabled = false;
            //Frm.btnTitle.Enabled = Frm.btnRemoveCG.Enabled = Frm.btnCg.Enabled = Frm.btnSCapture.Enabled = Frm.btnCMix.Enabled = Frm.btnAMix.Enabled = false;
            //}

            /// Add background video and audio streams.
            v_pItem = null;
            a_pItem = null;
            if (!string.IsNullOrEmpty(_video)) StreamsAdd(AgileSng.AppData.Song.Stream.SBV, _video, _vLoop, _vMute, out v_pItem);
            if (!string.IsNullOrEmpty(_audio)) StreamsAdd(AgileSng.AppData.Song.Stream.SBA, _audio, _aLoop, _aMute, out a_pItem);
            //Form1.sConfig.mStreamsList1.UpdateList(false);

            if (string.IsNullOrEmpty(_video))
            {
                a_pItem.OnFrame -= Helper.Master.MainObject_OnFrameSafe;
                Helper.Master.AudMain.SetControlledObject(a_pItem);
                a_pItem.OnFrame += new IMEvents_OnFrameEventHandler(Helper.Master.MainObject_OnFrameSafe);
            }
            else
            {
                v_pItem.OnFrame -= Helper.Master.MainObject_OnFrameSafe;
                Helper.Master.AudMain.SetControlledObject(v_pItem);
                v_pItem.OnFrame += new IMEvents_OnFrameEventHandler(Helper.Master.MainObject_OnFrameSafe);
            }

            /// Get active scene.
            int nIndex;
            if (active_scene_id == "") { MPHelper.Control.ScenesActiveGet(out active_scene_id, out nIndex, out active_scene); }

            /// Change to song scene.
            MPHelper.Control.ScenesAdd(Defines.Scenes.song, scene_id, out spro_scene);
            MPHelper.Control.ScenesActiveSet(scene_id, "");
            //Form1.sConfig.mScenesCombo1.UpdateCombo();

            /// Set to song playing mode.
            Helper.IsSongPlaying = true;

            ///Get Trans Count
            _TransCount = flpTransChild.Children.Count;
            _TransChild = flpTransChild;

            /// Read song transitions.
            thrd_1 = new Thread(ParentThread);
            thrd_1.Start();
        }

        void ParentThread()
        {

            //AgileAgileSng.GItem11 g;
            string file = "";
            while (f_index < _TransCount)
            {
                /// Read transition file.
                Dispatcher.Invoke(() =>
                {
                    are_1 = new AutoResetEvent(false);
                    AgileSng.GItem1 g = (AgileSng.GItem1)flpTransChild.Children[f_index];
                    //g = (AgileAgileSng.GItem11)_TransChild.Children[f_index];
                    SelectedItem(g, null);
                    file = g.btn1.Tag.ToString();
                });

                if (!string.IsNullOrEmpty(file))
                {
                    AgileSng.SHelper.LoadFile(file);
                    efx_script = AgileSng.MHelper.GetEffectScript;

                    thrd_2 = new Thread(ChildThread);
                    thrd_2.Start();

                    are_1.WaitOne();
                    Thread.Sleep(20);

                    AgileSng.TrnsHelper.Transition_remove();
                    f_index++;
                }
            }

            thrd_3 = new Thread(StopSong);
            thrd_3.Start();
        }

        void ChildThread()
        {
            double _hold = 0.0;
            double _duration = 0.0;

            t_index = 0;
            AgileSng.MHelper.GetEffectScript = efx_script;
            while (AgileSng.MHelper.mTransitionViewCount > t_index)
            {
                if (t_index == 0)
                {
                    if (v_pItem != null) v_pItem.FilePlayStart();
                    if (a_pItem != null) a_pItem.FilePlayStart();
                }

                _hold = 0.0;
                _duration = 0.0;
                AgileSng.MHelper.Apply_Transition(t_index, (double)AgileSng.SHelper.mTranstionDuration, out _duration, out _hold);
                if (_hold < 0.0) _hold = (0.0 - _hold) * _duration;
                Thread.Sleep((Int32)(_duration + _hold) * 1000);

                while (true) { if (status < 0) Thread.Sleep(40); else break; }
                t_index++;
            }

            are_1.Set();
        }

        void StopSong()
        {
            /// Stop all threads.
            if (thrd_1 != null) { try { thrd_1.Abort(); } catch { } }
            if (thrd_2 != null) { try { thrd_2.Abort(); } catch { } }
            if (are_1 != null) { try { are_1.Close(); } catch { } }

            /// Remove all song streams.
            if (!string.IsNullOrEmpty(_video)) { MPHelper.ComposerStreamsRemove(AppData.Song.Stream.SBV, 0.0); AgileSng.MHelper.ReleaseComObject(v_pItem); }
            if (!string.IsNullOrEmpty(_audio)) { MPHelper.ComposerStreamsRemove(AgileSng.AppData.Song.Stream.SBA, 0.0); AgileSng.MHelper.ReleaseComObject(a_pItem); }

            /// Return to switcher mode.
            if (string.IsNullOrEmpty(active_scene_id)) return;
            MPHelper.Control.ScenesActiveSet(active_scene_id, "");
            try { AgileSng.MHelper.Control.ScenesRemove(scene_id); } catch { };
            //Form1.sConfig.mScenesCombo1.UpdateCombo();
            //Form1.sConfig.mStreamsList1.UpdateList(false);

            //if (Frm != null)
            //{
            Dispatcher.BeginInvoke((MethodInvoker)delegate
            {
                MPHelper.MuteAllStreams = false;
                //Helper.Master.btnCGEditor.IsEnabled = true;
                //Helper.Master.btnAudioChanel.IsEnabled = true;
                //Helper.Master.btnVideoMixer.IsEnabled = true;
                Helper.Master.mPlaylistTimeline1.Enabled = true;
                Helper.Master.mPlaylistTimeline2.Enabled = true;
                //Helper.Master.btnTitlesequencer.IsEnabled = true;
            });

            //BeginInvoke((MethodInvoker)delegate { 
            //    /// Enable all other options.
            //    Frm.MFC1.Enabled = Frm.MFC2.Enabled = Frm.MFC3.Enabled = Frm.MFC4.Enabled = Frm.MFC5.Enabled = Frm.MFC6.Enabled = true;
            //    Frm.sequencer1.Enabled = true;
            //    Frm.mPlaylistTimeline1.Enabled = Frm.mPlaylistTimeline2.Enabled = true;
            //    Frm.FPanel1.Enabled = Frm.EPanel1.Enabled = true;
            //    Frm.TGallery1.Enabled = true;
            //    Frm.btnTitle.Enabled = Frm.btnRemoveCG.Enabled = Frm.btnCg.Enabled = Frm.btnSCapture.Enabled = Frm.btnCMix.Enabled = Frm.btnAMix.Enabled = true;

            //    btnPause.Visible = false; btnPlay.Visible = true;
            //});
            //}

            v_pItem = a_pItem = null;
            Helper.IsSongPlaying = false;
            status = 0; f_index = 0; t_index = 0;

            try
            {
                Dispatcher.BeginInvoke((MethodInvoker)delegate { btnPause.Visibility = Visibility.Hidden; btnPlay.Visibility = Visibility.Visible; });
            }
            catch { }
        }
        #endregion

        #region Make Song List
        int FindIndex(double x)
        {
            foreach (AgileSng.GItem1 g in flpTransChild.Children)
            {
                var gitemLocation = g.TranslatePoint(new Point(0, 0), flpTransChild);
                if (x <= gitemLocation.X)
                    return flpTransChild.Children.IndexOf(g) - 1;
                else if (gitemLocation.X > x && (gitemLocation.X + g.Width) > x)
                    return flpChild.Children.IndexOf(g);
            }
            return flpTransChild.Children.Count;
        }

        private void flpTransChild_DragDrop(object sender, System.Windows.DragEventArgs e)
        {
            try
            {
                string strFile = (string)e.Data.GetData(System.Windows.DataFormats.Text);
                //FileInfo f = new FileInfo(strFile);
                //AgileSng.GItem1 g = new AgileSng.GItem1();
                //Stream s;
                //var mouseposition = Mouse.GetPosition(flpTransChild);
                int k = FindIndex(e.GetPosition(flpTransChild).X);
                //int k = FindIndex(this.PointToScreen(new Point(e.X, e.Y)).X);

                Insert(strFile, k);

                //s = File.Open(f.FullName.Replace(f.Extension, ".ahi"), FileMode.Open);
                //g.pic1.Tag = strFile;
                //g.pic1.BackgroundImage = Image.FromStream(s);
                //g.pic1.BackgroundImageLayout = ImageLayout.Center;
                //s.Close(); s.Dispose();

                //g.btn1.Tag = f.FullName.Replace(f.Extension, ".ahp");
                //g.btn1.Text = f.Name.Replace(f.Extension, "");
                //g.AllowToDrage = false;
                //g.onClick += new EventHandler(SelectedItem);

                //flpChild.Controls.Add(g);
                //flpChild.Controls.SetChildIndex(g, k);
            }
            catch (Exception ex) { }
        }

        private void flpTransChild_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(System.Windows.DataFormats.Text) && (System.Windows.DragDropEffects.Copy) != 0)
                    e.Effects = System.Windows.DragDropEffects.Copy;
            }
            catch { }
        }

        //private void flpChild_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        //{
        //    e.UseDefaultCursors = false;
        //    if ((e.Effect & DragDropEffects.Move) == DragDropEffects.Move)
        //        Cursor.Current = Cursors.Cross;
        //    else
        //        Cursor.Current = Cursors.Default;
        //}

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (flpTransChild.Children.Count == 0)
            {
                System.Windows.MessageBox.Show("Please select any one item!");
                return;
            }

            SaveFileDialog sd = new SaveFileDialog();
            sd.InitialDirectory = AgileSng.AppData.Asset.title_project_path + ((_pIndex != -1) ? @"\" + ((AgileSng.GItem1)flpChild.Children[_pIndex]).btn1.Content : "");
            sd.Filter = AgileSng.AppData.Filters.s_project;
            if (sd.ShowDialog() == DialogResult.OK)
            {
                FileInfo f = new FileInfo(sd.FileName);
                string x = string.Format(AgileSng.AppData.Song.Header, f.Name.Replace(f.Extension, ""), _video, _vLoop, _vMute, _audio, _aLoop, _aMute);
                string y = "";
                AgileSng.GItem1 g;
                foreach (AgileSng.GItem1 c in flpTransChild.Children)
                {
                    g = (AgileSng.GItem1)c;
                    y += string.Format(AgileSng.AppData.Song.Detail, g.btn1.Tag.ToString(), "0", "0");
                }
                x = x.Replace(@"<items/>", string.Format(@"<items>{0}</items>", y));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(x);
                doc.Save(sd.FileName);
                //doc.Save(string.Format(@"{0}\{1}{2}", txtPLoc.Text, txtPT.Text, AppData.Song.Extension));

                //if (_pIndex < flpParent.Controls.Count) { FolderClick(flpParent.Controls[_pIndex], null); if (_cIndex != -1) LoadSongItems(flpItems.Controls[_cIndex], null); }
                System.Windows.MessageBox.Show("Project saved successfully!");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (f_index < flpTransChild.Children.Count)
            {
                if (System.Windows.Forms.MessageBox.Show("Do you want to remove?", "Remove", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    flpTransChild.Children.RemoveAt(f_index);
                    btnRemove.Visibility = Visibility.Hidden;
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            //if (Frm.EPanel1.lblCode.Tag == null) return;
            //Insert(Frm.EPanel1.lblCode.Tag.ToString(), f_index);
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            //if (Frm.EPanel1.lblCode.Tag == null) return;
            //Insert(Frm.EPanel1.lblCode.Tag.ToString(), flpChild.Controls.Count);
        }

        void Insert(string _file, int i)
        {
            string tempDir = @"D:\ahd_assets\temp\" + _sngItemName;
            string tempfile = tempDir;

            FileInfo f = new FileInfo(_file);
            AgileSng.GItem1 g = new AgileSng.GItem1();
            //System.IO.Stream s;

            //s = File.Open(f.FullName.Replace(f.Extension, ".jpg"), FileMode.Open);

            string fname = f.FullName.Replace(f.Extension, ".ahi");

            tempfile = string.Format("{0}\\{1}", tempfile, f.Name.Replace(f.Extension, ".jpg"));
            if (!File.Exists(tempfile))
            {
                File.Copy(f.FullName, tempfile);
            }

            g.pic1.Tag = _file;
            g.pic1.Source = new BitmapImage(new Uri(tempfile));
            g.pic1.Stretch = Stretch.Uniform;
            //s.Close(); s.Dispose();

            g.btn1.Tag = f.FullName.Replace(f.Extension, ".ahp");
            g.btn1.Content = f.Name.Replace(f.Extension, "");
            g.Height = 210;
            g.Width = 203;
            g.Margin = new Thickness(5, 2, 5, 2);
            g.AllowToDrage = false;
            g.MouseDoubleClick += new MouseButtonEventHandler(SelectedItem);

            //flpChild.Children.Add(g);
            flpTransChild.Children.Insert(i, g);
        }
        #endregion

        #region Background Files
        private AgileSng.vFile v;
        private short fType = 0;

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {

        }

        private OpenFileDialog od;

        private void btnAudio_Click(object sender, EventArgs e)
        {
            fType = 2;
            v.Text = v.lblTitle.Text = "Audio File";
            v.txtVF.Text = _audio;
            v.chkVL.Checked = (_aLoop == "1") ? true : false;
            v.chkVM.Checked = (_aMute == "1") ? true : false;

            if (v.ShowDialog() == DialogResult.OK)
            {
                _audio = v.txtVF.Text;
                _aLoop = v.chkVL.Checked ? "1" : "0";
                _aMute = v.chkVM.Checked ? "1" : "0";
            }
        }

        private void btnVideo_Click(object sender, EventArgs e)
        {
            fType = 1;
            v.Text = v.lblTitle.Text = "Video File";
            v.txtVF.Text = _video;
            v.chkVL.Checked = (_vLoop == "1") ? true : false;
            v.chkVM.Checked = (_vMute == "1") ? true : false;

            if (v.ShowDialog() == DialogResult.OK)
            {
                _video = v.txtVF.Text;
                _vLoop = v.chkVL.Checked ? "1" : "0";
                _vMute = v.chkVM.Checked ? "1" : "0";
            }
        }

        private void bgBrowse_Click(object sender, EventArgs e)
        {
            od.Filter = (fType == 1) ? AgileSng.AppData.Filters.video : AgileSng.AppData.Filters.audio;
            v.txtVF.Text = string.Empty;
            if (od.ShowDialog() == DialogResult.OK) v.txtVF.Text = od.FileName;
        }
        #endregion

        private void btnOpen_Click(object sender, EventArgs e)
        {
            _pIndex = -1;
            _cIndex = -1;

            OpenFileDialog op = new OpenFileDialog();
            op.Filter = Defines.Filters.EfxSequence;
            op.InitialDirectory = Defines.Asset.song_project_path;
            op.Multiselect = false;
            if (op.ShowDialog() == DialogResult.OK)
            {
                LoadSongDetail(op.FileName);
            }
        }

        #region Variables
        public EventHandler onClick = null;
        bool _drag = true;
        #endregion

        #region Members
        public bool AllowToDrage
        {
            get { return _drag; }
            set { _drag = value; }
        }
        #endregion
    }
}
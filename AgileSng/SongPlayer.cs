using System;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using MPLATFORMLib;
using System.Collections.Generic;

namespace Mep_Agh.AuraSng
{
    public partial class SongPlayer : UserControl
    {
        public SongPlayer()
        {
            InitializeComponent();

            /// Initialize background file control.
            v = new Mep_Agh.AuraSng.vFile();
            v.btnVb.Click += new EventHandler(bgBrowse_Click);
            od = new OpenFileDialog();
            od.Multiselect = false;
        }
        
        #region Window events
        private void SongPlayer_Load(object sender, EventArgs e)
        {
        }

        private void SongPlayer_Resize(object sender, EventArgs e)
        {
            /// Resize panel height & width.
            flpItems.Width = pnlR2.Width + 20;
            flpItems.Height = pnlR2.Height;
            flpItems.PerformLayout();

            flpChild.Height = pnlR3.Height + 20;
            flpChild.Width = pnlR3.Width - 5;
            flpChild.PerformLayout();
        }
        #endregion

        #region Load Song Gallery
        string _video = "", _vLoop = "0", _vMute = "0";
        string _audio = "",  _aLoop = "0",  _aMute = "0";
        string _parent = "";

        int _pIndex = -1;
        int _cIndex = -1;

        public void LoadSongFolder()
        {
            _parent = AuraSng.AppData.Asset.song_project_path;

            /// Load song project folder items.
            new Thread(LoadParentFolders).Start();
        }

        public void LoadTitleFolder()
        {
            _parent = AuraSng.AppData.Asset.title_project_path;

            /// Load song project folder items.
            new Thread(LoadParentFolders).Start();
        }

        void LoadParentFolders()
        {
            flpParent.Controls.Clear();

            BeginInvoke((MethodInvoker)delegate {
                DirectoryInfo di = new DirectoryInfo(_parent);
                foreach (DirectoryInfo d in di.GetDirectories())
                {
                    flpParent.Controls.Add(CreateTitle(d.Name, d.FullName)); 
                }

                /// Select first node by default.
                if (flpParent.Controls.Count > 0) { FolderClick(flpParent.Controls[0], null); }
            });           
        }

        Button CreateTitle(string _title, string _path)
        {
            try
            {
                Button bt = new Button();
                bt.Text = _title.ToUpper();
                bt.Tag = _path;
                bt.Width = 100;
                bt.Height = 30;
                bt.BackColor = Defines.Theme .TG_Parent_H;
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
            try
            {
                Button bt = sender as Button;
                GItem gi;

                /// Highlight color.
                foreach (Control c in flpParent.Controls)
                {
                    ((Button)c).BackColor = Defines.Theme.TG_Parent_H;
                }
                bt.BackColor = Defines.Theme.TG_Parent_D;

                _pIndex = flpParent.Controls.GetChildIndex(bt);
                _cIndex = -1;

                /// Reset songs and lists.
                flpItems.Controls.Clear();
                flpChild.Controls.Clear();     

                DirectoryInfo di = new DirectoryInfo(bt.Tag.ToString());
                FileStream fs = null;
                foreach (FileInfo f in di.GetFiles("*.ahs"))
                {
                    gi = new GItem();
                    gi.pic1.Tag = f.FullName.Replace(f.Extension, ".ahs");
                    gi.btn1.Text = f.Name.Replace(f.Extension, "");
                    gi.AllowToDrage = false;
                    gi.onClick += new EventHandler(LoadSongItems);

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

                                fs = File.Open(_file.Replace(".ahp", ".ahi"), FileMode.Open);
                                gi.pic1.BackgroundImage = Image.FromStream(fs);
                                gi.pic1.BackgroundImageLayout = ImageLayout.Center;                               
                                fs.Close();
                                fs.Dispose();
                            }
                        }
                        catch { }
                    }
                    
                    flpItems.Controls.Add(gi);
                }

                /// Reset scroll.
                csdr1.Value = 0;
                csdr1.Minimum = 0;
                csdr1.Maximum = flpItems.VerticalScroll.Maximum;
                csdr1.SmallChange = (uint)flpItems.VerticalScroll.SmallChange;
                csdr1.LargeChange = (uint)flpItems.VerticalScroll.LargeChange;
                csdr1.PerformLayout();

                if (flpItems.Controls.Count > 0) { LoadSongItems(flpItems.Controls[0], null); }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void LoadSongItems(object sender, EventArgs e)
        {
            GItem gi = sender as GItem;

            /// Highlight selected item.
            foreach (Control c in flpItems.Controls)
            {
                ((GItem)c).btn1.BackColor = Defines.Theme.SG_Child_D;
            }
            gi.btn1.BackColor = Defines.Theme.SG_Child_H;

            _cIndex = flpItems.Controls.GetChildIndex(gi);

            LoadSongDetail(gi.pic1.Tag.ToString());
        }

        void SelectedItem(object sender, EventArgs e)
        {
            AuraSng.GItem g = sender as AuraSng.GItem;

            /// Highlight selected item.
            foreach (Control c in flpChild.Controls)
            {
                ((AuraSng.GItem)c).btn1.BackColor = Defines.Theme.SG_Item_D; //Defines.Theme.TG_Child_D;
            }
            g.btn1.BackColor = Defines.Theme.SG_Item_H; //Defines.Theme.TG_Child_H;
            f_index = flpChild.Controls.GetChildIndex(g);
            BeginInvoke((MethodInvoker)delegate { btnRemove.Visible = true; });            
            //MessageBox.Show(g.btn1.Tag.ToString());
        }

        void LoadSongDetail(string fn)
        {
            /// Reset list.
            flpChild.Controls.Clear();

            //btnVideo.Visible = btnAudio.Visible = false;
            btnRemove.Visible = false;
            _video = _audio = "";
            _vLoop = _vMute = _aLoop = _aMute = "0";


            /// Show all items.
            XmlDocument xd = new XmlDocument();
            string _file;
            FileStream fs = null;
            AuraSng.GItem g;
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
                        g = new AuraSng.GItem();
                        if (x != null && x.Name == "item")
                        {
                            _file = (x.Attributes["file"] != null) ? x.Attributes["file"].Value.ToLower() : "";

                            if (!string.IsNullOrEmpty(_file))
                            {
                                FileInfo f = new FileInfo(_file);
                                //arrSng.Add(f.FullName);

                                g.btn1.Tag = f.FullName.Replace(f.Extension, ".ahp");
                                g.btn1.Text = f.Name.Replace(f.Extension, "");
                                fs = File.Open(_file.Replace(".ahp", ".ahi"), FileMode.Open);

                                g.pic1.BackgroundImage = Image.FromStream(fs);
                                g.pic1.BackgroundImageLayout = ImageLayout.Center;
                                g.AllowToDrage = false;
                                g.onClick += new EventHandler(SelectedItem);
                                fs.Close();
                                fs.Dispose();
                            }
                        }

                        flpChild.Controls.Add(g);
                    }
                }

                /// Reset scroll.
                //int rem;
                //int quotient = Math.DivRem(flpChild.Controls.Count, 4, out rem);

                //if (flpChild.Controls.Count > 25)
                //    csdr1.ThumbSize = 30;
                //else
                //    csdr1.ThumbSize = flpParent.Width - 1;

                csdr2.Value = 0;
                csdr2.Minimum = 0;
                csdr2.Maximum = flpChild.HorizontalScroll.Maximum - 630;
                csdr2.SmallChange = (uint)flpChild.HorizontalScroll.SmallChange;
                csdr2.LargeChange = (uint)flpChild.HorizontalScroll.LargeChange;
                csdr2.PerformLayout();
            }
            catch { }
        }
        #endregion

        #region Make Song List
        int FindIndex(int x)
        {
            foreach (AuraSng.GItem g in flpChild.Controls)
            {
                if (x <= (flpChild.Location.X + g.Location.X))
                    return flpChild.Controls.GetChildIndex(g) - 1;
                else if (g.Location.X > x && (g.Location.X + g.Width) > x)
                        return flpChild.Controls.GetChildIndex(g, false);
            }
            return flpChild.Controls.Count;
        }

        private void flpChild_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string strFile = (string)e.Data.GetData(DataFormats.Text);
                //FileInfo f = new FileInfo(strFile);
                //Sng.GItem g = new Sng.GItem();
                //Stream s;

                int k =FindIndex(this.PointToClient(new Point(e.X, e.Y)).X);

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
            catch { }
        }

        private void flpChild_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.Text) && (e.AllowedEffect & DragDropEffects.Copy) != 0)
                    e.Effect = DragDropEffects.Copy;
            }
            catch { }
        }

        private void flpChild_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            if ((e.Effect & DragDropEffects.Move) == DragDropEffects.Move)
                Cursor.Current = Cursors.Cross;
            else
                Cursor.Current = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (flpChild.Controls.Count == 0)
            {
                MessageBox.Show("Please select any one item!");
                return;
            }

            SaveFileDialog sd = new SaveFileDialog();
            sd.InitialDirectory = AuraSng.AppData.Asset.song_project_path + ((_pIndex != -1) ? @"\" + flpParent.Controls[_pIndex].Text : "");
            sd.Filter = AuraSng.AppData.Filters.s_project;
            if (sd.ShowDialog() == DialogResult.OK)
            {
                FileInfo f = new FileInfo(sd.FileName);
                string x = string.Format(AuraSng.AppData.Song.Header, f.Name.Replace(f.Extension, ""), _video, _vLoop, _vMute, _audio, _aLoop, _aMute);
                string y = "";
                AuraSng.GItem g;
                foreach (Control c in flpChild.Controls)
                {
                    g = (AuraSng.GItem)c;
                    y += string.Format(AuraSng.AppData.Song.Detail, g.btn1.Tag.ToString(), g.Duration, g.Hold);
                }
                x = x.Replace(@"<items/>", string.Format(@"<items>{0}</items>", y));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(x);
                doc.Save(sd.FileName);
                //doc.Save(string.Format(@"{0}\{1}{2}", txtPLoc.Text, txtPT.Text, AppData.Song.Extension));

                if (_pIndex < flpParent.Controls.Count) { FolderClick(flpParent.Controls[_pIndex], null); if (_cIndex != -1) LoadSongItems(flpItems.Controls[_cIndex], null); }
                MessageBox.Show("Project saved successfully!");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (f_index < flpChild.Controls.Count)
            {
                if (MessageBox.Show("Do you want to remove?", "Remove", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    flpChild.Controls.RemoveAt(f_index);
                    btnRemove.Visible = false;
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
            FileInfo f = new FileInfo(_file);
            AuraSng.GItem g = new AuraSng.GItem();
            System.IO.Stream s;

            s = File.Open(f.FullName.Replace(f.Extension, ".ahi"), FileMode.Open);
            g.pic1.Tag = _file;
            g.pic1.BackgroundImage = Image.FromStream(s);
            g.pic1.BackgroundImageLayout = ImageLayout.Center;
            s.Close(); s.Dispose();

            g.btn1.Tag = f.FullName.Replace(f.Extension, ".ahp");
            g.btn1.Text = f.Name.Replace(f.Extension, "");
            g.AllowToDrage = false;
            g.onClick += new EventHandler(SelectedItem);

            flpChild.Controls.Add(g);
            flpChild.Controls.SetChildIndex(g, i);
        }
        #endregion

        #region Background Files
        private Mep_Agh.AuraSng.vFile v;
        private short fType = 0;
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
            od.Filter = (fType == 1) ? AuraSng.AppData.Filters.video : AuraSng.AppData.Filters.audio;
            v.txtVF.Text = string.Empty;
            if (od.ShowDialog() == DialogResult.OK) v.txtVF.Text = od.FileName;
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

        private void btnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                if (MPHelper.Control == null || flpChild.Controls.Count < 1 || status == 1) return;

                if (status == 0)
                {
                    f_index = 0;
                    status = 1;
                    //timer1.Start();
                    PlaySong();
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

                btnPause.Visible = true;
                btnPlay.Visible = false;
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


                btnPause.Visible = false;
                btnPlay.Visible = true;
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
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (f_index >= flpChild.Controls.Count) { btnStop_Click(sender, e); return; }

            status = 1;
            //if (f_index == 0) new Thread(ChangeToSongMode).Start();                
            if (f_index == 0) ChangeToSongMode();
            new Thread(ReadEffect).Start();
        }

        void ReadEffect()
        {
            AuraSng.GItem g = (AuraSng.GItem) flpChild.Controls[f_index];
            SelectedItem(flpChild.Controls[f_index], null);
            AuraSng.SHelper.LoadFile(g.btn1.Tag.ToString());
            efx_script = AuraSng.MHelper.GetEffectScript;

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
                AuraSng.MHelper.GetEffectScript = efx_script;

                while (t_index < AuraSng.MHelper.mTransitionViewCount)
                {
                    if (t_index == 0)
                    {
                        if (v_pItem != null) v_pItem.FilePlayStart();
                        if (a_pItem != null) a_pItem.FilePlayStart();
                    }

                    _hold = 0.0;
                    _duration = 0.0;
                    AuraSng.MHelper.Apply_Transition(t_index, (double)AuraSng.SHelper.mTranstionDuration, out _duration, out _hold);
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
                BeginInvoke((MethodInvoker)delegate
                {
                    Thread.Sleep(50);
                    AuraSng.TrnsHelper.Transition_remove();
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
            AuraSng.MHelper.Init(MPHelper.Control);
            //Sng.MHelper.mMixerList = Form1.sConfig.mStreamsList1;
            //Sng.MHelper.mElementTree = Form1.sConfig.mComposerElementsTree1;
            //Sng.MHelper.mAttributesList = Form1.sConfig.mAttributesList1;
        }

        void ChangeToSongMode()
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
                Helper.Master.mPlaylistTimeline1.Enabled = Helper.Master.mPlaylistTimeline2.Enabled = false;
            //}

            MPHelper.Control.ScenesAdd(Defines.Scenes.song, scene_id, out spro_scene);
            MPHelper.Control.ScenesActiveSet(scene_id, "");
            //Form1.sConfig.mScenesCombo1.UpdateCombo();

            v_pItem = null;
            a_pItem = null;
            if (!string.IsNullOrEmpty(_video)) StreamsAdd(AuraSng.AppData.Song.Stream.SBV, _video, _vLoop, _vMute, out v_pItem);
            if (!string.IsNullOrEmpty(_audio)) StreamsAdd(AuraSng.AppData.Song.Stream.SBA, _audio, _aLoop, _aMute, out a_pItem);

            //Form1.sConfig.mStreamsList1.UpdateList(false);
            Helper.IsSongPlaying = true;
        }

        void ChangeToSwitcherMode()
        {
            BeginInvoke((MethodInvoker)delegate {
                if (string.IsNullOrEmpty(active_scene_id)) return;

                try
                {
                    AuraSng.MHelper.Control.ScenesActiveSet(active_scene_id, "");
                    AuraSng.MHelper.Control.ScenesRemove(scene_id);
                }
                catch { };

                /// Remove all song streams.
                if (!string.IsNullOrEmpty(_video)) { MPHelper.ComposerStreamsRemove(AuraSng.AppData.Song.Stream.SBV, 0.0); AuraSng.MHelper.ReleaseComObject(v_pItem); }
                if (!string.IsNullOrEmpty(_audio)) { MPHelper.ComposerStreamsRemove(AuraSng.AppData.Song.Stream.SBA, 0.0); AuraSng.MHelper.ReleaseComObject(a_pItem); }

                //if (Frm != null)
                //{
                //Frm.MuteAll(false);
                MPHelper.MuteAllStreams = false;
                Helper.Master.L2.IsEnabled = true;
                Helper.Master.mPlaylistTimeline1.Enabled = Helper.Master.mPlaylistTimeline2.Enabled = true;

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
            });
        }

        void StreamsAdd(string _stream, string _path, string _loop, string _mute, out MItem _pItem)
        {
            //MItem 
            _pItem = null;
            try {
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

        #region Scroll Songs
        bool Iscsdr1_scolling = false;

        private void csdr1_Scroll(object sender, ScrollEventArgs e)
        {
            Iscsdr1_scolling = true;
            flpItems.VerticalScroll.Value = csdr1.Value;
            flpItems.PerformLayout();
        }

        private void csdr1_ValueChanged(object sender, EventArgs e)
        {
            Iscsdr1_scolling = true;
            flpItems.VerticalScroll.Value = csdr1.Value;
            flpItems.PerformLayout();
        }

        private void flpItems_Paint(object sender, PaintEventArgs e)
        {
            if (!Iscsdr1_scolling)
                csdr1.Value = flpItems.VerticalScroll.Value;
            Iscsdr1_scolling = false;
        }

        private void flpItems_MouseEnter(object sender, EventArgs e)
        {
            csdr1.Value = flpItems.VerticalScroll.Value;
            flpItems.PerformLayout();
            flpItems.Focus();
        }
        #endregion

        #region Scroll Song Items
        bool Iscsdr2_scolling = false;

        private void csldr2_Scroll(object sender, ScrollEventArgs e)
        {
            Iscsdr2_scolling = true;
            flpChild.HorizontalScroll.Value = csdr2.Value;
            flpChild.PerformLayout();
        }

        private void csldr2_ValueChanged(object sender, EventArgs e)
        {
            Iscsdr2_scolling = true;
            flpChild.HorizontalScroll.Value = csdr2.Value;
            flpChild.PerformLayout();
        }

        private void flpChild_Paint(object sender, PaintEventArgs e)
        {
            if (!Iscsdr2_scolling) csdr2.Value = flpChild.HorizontalScroll.Value;
            Iscsdr2_scolling = false;
        }

        private void flpChild_MouseEnter(object sender, EventArgs e)
        {
            csdr2.Value = flpChild.HorizontalScroll.Value;
            flpChild.PerformLayout();
            flpChild.Focus();
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

        #region Play Song Transitions
        void PlaySong()
        {
            /// Mute all streams
            //if (Frm != null)
            //{
            //Frm.MuteAll(true);
            BeginInvoke((MethodInvoker)delegate { 
                MPHelper.MuteAllStreams = true;
                Helper.Master.L1.IsEnabled = false;
                Helper.Master.L2.IsEnabled = false;
                Helper.Master.L3.IsEnabled = false;
                Helper.Master.L4.IsEnabled = false;
                Helper.Master.L5.IsEnabled = false;
                Helper.Master.M1.IsEnabled = false;
                Helper.Master.M2.IsEnabled = false;
                Helper.Master.G1.IsEnabled = false;
                Helper.Master.G2.IsEnabled = false;

                //Helper.Master.ePanel1.Enabled = false;
                //Helper.Master.fPanel1.Enabled = false;

                //Helper.Master.sequencer1.Enabled = false;
                Helper.Master.mPlaylistTimeline1.Enabled = false;
                Helper.Master.mPlaylistTimeline2.Enabled = false;
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
            if (!string.IsNullOrEmpty(_video)) StreamsAdd(AuraSng.AppData.Song.Stream.SBV, _video, _vLoop, _vMute, out v_pItem);
            if (!string.IsNullOrEmpty(_audio)) StreamsAdd(AuraSng.AppData.Song.Stream.SBA, _audio, _aLoop, _aMute, out a_pItem);
            //Form1.sConfig.mStreamsList1.UpdateList(false);

            /// Get active scene.
            int nIndex;
            if (active_scene_id == "") { MPHelper.Control.ScenesActiveGet(out active_scene_id, out nIndex, out active_scene); }

            /// Change to song scene.
            MPHelper.Control.ScenesAdd(Defines.Scenes.song, scene_id, out spro_scene);
            MPHelper.Control.ScenesActiveSet(scene_id, "");
            //Form1.sConfig.mScenesCombo1.UpdateCombo();

            /// Set to song playing mode.
            Helper.IsSongPlaying = true;

            /// Read song transitions.
            thrd_1 = new Thread(ParentThread);
            thrd_1.Start();
        }

        void ParentThread()
        {
            are_1 = new AutoResetEvent(false);

            while(f_index < flpChild.Controls.Count)
            {
                /// Read transition file.
                AuraSng.GItem g = (AuraSng.GItem)flpChild.Controls[f_index];
                SelectedItem(flpChild.Controls[f_index], null);
                AuraSng.SHelper.LoadFile(g.btn1.Tag.ToString());
                efx_script = AuraSng.MHelper.GetEffectScript;

                thrd_2 = new Thread(ChildThread);
                thrd_2.Start();

                are_1.WaitOne();
                Thread.Sleep(20);

                AuraSng.TrnsHelper.Transition_remove();
                f_index++;
            }

            thrd_3 = new Thread(StopSong);
            thrd_3.Start();
        }

        void ChildThread()
        {
            double _hold = 0.0;
            double _duration = 0.0;

            t_index = 0;
            AuraSng.MHelper.GetEffectScript = efx_script;
            while (AuraSng.MHelper.mTransitionViewCount > t_index)
            {
                if (t_index == 0)
                {
                    if (v_pItem != null) v_pItem.FilePlayStart();
                    if (a_pItem != null) a_pItem.FilePlayStart();
                }

                _hold = 0.0;
                _duration = 0.0;
                AuraSng.MHelper.Apply_Transition(t_index, (double)AuraSng.SHelper.mTranstionDuration, out _duration, out _hold);
                if (_hold < 0.0) _hold = (0.0 - _hold) * _duration;
                Thread.Sleep((Int32)(_duration + _hold) * 1000);

                while (true){ if (status < 0) Thread.Sleep(40); else break; }
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
            if (!string.IsNullOrEmpty(_video)) { MPHelper.ComposerStreamsRemove(AppData.Song.Stream.SBV, 0.0); AuraSng.MHelper.ReleaseComObject(v_pItem); }
            if (!string.IsNullOrEmpty(_audio)) { MPHelper.ComposerStreamsRemove(AuraSng.AppData.Song.Stream.SBA, 0.0); AuraSng.MHelper.ReleaseComObject(a_pItem); }

            /// Return to switcher mode.
            if (string.IsNullOrEmpty(active_scene_id)) return;
            MPHelper.Control.ScenesActiveSet(active_scene_id, "");
            try { AuraSng.MHelper.Control.ScenesRemove(scene_id); } catch { };
            //Form1.sConfig.mScenesCombo1.UpdateCombo();
            //Form1.sConfig.mStreamsList1.UpdateList(false);

            //if (Frm != null)
            //{
            BeginInvoke((MethodInvoker)delegate { 
                MPHelper.MuteAllStreams = false;
                Helper.Master.L1.IsEnabled = true;
                Helper.Master.L2.IsEnabled = true;
                Helper.Master.L3.IsEnabled = true;
                Helper.Master.L4.IsEnabled = true;
                Helper.Master.L5.IsEnabled = true;
                Helper.Master.M1.IsEnabled = true;
                Helper.Master.M2.IsEnabled = true;
                Helper.Master.G1.IsEnabled = true;
                Helper.Master.G2.IsEnabled = true;

                //Helper.Master.ePanel1.Enabled = true;
                //Helper.Master.fPanel1.Enabled = true;

                //Helper.Master.sequencer1.Enabled = true;
                Helper.Master.mPlaylistTimeline1.Enabled = true;
                Helper.Master.mPlaylistTimeline2.Enabled = true;
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

            BeginInvoke((MethodInvoker)delegate {
                try
                {
                    btnPause.Visible = false; btnPlay.Visible = true;
                }
                catch { }
            });
        }
        #endregion
    }
}

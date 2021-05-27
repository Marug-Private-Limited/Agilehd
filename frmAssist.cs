using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgileHDWPF
{
    public partial class frmAssist : Form
    {
        public frmAssist()
        {
            InitializeComponent();
        }

        #region Customize Properties
        short waitSeconds = 10;
        int recheckSeconds = 15;
        int showItSeconds = 5;

        MainWindow frmHome = new MainWindow();
        #endregion

        #region Static Properties
        bool IsLoaded = false;
        bool IsWaiting = false;
        short _count = 0;
        bool IsLoading = true;     
        
        int successInterval
        {
            get { return (1000 * recheckSeconds); }
       }

        int failedInterval = 1000;
        #endregion

        #region Event Handlers
        private void Form1_Shown(object sender, EventArgs e)
        {
            timer1.Interval = failedInterval;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                timer1.Stop();
                new Thread(ShowWindow).Start();
                return;
            }

            //SHelper.checkDevice(out msg, out success);
           
            DeviceHandler.checkDevice(out string msg, out short success, out int status);
            
            lblStatus.Text = msg;
            lblStatus.ForeColor = (success < 0) ? Color.Red : Color.WhiteSmoke;
            //Helper.isRuning = Helper.get_run_code;
            if ((success > 0 && status == 1) && !IsLoaded)
            {
                new Thread(ShowWindow).Start();
                IsLoaded = true;
                timer1.Interval = successInterval;
            }
            else if (success > 0 && status != 1)
            {
                timer1.Stop();
                return;
            }
            else if (success < 1)
            {
                lblStatus.Text += "  This application will close after " + waitSeconds + " seconds! ";
                this.Show();

                if (!IsWaiting)
                {
                    timer1.Interval = failedInterval;
                    _count = waitSeconds;
                    IsWaiting = true;
                    Thread.Sleep(500);
                    timer2.Start();
                }
                return;
            }
            else
            {
                this.Hide();
            }

            if (IsWaiting)
            {
                label1.Text = string.Empty;
                timer2.Stop();
                IsWaiting = false;

                timer1.Interval = successInterval;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label1.Text = _count.ToString();
            _count--;
            if (_count < 0)
            {
                timer2.Stop();
                Application.Exit();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void ShowWindow()
        {
            BeginInvoke((MethodInvoker)delegate {
                //if (frmHome == null) frmHome = new Form2();
                Thread.Sleep(1000 * showItSeconds);
                frmHome.Show();
                this.Hide();
            });
        }
        #endregion
    }
}
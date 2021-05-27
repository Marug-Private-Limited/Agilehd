using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgileHDWPF.AgileSng
{
    public partial class GItem : UserControl
    {
        public GItem()
        {
            InitializeComponent();
        }
        #region Variables
        public EventHandler onClick = null;
        bool _drag = true;
        #endregion

        #region Event Handlers
        private void GItem_Click(object sender, EventArgs e)
        {
            if (this.onClick != null)
                onClick(this, e);
        }

        /// Start the drag.
        private void pic1_MouseDown(object sender, MouseEventArgs e)
        {
            /// Start the drag if it's the right mouse button.
            if (e.Button == MouseButtons.Left && AllowToDrage)
            {
                pic1.DoDragDrop(pic1.Tag.ToString(), DragDropEffects.Copy);
            }
        }
        #endregion

        #region Members
        public bool AllowToDrage
        {
            get { return _drag; }
            set { _drag = value; }
        }
        #endregion

        public string Hold
        {
            get { try { return label1.Text; } catch { return "0.0"; } }
            set { label1.Text = value; }
        }

        public string Duration
        {
            get { try { return label1.Tag.ToString(); } catch { return "0.0"; } }
            set { label1.Tag = value; }
        }

    }
}

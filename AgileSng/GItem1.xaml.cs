using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AgileHDWPF.AgileSng
{
    /// <summary>
    /// Interaction logic for GItem1.xaml
    /// </summary>
    public partial class GItem1 : UserControl
    {
        public GItem1()
        {
            InitializeComponent();
        }
        private void pic1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && AllowToDrage)
            {
                Image image = e.Source as Image;

                DataObject data = new DataObject(typeof(string), image.Source.ToString().Replace("file:///", ""));

                DragDrop.DoDragDrop(image, data, DragDropEffects.Copy);
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

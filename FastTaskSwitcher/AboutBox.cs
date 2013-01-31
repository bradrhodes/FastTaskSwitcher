using System;
using System.Windows.Forms;

namespace FastTaskSwitcher
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

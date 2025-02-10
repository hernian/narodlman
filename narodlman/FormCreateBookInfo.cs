using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace narodlman
{
    public partial class FormCreateBookInfo : Form
    {
        private static readonly string[] URL_DATA_FORMAT = ["UniformResourceLocator", "UniformResourceLocatorW"];
        public FormCreateBookInfo()
        {
            InitializeComponent();
        }

        private void FormCreateBookInfo_DragEnter(object sender, DragEventArgs e)
        {
            var dataObj = e.Data;
            if (dataObj == null)
            {
                return;
            }
            foreach (var format in URL_DATA_FORMAT)
            {
                if (dataObj.GetDataPresent(format))
                {
                    e.Effect = DragDropEffects.Copy;
                    break;
                }
            }
        }

        public string UrlTitlePage
        {
            get
            {
                return textBoxUrlTitlePage.Text;
            }
        }

        public string PathBookInfo
        {
            get
            {
                return textBoxPathBookInfo.Text;
            }
        }

        private void FormCreateBookInfo_DragDrop(object sender, DragEventArgs e)
        {
            var dataObj = e.Data;
            if (dataObj == null)
            {
                return;
            }
            textBoxUrlTitlePage.Text = dataObj.GetData(DataFormats.Text)?.ToString() ?? "";
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                textBoxPathBookInfo.Text = saveFileDialog.FileName;
            }

        }
    }
}

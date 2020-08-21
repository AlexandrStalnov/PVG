using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PVGPOSM
{
    public partial class Form2 : Form
    {
        /// основное тело формы
        public Form2(string photo)
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(photo);
        }
    }
}

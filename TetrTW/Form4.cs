using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TetrTW
{
    public partial class Form4 : Form
    {
        string log;
        int money;
        int bumb;
        int frozen;
        int flash;
        int switc;
        int Buy_Focus = 1;
        string come;

        public Form4()
        {
            InitializeComponent();
        }

        public Form4(string value1, string value2)
        {
            InitializeComponent();
            log = value1;
            come = value2;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); //雙缓冲
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}

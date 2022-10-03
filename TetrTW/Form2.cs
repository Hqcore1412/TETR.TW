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
    public partial class Form2 : Form
    {
        // 建立暫存矩陣 data["Log Path","模式"]
        string log;

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(string value)
        {
            InitializeComponent();
            log = value;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); //雙缓冲
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // 全螢幕
            this.FormBorderStyle = FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            // 讀取 Log檔
            StreamReader str = new StreamReader(log);
            str.Close();
            // 物件設置
            pictureBox1.Visible = false;
            back.Visible = false;
            resume.Visible = false;
            close.Visible = false;
            back.Parent = pictureBox1;
            resume.Parent = pictureBox1;
            close.Parent = pictureBox1;
            // mode Button 事件
            mode1.Click += new EventHandler(Mode_Click);
            mode1.Tag = "mode1";
            mode2.Click += new EventHandler(Mode_Click);
            mode2.Tag = "mode2";
            mode3.Click += new EventHandler(Mode_Click);
            mode3.Tag = "mode3";
            mode4.Click += new EventHandler(Mode_Click);
            mode4.Tag = "mode4";

            panel1.Visible = false;

            StreamReader strTmp = new StreamReader(log);
            Player_Name.Text = strTmp.ReadLine();
            Player_Money.Text = strTmp.ReadLine();
            Bump_Count.Text = strTmp.ReadLine();
            Frozen_Count.Text = strTmp.ReadLine();
            Flash_Count.Text = strTmp.ReadLine();
            Switch_Count.Text = strTmp.ReadLine();
            strTmp.Close();

            new_money_show.Text = Convert.ToString(Bump_Count.Text);
        }


        // 每個 Log檔 button 的動作
        void Mode_Click(object sender, EventArgs e)
        {
            // 帶 Log檔路徑、mode 跳轉
            if (Convert.ToString((sender as Label).Tag) == "mode1")
            {
                Form3 f3 = new Form3(log, Convert.ToString((sender as Label).Tag));
                f3.Show();
                this.Hide();
                Form1.page_sound();
            }
            else if (Convert.ToString((sender as Label).Tag) == "mode2")
            {
                Form5 f5 = new Form5(log, Convert.ToString((sender as Label).Tag));
                f5.Show();
                this.Hide();
                Form1.page_sound();
            }
            else if (Convert.ToString((sender as Label).Tag) == "mode3")
            {
                Form6 f6 = new Form6(log, Convert.ToString((sender as Label).Tag));
                f6.Show();
                this.Hide();
                Form1.page_sound();
            }
            else if (Convert.ToString((sender as Label).Tag) == "mode4")
            {
                Form7 f7 = new Form7(log, Convert.ToString((sender as Label).Tag));
                f7.Show();
                this.Hide();
                Form1.page_sound();
            }
            
        }
        private void Store_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8(log, "f2");
            f8.Show();
            this.Hide();
            Form1.click_sound();
        }

        private void Backpack_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            Form1.click_sound();
        }

        private void Backpack_Close_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            Form1.click_sound();
        }

        private void icon1_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Image.FromFile("PicData/item/bag1.png");
            new_money_show.Text = Convert.ToString(Bump_Count.Text);
            Form1.click_sound();
        }

        private void icon2_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Image.FromFile("PicData/item/bag2.png");
            new_money_show.Text = Convert.ToString(Frozen_Count.Text);
            Form1.click_sound();
        }

        private void icon3_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Image.FromFile("PicData/item/bag3.png");
            new_money_show.Text = Convert.ToString(Flash_Count.Text);
            Form1.click_sound();
        }

        private void icon4_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Image.FromFile("PicData/item/bag4.png");
            new_money_show.Text = Convert.ToString(Switch_Count.Text);
            Form1.click_sound();
        }

        private void menu_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            back.Visible = true;
            resume.Visible = true;
            close.Visible = true;
            Form1.click_sound();
        }

        private void resume_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            back.Visible = false;
            resume.Visible = false;
            close.Visible = false;
            Form1.click_sound();
        }

        private void back_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
            Form1.click_sound();
        }

        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Form1.click_sound();
        }
        private void mode1_MouseHover(object sender, EventArgs e)
        {
            Form1.hover_sound();
        }

        private void mode2_MouseHover(object sender, EventArgs e)
        {
            Form1.hover_sound();
        }

        private void mode3_MouseHover(object sender, EventArgs e)
        {
            Form1.hover_sound();
        }

        private void mode4_MouseHover(object sender, EventArgs e)
        {
            Form1.hover_sound();
        }
        private void MouseHoverSound(object sender, EventArgs e)
        {
            Form1.hover_sound();
        }
    }
}

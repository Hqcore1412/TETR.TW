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
    public partial class Form8 : Form
    {
        string log;
        int money;
        int bumb;
        int frozen;
        int flash;
        int switc;
        int Buy_Focus = 1;
        string come;

        public Form8()
        {
            InitializeComponent();
        }

        public Form8(string value1, string value2)
        {
            InitializeComponent();
            log = value1;
            come = value2;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); //雙缓冲
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            // 全螢幕
            this.FormBorderStyle = FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;

            StreamReader strTmp = new StreamReader(log);
            Player_Name.Text = strTmp.ReadLine();
            money = Convert.ToInt32(strTmp.ReadLine());
            Player_Money.Text = Convert.ToString(money);
            bumb = Convert.ToInt32(strTmp.ReadLine());
            frozen = Convert.ToInt32(strTmp.ReadLine());
            flash = Convert.ToInt32(strTmp.ReadLine());
            switc = Convert.ToInt32(strTmp.ReadLine());
            strTmp.Close();

            // 物件設置
            pictureBox1.Visible = false;
            list_back.Visible = false;
            resume.Visible = false;
            close.Visible = false;
            list_back.Parent = pictureBox1;
            resume.Parent = pictureBox1;
            close.Parent = pictureBox1;

            panel1.Left = this.ClientSize.Width / 2 - panel1.Width / 2;
            panel1.Top = this.ClientSize.Height / 2 - panel1.Height / 2;
            panel1.Visible = false;

            Bump_Count.Text = Convert.ToString(bumb);
            Frozen_Count.Text = Convert.ToString(frozen);
            Flash_Count.Text = Convert.ToString(flash);
            Switch_Count.Text = Convert.ToString(switc);
            
            new_money_show.Text = Convert.ToString(bumb);
        }

        private void Bumb_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("PicData/background/shop-01.jpg");
            Buy_Focus = 1;
            Form1.click_sound();
        }

        private void Frozen_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("PicData/background/shop-02.jpg");
            Buy_Focus = 2;
            Form1.click_sound();
        }

        private void Flash_Time_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("PicData/background/shop-03.jpg");
            Buy_Focus = 3;
            Form1.click_sound();
        }

        private void Switch_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("PicData/background/shop-04.jpg");
            Buy_Focus = 4;
            Form1.click_sound();
        }

        private void Buy_Click(object sender, EventArgs e)
        {
            Form1.buy_sound();
            if (Buy_Focus == 1)
            {
                if (money >= 200)
                {
                    money -= 200;
                    bumb += 1;
                }
            }
            else if (Buy_Focus == 2)
            {
                if (money >= 100)
                {
                    money -= 100;
                    frozen += 1;
                }
            }
            else if (Buy_Focus == 3)
            {
                if (money >= 150)
                {
                    money -= 150;
                    flash += 1;
                }
            }
            else if (Buy_Focus == 4)
            {
                if (money >= 100)
                {
                    money -= 100;
                    switc += 1;
                }
            }
            Player_Money.Text = Convert.ToString(money);
            Bump_Count.Text = Convert.ToString(bumb);
            Frozen_Count.Text = Convert.ToString(frozen);
            Flash_Count.Text = Convert.ToString(flash);
            Switch_Count.Text = Convert.ToString(switc);

            StreamWriter strTmp = new StreamWriter(log);
            strTmp.WriteLine(Player_Name.Text);
            strTmp.WriteLine(Convert.ToString(money));
            strTmp.WriteLine(Convert.ToString(bumb));
            strTmp.WriteLine(Convert.ToString(frozen));
            strTmp.WriteLine(Convert.ToString(flash));
            strTmp.WriteLine(Convert.ToString(switc));
            strTmp.Close();

        }

        private void Backpack_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            Form1.click_sound();
        }

        private void icon1_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Image.FromFile("PicData/item/bag1.png");
            new_money_show.Text = Convert.ToString(bumb);
            Form1.click_sound();
        }

        private void icon2_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Image.FromFile("PicData/item/bag2.png");
            new_money_show.Text = Convert.ToString(frozen);
            Form1.click_sound();
        }

        private void icon3_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Image.FromFile("PicData/item/bag3.png");
            new_money_show.Text = Convert.ToString(flash);
            Form1.click_sound();
        }

        private void icon4_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Image.FromFile("PicData/item/bag4.png");
            new_money_show.Text = Convert.ToString(switc);
            Form1.click_sound();
        }

        private void Backpack_Close_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            Form1.click_sound();
        }

        private void Back_Click(object sender, EventArgs e)
        {
            Form1.click_sound();
            if (come == "f2")
            {
                Form2 f2 = new Form2(log);
                f2.Show();
                this.Hide();
            }
            else if (come == "f3")
            {
                Form3 f3 = new Form3(log, "mode1");
                f3.Show();
                this.Hide();
            }
            else if (come == "f5")
            {
                Form5 f5 = new Form5(log, "mode2");
                f5.Show();
                this.Hide();
            }
            else if (come == "f6")
            {
                Form6 f6 = new Form6(log, "mode3");
                f6.Show();
                this.Hide();
            }
            else if (come == "f7")
            {
                Form7 f7 = new Form7(log, "mode4");
                f7.Show();
                this.Hide();
            }
        }

        private void menu_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            list_back.Visible = true;
            resume.Visible = true;
            close.Visible = true;
            Form1.click_sound();
        }

        private void resume_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            list_back.Visible = false;
            resume.Visible = false;
            close.Visible = false;
            Form1.click_sound();
        }

        private void list_back_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
            Form1.click_sound();
        }

        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

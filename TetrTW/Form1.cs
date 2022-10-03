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
using System.Media;

namespace TetrTW
{
    public partial class Form1 : Form
    {
        int log_count = 0;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); //雙缓冲
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 視窗設置
            this.Bounds = Screen.PrimaryScreen.Bounds;
            // 物件設置
            textBox1.Visible = false;
            panel1.Visible = false;
            record.Enabled = false;
            record.Parent = pictureBox1;
            panel1.Parent = pictureBox1;
            back.Parent = pictureBox1;

            // 讀取秀出現有的紀錄檔
            StreamReader str = new StreamReader("LogFile/LogCount.txt");
            log_count = Convert.ToInt32(str.ReadLine());
            str.Close();
            Label[] buttons = new Label[log_count];
            for (int i = 1; i <= log_count; i++)
            {
                StreamReader strTmp = new StreamReader("LogFile/Player" + Convert.ToString(i) + ".txt");
                string ReadName = strTmp.ReadLine();
                strTmp.Close();
                buttons[i - 1] = new Label();
                buttons[i - 1].Name = "Player" + Convert.ToString(i);
                buttons[i - 1].Font = new Font("華康中特圓體", 22.2f);
                buttons[i - 1].FlatStyle = FlatStyle.Flat;
                buttons[i - 1].TextAlign = ContentAlignment.MiddleCenter;
                buttons[i - 1].BackgroundImage = Properties.Resources.Button資產_8;
                buttons[i - 1].BackgroundImageLayout = ImageLayout.Zoom;
                buttons[i - 1].ForeColor = Color.White;
                buttons[i - 1].Size = new Size(330, 65);
                buttons[i - 1].Text = ReadName;
                buttons[i - 1].Left = 0;
                if (i == 1)
                {
                    buttons[i - 1].Location = new Point(0,10);
                }
                else
                {
                    buttons[i - 1].Top = buttons[i - 2].Top + buttons[i - 2].Height + 25;
                }
                buttons[i - 1].Tag = i;
                buttons[i - 1].Click += new EventHandler(Buttons_Click);
                buttons[i - 1].MouseHover += new EventHandler(Buttons_MouseHover);
                buttons[i - 1].BringToFront();
            }
            this.Controls.AddRange(buttons);

            // 生成可捲動的 Panel
            for (int i=0; i < log_count; i++)
            {
                panel1.Controls.Add(buttons[i]);
            }
        }

        // 每個 Log檔 button 的動作
        void Buttons_Click(object sender, EventArgs e)
        {
            // MessageBox.Show((sender as Button).Text, "測試");
            // 帶 Log檔路徑跳轉
            Form2 f2 = new Form2("LogFile/Player" + Convert.ToString((sender as Label).Tag) + ".txt");
            f2.Show();
            this.Hide();
        }
        private void Buttons_MouseHover(object sender, EventArgs e)
        {
            Form1.hover_sound();
        }

        // 按鈕「創建」控制 Ex：測試創建 Log 紀錄
        private void record_Click(object sender, EventArgs e)
        {
            click_sound();
            // 驗證暱稱是否為空
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("請輸入暱稱！", "警告");
            }
            else
            {
                // 更新 LogCount.txt 內個數
                log_count += 1;
                StreamWriter str = new StreamWriter("LogFile/LogCount.txt");
                str.WriteLine(Convert.ToString(log_count));
                str.Close();

                // 新建 Log檔
                StreamWriter strNew = new StreamWriter("LogFile/Player" + Convert.ToString(log_count) + ".txt");
                strNew.WriteLine(textBox1.Text);
                strNew.Close();

                // 帶 Log檔名稱跳轉
                Form2 f2 = new Form2("LogFile/Player" + Convert.ToString(log_count) + ".txt");
                f2.Show();
                this.Hide();
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            bgmusic.PlayLooping();
            click_sound();
            start.Enabled = false;
            pictureBox1.Visible = true;
            textBox1.Visible = true;
            record.Enabled = true;
            panel1.Visible = true;
            panel1.BringToFront();
        }

        private void back_Click(object sender, EventArgs e)
        {
            bgmusic.Stop();
            click_sound();
            start.Enabled = true;
            pictureBox1.Visible = false;
            textBox1.Visible = false;
            record.Enabled = false;
            panel1.Visible = false;
            panel1.BringToFront();
        }
        // 背景音樂
        public static SoundPlayer game_bgmusic = new SoundPlayer(Properties.Resources.game_bgmusic);
        public static SoundPlayer bgmusic = new SoundPlayer(Properties.Resources.bgmusic);

        // 特效音樂
        public static void click_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\msclick.wav";
        }
        public static void hover_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\mshover.wav";
        }
        public static void gameover_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\gameover.wav";
        }
        public static void clear_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\clear.wav";
        }
        public static void space_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\space.wav";
        }
        public static void quit_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\quit.wav";
        }
        public static void page_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\page2.wav";
        }
        public static void buy_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\buy.wav";
        }
        public static void switch_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\switch.wav";
        }
        public static void bomb_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\boom.wav";
        }
        public static void forzen_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\forzen.wav";
        }
        public static void flash_sound()
        {
            var mshover = new WMPLib.WindowsMediaPlayer();
            mshover.URL = @"sound\flash.wav";
        }
    }
}

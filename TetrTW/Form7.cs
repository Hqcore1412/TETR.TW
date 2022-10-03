﻿using System;
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
    public partial class Form7 : Form
    {
        string log, mode;

        // 參考程式碼定義
        Label[,] grids = new Label[20, 10]; //main area, total 200 grids 
        Label[,] next1 = new Label[4, 3];   //next area, total 12 grids 
        Label[,] next2 = new Label[4, 3];   //next area, total 12 grids 
        Label[,] next3 = new Label[4, 3];   //next area, total 12 grids 
        Label[,] next4 = new Label[4, 3];   //next area, total 12 grids 
        Label[,] next5 = new Label[4, 3];   //next area, total 12 grids 
        Label[,] save = new Label[4, 3];   //next area, total 12 grids
        bool[,] signs = new bool[24, 10];
        Color[,] grids_color = new Color[24, 10];
        uint block_row = 20;
        uint block_col = 4;
        uint block_type;
        uint block_row_pre = 20;
        uint block_col_pre = 4;
        uint block_type_pre;
        uint block_type_next;
        bool block_changed = false;
        Random rander = new Random(System.DateTime.Now.Millisecond);
        int timer_interval = 1010;
        int game_mode = 1;
        uint block_count = 0;
        int score = 0;
        uint[] all_block_type = { 0, 1, 2, 3, 4, 5, 6 };
        uint type_count;
        bool save_detect = false;
        uint save_type;
        uint clear_line;
        int money;

        int bumb;
        int frozen;
        int flash;
        int switc;

        public Form7()
        {
            InitializeComponent();
        }

        public Form7(string value1, string value2)
        {
            InitializeComponent();
            log = value1;
            mode = value2;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); //雙缓冲
        }

        // 打亂陣列
        public static void Shuffle<T>(T[] Source)
        {
            if (Source == null) return;
            int len = Source.Length;// 用變數記會快一點點點
            Random rd = new Random();
            int r;// 記下隨機產生的號碼
            T tmp;// 暫存用
            for (int i = 0; i < len - 1; i++)
            {
                r = rd.Next(i, len);// 取亂數，範圍從自己到最後，決定要和哪個位置交換，因此也不用跑最後一圈了
                if (i == r) continue;
                tmp = Source[i];
                Source[i] = Source[r];
                Source[r] = tmp;
            }
        }

        // 更新方塊
        void update_block(uint i, uint j, uint type)
        {
            switch (type)
            {
                case 1:
                    signs[i, j] = signs[i + 1, j] = signs[i + 2, j] = signs[i + 3, j] = true;
                    grids_color[i, j] = grids_color[i + 1, j] = grids_color[i + 2, j] = grids_color[i + 3, j] = Color.Blue;
                    break;
                case 11:
                    signs[i, j] = signs[i, j + 1] = signs[i, j + 2] = signs[i, j + 3] = true;
                    grids_color[i, j] = grids_color[i, j + 1] = grids_color[i, j + 2] = grids_color[i, j + 3] = Color.Blue;
                    break;
                case 2:
                    signs[i, j] = signs[i + 1, j] = signs[i, j + 1] = signs[i + 1, j + 1] = true;
                    grids_color[i, j] = grids_color[i + 1, j] = grids_color[i, j + 1] = grids_color[i + 1, j + 1] = Color.Yellow;
                    break;
                case 3:
                    signs[i, j] = signs[i + 1, j] = signs[i + 1, j - 1] = signs[i, j + 1] = true;
                    grids_color[i, j] = grids_color[i + 1, j] = grids_color[i + 1, j - 1] = grids_color[i, j + 1] = Color.Red;
                    break;
                case 13:
                    signs[i, j] = signs[i - 1, j] = signs[i, j + 1] = signs[i + 1, j + 1] = true;
                    grids_color[i, j] = grids_color[i - 1, j] = grids_color[i, j + 1] = grids_color[i + 1, j + 1] = Color.Red;
                    break;
                case 4:
                    signs[i, j] = signs[i, j - 1] = signs[i + 1, j] = signs[i + 1, j + 1] = true;
                    grids_color[i, j] = grids_color[i, j - 1] = grids_color[i + 1, j] = grids_color[i + 1, j + 1] = Color.Green;
                    break;
                case 14:
                    signs[i, j] = signs[i + 1, j] = signs[i, j + 1] = signs[i - 1, j + 1] = true;
                    grids_color[i, j] = grids_color[i + 1, j] = grids_color[i, j + 1] = grids_color[i - 1, j + 1] = Color.Green;
                    break;
                case 5:
                    signs[i, j] = signs[i + 1, j] = signs[i + 1, j + 1] = signs[i + 1, j + 2] = true;
                    grids_color[i, j] = grids_color[i + 1, j] = grids_color[i + 1, j + 1] = grids_color[i + 1, j + 2] = Color.Orange;
                    break;
                case 15:
                    signs[i, j] = signs[i, j - 1] = signs[i + 1, j - 1] = signs[i + 2, j - 1] = true;
                    grids_color[i, j] = grids_color[i, j - 1] = grids_color[i + 1, j - 1] = grids_color[i + 2, j - 1] = Color.Orange;
                    break;
                case 25:
                    signs[i, j] = signs[i - 1, j] = signs[i - 1, j - 1] = signs[i - 1, j - 2] = true;
                    grids_color[i, j] = grids_color[i - 1, j] = grids_color[i - 1, j - 1] = grids_color[i - 1, j - 2] = Color.Orange;
                    break;
                case 35:
                    signs[i, j] = signs[i, j + 1] = signs[i - 1, j + 1] = signs[i - 2, j + 1] = true;
                    grids_color[i, j] = grids_color[i, j + 1] = grids_color[i - 1, j + 1] = grids_color[i - 2, j + 1] = Color.Orange;
                    break;
                case 6:
                    signs[i, j] = signs[i + 1, j] = signs[i + 1, j - 1] = signs[i + 1, j - 2] = true;
                    grids_color[i, j] = grids_color[i + 1, j] = grids_color[i + 1, j - 1] = grids_color[i + 1, j - 2] = Color.LightBlue;
                    break;
                case 16:
                    signs[i, j] = signs[i, j + 1] = signs[i + 1, j + 1] = signs[i + 2, j + 1] = true;
                    grids_color[i, j] = grids_color[i, j + 1] = grids_color[i + 1, j + 1] = grids_color[i + 2, j + 1] = Color.LightBlue;
                    break;
                case 26:
                    signs[i, j] = signs[i - 1, j] = signs[i - 1, j + 1] = signs[i - 1, j + 2] = true;
                    grids_color[i, j] = grids_color[i - 1, j] = grids_color[i - 1, j + 1] = grids_color[i - 1, j + 2] = Color.LightBlue;
                    break;
                case 36:
                    signs[i, j] = signs[i, j - 1] = signs[i - 1, j - 1] = signs[i - 2, j - 1] = true;
                    grids_color[i, j] = grids_color[i, j - 1] = grids_color[i - 1, j - 1] = grids_color[i - 2, j - 1] = Color.LightBlue;
                    break;

                case 7:
                    signs[i, j] = signs[i, j - 1] = signs[i, j + 1] = signs[i + 1, j] = true;
                    grids_color[i, j] = grids_color[i, j - 1] = grids_color[i, j + 1] = grids_color[i + 1, j] = Color.Purple;
                    break;
                case 17:
                    signs[i, j] = signs[i, j + 1] = signs[i - 1, j] = signs[i + 1, j] = true;
                    grids_color[i, j] = grids_color[i, j + 1] = grids_color[i - 1, j] = grids_color[i + 1, j] = Color.Purple;
                    break;
                case 27:
                    signs[i, j] = signs[i, j - 1] = signs[i, j + 1] = signs[i - 1, j] = true;
                    grids_color[i, j] = grids_color[i, j - 1] = grids_color[i, j + 1] = grids_color[i - 1, j] = Color.Purple;
                    break;
                case 37:
                    signs[i, j] = signs[i, j - 1] = signs[i + 1, j] = signs[i - 1, j] = true;
                    grids_color[i, j] = grids_color[i, j - 1] = grids_color[i + 1, j] = grids_color[i - 1, j] = Color.Purple;
                    break;
            }
        }

        // 擦掉方塊
        void erase_block(uint i, uint j, uint type)
        {
            switch (type)
            {
                case 1:
                    signs[i, j] = signs[i + 1, j] = signs[i + 2, j] = signs[i + 3, j] = false;
                    break;
                case 11:
                    signs[i, j] = signs[i, j + 1] = signs[i, j + 2] = signs[i, j + 3] = false;
                    break;
                case 2:
                    signs[i, j] = signs[i + 1, j] = signs[i, j + 1] = signs[i + 1, j + 1] = false;
                    break;
                case 3:
                    signs[i, j] = signs[i + 1, j] = signs[i + 1, j - 1] = signs[i, j + 1] = false;
                    break;
                case 13:
                    signs[i, j] = signs[i - 1, j] = signs[i, j + 1] = signs[i + 1, j + 1] = false;
                    break;
                case 4:
                    signs[i, j] = signs[i, j - 1] = signs[i + 1, j] = signs[i + 1, j + 1] = false;
                    break;
                case 14:
                    signs[i, j] = signs[i + 1, j] = signs[i, j + 1] = signs[i - 1, j + 1] = false;
                    break;
                case 5:
                    signs[i, j] = signs[i + 1, j] = signs[i + 1, j + 1] = signs[i + 1, j + 2] = false;
                    break;
                case 15:
                    signs[i, j] = signs[i, j - 1] = signs[i + 1, j - 1] = signs[i + 2, j - 1] = false;
                    break;
                case 25:
                    signs[i, j] = signs[i - 1, j] = signs[i - 1, j - 1] = signs[i - 1, j - 2] = false;
                    break;
                case 35:
                    signs[i, j] = signs[i, j + 1] = signs[i - 1, j + 1] = signs[i - 2, j + 1] = false;
                    break;
                case 6:
                    signs[i, j] = signs[i + 1, j] = signs[i + 1, j - 1] = signs[i + 1, j - 2] = false;
                    break;
                case 16:
                    signs[i, j] = signs[i, j + 1] = signs[i + 1, j + 1] = signs[i + 2, j + 1] = false;
                    break;
                case 26:
                    signs[i, j] = signs[i - 1, j] = signs[i - 1, j + 1] = signs[i - 1, j + 2] = false;
                    break;
                case 36:
                    signs[i, j] = signs[i, j - 1] = signs[i - 1, j - 1] = signs[i - 2, j - 1] = false;
                    break;
                case 7:
                    signs[i, j] = signs[i, j - 1] = signs[i, j + 1] = signs[i + 1, j] = false;
                    break;
                case 17:
                    signs[i, j] = signs[i, j + 1] = signs[i - 1, j] = signs[i + 1, j] = false;
                    break;
                case 27:
                    signs[i, j] = signs[i, j - 1] = signs[i, j + 1] = signs[i - 1, j] = false;
                    break;
                case 37:
                    signs[i, j] = signs[i, j - 1] = signs[i + 1, j] = signs[i - 1, j] = false;
                    break;
            }
        }

        // i 是 row（橫的）、j 是 col（直的）
        // 判定每個方塊下面是否有物體，是否可以下移一格
        bool y_direction(uint type, uint i, uint j)
        {
            switch (type)
            {
                case 1:
                    if (i != 0 && !signs[i - 1, j]) return true;
                    else return false;

                case 11:
                    if (i != 0 && !signs[i - 1, j] && !signs[i - 1, j + 1] && !signs[i - 1, j + 2] && !signs[i - 1, j + 3]) return true;
                    else return false;

                case 2:
                    if (i != 0 && !signs[i - 1, j] && !signs[i - 1, j + 1]) return true;
                    else return false;

                case 3:
                    if (i != 0 && !signs[i, j - 1] && !signs[i - 1, j] && !signs[i - 1, j + 1]) return true;
                    else return false;

                case 13:
                    if (i != 1 && !signs[i - 2, j] && !signs[i - 1, j + 1]) return true;
                    else return false;

                case 4:
                    if (i != 0 && !signs[i, j + 1] && !signs[i - 1, j] && !signs[i - 1, j - 1]) return true;
                    else return false;

                case 14:
                    if (i != 1 && !signs[i - 1, j] && !signs[i - 2, j + 1]) return true;
                    else return false;

                case 5:
                    if (i != 0 && !signs[i - 1, j] && !signs[i, j + 1] && !signs[i, j + 2]) return true;
                    else return false;

                case 15:
                    if (i != 0 && !signs[i - 1, j] && !signs[i - 1, j - 1]) return true;
                    else return false;

                case 25:
                    if (i != 1 && !signs[i - 2, j] && !signs[i - 2, j - 1] && !signs[i - 2, j - 2]) return true;
                    else return false;

                case 35:
                    if (i != 2 && !signs[i - 1, j] && !signs[i - 3, j + 1]) return true;
                    else return false;

                case 6:
                    if (i != 0 && !signs[i, j - 1] && !signs[i, j - 2] && !signs[i - 1, j]) return true;
                    else return false;

                case 16:
                    if (i != 0 && !signs[i - 1, j] && !signs[i - 1, j + 1]) return true;
                    else return false;

                case 26:
                    if (i != 1 && !signs[i - 2, j] && !signs[i - 2, j + 1] && !signs[i - 2, j + 2]) return true;
                    else return false;

                case 36:
                    if (i != 2 && !signs[i - 1, j] && !signs[i - 3, j - 1]) return true;
                    else return false;

                case 7:
                    if (i != 0 && !signs[i - 1, j - 1] && !signs[i - 1, j] && !signs[i - 1, j + 1]) return true;
                    else return false;

                case 17:
                    if (i != 1 && !signs[i - 2, j] && !signs[i - 1, j + 1]) return true;
                    else return false;

                case 27:
                    if (i != 1 && !signs[i - 1, j - 1] && !signs[i - 1, j + 1] && !signs[i - 2, j]) return true;
                    else return false;

                case 37:
                    if (i != 1 && !signs[i - 2, j] && !signs[i - 1, j - 1]) return true;
                    else return false;

                default:
                    return false;
            }
        }


        // i 是 row（橫的）、j 是 col（直的）
        // 判定每個方塊（所有種類，包括旋轉後的）的按下左右鍵是否可以移動
        bool x_direction(uint type, uint i, uint j, int d)
        {
            switch (type)
            {
                case 1:
                    if (d == -1)
                    {
                        if (j != 0 && !signs[i, j - 1] && !signs[i + 1, j - 1] && !signs[i + 2, j - 1] && !signs[i + 3, j - 1]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 9 && !signs[i, j + 1] && !signs[i + 1, j + 1] && !signs[i + 2, j + 1] && !signs[i + 3, j + 1]) return true;
                        else return false;
                    }

                case 11:
                    if (d == -1)
                    {
                        if (j != 0 && !signs[i, j - 1]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 6 && !signs[i, j + 4]) return true;
                        else return false;
                    }

                case 2:
                    if (d == -1)
                    {
                        if (j != 0 && !signs[i, j - 1] && !signs[i + 1, j - 1]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 8 && !signs[i, j + 2] && !signs[i + 1, j + 2]) return true;
                        else return false;
                    }

                case 3:
                    if (d == -1)
                    {
                        if (j != 1 && !signs[i, j - 1] && !signs[i + 1, j - 2]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 8 && !signs[i, j + 2] && !signs[i + 1, j + 1]) return true;
                        else return false;
                    }

                case 13:
                    if (d == -1)
                    {
                        if (j != 0 && !signs[i, j - 1] && !signs[i + 1, j] && !signs[i + 1, j - 1]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 8 && !signs[i, j + 2] && !signs[i + 1, j + 2] && !signs[i - 1, j + 1]) return true;
                        else return false;
                    }

                case 4:
                    if (d == -1)
                    {
                        if (j != 1 && !signs[i, j - 2] && !signs[i + 1, j - 1]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 8 && !signs[i, j + 1] && !signs[i + 1, j + 2]) return true;
                        else return false;
                    }

                case 14:
                    if (d == -1)
                    {
                        if (j != 0 && !signs[i, j - 1] && !signs[i + 1, j - 1] && !signs[i - 1, j]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 8 && !signs[i, j + 2] && !signs[i + 1, j + 1] && !signs[i - 1, j + 2]) return true;
                        else return false;
                    }

                case 5:
                    if (d == -1)
                    {
                        if (j != 0 && !signs[i, j - 1] && !signs[i + 1, j - 1]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 7 && !signs[i, j + 1] && !signs[i + 1, j + 3]) return true;
                        else return false;
                    }

                case 15:
                    if (d == -1)
                    {
                        if (j != 1 && !signs[i, j - 2] && !signs[i + 1, j - 2] && !signs[i + 2, j - 2]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 9 && !signs[i, j + 1] && !signs[i + 1, j] && !signs[i + 2, j]) return true;
                        else return false;
                    }

                case 25:
                    if (d == -1)
                    {
                        if (j != 2 && !signs[i, j - 1] && !signs[i - 1, j - 3]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 9 && !signs[i, j + 1] && !signs[i - 1, j + 1]) return true;
                        else return false;
                    }

                case 35:
                    if (d == -1)
                    {
                        if (j != 0 && !signs[i, j - 1] && !signs[i - 1, j] && !signs[i - 2, j]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 8 && !signs[i, j + 2] && !signs[i - 1, j + 2] && !signs[i - 2, j + 2]) return true;
                        else return false;
                    }

                case 6:
                    if (d == -1)
                    {
                        if (j != 2 && !signs[i, j - 1] && !signs[i + 1, j - 3]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 9 && !signs[i, j + 1] && !signs[i + 1, j + 1]) return true;
                        else return false;
                    }

                case 16:
                    if (d == -1)
                    {
                        if (j != 0 && !signs[i, j - 1] && !signs[i + 1, j] && !signs[i + 2, j]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 8 && !signs[i, j + 2] && !signs[i + 1, j + 2] && !signs[i + 2, j + 2]) return true;
                        else return false;
                    }

                case 26:
                    if (d == -1)
                    {
                        if (j != 0 && !signs[i, j - 1] && !signs[i - 1, j - 1]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 7 && !signs[i, j + 1] && !signs[i - 1, j + 3]) return true;
                        else return false;
                    }

                case 36:
                    if (d == -1)
                    {
                        if (j != 1 && !signs[i, j - 2] && !signs[i - 1, j - 2] && !signs[i - 2, j - 2]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 9 && !signs[i, j + 1] && !signs[i - 1, j] && !signs[i - 2, j]) return true;
                        else return false;
                    }

                case 7:
                    if (d == -1)
                    {
                        if (j != 1 && !signs[i, j - 2] && !signs[i + 1, j - 1]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 8 && !signs[i, j + 2] && !signs[i + 1, j + 1]) return true;
                        else return false;
                    }

                case 17:
                    if (d == -1)
                    {
                        if (j != 0 && !signs[i, j - 1] && !signs[i + 1, j - 1] && !signs[i - 1, j - 1]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 8 && !signs[i, j + 2] && !signs[i + 1, j + 1] && !signs[i - 1, j + 1]) return true;
                        else return false;
                    }

                case 27:
                    if (d == -1)
                    {
                        if (j != 1 && !signs[i, j - 2] && !signs[i - 1, j - 1]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 8 && !signs[i, j + 2] && !signs[i - 1, j + 1]) return true;
                        else return false;
                    }

                case 37:
                    if (d == -1)
                    {
                        if (j != 1 && !signs[i, j - 2] && !signs[i + 1, j - 1] && !signs[i - 1, j - 1]) return true;
                        else return false;
                    }
                    else
                    {
                        if (j != 9 && !signs[i, j + 1] && !signs[i + 1, j + 1] && !signs[i - 1, j + 1]) return true;
                        else return false;
                    }

                default:
                    return false;
            }
        }

        // 紀錄已有的方塊並更新？
        void show_grids()
        {
            int i, j;
            for (i = 0; i < 20; i++)
                for (j = 0; j < 10; j++)
                    if (signs[i, j])
                        grids[i, j].BackColor = grids_color[i, j];
                    else
                        grids[i, j].BackColor = Color.FromArgb(60, 255, 255, 255);
        }

        // 顯示右邊下一個方塊的顯示
        void display_next1_block(uint type)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                    next1[i, j].BackColor = Color.FromArgb(0);

            switch (type)
            {
                case 1:
                    next1[0, 1].BackColor = next1[1, 1].BackColor = next1[2, 1].BackColor = next1[3, 1].BackColor = Color.Blue;
                    break;
                case 2:
                    next1[1, 0].BackColor = next1[1, 1].BackColor = next1[2, 0].BackColor = next1[2, 1].BackColor = Color.Yellow;
                    break;
                case 3:
                    next1[2, 0].BackColor = next1[2, 1].BackColor = next1[1, 1].BackColor = next1[1, 2].BackColor = Color.Red;
                    break;
                case 4:
                    next1[1, 0].BackColor = next1[1, 1].BackColor = next1[2, 1].BackColor = next1[2, 2].BackColor = Color.Green;
                    break;
                case 5:
                    next1[1, 0].BackColor = next1[2, 0].BackColor = next1[2, 1].BackColor = next1[2, 2].BackColor = Color.Orange;
                    break;
                case 6:
                    next1[2, 0].BackColor = next1[2, 1].BackColor = next1[2, 2].BackColor = next1[1, 2].BackColor = Color.LightBlue;
                    break;
                case 7:
                    next1[1, 0].BackColor = next1[1, 1].BackColor = next1[1, 2].BackColor = next1[2, 1].BackColor = Color.Purple;
                    break;
            }
        }

        void display_next2_block(uint type)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                    next2[i, j].BackColor = Color.FromArgb(0);

            switch (type)
            {
                case 1:
                    next2[0, 1].BackColor = next2[1, 1].BackColor = next2[2, 1].BackColor = next2[3, 1].BackColor = Color.Blue;
                    break;
                case 2:
                    next2[1, 0].BackColor = next2[1, 1].BackColor = next2[2, 0].BackColor = next2[2, 1].BackColor = Color.Yellow;
                    break;
                case 3:
                    next2[2, 0].BackColor = next2[2, 1].BackColor = next2[1, 1].BackColor = next2[1, 2].BackColor = Color.Red;
                    break;
                case 4:
                    next2[1, 0].BackColor = next2[1, 1].BackColor = next2[2, 1].BackColor = next2[2, 2].BackColor = Color.Green;
                    break;
                case 5:
                    next2[1, 0].BackColor = next2[2, 0].BackColor = next2[2, 1].BackColor = next2[2, 2].BackColor = Color.Orange;
                    break;
                case 6:
                    next2[2, 0].BackColor = next2[2, 1].BackColor = next2[2, 2].BackColor = next2[1, 2].BackColor = Color.LightBlue;
                    break;
                case 7:
                    next2[1, 0].BackColor = next2[1, 1].BackColor = next2[1, 2].BackColor = next2[2, 1].BackColor = Color.Purple;
                    break;
            }
        }

        void display_next3_block(uint type)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                    next3[i, j].BackColor = Color.FromArgb(0);

            switch (type)
            {
                case 1:
                    next3[0, 1].BackColor = next3[1, 1].BackColor = next3[2, 1].BackColor = next3[3, 1].BackColor = Color.Blue;
                    break;
                case 2:
                    next3[1, 0].BackColor = next3[1, 1].BackColor = next3[2, 0].BackColor = next3[2, 1].BackColor = Color.Yellow;
                    break;
                case 3:
                    next3[2, 0].BackColor = next3[2, 1].BackColor = next3[1, 1].BackColor = next3[1, 2].BackColor = Color.Red;
                    break;
                case 4:
                    next3[1, 0].BackColor = next3[1, 1].BackColor = next3[2, 1].BackColor = next3[2, 2].BackColor = Color.Green;
                    break;
                case 5:
                    next3[1, 0].BackColor = next3[2, 0].BackColor = next3[2, 1].BackColor = next3[2, 2].BackColor = Color.Orange;
                    break;
                case 6:
                    next3[2, 0].BackColor = next3[2, 1].BackColor = next3[2, 2].BackColor = next3[1, 2].BackColor = Color.LightBlue;
                    break;
                case 7:
                    next3[1, 0].BackColor = next3[1, 1].BackColor = next3[1, 2].BackColor = next3[2, 1].BackColor = Color.Purple;
                    break;
            }
        }

        void display_next4_block(uint type)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                    next4[i, j].BackColor = Color.FromArgb(0);

            switch (type)
            {
                case 1:
                    next4[0, 1].BackColor = next4[1, 1].BackColor = next4[2, 1].BackColor = next4[3, 1].BackColor = Color.Blue;
                    break;
                case 2:
                    next4[1, 0].BackColor = next4[1, 1].BackColor = next4[2, 0].BackColor = next4[2, 1].BackColor = Color.Yellow;
                    break;
                case 3:
                    next4[2, 0].BackColor = next4[2, 1].BackColor = next4[1, 1].BackColor = next4[1, 2].BackColor = Color.Red;
                    break;
                case 4:
                    next4[1, 0].BackColor = next4[1, 1].BackColor = next4[2, 1].BackColor = next4[2, 2].BackColor = Color.Green;
                    break;
                case 5:
                    next4[1, 0].BackColor = next4[2, 0].BackColor = next4[2, 1].BackColor = next4[2, 2].BackColor = Color.Orange;
                    break;
                case 6:
                    next4[2, 0].BackColor = next4[2, 1].BackColor = next4[2, 2].BackColor = next4[1, 2].BackColor = Color.LightBlue;
                    break;
                case 7:
                    next4[1, 0].BackColor = next4[1, 1].BackColor = next4[1, 2].BackColor = next4[2, 1].BackColor = Color.Purple;
                    break;
            }
        }

        void display_next5_block(uint type)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                    next5[i, j].BackColor = Color.FromArgb(0);

            switch (type)
            {
                case 1:
                    next5[0, 1].BackColor = next5[1, 1].BackColor = next5[2, 1].BackColor = next5[3, 1].BackColor = Color.Blue;
                    break;
                case 2:
                    next5[1, 0].BackColor = next5[1, 1].BackColor = next5[2, 0].BackColor = next5[2, 1].BackColor = Color.Yellow;
                    break;
                case 3:
                    next5[2, 0].BackColor = next5[2, 1].BackColor = next5[1, 1].BackColor = next5[1, 2].BackColor = Color.Red;
                    break;
                case 4:
                    next5[1, 0].BackColor = next5[1, 1].BackColor = next5[2, 1].BackColor = next5[2, 2].BackColor = Color.Green;
                    break;
                case 5:
                    next5[1, 0].BackColor = next5[2, 0].BackColor = next5[2, 1].BackColor = next5[2, 2].BackColor = Color.Orange;
                    break;
                case 6:
                    next5[2, 0].BackColor = next5[2, 1].BackColor = next5[2, 2].BackColor = next5[1, 2].BackColor = Color.LightBlue;
                    break;
                case 7:
                    next5[1, 0].BackColor = next5[1, 1].BackColor = next5[1, 2].BackColor = next5[2, 1].BackColor = Color.Purple;
                    break;
            }
        }

        // 顯示左邊 Hold 的方塊
        void display_save_block(uint type)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                    save[i, j].BackColor = Color.FromArgb(0);

            switch (type)
            {
                case 1:
                    save[0, 1].BackColor = save[1, 1].BackColor = save[2, 1].BackColor = save[3, 1].BackColor = Color.Blue;
                    break;
                case 2:
                    save[1, 0].BackColor = save[1, 1].BackColor = save[2, 0].BackColor = save[2, 1].BackColor = Color.Yellow;
                    break;
                case 3:
                    save[2, 0].BackColor = save[2, 1].BackColor = save[1, 1].BackColor = save[1, 2].BackColor = Color.Red;
                    break;
                case 4:
                    save[1, 0].BackColor = save[1, 1].BackColor = save[2, 1].BackColor = save[2, 2].BackColor = Color.Green;
                    break;
                case 5:
                    save[1, 0].BackColor = save[2, 0].BackColor = save[2, 1].BackColor = save[2, 2].BackColor = Color.Orange;
                    break;
                case 6:
                    save[2, 0].BackColor = save[2, 1].BackColor = save[2, 2].BackColor = save[1, 2].BackColor = Color.LightBlue;
                    break;
                case 7:
                    save[1, 0].BackColor = save[1, 1].BackColor = save[1, 2].BackColor = save[2, 1].BackColor = Color.Purple;
                    break;
            }
        }

        // 檢查所有行，有滿行的刪除
        void full_line_check()
        {
            uint row_sum;
            uint i, j;
            clear_line = 0;
            i = 0;
            while (i < 20)
            {
                row_sum = 0;
                for (j = 0; j < 10; j++)
                    if (signs[i, j]) row_sum++;

                if (row_sum == 10)
                {
                    clear_line++;
                    Form1.clear_sound();
                    for (j = 0; j < 10; j++)
                        signs[i, j] = false;

                    show_grids(); // show a black line 

                    for (uint y = i; y < 21; y++)
                        for (j = 0; j < 10; j++)
                        {
                            signs[y, j] = signs[y + 1, j];
                            grids_color[y, j] = grids_color[y + 1, j];
                        }
                    show_grids();
                }
                else i++;
            }
            label_line.Text = "Line：" + Convert.ToString(clear_line);
            if (clear_line == 1)
            {
                score += 5;
                label_score.Text = score.ToString();
            }
            else if (clear_line == 2)
            {
                score += 10;
                label_score.Text = score.ToString();
            }
            else if (clear_line == 3)
            {
                score += 20;
                label_score.Text = score.ToString();
            }
            else if (clear_line == 4)
            {
                score += 40;
                label_score.Text = score.ToString();
            }
        }

        // 按上轉方向時用到
        uint next_block_type(uint type, uint i, uint j)
        {
            switch (type)
            {
                case 1:
                    if (j <= 7 && j >= 1 && !signs[i + 2, j - 1] && !signs[i + 2, j + 1] && !signs[i + 2, j + 2])
                    {
                        block_row = i + 2; block_col = j - 1;
                        return 11;
                    }
                    else return 1;

                case 11:
                    if (i >= 2 && !signs[i - 1, j + 1] && !signs[i - 2, j + 1] && !signs[i + 1, j + 1])
                    {
                        block_row = i - 2; block_col = j + 1;
                        return 1;
                    }
                    else return 11;

                case 2: return 2;

                case 3:
                    if (i >= 1 && !signs[i + 1, j + 1] && !signs[i - 1, j])
                        return 13;
                    else return 3;

                case 13:
                    if (j >= 1 && !signs[i + 1, j] && !signs[i + 1, j - 1])
                        return 3;
                    else return 13;

                case 4:
                    if (i >= 1 && !signs[i, j + 1] && !signs[i - 1, j + 1])
                        return 14;
                    else return 4;

                case 14:
                    if (j >= 1 && !signs[i, j - 1] && !signs[i + 1, j + 1])
                        return 4;
                    else return 14;

                case 5:
                    if (!signs[i + 2, j] && !signs[i, j + 1])
                    {
                        block_col = j + 1;
                        return 15;
                    }
                    else return 5;

                case 15:
                    if (j >= 2 && !signs[i, j - 2] && !signs[i + 1, j])
                    {
                        block_row = i + 1;
                        return 25;
                    }
                    else return 15;

                case 25:
                    if (i >= 2 && !signs[i, j - 1] && !signs[i - 2, j])
                    {
                        block_col = j - 1;
                        return 35;
                    }
                    else return 25;

                case 35:
                    if (j <= 7 && !signs[i - 1, j] && !signs[i, j + 2])
                    {
                        block_row = i - 1;
                        return 5;
                    }
                    else return 35;

                case 6:
                    if (!signs[i, j - 1] && !signs[i + 2, j])
                    {
                        block_col = j - 1;
                        return 16;
                    }
                    else return 6;

                case 16:
                    if (j <= 7 && !signs[i - 1, j] && !signs[i, j + 2])
                    {
                        block_row = i + 1;
                        return 26;
                    }
                    else return 16;

                case 26:
                    if (i >= 2 && !signs[i, j + 1] && !signs[i - 2, j])
                    {
                        block_col = j + 1;
                        return 36;
                    }
                    else return 26;

                case 36:
                    if (j >= 2 && !signs[i, j - 2] && !signs[i - 1, j])
                    {
                        block_row = i - 1;
                        return 6;
                    }
                    else return 36;

                case 7:
                    if (i >= 1 && !signs[i - 1, j])
                        return 17;
                    else return 7;

                case 17:
                    if (j >= 1 && !signs[i, j - 1])
                        return 27;
                    else return 17;

                case 27:
                    if (!signs[i + 1, j])
                        return 37;
                    else return 27;

                case 37:
                    if (j <= 8 && !signs[i, j + 1])
                        return 7;
                    else return 37;

                default: return 0;
            }
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            // 全螢幕
            this.FormBorderStyle = FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;

            // 讀取玩家名字
            StreamReader str = new StreamReader(log);
            Player_Name.Text = str.ReadLine();
            money = Convert.ToInt32(str.ReadLine());
            str.Close();


            // 物件設置
            pictureBox1.Visible = false;
            resume.Visible = false;
            restart.Visible = false;
            quit.Visible = false;
            timer1.Enabled = false;
            panel2.Visible = false;
            button1.Visible = true;
            button1.Enabled = true;
            label_money.Visible = false;
            pictureBox2.Visible = false;
            result_ok.Visible = false;
            re_score.Visible = false;
            resume.Parent = pictureBox1;
            restart.Parent = pictureBox1;
            quit.Parent = pictureBox1;
            label_money.Parent = pictureBox2;
            result_ok.Parent = pictureBox2;
            re_score.Parent = pictureBox2;


            // 參考程式碼 type1：I，type2：o，type3：z，type4：s，type5：L，type6：J，type7：T
            // 給定一個預設值，讓 block_type_pre 和 block_type_next 也有初始值
            // 好吧，不知道這裡有什麼用，拿掉了也沒事
            // block_type = (uint)rander.Next(0, 7) + 1;
            // block_type = 1;
            // block_type_pre = block_type;
            // block_type_next = block_type;

            // generate 20x10 labels for "main" area, dynamically.
            // 初始版面配置，由 20 x 10 個小 label 組成
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 10; j++)
                {
                    grids[i, j] = new Label();
                    grids[i, j].Width = panel2.Width / 10 - 2;
                    grids[i, j].Height = panel2.Height / 20 - 2;
                    //grids[i, j].BorderStyle = BorderStyle.FixedSingle;
                    grids[i, j].BackColor = Color.FromArgb(60, 255, 255, 255);
                    grids[i, j].Left = panel2.Width / 10 * j + 1;
                    grids[i, j].Top = panel2.Height - (i + 1) * panel2.Height / 20 - 1;
                    grids[i, j].Visible = true;
                    panel2.Controls.Add(grids[i, j]);
                }
            // generate 4x3 labels for "next" area, dynamically.
            // 旁邊的 4 x 3 框框，用來存放下一個要出線的方塊
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    next1[i, j] = new Label();
                    next1[i, j].Width = 20;
                    next1[i, j].Height = 20;
                    // next[i, j].BorderStyle = BorderStyle.FixedSingle;
                    next1[i, j].BackColor = Color.FromArgb(0);
                    next1[i, j].Left = 10 + 20 * j;
                    next1[i, j].Top = NEXT_1.Height - (i + 1) * 20;
                    next1[i, j].Visible = true;
                    NEXT_1.Controls.Add(next1[i, j]);
                }
            // generate 4x3 labels for "next" area, dynamically.
            // 旁邊的 4 x 3 框框，用來存放下一個要出線的方塊
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    next2[i, j] = new Label();
                    next2[i, j].Width = 20;
                    next2[i, j].Height = 20;
                    // next[i, j].BorderStyle = BorderStyle.FixedSingle;
                    next2[i, j].BackColor = Color.FromArgb(0);
                    next2[i, j].Left = 10 + 20 * j;
                    next2[i, j].Top = NEXT_2.Height - (i + 1) * 20;
                    next2[i, j].Visible = true;
                    NEXT_2.Controls.Add(next2[i, j]);
                }
            // generate 4x3 labels for "next" area, dynamically.
            // 旁邊的 4 x 3 框框，用來存放下一個要出線的方塊
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    next3[i, j] = new Label();
                    next3[i, j].Width = 20;
                    next3[i, j].Height = 20;
                    // next[i, j].BorderStyle = BorderStyle.FixedSingle;
                    next3[i, j].BackColor = Color.FromArgb(0);
                    next3[i, j].Left = 10 + 20 * j;
                    next3[i, j].Top = NEXT_3.Height - (i + 1) * 20;
                    next3[i, j].Visible = true;
                    NEXT_3.Controls.Add(next3[i, j]);
                }
            // generate 4x3 labels for "next" area, dynamically.
            // 旁邊的 4 x 3 框框，用來存放下一個要出線的方塊
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    next4[i, j] = new Label();
                    next4[i, j].Width = 20;
                    next4[i, j].Height = 20;
                    // next[i, j].BorderStyle = BorderStyle.FixedSingle;
                    next4[i, j].BackColor = Color.FromArgb(0);
                    next4[i, j].Left = 10 + 20 * j;
                    next4[i, j].Top = NEXT_4.Height - (i + 1) * 20;
                    next4[i, j].Visible = true;
                    NEXT_4.Controls.Add(next4[i, j]);
                }
            // generate 4x3 labels for "next" area, dynamically.
            // 旁邊的 4 x 3 框框，用來存放下一個要出線的方塊
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    next5[i, j] = new Label();
                    next5[i, j].Width = 20;
                    next5[i, j].Height = 20;
                    // next[i, j].BorderStyle = BorderStyle.FixedSingle;
                    next5[i, j].BackColor = Color.FromArgb(0);
                    next5[i, j].Left = 10 + 20 * j;
                    next5[i, j].Top = NEXT_5.Height - (i + 1) * 20;
                    next5[i, j].Visible = true;
                    NEXT_5.Controls.Add(next5[i, j]);
                }
            // generate 4x3 labels for "next" area, dynamically.
            // 左邊的 4 x 3 框框，用來存放按下 C 後 save 的方塊
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    save[i, j] = new Label();
                    save[i, j].Width = 20;
                    save[i, j].Height = 20;
                    // next[i, j].BorderStyle = BorderStyle.FixedSingle;
                    save[i, j].BackColor = Color.FromArgb(0);
                    save[i, j].Left = 10 + 20 * j;
                    save[i, j].Top = HOLD.Height - (i + 1) * 20;
                    save[i, j].Visible = true;
                    HOLD.Controls.Add(save[i, j]);
                }
            // init variables of the game
            panel3.Left = this.ClientSize.Width / 2 - panel3.Width / 2;
            panel3.Top = this.ClientSize.Height / 2 - panel3.Height / 2;
            panel3.Visible = false;

            StreamReader strTmp = new StreamReader(log);
            Player_Name.Text = strTmp.ReadLine();
            Player_Money.Text = strTmp.ReadLine();
            bumb = Convert.ToInt32(strTmp.ReadLine());
            frozen = Convert.ToInt32(strTmp.ReadLine());
            flash = Convert.ToInt32(strTmp.ReadLine());
            switc = Convert.ToInt32(strTmp.ReadLine());
            Bump_Count.Text = Convert.ToString(bumb);
            Frozen_Count.Text = Convert.ToString(frozen);
            Flash_Count.Text = Convert.ToString(flash);
            Switch_Count.Text = Convert.ToString(switc);
            strTmp.Close();

            label_money.Text = "0";
            new_money_show.Text = Convert.ToString(bumb);

            // 初始化遊戲的函數
            init_game();

            // System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            // player.SoundLocation = "xblocks.wav";
            // player.Load();
            // player.PlayLooping();
        }

        // 參考程式碼
        void init_game()
        {
            // 隨機給定第一個方塊的 type
            // block_type = (uint)rander.Next(0, 7) + 1;
            type_count = 0;
            Shuffle(all_block_type);
            // 測試
            // label2.Text = Convert.ToString(type_count);
            block_type = all_block_type[type_count] + 1;
            type_count++;

            // 給定前一個方塊的初值
            block_type_pre = block_type;
            block_row = 20;
            block_col = 4;
            block_row_pre = 20;
            block_col_pre = 4;
            block_type_pre = block_type;
            block_type_next = block_type;
            block_changed = false;
            timer_interval = 1010;
            timer1.Interval = timer_interval;
            block_count = 0;
            score = 0;
            game_mode = 1;

            // 判定每一個有無方塊 0 ～ 24 是加上下一個方塊還沒出現時的高度（呈現出降落的感覺）
            for (uint i = 0; i < 24; i++)
                for (uint j = 0; j < 10; j++)
                    signs[i, j] = false;

            // 清空 Hold 欄
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                    save[i, j].BackColor = Color.FromArgb(0);

            save_detect = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (y_direction(block_type, block_row, block_col))
            {
                block_row_pre = block_row; block_row_pre = block_row; block_type_pre = block_type;
                block_row--;

                if (block_row == 19)
                {
                    if (type_count == 7)
                    {
                        Shuffle(all_block_type);
                        type_count = 0;
                    }
                    // block_type_next = (uint)rander.Next(0, 7) + 1;
                    block_type_next = all_block_type[type_count] + 1;
                    // 測試
                    // label3.Text = Convert.ToString(type_count);
                    display_next1_block(all_block_type[type_count % 7] + 1);
                    display_next2_block(all_block_type[(type_count + 1) % 7] + 1);
                    display_next3_block(all_block_type[(type_count + 2) % 7] + 1);
                    display_next4_block(all_block_type[(type_count + 3) % 7] + 1);
                    display_next5_block(all_block_type[(type_count + 4) % 7] + 1);
                    type_count++;
                    // block_type_next = 1;

                    block_count++;
                    if (game_mode == 1)
                    {
                        // timer_interval = 1010 - (int)(score / 150) * 50;
                        if (timer_interval <= 0)
                            timer_interval = 10;

                        // timer1.Interval = timer_interval;
                        label_level.Text = "Level " + score / 300;
                        // money = score / 300 * 50;
                        label_money.Text = Convert.ToString(money);
                    }
                }
                erase_block(block_row_pre, block_col_pre, block_type_pre);
                update_block(block_row, block_col, block_type);
                show_grids();
                block_row_pre = block_row;
                block_changed = false;
            }
            else
            {
                show_grids();
                full_line_check();
                if (block_row == 20)
                {
                    //Game Over!
                    Form1.gameover_sound();
                    Form1.bgmusic.Play();
                    pictureBox2.Visible = true;
                    label_money.Visible = true;
                    timer1.Enabled = false;
                    panel2.Visible = false;
                    button1.Visible = true;
                    button1.Enabled = true;
                    result_ok.Visible = true;
                    re_score.Visible = true;
                    re_score.Text = Convert.ToString(score);
                    return;

                };
                block_type = block_type_next;
                block_row = 20;
                block_col = 4;
                block_row_pre = 20;
                block_col_pre = 4;
                block_type_pre = block_type;
                block_changed = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            init_game();
            panel2.Visible = true;
            button1.Visible = false;
            button1.Enabled = false;
            timer1.Enabled = true;
            Form1.click_sound();
            Form1.game_bgmusic.PlayLooping();
        }

        private void Form7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                if (x_direction(block_type, block_row, block_col, -1))
                {
                    block_col_pre = block_col; block_col--;
                    block_changed = true;
                }
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                if (x_direction(block_type, block_row, block_col, 1))
                {
                    block_col_pre = block_col; block_col++;
                    block_changed = true;
                }
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Space)
            {
                block_row_pre = block_row;
                while (y_direction(block_type, block_row, block_col))
                {
                    block_row--;
                }
                erase_block(block_row_pre, block_col_pre, block_type_pre);
                update_block(block_row, block_col, block_type);
                show_grids();
                block_row = 20;
                block_col = 4;
                block_row_pre = 20;
                block_col_pre = 4;
                block_type = block_type_next;
                block_changed = false;
                full_line_check();
                Form1.space_sound();
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Up)
            {
                block_type_pre = block_type;
                block_col_pre = block_col; block_row_pre = block_row;
                block_type = next_block_type(block_type, block_row, block_col);
                if (block_type != block_type_pre)
                    block_changed = true;
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Escape)
            {
                timer1.Stop();
                if (MessageBox.Show("是否離開遊戲？", "通知", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Form2 f2 = new Form2(log);
                    f2.Show();
                    this.Hide();
                }
                else
                {
                    timer1.Start();
                }
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.S)
            {
                score += 300;
                label_score.Text = Convert.ToString(score);
                // timer_interval = 1010 - (int)(score / 150) * 50;
                if (timer_interval <= 0)
                    timer_interval = 10;

                // timer1.Interval = timer_interval;
                label_level.Text = "Level " + 1;
                // money = score / 300 * 50;
                label_money.Text = Convert.ToString(money);
            }

            if (e.KeyCode == Keys.R)
            {
                init_game();
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.C)
            {
                if (!save_detect)
                {
                    save_type = block_type;
                    block_type_pre = block_type;
                    block_col_pre = block_col; block_row_pre = block_row;
                    if (save_type > 10)
                    {
                        save_type = save_type % 10;
                    }
                    display_save_block(save_type);
                    erase_block(block_row_pre, block_col_pre, block_type_pre);
                    show_grids();
                    save_detect = true;
                    block_row = 20;
                    block_col = 4;
                    block_row_pre = 20;
                    block_col_pre = 4;
                    block_type = block_type_next;
                    block_changed = false;
                }
                else
                {
                    uint tmp = save_type;
                    save_type = block_type;
                    if (save_type > 10)
                    {
                        save_type = save_type % 10;
                    }
                    block_col_pre = block_col; block_row_pre = block_row;
                    erase_block(block_row_pre, block_col_pre, block_type);
                    block_type = tmp;
                    // label3.Text = Convert.ToString(block_type);
                    update_block(block_row, block_col, block_type);
                    show_grids();
                    display_save_block(save_type);
                }
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Down)
            {
                timer1.Interval = 15;
                e.SuppressKeyPress = true;
            }
                

            if (block_changed)
            {
                erase_block(block_row_pre, block_col_pre, block_type_pre);
                update_block(block_row, block_col, block_type);
                show_grids();
                block_row_pre = block_row; block_col_pre = block_col; block_type_pre = block_type;
                block_changed = false;
            }
        }

        bool add_detect = false;
        private void label3_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8(log, "f7");
            f8.Show();
            this.Hide();
            Form1.click_sound();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            add_detect = false;
            Form1.click_sound();
        }

        private void Backpack_Close_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            Form1.click_sound();
        }

        int Icon_Chose = 1;
        private void icon1_Click(object sender, EventArgs e)
        {
            panel3.BackgroundImage = Image.FromFile("PicData/item/bag1.png");
            new_money_show.Text = Convert.ToString(bumb);
            Icon_Chose = 1;
            Form1.click_sound();
        }

        private void icon2_Click(object sender, EventArgs e)
        {
            panel3.BackgroundImage = Image.FromFile("PicData/item/bag2.png");
            new_money_show.Text = Convert.ToString(frozen);
            Icon_Chose = 2;
            Form1.click_sound();
        }

        private void icon3_Click(object sender, EventArgs e)
        {
            panel3.BackgroundImage = Image.FromFile("PicData/item/bag3.png");
            new_money_show.Text = Convert.ToString(flash);
            Icon_Chose = 3;
            Form1.click_sound();
        }

        private void icon4_Click(object sender, EventArgs e)
        {
            panel3.BackgroundImage = Image.FromFile("PicData/item/bag4.png");
            new_money_show.Text = Convert.ToString(switc);
            Icon_Chose = 4;
            Form1.click_sound();
        }

        private void Form7_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                timer1.Interval = timer_interval;
            }
        }

        private void resume_Click(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
            pictureBox1.Visible = false;
            resume.Visible = false;
            restart.Visible = false;
            quit.Visible = false;
            Form1.click_sound();
        }

        private void restart_Click(object sender, EventArgs e)
        {
            init_game();
            pictureBox1.Visible = false;
            resume.Visible = false;
            restart.Visible = false;
            quit.Visible = false;
            timer1.Start();
            Form1.click_sound();
        }

        private void quit_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2(log);
            f2.Show();
            this.Hide();
            Form1.click_sound();
            Form1.quit_sound();
            Form1.bgmusic.Play();
        }

        private void result_ok_Click(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
            label_money.Visible = false;
            re_score.Visible = false;
            result_ok.Visible = false;
            Form1.click_sound();
        }

        private void menu_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            pictureBox1.Visible = true;
            resume.Visible = true;
            restart.Visible = true;
            quit.Visible = true;
            Form1.click_sound();
        }
        private void MouseHoverSound(object sender, EventArgs e)
        {
            Form1.hover_sound();
        }
    }
}

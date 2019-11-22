using System.Net.Sockets;
using System.Net;
using System.Threading;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Forms;

using AR600_NS;

namespace HandCSharp
{
    public struct moves
    {
        public int Num;
        public int Pos;
        public bool Record;        
    }

    public partial class Form1 : Form
    {
        _AR600 robot = new _AR600();
        int numMot = 23;
        bool trace = false;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            numMot = Convert.ToInt32(numericUpDown1.Value);


            robot.MOT_TPOS(numMot, Convert.ToInt32(textBox1.Text));
            Thread.Sleep(100);
            robot.MOT_TIME(numMot, Convert.ToInt32(textBox2.Text));
            Thread.Sleep(100);
            robot.MOT_CMD(numMot, 7);  // GO
            
        }

        void GO_MOT(int numMot, int pos, int time)
        {
            robot.MOT_TPOS(numMot, pos);
            Thread.Sleep(100);
            robot.MOT_TIME(numMot, time);
            Thread.Sleep(100);
            robot.MOT_CMD(numMot, 7);  // GO
            Thread.Sleep(100);
        }

        void droping()
        {
            if (movgb == 0)
            {
                if (Math.Abs(robot.GET_MOT_POS(16)) > 100)
                    movst(16, 0, 300);
                if (Math.Abs(robot.GET_MOT_POS(17)) > 100)
                    movst(17, 0, 300);
                if (Math.Abs(robot.GET_MOT_POS(18) - 0) > 100)
                    movst(18, 0, 300);
                if (Math.Abs(robot.GET_MOT_POS(19)) > 100)
                    movst(19, 0, 300);
                if (Math.Abs(robot.GET_MOT_POS(20)) > 100)
                    movst(20, 0, 300);

                movst(21, 0, 300);
                movst(22, 0, 300);
                movst(23, 0, 300);
                movst(24, 0, 300);
                movst(25, 0, 300);
                movgb = 1;
                
            }

            if (movgb == 1)
            if ((Math.Abs(robot.GET_MOT_POS(16)) > 100) ||
                (Math.Abs(robot.GET_MOT_POS(17)) > 100) ||
                (Math.Abs(robot.GET_MOT_POS(18)) > 100) ||
                (Math.Abs(robot.GET_MOT_POS(19)) > 100) ||
                (Math.Abs(robot.GET_MOT_POS(20)) > 100))
            {
                return;
            }
            else movgb = 2;

            if (drop == true)
            {
                if (!palm1) palm1 = palm();
                if (palm1) movup(8000);
                if ((mov == -1) && (Math.Abs(robot.GET_MOT_POS(16) - 8000) < 100))
                {
                    mov = 0;
                    palm1 = false;
                    drop = false;
                }
            }
            else
            {
                if ((robot.RH_DIST2 > 17300))// || )
                {
                    if (!fist1) fist1 = fist();
                    if (fist1) movdown();
                }
                else
                {
                    fist1 = false;
                    drop = true;
                }


                if ((mov == 1) && (Math.Abs(robot.GET_MOT_POS(16)) < 100))
                {
                    drop = true;
                    fist1 = false;
                    mov = 0;
                }
            }
        }

        bool hs = false;
        bool ending = false;
        int an = 0;
        int pos1 = 0, pos2 = 0;

        int ob = 0;

        void handshake()
        {
            if ((hs) && (robot.RH_DIST2 < 17300))
            {
                hs = false;
                ending = true;
                palm1 = false;
                fist1 = false;
            }

            if (movgb == 0)
            {
                if (Math.Abs(robot.GET_MOT_POS(16)) > 100)
                    movst(16, 0, 300);
                if (Math.Abs(robot.GET_MOT_POS(17)) > 100)
                    movst(17, 0, 300);
                if (Math.Abs(robot.GET_MOT_POS(18) - 0) > 100)
                    movst(18, 0, 300);
                if (Math.Abs(robot.GET_MOT_POS(19)) > 100)
                    movst(19, 0, 300);
                if (Math.Abs(robot.GET_MOT_POS(20)) > 100)
                    movst(20, 0, 300);

                movst(21, 0, 300);
                movst(22, 0, 300);
                movst(23, 0, 300);
                movst(24, 0, 300);
                movst(25, 0, 300);
                movgb = 1;
                ob = 0;

            }

            if (movgb == 1)
                if ((Math.Abs(robot.GET_MOT_POS(16)) > 100) ||
                    (Math.Abs(robot.GET_MOT_POS(17)) > 100) ||
                    (Math.Abs(robot.GET_MOT_POS(18)) > 100) ||
                    (Math.Abs(robot.GET_MOT_POS(19)) > 100) ||
                    (Math.Abs(robot.GET_MOT_POS(20)) > 100))
                {
                    return;
                }
                else movgb = 2;



            if (ending)
            {
                if (!palm1) palm1 = palm();
                else
                {
                    ending = false;
                    fist1 = false;
                    movb = 0;
                    mov = 0;
                }
            }

            if (((Math.Abs(robot.GET_MOT_POS(16) - 4500) > 100) || (Math.Abs(robot.GET_MOT_POS(19) - 0) > 100)) && (hs == false))
            {
                if ((Math.Abs(robot.GET_MOT_POS(16) - 4500) > 100) && (hs == false))
                {
                    movup(4500);
                }
                else mov = 0;

                if ((Math.Abs(robot.GET_MOT_POS(19) - 0) > 100) && (hs == false))
                    movup2();
                else movb = 0;
                return;
            }
            if (hs)
            {
                an+=24;

                if (ob < 6)
                {
                    if ((Math.Sin(an / 180.0f * Math.PI) < -0.95) && (moving == 1))
                    {
                        movst(16, 7000, 120);
                        movst(19, -500, 80);
                        moving = 2;
                        ob++;
                    }
                    if ((Math.Sin(an / 180.0f * Math.PI) > +0.95) && (moving == 2))
                    {
                        movst(16, 3000, 120);
                        movst(19, 1000, 80);

                        moving = 1;
                        ob++;
                    }
                } else
                if (Math.Abs(robot.GET_MOT_POS(16)-3000)<100)//an > 1060)
                {
                    hs = false;
                    ending = true;
                    palm1 = false;
                }
                return;
            }
            if (robot.RH_DIST2 > 17300)
            {
                if (!fist1) fist1 = fist();
                else
                {
                    hs = true;
                    an = 0;
                    ob = 0;
                    pos1 = robot.GET_MOT_POS(16);
                    pos2 = robot.GET_MOT_POS(19);
                    moving = 1;
                }
            }
            else
            {
                ending = true;
                fist1 = false;
            }
        }


        int hsr = 0;  // 0 - вход, 1 - движение в исходную, 2 - изходная
        void hs2()
        {
            if ((hsr > 3) && (robot.RH_DIST2 < 17300))
                hsr = 0;

            if (hsr == 0)
            {
                int[] time = { 16, 100, 17, 100, 18, 100, 19, 100, 20, 100, 21, 100, 22, 100, 23, 100, 24, 100, 25, 100 };
                int[] pos = { 16, 4500, 17, 0, 18, 0, 19, 0, 20, 0, 21, 0, 22, 0, 23, 0, 24, 0, 25, 0 };
                int[] go = { 16, 0, 17, 0, 18, 0, 19, 0, 20, 0, 21, 0, 22, 0, 23, 0, 24, 0, 25, 0 };
                robot.GROUP_TIME(robot.group_setVal(time));
                robot.GROUP_TPOS(robot.group_setVal(pos));
                robot.GROUP_GO(robot.group_setVal(go));

                hsr = 1;
            }

            if ((hsr == 1) && (waitMot(16, 4500) < 150) && (waitMot(19, 0) < 150))
            {
                hsr = 2;
            }

            if ((hsr == 2) && (robot.RH_DIST2 > 17300))
            {
                int[] time = { 21, 100, 22, 100, 23, 100, 24, 100, 25, 100 };
                int[] pos = { 21, 7500, 22, 7500, 23, 7500, 24, 7500, 25, 7500 };
                int[] go = { 21, 0, 22, 0, 23, 0, 24, 0, 25, 0 };
                robot.GROUP_TIME(robot.group_setVal(time));
                robot.GROUP_TPOS(robot.group_setVal(pos));
                robot.GROUP_GO(robot.group_setVal(go));
                hsr = 3;
            }
            int tmhs = 100;
            if ((hsr == 3) && (waitMot(21, 7000) < 200))
            {
                int[] time = { 16, tmhs, 19, tmhs };
                int[] pos = { 16, 3000, 19, 1000 };
                int[] go = { 16, 0, 19, 0 };
                robot.GROUP_TIME(robot.group_setVal(time));
                robot.GROUP_TPOS(robot.group_setVal(pos));
                robot.GROUP_GO(robot.group_setVal(go));

                hsr = 4;
            }
            if ((hsr == 4) && (waitMot(16,3000) < 400))
            {
                int[] time = { 16, tmhs, 19, tmhs };
                int[] pos = { 16, 7000, 19, -500 };
                int[] go = { 16, 0, 19, 0 };
                robot.GROUP_TIME(robot.group_setVal(time));
                robot.GROUP_TPOS(robot.group_setVal(pos));
                robot.GROUP_GO(robot.group_setVal(go));

                hsr = 5;
            }

            if ((hsr == 5) && (waitMot(16, 7000) < 400))
            {
                int[] time = { 16, tmhs, 19, tmhs };
                int[] pos = { 16, 3000, 19, 1000 };
                int[] go = { 16, 0, 19, 0 };
                robot.GROUP_TIME(robot.group_setVal(time));
                robot.GROUP_TPOS(robot.group_setVal(pos));
                robot.GROUP_GO(robot.group_setVal(go));

                hsr = 6;
            }
            if ((hsr == 6) && (waitMot(16, 3000) < 400))
            {
                int[] time = { 16, tmhs, 19, tmhs };
                int[] pos = { 16, 7000, 19, -500 };
                int[] go = { 16, 0, 19, 0 };
                robot.GROUP_TIME(robot.group_setVal(time));
                robot.GROUP_TPOS(robot.group_setVal(pos));
                robot.GROUP_GO(robot.group_setVal(go));

                hsr = 7;
            }

            if ((hsr == 7) && (waitMot(16, 7000) < 400))
            {
                int[] time = { 16, tmhs, 19, tmhs };
                int[] pos = { 16, 4500, 19, 0 };
                int[] go = { 16, 0, 19, 0 };
                robot.GROUP_TIME(robot.group_setVal(time));
                robot.GROUP_TPOS(robot.group_setVal(pos));
                robot.GROUP_GO(robot.group_setVal(go));

                hsr = 8;
            }

            if ((hsr == 8) && (waitMot(16, 4500) < 100))
            {
                int[] time = { 21, 100, 22, 100, 23, 100, 24, 100, 25, 100 };
                int[] pos = { 21, 0, 22, 0, 23, 0, 24, 0, 25, 0 };
                int[] go = { 21, 0, 22, 0, 23, 0, 24, 0, 25, 0 };
                robot.GROUP_TIME(robot.group_setVal(time));
                robot.GROUP_TPOS(robot.group_setVal(pos));
                robot.GROUP_GO(robot.group_setVal(go));
                hsr = 9;
            }
            if ((hsr == 9) && (waitMot(21, 0) < 100))
            {
                hsr = 0;
            }

        }

        int waitMot(int numMot, int pos)
        {
            return Math.Abs(robot.GET_MOT_POS(numMot) - pos);
        }

        int movgb = 0;
        int moving = 0;
        void goodbye()
        {
            if (movgb == 0)
            {
                if (Math.Abs(robot.GET_MOT_POS(16) - 10000) > 100)
                    movst(16, 10000,300);
                if (Math.Abs(robot.GET_MOT_POS(17)) > 100)
                    movst(17, 0, 300);
                if (Math.Abs(robot.GET_MOT_POS(18) - 0) > 100)
                    movst(18, 0, 300);
                if (Math.Abs(robot.GET_MOT_POS(19) - 6000) > 100)
                    movst(19, 6000, 300);
                if (Math.Abs(robot.GET_MOT_POS(20) - 9000) > 100)
                    movst(20, 9000, 300);

                an = 270;
                movgb = 1;
                moving = 0;
            }
            if (moving == 0)
                if ((Math.Abs(robot.GET_MOT_POS(16) - 10000) > 100) ||
                    (Math.Abs(robot.GET_MOT_POS(17)) > 100) ||
                        (Math.Abs(robot.GET_MOT_POS(18) - 0) > 100) ||
                        (Math.Abs(robot.GET_MOT_POS(19) - 6000) > 100) ||
                        (Math.Abs(robot.GET_MOT_POS(20) - 9000) > 100))
                {
                    return;
                }
                else moving = 1;
            an = (an+6)%360;


            if ((Math.Sin(an / 180.0f * Math.PI) < -0.99) && (moving == 1)) { 
                movst(17, -4000, 200); moving = 2; }
            if ((Math.Sin(an / 180.0f * Math.PI) > +0.99) && (moving == 2)) { 
                movst(17, 3000, 200); moving = 1; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (regime)
            {
                case 1:
                    droping();
                    break;
                case 2:
                    hs2();//handshake();
                    break;
                case 3:
                    goodbye();
                    break;
            } 
            return;            
            int X = (robot.RH_FX - middleX);
            int Y = (robot.RH_FY - middleY);
            int Z = (robot.RH_FZ - middleZ);
            // if (Y == 0) b = true;

            //return;


            if (robot.GET_MOT_POS(16) > 1500)
            {
                if (Math.Abs(Z) > 20)
                {
                    int speed = 0;
                    if (Z < 0) speed = 4;
                    if (Z > 0) speed = -4;
                    Move(19, speed);
                }

                if (Math.Abs(Y) > 200)
                {
                    int speed = 0;
                    if (Y < 0) speed = -5;
                    if (Y > 0) speed = 5;
                    Move(17, speed);
                }
            } 

            if (robot.GET_MOT_POS(16) > 5000)
            {
                if (Math.Abs(Y) > 200)
                {
                    int speed = 0;
                    if (Y < 0) speed = -5;
                    if (Y > 0) speed = 5;
                    Move(17, speed);
                }
            }
            else
            {
                if (Math.Abs(Y) > 200)
                {
                    int speed = 0;
                    if (Y < 0) speed = -4;
                    if (Y > 0) speed = 4;
                    Move(18, speed);
                }
            }

            if (Math.Abs(X) > 200)
            {
                int speed = 0;
                if (X < 0) speed = 6;
                if (X > 0) speed = -6;
                Move(16, speed);
            }
        }
        int middleX = 64815;
        int middleY = 64700;
        int middleZ = 65493;

        bool b = false;

        bool fist1 = false;
        bool palm1 = false;
        int mov = 0;
        int movb = 0;
        bool drop = false;

        void movdown()
        {
            if (mov == 1) return;

            robot.MOT_TPOS(16, 0);
            Thread.Sleep(100);
            robot.MOT_TIME(16, 300);
            Thread.Sleep(100);
            robot.MOT_CMD(16, 7);  // GO

            mov = 1;
        }
        void movup(int ang)
        {
            if (mov == -1) return;
            int tim = (int)(Math.Abs(robot.GET_MOT_POS(16) - ang) / 15.0f);
            robot.MOT_TPOS(16, ang);//8000);
            Thread.Sleep(100);
            robot.MOT_TIME(16, tim);//300);
            Thread.Sleep(100);
            robot.MOT_CMD(16, 7);  // GO
            Thread.Sleep(100);
            mov = -1;
        }
        void movup2()
        {
            if (movb == -1) return;
            int tim = (int)(Math.Abs(robot.GET_MOT_POS(19)) / 15.0f);
            robot.MOT_TPOS(19, 0);
            Thread.Sleep(100);
            robot.MOT_TIME(19, tim);//300);
            Thread.Sleep(100);
            robot.MOT_CMD(19, 7);  // GO
            Thread.Sleep(100);
            movb = -1;
        }



        void movst(int mot, int ang, int time)
        {
            robot.MOT_TPOS(mot, ang);//8000);
            Thread.Sleep(100);
            robot.MOT_TIME(mot, time);
            Thread.Sleep(100);
            robot.MOT_CMD(mot, 7);  // GO
            Thread.Sleep(100);
        }

        void movstop(int mot)
        {
            robot.MOT_CMD(mot, 2);  
            Thread.Sleep(100);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            numMot = Convert.ToInt32(numericUpDown1.Value);
            robot.MOT_CMD(numMot, 2);  // STOP
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (robot.START())
            {
                robot.ON_12V(); Thread.Sleep(100);
                robot.ON_48V(); Thread.Sleep(100);
                robot.ON_8V1(); Thread.Sleep(100);
                robot.ON_8V2(); Thread.Sleep(100);
                robot.ON_6V1(); Thread.Sleep(100);
                robot.ON_6V2(); Thread.Sleep(100);




                numericUpDown1_ValueChanged(this, EventArgs.Empty);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        bool fist()
        {
            bool ft = true;
            if (movfing == false)
            {
                for (int i = 21; i <= 25; i++)
                {
                    //movst(i, robot.GET_MOT_MAXPOS(i), 50);
                    robot.MOT_TPOS(i, robot.GET_MOT_MAXPOS(i));//8000);
                    Thread.Sleep(100);
                    robot.MOT_TIME(i, 100);
                    Thread.Sleep(100);

                    movfing = true;
                }
                for (int i = 21; i <= 25; i++)
                {
                    robot.MOT_CMD(i, 7);  // GO
                    Thread.Sleep(100);
                }

            }
            else
            {
                movfing = false;
                for (int i = 21; i <= 25; i++)
                    if (Math.Abs(robot.GET_MOT_POS(i) - robot.GET_MOT_MAXPOS(i)) > 100)
                    {
                        ft = false;
                        movfing = false;
                    }
            }
            return ft;
        }

        bool movfing = false;
        bool palm()
        {
            bool ft = true;
            if (movfing == false)
            {
                for (int i = 21; i <= 25; i++)
                {
                    robot.MOT_TPOS(i, 0);
                    Thread.Sleep(100);
                    robot.MOT_TIME(i, 100);
                    Thread.Sleep(100);

                    movfing = true;
                }
                for (int i = 21; i <= 25; i++)
                {
                    robot.MOT_CMD(i, 7);  // GO
                    Thread.Sleep(100);
                }


            }
            else
            {
                movfing = false;
                for (int i = 21; i <= 25; i++)
                    if (Math.Abs(robot.GET_MOT_POS(i)) > 100)
                    {
                        ft = false;
                        movfing = false;
                    }
            }
            return ft;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int[] time = { 16, 100, 17, 100, 18, 100, 19, 100, 20, 100, 21, 100, 22, 100, 23, 100, 24, 100, 25, 100 };
            int[] pos = { 16, 0, 17, 0, 18, 0 ,19,0, 20, 0, 21, 0, 22, 0, 23, 0, 24, 0, 25, 0};
            int[] go = { 16, 0, 17, 0, 18, 0, 19, 0, 20, 0, 21, 0, 22, 0, 23, 0, 24, 0, 25, 0 };
            robot.GROUP_TIME(robot.group_setVal(time));
            robot.GROUP_TPOS(robot.group_setVal(pos));
            robot.GROUP_GO(robot.group_setVal(go));

        }


        bool Move(int Mot, int speed)
        {
            bool res = true;
            int i = robot.GET_MOT_POS(Mot) + 100*speed;
            if (i > robot.GET_MOT_MAXPOS(Mot)) { res = false; i = robot.GET_MOT_MAXPOS(Mot); }
            if (i < robot.GET_MOT_MINPOS(Mot)) { res = false; i = robot.GET_MOT_MINPOS(Mot); }
            robot.MOT_PTB(Mot, i);
            return res;
        }

        bool MoveAngle(int Mot, int POS, int angl)
        {
            bool res = true;
            int i = POS + 100 * angl;
            if (i > robot.GET_MOT_MAXPOS(Mot)) { res = false; i = robot.GET_MOT_MAXPOS(Mot); }
            if (i < robot.GET_MOT_MINPOS(Mot)) { res = false; i = robot.GET_MOT_MINPOS(Mot); }
            robot.MOT_PTB(Mot, i);
            return res;
        }

        void getStartAndStop(bool stop)
        {
            for (int i = 16; i <= 25; i++)
                movst(i, 0, 300);

            if (stop)
            {
                Thread.Sleep(4000);
                for (int i = 16; i <= 25; i++)
                {
                    robot.MOT_CMD(i, 2);
                    Thread.Sleep(50);
                }

                for (int i = 16; i <= 25; i++)
                {
                    robot.MOT_CMD(i, 5);
                    Thread.Sleep(50);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Text = "Закрытие";
            getStartAndStop(true);
            robot.OFF_48V(); Thread.Sleep(100);
            robot.OFF_8V1(); Thread.Sleep(100);
            robot.OFF_8V2(); Thread.Sleep(100);
            robot.OFF_6V1(); Thread.Sleep(100);
            robot.OFF_6V2(); Thread.Sleep(100);
            robot.OFF_12V(); Thread.Sleep(100);
            robot.CLOSE();
        }


        int regime = 0;

        private void button3_Click(object sender, EventArgs e)
        {
            regime = 1; movgb = 0;
            timer1.Enabled = true;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            regime = 3; movgb = 0;
            timer1.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            regime = 2;
            movgb = 0;
            timer1.Enabled = true;
        }

        int Last = -1;
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numMot = (int)numericUpDown1.Value;

            if (Last != -1) robot.MOT_CMD(Last, 2);

            trackBar1.Enabled = false;
            trace = false;
            checkBox1.Checked = false;

            trackBar1.Minimum = robot.GET_MOT_MINPOS(numMot);
            trackBar1.Maximum = robot.GET_MOT_MAXPOS(numMot);

            label2.Text = trackBar1.Minimum.ToString();
            label6.Text = trackBar1.Maximum.ToString();

            
            try
            {
                trackBar1.Value = robot.GET_MOT_POS(numMot);
                label7.Text = trackBar1.Value.ToString();
            }
            catch
            {
            }
            Last = numMot;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trace)
                robot.MOT_PTB(numMot, trackBar1.Value);

        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                trackBar1.Value = robot.GET_MOT_POS(numMot);
                robot.MOT_TIME(numMot, 10); Thread.Sleep(100);
                robot.MOT_TPOS(numMot, robot.GET_MOT_POS(numMot)); Thread.Sleep(100);
                robot.MOT_CMD(numMot, 7);
                trace = true;
            }
            else
            {
                robot.MOT_CMD(numMot, 2); Thread.Sleep(100);
                trace = false;
            }

            button1.Enabled = !trace;
            button2.Enabled = !trace;
            trackBar1.Enabled = trace;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            for (int i = 16; i <= 25; i++)
               robot.MOT_CMD(i, 2); Thread.Sleep(100);

        }


    }




}

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
using System.Timers;

using System.IO;
using System.Windows.Forms;


using AR600_NS;

using Microsoft.Speech.Recognition;

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


        int[] positionEngines = new int[30];//num engines = andValue
        int[] finalAimEngines = new int[30];//num engines = andValue
        int[] finalTimeEngines = new int[30];//num engines = andValue
        int[] startEngines = new int[30];//num engines = andValue

       
        public Form1()
        {
            for (int i = 16; i <= 25; i++)
            {
                positionEngines[i] = 0;
                finalAimEngines[i] = -100000;
                finalTimeEngines[i] = 0;
                startEngines[i] = 0;
            }
            InitializeComponent();
            
        }

        static Label lol;

        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > 0.7) lol.Text = e.Result.Text;
        }

        private void Form1_Shown_1(object sender, EventArgs e)
        {
            lol = label27;

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-ru");
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);
            sre.SetInputToDefaultAudioDevice();

            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);


            Choices numbers = new Choices();
            numbers.Add(new string[] { "один", "два", "три", "четыре", "пять", "хелло", "кинь мяч" });


            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ci;
            gb.Append(numbers);


            Grammar g = new Grammar(gb);
            sre.LoadGrammar(g);

            sre.RecognizeAsync(RecognizeMode.Multiple);
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
        int frequency = 0;
        void CheckSensor()
        {
            if (frequency > 2)
            {
                frequency = 0;               
                label8.Text = "sensor 0: " + robot.RH_DIST0.ToString();
                label9.Text = "sensor 1: " + robot.RH_DIST1.ToString();
                label10.Text = "sensor 2: " + robot.RH_DIST2.ToString();
                label11.Text = "sensor 3: " + robot.RH_DIST3.ToString();
                //label12.Text = "RH_FZ: " + robot.RH_KEYS[1].ToString();
            }
            else
            {
                frequency++;
            }
        }
        
        private void OnTimeoutEngines(object sender, EventArgs e)
        {
            CheckFactEnginePos();
            if (regime == 4)
            {                               
                TimerEngineTrace();
            }
        }
        private void CheckFactEnginePos()
        {
            int factpos1 = positionEngines[18];
            int factpos2 = positionEngines[19];
            factpos1 += 1000;//до нуля доводим типо от нуля
            factpos2 += 2000;

            int posKvX = (factpos1 * 200) / 9500+5;//ход
            int posKvY = (factpos2 * 200) / 9500+5;//градус типо
            pictureBox2.Location = new Point(posKvX, posKvY);  

            factpos1 = positionEngines[16];
            factpos2 = positionEngines[17];
            factpos1 += 0;//до нуля доводим типо от нуля
            factpos2 += 4000;

            posKvX = (factpos1 * 200) / 11000 + 5;//ход
            posKvY = (factpos2 * 200) / 8200 + 5;//градус типо
            pictureBox3.Location = new Point(posKvX, posKvY);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckSensor();
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
                case 4:
                    
                    break;
            }            
            return;            
            /*int X = (robot.RH_FX - middleX);
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
            }*/
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
            timer3.Enabled = false;
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

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label10_Click(object sender, EventArgs e)
        {

        }

        private void Label11_Click(object sender, EventArgs e)
        {

        }

        private void Label12_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox5_Move(object sender, EventArgs e)
        {
           
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }



        
        

        private void MainEngine(int[] idEngines, int[] posEngines)//ид двигателя и позиция
        {
            for (int i = 0; i < idEngines.Length; i++)
            {                 
                int itogLast = 0;//прошлое расстояние до цели
                if (robot.GET_MOT_POS(idEngines[i]) > finalAimEngines[idEngines[i]])
                {
                    itogLast = robot.GET_MOT_POS(idEngines[i]) - finalAimEngines[idEngines[i]];
                }
                else
                {
                    itogLast = finalAimEngines[idEngines[i]] - robot.GET_MOT_POS(idEngines[i]);
                }
                finalAimEngines[idEngines[i]] = posEngines[i];
                int itog = 0;//новое расстояние до цели
                if (robot.GET_MOT_POS(idEngines[i]) > finalAimEngines[idEngines[i]])
                {
                    itog = robot.GET_MOT_POS(idEngines[i]) - finalAimEngines[idEngines[i]];
                }
                else
                {
                    itog = finalAimEngines[idEngines[i]] - robot.GET_MOT_POS(idEngines[i]);
                }                
                if (finalTimeEngines[idEngines[i]] <= 0)
                {                  
                    finalTimeEngines[idEngines[i]] = 40;                   
                    startEngines[idEngines[i]] = 1;
                }
            }            
            regime = 4;            
        }


        private void TimerEngineTrace()
        {
            int xod = 0;            
            int[] goengine = new int[20];
            for (int i = 16; i <= 25; i++)
            {
                
                if (finalAimEngines[i] != -100000)
                {
                    positionEngines[i] = robot.GET_MOT_POS(i);//position engines now
                    //Thread.Sleep(5);

                    /*if (Math.Abs(robot.GET_MOT_POS(i)) - Math.Abs(finalAimEngines[i]) < 100 ||//типо ограничитель чтоб моторы не пошли за диапозон
                        robot.GET_MOT_POS(i)+100 > robot.GET_MOT_MAXPOS(i) || robot.GET_MOT_POS(i) - 100 < robot.GET_MOT_MINPOS(i))
                    {
                        finalTimeEngines[i] = 0;
                        finalAimEngines[i] = -100000;
                        continue;
                    }*/
                    int finalPos = 0;
                    finalPos = finalAimEngines[i];
                    int itog = 0;
                    if (positionEngines[i] > finalPos)
                    {
                        itog = positionEngines[i] - finalPos;
                    }
                    else
                    {
                        itog = finalPos - positionEngines[i];//расстояние
                    }
                    //itog - оставшееся единицы  расстояния    

                    /*******************Механника движения по скорости*************************/
                    if (itog > 3000)
                    {
                        finalTimeEngines[i] += 75;//скорость
                    }
                    else if (itog <= 3000 && itog > 2000)
                    {
                        if (finalTimeEngines[i] < 450) {finalTimeEngines[i] += 60;}//удерживаем скорость 450
                        else if (finalTimeEngines[i] == 450) { }
                        else {finalTimeEngines[i] -= 50;}                        
                        //if (finalTimeEngines[i] < 200) {finalTimeEngines[i] += 40;}
                    }
                    else if (itog <= 2000 && itog > 1000)
                    {
                        if (finalTimeEngines[i] < 400) { finalTimeEngines[i] += 50;}//удерживаем скорость 400
                        else if (finalTimeEngines[i] == 400) { }
                        else {finalTimeEngines[i] -= 40;}
                        
                        //if (finalTimeEngines[i] < 100) { finalTimeEngines[i] += 20; }
                    }                    
                    else if (itog <= 1000 && itog > 200)
                    {
                        if (finalTimeEngines[i] < 300) { finalTimeEngines[i] += 30; }//удерживаем скорость 300
                        else if (finalTimeEngines[i] == 300) { }
                        else { finalTimeEngines[i] -= 30; }
                    }
                    else
                    {
                        finalTimeEngines[i] = 150;
                    }
                    if (finalTimeEngines[i] < 50) { finalTimeEngines[i] = 50; }
                    //if (itog < finalTimeEngines[i]) { finalTimeEngines[i] = itog - 10; }
                    if (finalTimeEngines[i] >= 1000)
                    {
                        finalTimeEngines[i] = 1000;
                    }
                    /**************************************************************************/

                    int finalfinal = 0;

                    int var1 = positionEngines[i] - finalTimeEngines[i];//от позиции руки минусуем шаг
                    int var2 = positionEngines[i] + finalTimeEngines[i];//от позиции руки плюсуем шаг

                    //какой из них ближе к точке $var1 или $var2
                    if (Math.Abs(var1 - finalPos) < Math.Abs(var2 - finalPos))
                    {
                        finalfinal = var1;
                        //минус
                        /*if (finalfinal - 30 < robot.GET_MOT_MINPOS(i))
                        {
                            finalTimeEngines[i] = 0;
                            finalAimEngines[i] = -100000;
                            continue;
                        }*/
                    }
                    else {
                        finalfinal = var2;
                        //плюс
                        /*if (finalfinal + 30 > robot.GET_MOT_MAXPOS(i))
                        {
                            finalTimeEngines[i] = 0;
                            finalAimEngines[i] = -100000;
                            continue;
                        }*/
                    }


                    if (itog <= 50)
                    {
                        finalTimeEngines[i] = 0;
                        finalfinal = finalAimEngines[i];
                        finalAimEngines[i] = -100000;
                        //StopThisEngine(i);
                        continue;
                    }
                    


                    if (i == 19 && finalfinal < 0) { finalfinal = 0; }//система защиты (инцидент со столом)
                    if (i == 18)//логи для отслеживания
                    {
                        label12.Text = "18 motor " + finalTimeEngines[i].ToString() + " dist" + itog;
                    }
                    if (i == 19)//логи для отслеживания
                    {
                        label24.Text = "19 motor " + finalTimeEngines[i].ToString() + " dist" + itog;
                    }

                    goengine[xod] = i;  
                    goengine[xod + 1] = finalfinal;
                    xod += 2;
                }
            }
            if (xod > 0)
            {
                robot.GROUP_PTB(robot.group_setVal(goengine));
            }
        }

        private void StopThisEngine(int engineid)
        {
            //movstop(engineid);
            robot.MOT_CMD(engineid, 5);
        }        

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (clickInJoy1 == true)
            {
                int newPosX = e.X - 10;
                int newPosY = e.Y - 10;
                if(newPosX < 0) { newPosX = 0; }
                if (newPosX > 199) { newPosX = 199; }
                if (newPosY < 0) { newPosY = 0; }
                if (newPosY > 199) { newPosY = 199; }

                label19.Text = newPosX.ToString();
                label20.Text = newPosY.ToString();
                pictureBox4.Location = new Point(newPosX, newPosY);


                //изменение позиции              
                int[] idEngineser = new int[2];//локоть и предплечье
                idEngineser[0] = 16;//Поворот плеча (влево, вправо) //newPosX -1000 - 8500 (9500 ход)
                idEngineser[1] = 17;//Поворот плеча (вперед, назад) //newPosY -2000 - 7500 (9500 ход)



                int[] posEngineser = new int[2];

                posEngineser[0] = (11000 * newPosX) / 200;//ход
                posEngineser[1] = (8200 * newPosY) / 200;//градус типо

                posEngineser[0] -= 0;
                posEngineser[1] -= 4000;

                MainEngine(idEngineser, posEngineser);
            }
        }
        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (clickInJoy2 == true)
            {
                int newPosX = e.X - 10;
                int newPosY = e.Y - 10;
                if (newPosX < 1) { newPosX = 1; }
                if (newPosX > 200) { newPosX = 200; }
                if (newPosY < 1) { newPosY = 1; }
                if (newPosY > 200) { newPosY = 200; }
                label21.Text = newPosX.ToString();
                label22.Text = newPosY.ToString();
                pictureBox1.Location = new Point(newPosX, newPosY);



                //изменние позиции
                int[] idEngineser = new int[2];
                idEngineser[0] = 18;//Поворот плеча (влево, вправо) //newPosX -1000 - 8500 (9500 ход)
                idEngineser[1] = 19;//Поворот плеча (вперед, назад) //newPosY -2000 - 7500 (9500 ход)



                int[] posEngineser = new int[2];

                posEngineser[0] = (9500 * newPosX) / 200;//ход
                posEngineser[1] = (9500 * newPosY) / 200;//градус типо

                posEngineser[0] -= 1000;
                posEngineser[1] -= 2000;

                MainEngine(idEngineser, posEngineser);
            }
        }


        bool clickInJoy1 = false;
        bool clickInJoy2 = false;
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            clickInJoy1 = !clickInJoy1;
        }
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            clickInJoy2 = !clickInJoy2;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (clickInJoy2 == true) { clickInJoy2 = !clickInJoy2; }
            if (clickInJoy1 == true) { clickInJoy1 = !clickInJoy1; }
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            if (clickInJoy2 == true) { clickInJoy2 = !clickInJoy2; }
            if (clickInJoy1 == true) { clickInJoy1 = !clickInJoy1; }
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
            /*int[] time = { 16, 100, 17, 100, 18, 100, 19, 100, 20, 100, 21, 100, 22, 100, 23, 100, 24, 100, 25, 100 };
            int[] pos = { 16, 4500, 17, 0, 18, 0, 19, 0, 20, 0, 21, 0, 22, 0, 23, 0, 24, 0, 25, 0 };
            int[] go = { 16, 0, 17, 0, 18, 0, 19, 0, 20, 0, 21, 0, 22, 0, 23, 0, 24, 0, 25, 0 };
            robot.GROUP_TIME(robot.group_setVal(time));
            robot.GROUP_TPOS(robot.group_setVal(pos));
            robot.GROUP_GO(robot.group_setVal(go));*/

                        //trackBar1.Value = robot.GET_MOT_POS(numMot);
                        //trackBar1.Value = robot.GET_MOT_POS(18);
                        //robot.MOT_TIME(18, 10); Thread.Sleep(100);
                        //robot.MOT_TPOS(18, robot.GET_MOT_POS(18)); Thread.Sleep(100);
                        //robot.MOT_CMD(18, 6);

                        robot.MOT_PTB(18, 1000);


            //trackBar1.Value = robot.GET_MOT_POS(19);
            //robot.MOT_TIME(19, 10); Thread.Sleep(100);
            //robot.MOT_TPOS(19, robot.GET_MOT_POS(19)); Thread.Sleep(100);
            //robot.MOT_CMD(19, 6);


            robot.MOT_PTB(19, 1000);
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            robot.MOT_PTB(18, 3000);
            robot.MOT_PTB(19, 3000);

        }
        private void Label26_Click(object sender, EventArgs e)
        {

        }

        private void TrackBar8_Scroll(object sender, EventArgs e)
        {
            //robot.MOT_CMD(18, 3);
            //robot.MOT_CMD(19, 3);  
            int[] idEngineser = new int[1];
            idEngineser[0] = 20;//Поворот кисти
            int[] posEngineser = new int[1];
            posEngineser[0] = trackBar8.Value;//ход            
            MainEngine(idEngineser, posEngineser);
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            int[] time = { 16, 17, 18,19 };
           
            robot.GROUP_STOP(robot.group_setVal(time));

            /*for (int i = 16; i <= 25; i++)
            {                
                robot.MOT_TIME(i, 10); 
                robot.MOT_TPOS(i, robot.GET_MOT_POS(i)); Thread.Sleep(100);
                robot.MOT_CMD(i, 7);
            }*/
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TrackBar3_Scroll(object sender, EventArgs e)
        {
            int[] idEngineser = new int[1];
            idEngineser[0] = 21;//Большой палец
            int[] posEngineser = new int[1];
            posEngineser[0] = trackBar3.Value;//ход            
            MainEngine(idEngineser, posEngineser);
        }

        private void TrackBar4_Scroll(object sender, EventArgs e)
        {
            int[] idEngineser = new int[1];
            idEngineser[0] = 22;//Указательный палец
            int[] posEngineser = new int[1];
            posEngineser[0] = trackBar4.Value;//ход            
            MainEngine(idEngineser, posEngineser);
        }

        private void TrackBar5_Scroll(object sender, EventArgs e)
        {
            int[] idEngineser = new int[1];
            idEngineser[0] = 23;//Средний
            int[] posEngineser = new int[1];
            posEngineser[0] = trackBar5.Value;//ход            
            MainEngine(idEngineser, posEngineser);
        }

        private void TrackBar6_Scroll(object sender, EventArgs e)
        {
            int[] idEngineser = new int[1];
            idEngineser[0] = 24;//Безымянный
            int[] posEngineser = new int[1];
            posEngineser[0] = trackBar6.Value;//ход            
            MainEngine(idEngineser, posEngineser);
        }

        private void TrackBar7_Scroll(object sender, EventArgs e)
        {
            int[] idEngineser = new int[1];
            idEngineser[0] = 25;//Мизинец
            int[] posEngineser = new int[1];
            posEngineser[0] = trackBar7.Value;//ход            
            MainEngine(idEngineser, posEngineser);
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            //trackBar2.Value - положение всех пальцев в %
            int posproc = trackBar2.Value;

            int[] idEngineser = new int[5];//локоть и предплечье
            idEngineser[0] = 21;//newPosX -1000 - 8500 (9500 ход)
            idEngineser[1] = 22;//newPosY -2000 - 7500 (9500 ход)
            idEngineser[2] = 23;//newPosY -2000 - 7500 (9500 ход)
            idEngineser[3] = 24;//newPosY -2000 - 7500 (9500 ход)
            idEngineser[4] = 25;//newPosY -2000 - 7500 (9500 ход)

            int[] posEngineser = new int[5];
            posEngineser[0] = (7100 * posproc) / 100;
            posEngineser[1] = (8300 * posproc) / 100;
            posEngineser[2] = (8500 * posproc) / 100;
            posEngineser[3] = (8500 * posproc) / 100;
            posEngineser[4] = (7900 * posproc) / 100;

            posEngineser[0] -= 100;
            posEngineser[1] -= 100;
            posEngineser[2] -= 100;
            posEngineser[3] -= 100;
            posEngineser[4] -= 100;
            MainEngine(idEngineser, posEngineser);
        }        

        private void button9_Click(object sender, EventArgs e)
        {
            for (int i = 16; i <= 25; i++)
               robot.MOT_CMD(i, 2); Thread.Sleep(100);

        }


    }




}

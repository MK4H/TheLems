using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheLems
{
    public partial class HlavniOkno : Form
    {
        
        Point[] MenuCoordinates;
        enum State { Menu, Hra, Pauza }
        State Stav;
        Bitmap ToPictureBox, Popredi, Pozadi, Menu, Lemmings;
        Logika Hra;
        Graphics Grafika;
        DateTime Cas; //FORTESTING
        TimeSpan UbehlyCas; //FORTESTING

        public HlavniOkno()
        {
            InitializeComponent();

            this.ClientSize = new Size(PictureBox.Width, PictureBox.Height);

            ToPictureBox = new Bitmap(1280, 768);
            Grafika = Graphics.FromImage(ToPictureBox);
            PictureBox.Image = ToPictureBox;

            Stav = State.Menu;

            SwitchToGame("1");

            //NASTAVENI RYCHLOSTI
            Timer.Interval = 25;
        }


        //AUTOMATICKE METODY
        private void PictureBox_Click(object sender, EventArgs e)
        {
           
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Cas = DateTime.Now; //FORTESTING

            int[] PoziceATypy = Hra.Tick();
            GameDraw(PoziceATypy);
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (Stav)
                {
                    case State.Menu:
                        break;
                    case State.Hra:
                        break;
                    case State.Pauza:
                        break;
                    default:
                        break;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                switch (Stav)
                {
                    case State.Menu:
                        break;
                    case State.Hra:
                        Hra.Select(0);
                        break;
                    case State.Pauza:
                        break;
                    default:
                        break;
                }
            }
        }
        
        
        // MOJE METODY
        private void SwitchToMenu()
        {

        }

        private void SwitchToGame(string Level)
        {
            string cesta = "Levels\\Level" + Level;

            switch (Stav)
            {
                case State.Menu:
                    //Menu.Dispose();

                    Popredi = new Bitmap(System.IO.Path.Combine(cesta + "_popredi.png"));
                    Pozadi = new Bitmap(System.IO.Path.Combine(cesta + "_pozadi.png"));
                    Lemmings = new Bitmap(@"Animations\Lemmings.png");

                    Hra = new Logika(Popredi,100,20);
                    Timer.Start();
                    break;
                
                case State.Pauza:
                    break;
                
            }
        }

        private void GameDraw(int[] PoziceATypy)
        {
            Grafika.Clear(Color.Empty);
            Grafika.DrawImage(Pozadi, 0, 0);
            Grafika.DrawImage(Popredi, 0, 0);
            for (int i = 0; i < PoziceATypy.Length; i += 3)
            {
                switch (PoziceATypy[i])
                {
                    case 1: Grafika.DrawImage(Lemmings, PoziceATypy[i + 1] - 4, PoziceATypy[i + 2] - 10);
                        break;
                }
            }
            Grafika.DrawString(UbehlyCas.Milliseconds.ToString(), new Font("Arial",10), Brushes.Black, 50, 50);//FORTESTING
            PictureBox.Refresh();
            UbehlyCas = DateTime.Now.Subtract(Cas);
        }
    }

    class Logika
    {
        abstract class Lemming
        {
            public Point Pozice;
            public int Falling;
            public int Smer;
            public bool detonate;
            protected int TicksToDetonation;
            public abstract bool Move(Bitmap Popredi); //bool protoze kdyz umre tak to vrati true a muzu ho zabit
            
        }

        class Walker : Lemming
        {
            public override bool Move(Bitmap Popredi)
            {
                if (Falling > 0)
                {
                    if (Popredi.GetPixel(Pozice.X, Pozice.Y + 1).A != 0)
                    {
                        if (Falling > 10)
                        {
                            return true;
                        }
                        else
                        {
                            Falling = 0;
                        }
                    }
                    else
                    {
                        Pozice.Y++;
                        Falling++;
                    }
                }
                else
                {
                    if (Smer != 0 )
                    {
                        int Posun = 0;
                        for (int i = 10; i >=0; i--)
                        {
                            if  (Popredi.GetPixel(Pozice.X + Smer,Pozice.Y - i).A != 0)
                            {
                                Posun = i;
                                break;
                            }
                        }

                        if ( Posun < 5) //Velikost lema / 2
                        {
                            Pozice.X++;
                            Pozice.Y -= Posun;
                        }
                        else
                        {
                            Smer = Smer * (-1);
                        }
                    }

                    if (Popredi.GetPixel(Pozice.X,Pozice.Y + 1).A == 0)
                    {
                        Falling = 1;
                    }
                }
                return false;
            }

            public Walker(Point Pozice, int Falling, int Smer,bool detonate)
            {
                this.Pozice = Pozice;
                this.Falling = Falling;
                this.Smer = Smer;
                this.detonate = detonate;
                TicksToDetonation = 5*1000 / 24; //5 sekund
            }
        }

        Lemming[] Lemmingove;
        Bitmap Popredi;
        int Selected;
        int CisloTicku;
        int RychlostSpawnu;
        int AktualniPocetZivichLemmingu;
        int PocetSpawnutych;
        int CelkovyPocetLemmingu;

        public int[] Tick() //vraci pro kazdyho 3 hodnoty v poradi TYP, X, Y
        {
            int[] navrat = new int[(AktualniPocetZivichLemmingu+1)*3]; //Predelat na vraceni jinyho typu
            int TempInt = 0; 
            
            //Move
            for (int i = 0; i < Lemmingove.Length; i++)
            {
                if (Lemmingove[i] != null)
                    if (Lemmingove[i].Move(Popredi))
                    {   
                        navrat[TempInt] = 0;
                        TempInt++;
                        navrat[TempInt] = Lemmingove[i].Pozice.X;
                        TempInt++;
                        navrat[TempInt] = Lemmingove[i].Pozice.Y;
                        TempInt++;
                        Lemmingove[i] = null;//DEATH
                        AktualniPocetZivichLemmingu--;
                    }
                    else
                    {
                        if (Lemmingove[i] is Walker)
                            navrat[TempInt] = 1;
                        else
                            navrat[TempInt] = 2; //atd
                        TempInt++;
                        navrat[TempInt] = Lemmingove[i].Pozice.X;
                        TempInt++;
                        navrat[TempInt] = Lemmingove[i].Pozice.Y;
                        TempInt++;
                    }
            }


            //Spawn
            if (CisloTicku == 0 && PocetSpawnutych < CelkovyPocetLemmingu)
            {
                Lemmingove[AktualniPocetZivichLemmingu] = new Walker(new Point(100,100),0,1,false);
                navrat[TempInt] = 1;
                TempInt++;
                navrat[TempInt] = Lemmingove[AktualniPocetZivichLemmingu].Pozice.X;
                TempInt++;
                navrat[TempInt] = Lemmingove[AktualniPocetZivichLemmingu].Pozice.Y;
                TempInt++;
                AktualniPocetZivichLemmingu++;
                PocetSpawnutych++;
            }

            CisloTicku = (CisloTicku + 1) % RychlostSpawnu;
            return navrat;
        }


        public void LemmingsClick(Point kde)
        {

        }

        

        public void Select(int co)
        {
            Selected = co;
        }


        public Logika(Bitmap Popredi, int PocetLemmingu, int RychlostSpawnu)
        {
            this.Popredi = Popredi;
            this.RychlostSpawnu = RychlostSpawnu;
            Lemmingove = new Lemming[PocetLemmingu];
            Selected = 0;
            CisloTicku = 0;
            AktualniPocetZivichLemmingu = 0;
            PocetSpawnutych = 0;
            CelkovyPocetLemmingu = PocetLemmingu;
        }

        


    }
}

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

            ForDrawing PoziceATypy = Hra.Tick();
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

                    Hra = new Logika(Popredi); //Bude vetsinu nacitat ze souboru pro danou mapu
                    Timer.Start();
                    break;
                
                case State.Pauza:
                    break;
                
            }
        }

        private void GameDraw(ForDrawing PoziceATypy)
        {
            Grafika.Clear(Color.Empty);
            Grafika.DrawImage(Pozadi, 0, 0);
            Grafika.DrawImage(Popredi, 0, 0);
            while (PoziceATypy != null)
            { 
                switch (PoziceATypy.Typ)
                {
                    case 1: Grafika.DrawImage(Lemmings, PoziceATypy.Souradnice.X - 4, PoziceATypy.Souradnice.Y - 10);
                        break;
                }
            }
            Grafika.DrawString(UbehlyCas.Milliseconds.ToString(), new Font("Arial",10), Brushes.Black, 50, 50);//FORTESTING
            PictureBox.Refresh();
            UbehlyCas = DateTime.Now.Subtract(Cas);
        }
    }

    class ForDrawing //Pro predavani informaci, kam nakreslit lemingy
    {
        public Point Souradnice;
        public int Typ;
        public int Smer;
        public bool Death;
        public bool detonation;
        public ForDrawing Dalsi;

        public ForDrawing(Point Souradnice, int Typ, int Smer, bool Death, bool detonation)
        {
            this.Souradnice = Souradnice;
            this.Typ = Typ;
            this.Smer = Smer;
            this.Death = Death;
            this.detonation = detonation;
            Dalsi = null;
        }

        public ForDrawing()
        {
            Typ = -1; //Oznaceni hlavy spojaku
            Dalsi = null;
        }
    }


    class Logika
    {

        class Spawn
        {
            int DobaMeziSpawny;
            int TickOdPredchozihoSpawnu;
            public Point Souradnice;

            public Lemming Tick()
            {
                TickOdPredchozihoSpawnu = (TickOdPredchozihoSpawnu + 1) % DobaMeziSpawny;
                if (TickOdPredchozihoSpawnu == 0)
                {
                    return new Walker(Souradnice, 0, 1, false);
                }
                else
                {
                    return null;
                }
            }


            public Spawn(int DobaMeziSpawny, Point Souradnice)
            {
                this.DobaMeziSpawny = DobaMeziSpawny;
                this.Souradnice = Souradnice;
                TickOdPredchozihoSpawnu = -1; //Mozne pridat delay pred prvnim spawnem
            }


        }

        abstract class Lemming
        {
            public int Typ;
            public Point Pozice;
            public int Falling;
            public int Smer;
            public bool detonate;
            protected int TicksToDetonation;
            public abstract int Move(Bitmap Popredi); //bool protoze kdyz umre tak to vrati true a muzu ho zabit
            
        }

        class Walker : Lemming
        {
            public override int Move(Bitmap Popredi)
            {
                if (Falling > 0)
                {
                    if (Popredi.GetPixel(Pozice.X, Pozice.Y + 1).A != 0)
                    {
                        if (Falling > 10)
                        {
                            return 1;
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
                return 0;
            }

            public Walker(Point Pozice, int Falling, int Smer,bool detonate)
            {
                this.Pozice = Pozice;
                this.Falling = Falling;
                this.Smer = Smer;
                this.detonate = detonate;
                Typ = 1;
                TicksToDetonation = 5*1000 / 24; //5 sekund
            }
        }

        Lemming[] Lemmingove;
        Spawn[] Spawny;
        Bitmap Popredi;
        int Selected;
        int AktualniPocetZivichLemmingu;
        int PocetSpawnutych;


        public ForDrawing Tick() 
        {
            ForDrawing navrat = new ForDrawing(); //Predelat na vraceni jinyho typu
            
            //Move
            for (int i = 0; i < Lemmingove.Length; i++)
            {
                if (Lemmingove[i] != null)
                    switch (Lemmingove[i].Move(Popredi))
                    {
                        case 0: //Zije
                            navrat.Dalsi = new ForDrawing(Lemmingove[i].Pozice, Lemmingove[i].Typ, Lemmingove[i].Smer, false, false);
                            navrat = navrat.Dalsi;
                            break;


                        case 1: //Spadnul
                            navrat.Dalsi = new ForDrawing(Lemmingove[i].Pozice, Lemmingove[i].Typ, Lemmingove[i].Smer,true,false);
                            navrat = navrat.Dalsi;
                            Lemmingove[i] = null;//DEATH
                            AktualniPocetZivichLemmingu--;
                        break;

                        case 2: //Detonate
                            break;
                   
                    }
            }


            //Spawn
            Lemming TempLemming;
            for (int i = 0; (PocetSpawnutych < Lemmingove.Length) && (i < Spawny.Length); i++)
            {
                TempLemming = Spawny[i].Tick();
                if (TempLemming != null)
                {
                    Lemmingove[PocetSpawnutych] = TempLemming;
                    PocetSpawnutych++;
                    AktualniPocetZivichLemmingu++;
                    navrat.Dalsi = new ForDrawing(TempLemming.Pozice, TempLemming.Typ, TempLemming.Smer, false, false);
                }
            }
            

            return navrat;
        }


        public void LemmingsClick(Point kde)
        {

        }

        

        public void Select(int co)
        {
            Selected = co;
        }


        public Logika(Bitmap Popredi) //Bude vetsinu nacitat ze souboru pro mapu
        {
            this.Popredi = Popredi;
            Lemmingove = new Lemming[1];
            Selected = 0;
            AktualniPocetZivichLemmingu = 0;
            PocetSpawnutych = 0;

            //FORTESTING
            Spawny = new Spawn[1];
            Spawny[0] = new Spawn(50, new Point(100, 100));
        }

        


    }
}

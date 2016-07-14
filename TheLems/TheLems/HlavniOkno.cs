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
        Bitmap ToPictureBox, Popredi, Pozadi;
        Bitmap[] ObrazkyLemmu;
        Logika Hra;
        Graphics GrafikaOkna;
        DateTime Cas; //FORTESTING
        TimeSpan UbehlyCas; //FORTESTING

        public HlavniOkno()
        {
            InitializeComponent();

            this.ClientSize = new Size(PictureBox.Width, PictureBox.Height);

            ToPictureBox = new Bitmap(1280, 768);
            GrafikaOkna = Graphics.FromImage(ToPictureBox);
            PictureBox.Image = ToPictureBox;

            Stav = State.Menu;

            SwitchToGame("1");

            //NASTAVENI RYCHLOSTI
            Timer.Interval = Konstanty.Rychlosthry;
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
                        Hra.LemmingsClick(e.Location);
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
                    Stav = State.Hra;
                    Bitmap TempBMP;

                    Popredi = new Bitmap(System.IO.Path.Combine(cesta + "_popredi.png"));
                    Pozadi = new Bitmap(System.IO.Path.Combine(cesta + "_pozadi.png"));
                    TempBMP = new Bitmap(@"Animations\TriZidiTest.png");
                    ObrazkyLemmu = new Bitmap[TempBMP.Height / Konstanty.velikostLemaY];

                    for (int i = 0; i < ObrazkyLemmu.Length; i++)
                    {
                        ObrazkyLemmu[i] = TempBMP.Clone(new Rectangle(0, i * Konstanty.velikostLemaY, Konstanty.velikostLemaX, Konstanty.velikostLemaY), System.Drawing.Imaging.PixelFormat.DontCare);
                    }

                    Hra = new Logika(Popredi); //Bude vetsinu nacitat ze souboru pro danou mapu
                    Hra.Select(2);

                    Timer.Start();
                    break;

                case State.Pauza:
                    break;

            }
        }

        private void GameDraw(ForDrawing PoziceATypy)
        {
            GrafikaOkna.Clear(Color.Empty);
            GrafikaOkna.DrawImage(Pozadi, 0, 0);
            GrafikaOkna.DrawImage(Popredi, 0, 0);
            while (PoziceATypy != null)
            {
                if (PoziceATypy.Typ >= 0)
                    GrafikaOkna.DrawImage(ObrazkyLemmu[PoziceATypy.Typ], PoziceATypy.Souradnice.X - (Konstanty.velikostLemaX / 2), PoziceATypy.Souradnice.Y - Konstanty.velikostLemaY);


                PoziceATypy = PoziceATypy.Dalsi;
            }
            GrafikaOkna.DrawString(UbehlyCas.Milliseconds.ToString(), new Font("Arial", 10), Brushes.Black, 50, 50);//FORTESTING
            PictureBox.Refresh();
            UbehlyCas = DateTime.Now.Subtract(Cas); //FORTESTING
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


    static class Konstanty
    {
        public static int velikostLemaX = 24;
        public static int velikostLemaY = 30;
        public static int velikostKurzoru = 40;
        public static int Rychlosthry = 25;
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
            public int TicksToDetonation;
            public virtual int Move(Bitmap Popredi)//hodnoty 0 - zije, 1 - spadnul, 2 - detonate, 3 - vycerpal special vec
            {
                if (Popredi.GetPixel(Pozice.X, Pozice.Y + 1).A == 0)
                {
                    return Fall(Popredi);
                }
                else
                {
                    if (Falling > Konstanty.velikostLemaY * 4) //BALANCHRY
                    {
                        return 1;
                    }
                    else
                    {
                        Falling = 0;
                    }
                    return Sideways(Popredi);
                }
            }

            protected virtual int Fall(Bitmap Popredi)
            { 
                Pozice.Y++;
                Falling++;
                return 0;
            }

            protected virtual int Sideways(Bitmap Popredi)
            {
                if (Smer != 0)
                {
                    int Posun = 0;
                    for (int i = Konstanty.velikostLemaY; i >= -Konstanty.velikostLemaY / 2; i--) //VELIKOSTLEMA
                    {
                        if (Popredi.GetPixel(Pozice.X + Smer, Pozice.Y - i).A != 0)
                        {
                            Posun = i;
                            break;
                        }
                    }

                    if (Posun < Konstanty.velikostLemaY / 2)
                    {
                        Pozice.X += Smer;
                        Pozice.Y -= Posun;
                    }
                    else
                    {
                        Smer = Smer * (-1);
                    }
                }

                if (Popredi.GetPixel(Pozice.X, Pozice.Y + 1).A == 0)
                {
                    Falling = 1;
                }
                return 0;
            }

            
        }

        class Walker : Lemming
        {
            public Walker(Point Pozice,int Falling, int Smer, bool detonate)
            {
                this.Typ = 0;
                this.Pozice = Pozice;
                this.Falling = Falling;
                this.Smer = Smer;
                this.detonate = detonate;
                TicksToDetonation = 5 * 1000 / Konstanty.Rychlosthry; //5 sekund
            }

            public Walker(Lemming Lemming)
            {
                Typ = 0;
                Pozice = Lemming.Pozice;
                Falling = Lemming.Falling;
                Smer = Lemming.Smer;
                detonate = Lemming.detonate;
                TicksToDetonation = Lemming.TicksToDetonation;
            }
        }

        class Floater : Lemming
        {
            protected override int Fall(Bitmap Popredi)
            {
                if (Falling == 2)
                {
                    Pozice.Y++;
                    Falling--;
                }
                else
                {
                    Falling++;
                }
                return 0;
            }
            

            public override int Move(Bitmap Popredi)
            {
                if (Popredi.GetPixel(Pozice.X, Pozice.Y + 1).A == 0)
                {
                    return Fall(Popredi);
                }
                else
                {
                    if (Falling > 0)
                    {
                        return 3;
                    }
                    return Sideways(Popredi);
                }
            }

            public Floater(Point Pozice, int Falling, int Smer, bool detonate)
            {
                Typ = 1;
                this.Pozice = Pozice;
                this.Falling = Falling;
                this.Smer = Smer;
                this.detonate = detonate;
                TicksToDetonation = 5 * 1000 / Konstanty.Rychlosthry; //5 sekund
            }

            public Floater(Lemming Lemming)
            {
                Typ = 1;
                Pozice = Lemming.Pozice;
                Falling = 1;
                Smer = Lemming.Smer;
                detonate = Lemming.detonate;
                TicksToDetonation = Lemming.TicksToDetonation;
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
            ForDrawing navrat = new ForDrawing();
            ForDrawing Aktualni = navrat;


            //Move
            for (int i = 0; i < Lemmingove.Length; i++)
            {
                if (Lemmingove[i] != null)
                    switch (Lemmingove[i].Move(Popredi))
                    {
                        case 0: //Zije
                            Aktualni.Dalsi = new ForDrawing(Lemmingove[i].Pozice, Lemmingove[i].Typ, Lemmingove[i].Smer, false, false);
                            Aktualni = Aktualni.Dalsi;
                            break;


                        case 1: //Spadnul
                            Aktualni.Dalsi = new ForDrawing(Lemmingove[i].Pozice, Lemmingove[i].Typ, Lemmingove[i].Smer, true, false);
                            Aktualni = Aktualni.Dalsi;
                            Lemmingove[i] = null;//DEATH
                            AktualniPocetZivichLemmingu--;
                            break;

                        case 2: //Detonate
                            break;

                        case 3:  //Zmena zpet na Walkera
                            
                            Lemmingove[i].Typ = 0;
                            Lemmingove[i] = new Walker(Lemmingove[i]);
                            Aktualni.Dalsi = new ForDrawing(Lemmingove[i].Pozice, Lemmingove[i].Typ, Lemmingove[i].Smer, true, false);
                            Aktualni = Aktualni.Dalsi;
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
                    Aktualni.Dalsi = new ForDrawing(TempLemming.Pozice, TempLemming.Typ, TempLemming.Smer, false, false);
                    Aktualni = Aktualni.Dalsi;
                }
            }


            return navrat;
        }


        public void LemmingsClick(Point kde)
        {
            int[] VKurzoru = new int[AktualniPocetZivichLemmingu];
            int TempInt = 0;
            int KliknutyLemming = -1; //index v poli lemingu
            Rectangle Kurzor = new Rectangle(kde, new Size(Konstanty.velikostKurzoru, Konstanty.velikostKurzoru));


            //Najdi lemmingy v kurzoru
            for (int i = 0; i < Lemmingove.Length; i++)
            {
                if (Lemmingove[i] != null)
                {
                    if (Kurzor.Contains(Lemmingove[i].Pozice))
                    {
                        VKurzoru[TempInt] = i;
                        TempInt++;
                    }
                }
            }

            //Najde lemminga nejblizsiho stredu kurzoru
            double MinVzdalenost = 2 * Konstanty.velikostKurzoru;
            double vzdalenost;

            for (int i = 0; i < TempInt; i++)
            {
                vzdalenost = Math.Sqrt(Math.Pow(Lemmingove[VKurzoru[i]].Pozice.X - kde.X, 2) + Math.Pow(Lemmingove[VKurzoru[i]].Pozice.Y - kde.Y, 2));
                if (vzdalenost < MinVzdalenost)
                {
                    KliknutyLemming = VKurzoru[i];
                    MinVzdalenost = vzdalenost;
                }
            }

            if (KliknutyLemming >= 0)
                switch (Selected)
                {
                    case 1:
                        break;
                    case 2://FLOATER
                        Lemmingove[KliknutyLemming] = new Floater(Lemmingove[KliknutyLemming]) ;
                        Lemmingove[KliknutyLemming].Typ = 1;
                        break;
                }
        }



        public void Select(int co)
        {
            Selected = co;
        }


        public Logika(Bitmap Popredi) //Bude vetsinu nacitat ze souboru pro mapu
        {
            this.Popredi = Popredi;
            Lemmingove = new Lemming[20];
            Selected = 0;
            AktualniPocetZivichLemmingu = 0;
            PocetSpawnutych = 0;

            //FORTESTING
            Spawny = new Spawn[2];
            Spawny[0] = new Spawn(50, new Point(100, 100));
            Spawny[1] = new Spawn(25, new Point(200, 100));
        }




    }
}

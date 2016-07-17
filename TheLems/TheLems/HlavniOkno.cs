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
        int OKolik = 5; //Pro eventy s klavesnici , posun
        Bitmap ToPictureBoxGame, Popredi, Pozadi, TlacitkaUp;
        Bitmap[] ObrazkyLemmu;
        Logika Hra;
        Graphics GrafikaGame, GrafikaButtons;
        Point PoziceMysiObrazovka;
        Rectangle ZobrazenaCast; //Popisuje cast vyriznutou z obrazku cele mapy a zobrazenou na obrazovce
        DateTime Cas; //FORTESTING
        TimeSpan UbehlyCas; //FORTESTING

        public HlavniOkno()
        {
            InitializeComponent();

            

            ToPictureBoxGame = new Bitmap(1280, 648);
            GrafikaGame = Graphics.FromImage(ToPictureBoxGame);
            PictureBoxGame.Image = ToPictureBoxGame;
            PictureBoxGame.Height = ToPictureBoxGame.Height;
            PictureBoxGame.Width = ToPictureBoxGame.Width;

            PictureBoxButtons.Left = 0;
            PictureBoxButtons.Top = 648;

            PictureBoxButtons.Height = 120;
            PictureBoxButtons.Width = 840;


            this.ClientSize = new Size(PictureBoxGame.Width, PictureBoxGame.Height + PictureBoxButtons.Height);
            


            Stav = State.Menu;

            SwitchToGame("1");

            //NASTAVENI RYCHLOSTI
            Timer.Interval = Konstanty.Rychlosthry;
        }


        //AUTOMATICKE METODY
        

        private void Timer_Tick(object sender, EventArgs e)
        {
            Cas = DateTime.Now; //FORTESTING

            ForDrawing PoziceATypy = Hra.Tick();
            GameDraw(PoziceATypy);
        }

        private void PictureBoxGame_MouseMove(object sender, MouseEventArgs e)
        {
            PoziceMysiObrazovka = e.Location;

        }

        private void PictureBoxGame_MouseUp(object sender, MouseEventArgs e)
        { 
            e.Location.Offset(ZobrazenaCast.X, ZobrazenaCast.Y);
            Hra.LemmingsClick(e.Location);
            ButtonDraw();
        }

        private void HlavniOkno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
                OKolik = 1;
            else
                OKolik = 5;

            switch (e.KeyCode)
            {
                case Keys.D0:
                    Hra.Selected = 0;
                    break;
                case Keys.D1:
                    Hra.Selected = 1;
                    break;
                case Keys.D2:
                    Hra.Selected = 2;
                    break;
                case Keys.D3:
                    Hra.Selected = 3;
                    break;
                case Keys.D4:
                    Hra.Selected = 4;
                    break;
                case Keys.D5:
                    Hra.Selected = 5;
                    break;
                case Keys.D6:
                    Hra.Selected = 6;
                    break;
                case Keys.D7:
                    Hra.Selected = 7;
                    break;
                case Keys.D8:
                    Hra.Selected = 8;
                    break;
                case Keys.D9:
                    Hra.Selected = 9;
                    break;
            }


        }

        private void HlavniOkno_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift)
                OKolik = 5;
            else
                OKolik = 1;
        }

        private void HlavniOkno_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w':
                    MoveZobrazenyRect(0, -OKolik);
                    break;
                case 'W':
                    MoveZobrazenyRect(0, -OKolik);
                    break;
                case 's':
                    MoveZobrazenyRect(0, OKolik);
                    break;
                case 'S':
                    MoveZobrazenyRect(0, OKolik);
                    break;
                case 'a':
                    MoveZobrazenyRect(-OKolik, 0);
                    break;
                case 'A':
                    MoveZobrazenyRect(-OKolik, 0);
                    break;
                case 'd':
                    MoveZobrazenyRect(OKolik, 0);
                    break;
                case 'D':
                    MoveZobrazenyRect(OKolik, 0);
                    break;
            }
        }

        private void PictureBoxButtons_MouseUp(object sender, MouseEventArgs e)
        {
            int KliknutyPole = 0;

            if (e.Button == MouseButtons.Left)
            {
                for (int i = 1; i <= 12; i++)
                {
                    if (e.X < (70 * i))
                    {
                        KliknutyPole = i;
                        break;
                    }
                }

                switch (KliknutyPole)
                {
                    case 0:
                        throw new Exception();
                    case 1:
                        Hra.ZmenaRychostiSpawnu(-1);
                        break;
                    case 2:
                        Hra.ZmenaRychostiSpawnu(1);
                        break;
                    default:
                        Hra.Selected = KliknutyPole - 3;
                        break;
                }

                ButtonDraw();
            }
        }

        private void PictureBoxButtons_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int KliknutyPole = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (e.X < (70 * i))
                {
                    KliknutyPole = i;
                    break;
                }
            }

            switch (KliknutyPole)
            {
                case 1:
                    Hra.ZmenaRychostiSpawnu(-100);
                    break;
                case 2:
                    Hra.ZmenaRychostiSpawnu(100);
                    break;
                case 12:
                    Hra.KaBOOOM();
                    break;
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
                    PictureBoxGame.Show();
                    PictureBoxButtons.Show();
                    

                    Popredi = new Bitmap(System.IO.Path.Combine(cesta + "_popredi.png"));
                    Pozadi = new Bitmap(System.IO.Path.Combine(cesta + "_pozadi.png"));
                    TlacitkaUp = new Bitmap(@"Animations\TlacitkaUp.png");

                    PictureBoxButtons.Image = TlacitkaUp;
                    GrafikaButtons = Graphics.FromImage(TlacitkaUp);

                    Bitmap TempBMP;
                    TempBMP = new Bitmap(@"Animations\TriZidiTest.png");
                    ObrazkyLemmu = new Bitmap[TempBMP.Height / Konstanty.velikostLemaY];
                    for (int i = 0; i < ObrazkyLemmu.Length; i++)
                    {
                        ObrazkyLemmu[i] = TempBMP.Clone(new Rectangle(0, i * Konstanty.velikostLemaY, 
                            Konstanty.velikostLemaX, Konstanty.velikostLemaY), System.Drawing.Imaging.PixelFormat.DontCare);
                    }

                    ZobrazenaCast = new Rectangle(0, 0, 1280, 500); //FORTESTING

                    Hra = new Logika(Popredi); //Bude vetsinu nacitat ze souboru pro danou mapu
                    Hra.Selected = 1;
                    ButtonDraw();

                    Timer.Start();
                    break;

                case State.Pauza:
                    break;

            }
        }

        

        private void GameDraw(ForDrawing DrawInfo)
        {
            

            CheckMousePosition(); // nacteni lemmingu v kurzoru

            GrafikaGame.Clear(Color.Black);
            GrafikaGame.DrawImage(Pozadi, 0, 0, ZobrazenaCast, GraphicsUnit.Pixel);
            GrafikaGame.DrawImage(Popredi, 0, 0, ZobrazenaCast, GraphicsUnit.Pixel);
            GrafikaGame.DrawRectangle(Pens.Chocolate, new Rectangle(PoziceMysiObrazovka.X - 5, PoziceMysiObrazovka.Y - 5, 10, 10)); //FORTESTING

            int PoziceLemmaObrazovkaX, PoziceLemmaObrazovkaY;

            while (DrawInfo != null) //Projet spojak
            {
                //Pozice leveho horniho rohu lemma
                PoziceLemmaObrazovkaX = DrawInfo.Souradnice.X - ZobrazenaCast.X - (Konstanty.velikostLemaX / 2);
                //Vyradit lemy mimo zobrazenou plochu
                if ((PoziceLemmaObrazovkaX > 0) && (PoziceLemmaObrazovkaX < ZobrazenaCast.Width)) 
                {
                    //Pozice leveho horniho rohu lemma
                    PoziceLemmaObrazovkaY = DrawInfo.Souradnice.Y - ZobrazenaCast.Y - Konstanty.velikostLemaY;
                    if ((PoziceLemmaObrazovkaY > 0) && (PoziceLemmaObrazovkaY < ZobrazenaCast.Height))
                    {
                        if (DrawInfo.Typ >= 0)
                        {
                            //Nakreslit Lemma
                            GrafikaGame.DrawImage(ObrazkyLemmu[DrawInfo.Typ], PoziceLemmaObrazovkaX, PoziceLemmaObrazovkaY);

                            //Pripadne nakreslit cas do detonace
                            if (DrawInfo.TicksToDetonation > 0)
                                GrafikaGame.DrawString(
                                    Math.Ceiling(Convert.ToDouble(DrawInfo.TicksToDetonation * Konstanty.Rychlosthry) / 1000).ToString(),
                                    new Font("Verdana", 10), Brushes.White,
                                    PoziceLemmaObrazovkaX, PoziceLemmaObrazovkaY - 20);
                        }

                            
                    }
                }
                

                DrawInfo = DrawInfo.Dalsi;
            }
            GrafikaGame.DrawString(UbehlyCas.Milliseconds.ToString(), new Font("Verdana", 10), Brushes.White, 50, 50);//FORTESTING
            GrafikaGame.DrawString(PoziceMysiObrazovka.X.ToString(), new Font("Verdana", 10), Brushes.White, 200, 50);//FORTESTING
            PictureBoxGame.Refresh();
            UbehlyCas = DateTime.Now.Subtract(Cas); //FORTESTING
        }


        private void ButtonDraw()
        {
            string AktString;
            Font FontProKresleni = new Font("Verdana", 10, FontStyle.Bold);

            for (int i = 0; i < 10; i++) //Prvnich 10 s pocitadlem
            {
                AktString = Hra.GetMinAktRychlostSpawnu_ZbyvajiciItemy(i).ToString();
                GrafikaButtons.FillRectangle(Brushes.Black,new Rectangle(10 + 70*i,30,49,29));
                GrafikaButtons.DrawString(AktString,FontProKresleni , Brushes.White, 30 + 70 * i, 35);
            }    
            PictureBoxButtons.Refresh();
        }

        private void CheckMousePosition() //Get Lemmingy v kurzoru
        {
            
        }

        private void MoveZobrazenyRect(int x, int y)
        {
            if ((ZobrazenaCast.Right + x <= Pozadi.Width) && (ZobrazenaCast.Left + x >= 0))
                ZobrazenaCast.X += x;

            if ((ZobrazenaCast.Bottom + y <= Pozadi.Height) && (ZobrazenaCast.Top + y >= 0))
                ZobrazenaCast.Y += y;
        }
    }

    class ForDrawing //Pro predavani informaci, kam nakreslit lemingy
    {
        public Point Souradnice;
        public int Typ;
        public int Smer;
        public bool Death;
        public int TicksToDetonation;
        public ForDrawing Dalsi;

        public ForDrawing(Point Souradnice, int Typ, int Smer, bool Death, int TicksToDetonation)
        {
            this.Souradnice = Souradnice;
            this.Typ = Typ;
            this.Smer = Smer;
            this.Death = Death;
            this.TicksToDetonation = TicksToDetonation;
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
            int MaxRychlostSpawnu;
            int DobaMeziSpawny;
            int TickOdPredchozihoSpawnu;
            public Point Souradnice;

            public void ZmenaRychlosti(int NaCo)
            {
                DobaMeziSpawny = Convert.ToInt32(Math.Round(MaxRychlostSpawnu * (100 / Convert.ToDouble(NaCo))));   
            }

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

            public Spawn(int MaxRychlostSpawnu, Point Souradnice)
            {
                this.MaxRychlostSpawnu = MaxRychlostSpawnu;
                this.DobaMeziSpawny = MaxRychlostSpawnu;
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
            protected int _TicksToDetonation;
            public int TicksToDetonation { get { return _TicksToDetonation; } protected set { _TicksToDetonation = value; } }

            public int Tick(Bitmap Popredi)
            {
                if(detonate)
                {
                    if (TicksToDetonation-- == 0)
                    {
                        return 2;
                    }
                }

                return Move(Popredi);

            }
            //hodnoty 0 - zije, 1 - spadnul, 2 - detonate, 3 - vycerpal special vec, 4 - otocil se
            protected virtual int Move(Bitmap Popredi)
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
                    for (int i = Konstanty.velikostLemaY; i >= -Konstanty.velikostLemaY / 2; i--) 
                    {
                        if (Popredi.GetPixel(Pozice.X + Smer, Pozice.Y - i).A != 0)
                        {
                            Posun = i + 1;
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
                        return 4;
                    }
                }

                if (Popredi.GetPixel(Pozice.X, Pozice.Y + 1).A == 0)
                {
                    Falling = 1;
                }
                return 0;
            }

            public bool Detonate(int ZaJakDlouho)//Pro global detonate
            {
                if (!detonate)
                {
                    detonate = true;
                    TicksToDetonation = ZaJakDlouho;
                    return true;
                }
                else
                    return false;
            }

            public bool Detonate() //Vraci true pokud se povedlo, tedy jeste nemel vybuchnout
            {
                if (!detonate)
                {
                    detonate = true;
                    TicksToDetonation = 5 * 1000 / Konstanty.Rychlosthry; //chci aby to bylo 5 sekund
                    return true;
                }
                else
                    return false;
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
                TicksToDetonation = -1;
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

        class Climber : Lemming
        {

            private int MoveUp(Bitmap Popredi)
            {
                if (Popredi.GetPixel(Pozice.X, Pozice.Y - Konstanty.velikostLemaY/2 - 1).A != 0) //- protoze nahoru
                {
                    return 3;
                }


                for (int i = Konstanty.velikostLemaY / 2; i >= 0 ; i--)
                {
                    if (Popredi.GetPixel(Pozice.X + Smer, Pozice.Y - i).A == 0)
                    {
                        Pozice.X += Smer;
                        Pozice.Y -= i;
                        return 3;
                    }
                }

                Pozice.Y--;
                return 0;

            }

            protected override int Move(Bitmap Popredi)
            {
                if (Typ == 0)
                {
                    int Navrat = base.Move(Popredi);
                    if (Navrat == 4)
                    {
                        Typ = 1;
                        Smer *= -1;
                    }

                    return Navrat;
                }
                else
                {
                    return MoveUp(Popredi);
                }
            }

            public Climber(Point Pozice, int Falling, int Smer, bool detonate)
            {
                Typ = 1;
                this.Pozice = Pozice;
                this.Falling = Falling;
                this.Smer = Smer;
                this.detonate = detonate;
                TicksToDetonation = -1;
            }

            public Climber(Lemming Lemming)
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
            

            protected override int Move(Bitmap Popredi)
            {
                if (Popredi.GetPixel(Pozice.X, Pozice.Y + 1).A == 0)
                {
                    Typ = 2;
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
                Typ = 2;
                this.Pozice = Pozice;
                this.Falling = Falling;
                this.Smer = Smer;
                this.detonate = detonate;
                TicksToDetonation = -1;
            }

            public Floater(Lemming Lemming)
            {
                Typ = 0;
                Pozice = Lemming.Pozice;
                Falling = 0; //Tomuhle je to vlastne jedno
                Smer = Lemming.Smer;
                detonate = Lemming.detonate;
                TicksToDetonation = Lemming.TicksToDetonation;
            }
        }

        
        Lemming[] Lemmingove;
        Spawn[] Spawny;
        Bitmap Popredi;
        int[] ZbyvajiciItemy;
        public int Selected; //Zvoleny button
        int AktualniPocetZivichLemmingu;
        int PocetSpawnutych;
        int AktRychlostSpawnu, MaxRychlostSpawnu, MinRychlostSpawnu;
        int BOOOOM,BOOOOMTimer; //Pro GlobalBOOM

        public ForDrawing Tick()
        {
            ForDrawing navrat = new ForDrawing();
            ForDrawing Aktualni = navrat;

            //TODO Global BOOM 
            int Cyklus = 0;
            if (BOOOOM >= 0 && BOOOOM < AktualniPocetZivichLemmingu)
            {
                

            }

            
            
            //Move
            Cyklus = 0; //v podstate vlastni for cyklus s promenlivym koncem a vice cyklech na jednom i
            while (Cyklus < AktualniPocetZivichLemmingu)
            { 
                switch (Lemmingove[Cyklus].Tick(Popredi))
                {
                    case 0: //Zije
                        Aktualni.Dalsi = new ForDrawing(Lemmingove[Cyklus].Pozice, 
                            Lemmingove[Cyklus].Typ, Lemmingove[Cyklus].Smer, false, Lemmingove[Cyklus].TicksToDetonation);
                        Aktualni = Aktualni.Dalsi;
                        break;


                    case 1: //Spadnul
                        Aktualni.Dalsi = new ForDrawing(Lemmingove[Cyklus].Pozice, Lemmingove[Cyklus].Typ,
                            Lemmingove[Cyklus].Smer, true, Lemmingove[Cyklus].TicksToDetonation);
                        Aktualni = Aktualni.Dalsi;
                        Lemmingove[Cyklus] = Lemmingove[--AktualniPocetZivichLemmingu];//DEATH
                        Lemmingove[AktualniPocetZivichLemmingu] = null;
                        continue;//znovu projde to same misto v poli, protoze sem tam presnunul noveho

                    case 2: //Detonate
                        Aktualni.Dalsi = new ForDrawing(Lemmingove[Cyklus].Pozice, Lemmingove[Cyklus].Typ,
                            Lemmingove[Cyklus].Smer, true, Lemmingove[Cyklus].TicksToDetonation);
                        Aktualni = Aktualni.Dalsi;
                        Lemmingove[Cyklus] = Lemmingove[--AktualniPocetZivichLemmingu];//DEATH
                        Lemmingove[AktualniPocetZivichLemmingu] = null;
                        break;

                    case 3:  //Zmena zpet na Walkera
                            
                        Lemmingove[Cyklus].Typ = 0;
                        Lemmingove[Cyklus] = new Walker(Lemmingove[Cyklus]);
                        Aktualni.Dalsi = new ForDrawing(Lemmingove[Cyklus].Pozice, 
                            Lemmingove[Cyklus].Typ, Lemmingove[Cyklus].Smer, true, Lemmingove[Cyklus].TicksToDetonation);
                        Aktualni = Aktualni.Dalsi;
                        break;

                    case 4: //Otocil se, signal pro climbera, tady nic nemeni oproti walkerovi
                        Aktualni.Dalsi = new ForDrawing(Lemmingove[Cyklus].Pozice, 
                            Lemmingove[Cyklus].Typ, Lemmingove[Cyklus].Smer, false, Lemmingove[Cyklus].TicksToDetonation);
                        Aktualni = Aktualni.Dalsi;
                        break;

                }
                Cyklus++;
            }


            //Spawn
            Lemming TempLemming;
            //projde vsechny spawnpojnty, pokud uz nejsou vsichni naspawnovani
            for (int i = 0; (PocetSpawnutych < Lemmingove.Length) && (i < Spawny.Length); i++) 
            {
                TempLemming = Spawny[i].Tick();
                if (TempLemming != null) //Lemming se spawnul
                {
                    Lemmingove[AktualniPocetZivichLemmingu] = TempLemming;
                    PocetSpawnutych++;
                    AktualniPocetZivichLemmingu++;
                    Aktualni.Dalsi = new ForDrawing(TempLemming.Pozice, TempLemming.Typ, TempLemming.Smer, false, Lemmingove[Cyklus].TicksToDetonation);
                    Aktualni = Aktualni.Dalsi;
                }
            }

            
            return navrat;
        }

        private int[] LemmingoveVKurzoru(Point StredKurzoru)
        {
            int[] VKurzoru = new int[AktualniPocetZivichLemmingu + 1];
            Point LevyHorniRoh = StredKurzoru;
            LevyHorniRoh.Offset(-Konstanty.velikostKurzoru / 2, -Konstanty.velikostKurzoru / 2); //Vycentruje kurzor okolo mysi
            Rectangle Kurzor = new Rectangle(LevyHorniRoh, new Size(Konstanty.velikostKurzoru, Konstanty.velikostKurzoru));
            Point StredLema;

            int TempInt = 0;
            //Najdi lemmingy v kurzoru
            for (int i = 0; i < AktualniPocetZivichLemmingu; i++)
            {
                StredLema = Lemmingove[i].Pozice;
                StredLema.Offset(0, -Konstanty.velikostLemaY / 2);
                if (Kurzor.Contains(StredLema))
                {
                    VKurzoru[TempInt] = i;
                    TempInt++;
                }  
            }

            VKurzoru[TempInt] = -1; //Zarazka

            //Presunuti minimalni vzdalenosti na pozici 0
            //Najde lemminga nejblizsiho stredu kurzoru
            double MinVzdalenost = Konstanty.velikostKurzoru*2;
            double Vzdalenost;
            int MinIndex = 0;
            int j = 0;
            while (VKurzoru[j] != -1)
            {
                Vzdalenost = Math.Sqrt((Lemmingove[VKurzoru[j]].Pozice.X - StredKurzoru.X) * (Lemmingove[VKurzoru[j]].Pozice.X - StredKurzoru.X) + (Lemmingove[VKurzoru[j]].Pozice.Y - StredKurzoru.Y) * (Lemmingove[VKurzoru[j]].Pozice.Y - StredKurzoru.Y));
                if (Vzdalenost < MinVzdalenost)
                {
                    MinVzdalenost = Vzdalenost;
                    MinIndex = j;
                }
                j++;
            }

            TempInt = VKurzoru[0]; //Recyklace TempIntu, kterej uz stejne nic nenesl
            VKurzoru[0] = VKurzoru[MinIndex];
            VKurzoru[MinIndex] = TempInt;
            

            return VKurzoru;
        }

        public void LemmingsClick(Point PoziceMysivMape)
        {
            int KliknutyLemming = -1; //index v poli lemingu
            int[] VKurzoru = LemmingoveVKurzoru(PoziceMysivMape);

            if (VKurzoru[0] != -1)
            {
                KliknutyLemming = VKurzoru[0];
            }

            if (KliknutyLemming >= 0)
                if (ZbyvajiciItemy[Selected] > 0)
                {
                    switch (Selected)
                    {
                        case 0://Climber
                            if (!(Lemmingove[KliknutyLemming] is Climber)) //Pro zamezeni vice kiku na jednoho
                            {
                                Lemmingove[KliknutyLemming] = new Climber(Lemmingove[KliknutyLemming]);
                                ZbyvajiciItemy[Selected]--;
                            }
                            break;
                        case 1://FLOATER
                            if (!(Lemmingove[KliknutyLemming] is Floater)) //Pro zamezeni vice kiku na jednoho
                            {
                                Lemmingove[KliknutyLemming] = new Floater(Lemmingove[KliknutyLemming]);
                                ZbyvajiciItemy[Selected]--;
                            }
                            break;
                        case 2://DETONATE
                            if (Lemmingove[KliknutyLemming].Detonate()) //Pro zamezeni vice kiku na jednoho
                            {
                                ZbyvajiciItemy[Selected]--;
                            }
                            break;
                    }
                }
        }

        public int GetMinAktRychlostSpawnu_ZbyvajiciItemy(int Ktery)
        {
            switch (Ktery - 2)
            {
                case -2:
                    return MinRychlostSpawnu;
                case -1:
                    return AktRychlostSpawnu;
                default:
                    return ZbyvajiciItemy[Ktery - 2];
            }
        }

        public void KaBOOOM()
        {
            BOOOOM = 0;
        }

        public void ZmenaRychostiSpawnu(int OKolik)
        {
            AktRychlostSpawnu += OKolik;
            if (AktRychlostSpawnu < MinRychlostSpawnu)
                AktRychlostSpawnu = MinRychlostSpawnu;
            if (AktRychlostSpawnu > MaxRychlostSpawnu)
                AktRychlostSpawnu = MaxRychlostSpawnu;
            

            for (int i = 0; i < Spawny.Length; i++)
            {
                Spawny[i].ZmenaRychlosti(AktRychlostSpawnu);
            }
        }

        public Logika(Bitmap Popredi) //Bude vetsinu nacitat ze souboru pro mapu
        {
            this.Popredi = Popredi;
            Lemmingove = new Lemming[20];
            Selected = 0;
            AktualniPocetZivichLemmingu = 0;
            PocetSpawnutych = 0;
            MaxRychlostSpawnu = 100;
            MinRychlostSpawnu = 20;
            AktRychlostSpawnu = 100;
            ZbyvajiciItemy = new int[8];
            BOOOOM = -1;
            BOOOOMTimer = 5 * 1000 / Konstanty.Rychlosthry; //chci aby to bylo 5 sekund

            //FORTESTING
            for (int i = 0; i < ZbyvajiciItemy.Length; i++)
            {
                ZbyvajiciItemy[i] = 50;
            }

            Spawny = new Spawn[1];
            Spawny[0] = new Spawn(30, new Point(500, 300));
            //Spawny[1] = new Spawn(25, new Point(50, 100));
        }




    }
}

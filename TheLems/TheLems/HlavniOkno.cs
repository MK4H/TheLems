﻿using System;
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
        Bitmap ToPictureBoxGame, ToPictureBoxButtons, ToPictureBoxMap, Game, Popredi, Pozadi, PozadiMini,TlacitkaUp, TlacitkaDown;
        Bitmap[] ObrazkyLemmu;
        Logika Hra;
        Graphics GrafikaGameDisplay, GrafikaGameLandscape, GrafikaButtons, GrafikaMap, GrafikaGame;
        Point PoziceMysiObrazovka;
        Rectangle ZobrazenaCast; //Popisuje cast vyriznutou z obrazku cele mapy a zobrazenou na obrazovce
        DateTime Cas; //FORTESTING
        TimeSpan UbehlyCas; //FORTESTING

        public HlavniOkno()
        {
            InitializeComponent();

            InitializeState();
        }


        //AUTOMATICKE METODY


        private void Timer_Tick(object sender, EventArgs e)
        {
            Cas = DateTime.Now; //FORTESTING

            DrawInfoTransfer DrawInfo = Hra.Tick();
            ImpactDraw(DrawInfo);
            GameDraw(DrawInfo);
            MapDraw(DrawInfo);
        }

        private void PictureBoxGame_MouseMove(object sender, MouseEventArgs e)
        {
            PoziceMysiObrazovka = e.Location;

        }

        private void PictureBoxGame_MouseUp(object sender, MouseEventArgs e)
        {
            Point Klik = e.Location;
            Klik.Offset(ZobrazenaCast.X, ZobrazenaCast.Y);
            Hra.LemmingsClick(Klik);
            ButtonNumbersDraw();
        }

        private void HlavniOkno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
                OKolik = 1;
            else
                OKolik = 5;

            switch (e.KeyCode)
            {
                case Keys.D1:
                    Hra.Selected = 0;
                    ButtonClickDraw(3);
                    break;
                case Keys.D2:
                    Hra.Selected = 1;
                    ButtonClickDraw(4);
                    break;
                case Keys.D3:
                    Hra.Selected = 2;
                    ButtonClickDraw(5);
                    break;
                case Keys.D4:
                    Hra.Selected = 3;
                    ButtonClickDraw(6);
                    break;
                case Keys.D5:
                    Hra.Selected = 4;
                    ButtonClickDraw(7);
                    break;
                case Keys.D6:
                    Hra.Selected = 5;
                    ButtonClickDraw(8);
                    break;
                case Keys.D7:
                    Hra.Selected = 6;
                    ButtonClickDraw(9);
                    break;
                case Keys.D8:
                    Hra.Selected = 7;
                    ButtonClickDraw(8);
                    break;
                case Keys.Escape:
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
                        ButtonNumbersDraw();
                        break;
                    case 2:
                        Hra.ZmenaRychostiSpawnu(1);
                        ButtonNumbersDraw();
                        break;
                    case 11:
                        break;
                    case 12:
                        break;
                    default:
                        Hra.Selected = KliknutyPole - 3;
                        ButtonClickDraw(KliknutyPole);
                        break;
                }

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

        private void InitializeState()//dalo by se udelat ve form designeru, ale tady se to lepe meni a je to vse pohromade
        {
            PictureBoxGame.Left = 0;
            PictureBoxGame.Top = 0;
            PictureBoxGame.Height = 598;
            PictureBoxGame.Width = 1280;

            PictureBoxText.Left = 0;
            PictureBoxText.Top = 598;
            PictureBoxText.Height = 50;
            PictureBoxText.Width = 840;

            PictureBoxButtons.Left = 0;
            PictureBoxButtons.Top = 648;
            PictureBoxButtons.Height = 120;
            PictureBoxButtons.Width = 840;

            PictureBoxMap.Left = 840;
            PictureBoxMap.Top = 598;
            PictureBoxMap.Height = 170;
            PictureBoxMap.Width = 440;

            PictureBoxMenu.Top = 0;
            PictureBoxMenu.Left = 0;
            PictureBoxMenu.Height = 768;
            PictureBoxMenu.Width = 1280;

            this.ClientSize = new Size(PictureBoxMenu.Width, PictureBoxMenu.Height);

            Stav = State.Menu;

            SwitchToGame("2");
            //NASTAVENI RYCHLOSTI
            Timer.Interval = Konstanty.Rychlosthry;
        }

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
                    PictureBoxMenu.Hide();
                    Stav = State.Hra;


                    PictureBoxGame.Show();
                    PictureBoxButtons.Show();
                    PictureBoxText.Show();
                    PictureBoxMap.Show();

                    ToPictureBoxGame = new Bitmap(PictureBoxGame.Width, PictureBoxGame.Height);
                    GrafikaGameDisplay = Graphics.FromImage(ToPictureBoxGame);
                    PictureBoxGame.Image = ToPictureBoxGame;

                    Popredi = new Bitmap(System.IO.Path.Combine(cesta + "_popredi.png"));
                    GrafikaGameLandscape = Graphics.FromImage(Popredi);
                    GrafikaGameLandscape.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

                    Pozadi = new Bitmap(System.IO.Path.Combine(cesta + "_pozadi.png"));
                    TlacitkaUp = new Bitmap(@"Animations\TlacitkaUp.png");
                    TlacitkaDown = new Bitmap(@"Animations\TlacitkaDown.png");
                    ToPictureBoxButtons = new Bitmap(TlacitkaUp.Width, TlacitkaUp.Height);
                    PictureBoxButtons.Image = ToPictureBoxButtons;
                    GrafikaButtons = Graphics.FromImage(ToPictureBoxButtons);

                    Game = new Bitmap(Popredi.Width, Popredi.Height);
                    GrafikaGame = Graphics.FromImage(Game);

                    ToPictureBoxMap = new Bitmap(PictureBoxMap.Width, PictureBoxMap.Height);
                    PictureBoxMap.Image = ToPictureBoxMap;
                    GrafikaMap = Graphics.FromImage(ToPictureBoxMap);
                    PozadiMini = new Bitmap(PictureBoxMap.Width, PictureBoxMap.Height);
                    Graphics.FromImage(PozadiMini).DrawImage(Pozadi, 0, 0, PictureBoxMap.Width - 10, PictureBoxMap.Height - 10);

                    Bitmap TempBMP;
                    TempBMP = new Bitmap(@"Animations\ZidiTest.png");
                    ObrazkyLemmu = new Bitmap[TempBMP.Height / Konstanty.velikostLemaY];
                    for (int i = 0; i < ObrazkyLemmu.Length; i++)
                    {
                        ObrazkyLemmu[i] = TempBMP.Clone(new Rectangle(0, i * Konstanty.velikostLemaY,
                            Konstanty.velikostLemaX, Konstanty.velikostLemaY), System.Drawing.Imaging.PixelFormat.DontCare);
                    }

                    ZobrazenaCast = new Rectangle(0, 0, ToPictureBoxGame.Width, ToPictureBoxGame.Height);

                    this.ClientSize = new Size(PictureBoxGame.Width, PictureBoxGame.Height + PictureBoxText.Height + PictureBoxButtons.Height);

                    Hra = new Logika(Popredi); //Bude vetsinu nacitat ze souboru pro danou mapu
                    Hra.Selected = 1;
                    ButtonClickDraw(3);

                    Timer.Start();
                    break;

                case State.Pauza:
                    break;

            }
        }

        private void ImpactDraw(DrawInfoTransfer DrawInfo) //Vykresli popredi, protoze to potom bere Logika
        {
            //Vykresli nove schody do popredi
            while (DrawInfo.Stairs != null)
            {

                if (DrawInfo.Stairs.Pozice != Point.Empty)
                {
                    if (DrawInfo.Stairs.Smer == 1)
                        DrawInfo.Stairs.Pozice.Offset(1, -1);
                    else
                        DrawInfo.Stairs.Pozice.Offset(-10, -1);

                    GrafikaGameLandscape.FillRectangle(new SolidBrush(Color.FromArgb(254, Color.Blue)),
                        new Rectangle(DrawInfo.Stairs.Pozice, new Size(9, 2)));
                }
                DrawInfo.Stairs = DrawInfo.Stairs.Dalsi;
            }

            while (DrawInfo.Spaces != null)
            {
                switch (DrawInfo.Spaces.Typ)
                {
                    case 0://Detonate
                        GrafikaGameLandscape.FillEllipse(Brushes.Transparent, 
                            new Rectangle(DrawInfo.Spaces.Pozice.X - Konstanty.velikostLemaX,
                            DrawInfo.Spaces.Pozice.Y - (Konstanty.velikostLemaY / 2)*3,
                            Konstanty.velikostLemaX * 2, Konstanty.velikostLemaY * 2));
                        break;
                    case 1://Sideways
                        if (DrawInfo.Spaces.Smer == 1) //Nastaveni spravne pozice X
                            DrawInfo.Spaces.Pozice.X += 1;
                        else
                            DrawInfo.Spaces.Pozice.X -= 15;

                        GrafikaGameLandscape.FillRectangle(Brushes.Transparent,
                            new Rectangle(DrawInfo.Spaces.Pozice.X,
                            DrawInfo.Spaces.Pozice.Y - Konstanty.velikostLemaY - 1,
                            15, Konstanty.velikostLemaY + 2));
                        break;
                    case 2://Diagonal
                        if (DrawInfo.Spaces.Smer == 1)
                            DrawInfo.Spaces.Pozice.X += 6 - (Konstanty.velikostLemaX / 2); 
                            //aby levej konec byl ve ctrtine leminga
                        else
                            DrawInfo.Spaces.Pozice.X += (- 17 - (Konstanty.velikostLemaX / 2)); 
                        //aby 35 - 17 byla ctvrtina velikosti leminga v X, tedy 18 - potom pravej konec je ve ctvrtine leminga
                        GrafikaGameLandscape.FillEllipse(Brushes.Transparent, 
                        new Rectangle(DrawInfo.Spaces.Pozice.X, DrawInfo.Spaces.Pozice.Y - Konstanty.velikostLemaY + 3,
                        35, 35));
                        break;
                    case 3://Down
                        GrafikaGameLandscape.FillRectangle(Brushes.Transparent,
                            DrawInfo.Spaces.Pozice.X - Konstanty.velikostLemaX / 2,
                            DrawInfo.Spaces.Pozice.Y + 1,
                            Konstanty.velikostLemaX, 2);
                        break;
                }
                DrawInfo.Spaces = DrawInfo.Spaces.Dalsi;
            }
        }

        private void GameDraw(DrawInfoTransfer DrawInfo)
        {
            GrafikaGame.Clear(Color.Black); //Vymazani obrazovky
            GrafikaGame.DrawImage(Pozadi, 0, 0, ZobrazenaCast, GraphicsUnit.Pixel);
            GrafikaGame.DrawImage(Popredi, 0, 0, ZobrazenaCast, GraphicsUnit.Pixel);
            
          
            //Draw Lemmings
            int PoziceLemmaObrazovkaX, PoziceLemmaObrazovkaY;
            DrawLemmings Lemming = DrawInfo.Lemmings;

            while (Lemming != null) //Projet spojak
            {
                //Pozice leveho horniho rohu lemma
                PoziceLemmaObrazovkaX = Lemming.Pozice.X - ZobrazenaCast.X - (Konstanty.velikostLemaX / 2);
                //Vyradit lemy mimo zobrazenou plochu
                if ((PoziceLemmaObrazovkaX > -Konstanty.velikostLemaX) && (PoziceLemmaObrazovkaX < ZobrazenaCast.Width))
                {
                    //Pozice leveho horniho rohu lemma
                    PoziceLemmaObrazovkaY = Lemming.Pozice.Y - ZobrazenaCast.Y - Konstanty.velikostLemaY;
                    if ((PoziceLemmaObrazovkaY > -Konstanty.velikostLemaY) && (PoziceLemmaObrazovkaY < ZobrazenaCast.Height))
                    {
                        if (Lemming.Typ >= 0)
                        {
                            //Nakreslit Lemma
                            GrafikaGameDisplay.DrawImage(ObrazkyLemmu[Lemming.Typ], PoziceLemmaObrazovkaX, PoziceLemmaObrazovkaY);

                            //Pripadne nakreslit cas do detonace
                            if (Lemming.TicksToDetonation > 0)
                                GrafikaGameDisplay.DrawString(
                                    Math.Ceiling(Convert.ToDouble(Lemming.TicksToDetonation * Konstanty.Rychlosthry) / 1000).ToString(),
                                    new Font("Verdana", 10), Brushes.White,
                                    PoziceLemmaObrazovkaX + 6, PoziceLemmaObrazovkaY - 15);
                        }
                    }
                }


                Lemming = Lemming.Dalsi;
            }


            GrafikaGameDisplay.DrawString(UbehlyCas.Milliseconds.ToString(), new Font("Verdana", 10), Brushes.White, 50, 50);//FORTESTING
            GrafikaGameDisplay.DrawString(PoziceMysiObrazovka.X.ToString(), new Font("Verdana", 10), Brushes.White, 200, 50);//FORTESTING
            PictureBoxGame.Refresh();
            UbehlyCas = DateTime.Now.Subtract(Cas); //FORTESTING
        }

        private void MapDraw(DrawInfoTransfer DrawInfo)
        {
            GrafikaMap.Clear(Color.Black);
            GrafikaMap.DrawImage(PozadiMini, 5, 5);
            GrafikaMap.DrawImage(Popredi, 5, 5, PictureBoxMap.Width - 10,PictureBoxMap.Height - 10);
            //GrafikaMap.DrawRectangle(Pens.White,)
            PictureBoxMap.Refresh();
        }

        private void TextDraw()
        {

        }

        private void ButtonNumbersDraw()
        {
            string AktString;
            Font FontProKresleni = new Font("Verdana", 10, FontStyle.Bold);

            for (int i = 0; i < 10; i++) //Prvnich 10 s pocitadlem
            {
                AktString = Hra.GetMinAktRychlostSpawnu_ZbyvajiciItemy(i).ToString();
                GrafikaButtons.FillRectangle(Brushes.Black, new Rectangle(10 + 70 * i, 30, 49, 29));
                GrafikaButtons.DrawString(AktString, FontProKresleni, Brushes.White, 30 + 70 * i, 35);
            }
            PictureBoxButtons.Refresh();
        }

        private void ButtonClickDraw(int Clicked)
        {
            GrafikaButtons.DrawImage(TlacitkaUp, 0, 0);
            GrafikaButtons.DrawImage(TlacitkaDown, (Clicked - 1) * 70, 0, new Rectangle((Clicked - 1) * 70, 0, 70, 120), GraphicsUnit.Pixel);
            ButtonNumbersDraw();
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


    

    class DrawInfoTransfer
    {
        public DrawLemmings Lemmings;
        public DrawSpace Spaces;
        public DrawStairs Stairs;

        public DrawInfoTransfer()
        {
            Lemmings = new DrawLemmings();
            Spaces = new DrawSpace();
            Stairs = new DrawStairs();

        }
    }

    class DrawLemmings //Pro predavani informaci, kam nakreslit lemingy
    {
        public Point Pozice;
        public int Typ;
        public int Smer;
        public bool Death;
        public int TicksToDetonation;
        public DrawLemmings Dalsi;

        public DrawLemmings(Point Pozice, int Typ, int Smer, bool Death, int TicksToDetonation)
        {
            this.Pozice = Pozice;
            this.Typ = Typ;
            this.Smer = Smer;
            this.Death = Death;
            this.TicksToDetonation = TicksToDetonation;
            Dalsi = null;
        }

        public DrawLemmings() //Hlava Spojaku
        {
            Typ = -1; 
            Dalsi = null;
        }
    }

    class DrawSpace
    {
        public int Typ;
        public Point Pozice;
        public int Smer;
        public DrawSpace Dalsi;

        public DrawSpace(int Typ, Point Pozice, int Smer)
        {
            this.Typ = Typ;
            this.Pozice = Pozice;
            this.Smer = Smer;
            Dalsi = null;
        }

        public DrawSpace() //Hlava spojaku
        {
            Typ = -1;
            Dalsi = null;
        }
    }

    class DrawStairs
    {
        public Point Pozice;
        public int Smer;
        public DrawStairs Dalsi;

        public DrawStairs(Point Pozice, int Smer)
        {
            this.Pozice = Pozice;
            this.Smer = Smer;
            Dalsi = null;
        }
        
        public DrawStairs() //Hlava spojaku
        {
            Pozice = Point.Empty;
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

            public int Tick(Bitmap Popredi, Blocker[] Blockerz)
            {
                if(detonate)
                {
                    if (TicksToDetonation-- == 0)
                    {
                        return 2;
                    }
                }

                return Move(Popredi, Blockerz);

            }
            //hodnoty 0 - zije, 1 - spadnul, 2 - detonate, 3 - vycerpal special vec, 4 - otocil se
            protected virtual int Move(Bitmap Popredi, Blocker[] Blockerz)
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
                    return Sideways(Popredi, Blockerz);
                }
            }

            protected virtual int Fall(Bitmap Popredi)
            {
                if (Popredi.GetPixel(Pozice.X,Pozice.Y + 2).A == 0)//TODO mozna pridat for cyklus, at padaj vic a rychlejc
                {
                    Pozice.Y += 2;
                    Falling += 2;
                }
                else
                {
                    Pozice.Y++;
                    Falling++;
                }
                return 10;
            }

            protected virtual int Sideways(Bitmap Popredi, Blocker[] Blockerz)
            {
                if (Smer != 0)
                {
                    int Posun = 0;
                    Posun = (Konstanty.velikostLemaX / 2) * (-Smer);

                    int Cyklus = 0;
                    while (Blockerz[Cyklus] != null)
                    { 
                        if ((Blockerz[Cyklus].Pozice.X + Posun == Pozice.X + Smer) && (Math.Abs(Blockerz[Cyklus].Pozice.Y - Pozice.Y) < 31))
                        { 
                            Smer *= -1;
                            return 5;
                        }
                        Cyklus++;
                    }

                    Posun = 0;
                    for (int i = Konstanty.velikostLemaY; i >= -Konstanty.velikostLemaY / 2; i--) 
                    {
                        if (Popredi.GetPixel(Pozice.X + Smer, Pozice.Y - i).A != 0)
                        {
                            if ((i > 2) && (Popredi.GetPixel(Pozice.X + Smer, Pozice.Y - i).A == 254))
                                continue;
                            else
                            {
                                Posun = i + 1;
                                break;
                            }
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

            public Walker(Lemming Zdroj)
            {
                Typ = 0;
                Pozice = Zdroj.Pozice;
                Falling = Zdroj.Falling;
                Smer = Zdroj.Smer;
                detonate = Zdroj.detonate;
                TicksToDetonation = Zdroj.TicksToDetonation;
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

            protected override int Move(Bitmap Popredi, Blocker[] Blockerz)
            {
                if (Typ == 0)
                {
                    int Navrat = base.Move(Popredi, Blockerz);
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

            public Climber(Lemming Zdroj)
            {
                Typ = 0;
                Pozice = Zdroj.Pozice;
                Falling = Zdroj.Falling;
                Smer = Zdroj.Smer;
                detonate = Zdroj.detonate;
                TicksToDetonation = Zdroj.TicksToDetonation;
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
                return 10;
            }
            

            protected override int Move(Bitmap Popredi, Blocker[] Blockerz)
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
                    return Sideways(Popredi, Blockerz);
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

            public Floater(Lemming Zdroj)
            {
                Typ = 0;
                Pozice = Zdroj.Pozice;
                Falling = 0; //Tomuhle je to vlastne jedno
                Smer = Zdroj.Smer;
                detonate = Zdroj.detonate;
                TicksToDetonation = Zdroj.TicksToDetonation;
            }
        }

        class Blocker : Lemming
        {
            public int MujIndex;

            public void OdstranSe(Blocker[] Blockerz, int AktPocetBlockeru)
            {
                Blockerz[MujIndex] = Blockerz[--AktPocetBlockeru];
                Blockerz[MujIndex].MujIndex = MujIndex;
                Blockerz[AktPocetBlockeru] = null;
            }

            public Blocker(Lemming Zdroj, Blocker[] Blockerz, int AktPocetBlockeru)
            {
                Typ = 3;
                Pozice = Zdroj.Pozice;
                Falling = Zdroj.Falling; 
                Smer = 0;
                detonate = Zdroj.detonate;
                TicksToDetonation = Zdroj.TicksToDetonation;

                Blockerz[AktPocetBlockeru] = this;
                MujIndex = AktPocetBlockeru;
            }
        }

        class Builder : Lemming
        {
            int Zasoba;
            int Waiting;

            protected override int Sideways(Bitmap Popredi, Blocker[] Blockerz)
            {
                if (Waiting > 0)
                {
                    Waiting--;
                    return 0;
                }
                else
                {
                    if (Waiting == 0)
                    {
                        Waiting--;
                        Zasoba--;
                        return 6;
                    }
                    else
                    {
                        int Navrat = base.Sideways(Popredi, Blockerz);
                        switch (Navrat)
                        {
                            case 4://Odrazil se od steny, zmena zpet na walkera
                                return 3;
                            case 5://Odrazil se od blockera, pokracovat opacnym smerem
                                Waiting = 10;
                                return 0;
                            case 0://Tri kroky a pak zas pockat, postavit atd.
                                if (Popredi.GetPixel(Pozice.X, Pozice.Y + 1).A != 254)
                                    return 3;
                                if (Waiting > -3)
                                    Waiting--;
                                else
                                {
                                    Waiting = 10;
                                    if (Zasoba == 0)
                                        return 3;
                                } 
                                return 0;
                            default:
                                throw new Exception();
                        }
                    }
                }
            }

            public Builder(Lemming Zdroj)
            {
                Typ = 4;
                Pozice = Zdroj.Pozice;
                Falling = Zdroj.Falling;
                Smer = Zdroj.Smer;
                detonate = Zdroj.detonate;
                TicksToDetonation = Zdroj.TicksToDetonation;

                Waiting = 10;
                Zasoba = 12; //BALANC HRY
            }
        }
            
        class Basher : Lemming
        {
            int Waiting;

            protected override int Move(Bitmap Popredi, Blocker[] Blockerz)
            {
                int Navrat = base.Move(Popredi, Blockerz);
                if (Navrat == 10)
                    return 3;
                else
                    return Navrat;
            }

            protected override int Sideways(Bitmap Popredi, Blocker[] Blockerz)
            {
                if (Waiting > 0) //delay pro animaci kopani
                {
                    Waiting--;
                    return 0;
                }
                else if (Waiting == 0) //kop
                {
                    Waiting--;
                    return 7;
                }
                else if (Waiting > -16) //Posun pro dalsi kop, pokud neco je v ceste, pak indestructible a odraz
                {
                    if (base.Sideways(Popredi, Blockerz) != 0)
                        return 3;
                    else
                    {
                        Waiting--;
                        return 0;
                    }
                }
                else if (Waiting == -16) //Konec posunu, pred nim by mela byt stena, pokud neni, tak se uz prokopal skrz a konec
                {
                    if (base.Sideways(Popredi, Blockerz) == 0)
                        return 3;
                    else
                    {
                        Smer *= -1;
                        Waiting = 20;
                        return 0;
                    }
                }
                else if (Waiting > -31) //Tolerance x + 16 pixelu
                    // pro inicializaci pri kliku, potom uz by se sem nikdy nemelo dostat
                    // je potreba dojit k stene, dat urcitou toleranci pri kliknuti, ze nemusi bejt uplne u steny
                {
                    if (base.Sideways(Popredi, Blockerz) != 0)
                    {
                        Smer *= -1;
                        Waiting = 20;
                        return 0;
                    }
                    else
                    {
                        Waiting--;
                        return 0;
                    }
                }
                else
                {
                    //konec tolerance, pokud ani tady neni stena, pak nema co kopat a zmena zpet
                    if (base.Sideways(Popredi, Blockerz) == 0)
                        return 3;
                    else
                    {
                        Smer *= -1;
                        Waiting = 20;
                        return 0;
                    }
                }
                
            }

            public Basher(Lemming Zdroj)
            {
                Typ = 5;
                Pozice = Zdroj.Pozice;
                Falling = Zdroj.Falling;
                Smer = Zdroj.Smer;
                detonate = Zdroj.detonate;
                TicksToDetonation = Zdroj.TicksToDetonation;

                Waiting = -20;
            }
        }

        class Miner : Lemming
        {
            int Waiting;

            protected override int Move(Bitmap Popredi, Blocker[] Blockerz)
            {
                int Navrat = base.Move(Popredi, Blockerz);
                if (Navrat == 10 && Waiting != -1)
                    return 3;
                else if (Navrat == 10 && Waiting == -1) //Po kazdym kopnuti, tedy pri waiting == -1, kousek pada
                {
                    if (Falling > 5)
                        return 3;
                    else
                        return 0;
                }
                else
                    return Navrat;
            }

            protected override int Sideways(Bitmap Popredi, Blocker[] Blockerz)
            {
                if (Waiting > 0) //delay pro animaci kopani
                {
                    Waiting--;
                    return 0;
                }
                else if (Waiting == 0) //mine
                {
                    Waiting--;
                    return 8;
                }
                else if (Waiting > -13) //Posun
                {
                    if (base.Sideways(Popredi, Blockerz) != 0)
                        return 3;
                    else
                    {
                        Waiting--;
                        return 0;
                    }
                }
                else //Konec posunu, animace
                {
                    Waiting = 20;
                    return 0;   
                }
                

            }
        

            public Miner(Lemming Zdroj)
            {
                Typ = 6;
                Pozice = Zdroj.Pozice;
                Falling = Zdroj.Falling;
                Smer = Zdroj.Smer;
                detonate = Zdroj.detonate;
                TicksToDetonation = Zdroj.TicksToDetonation;

                Waiting = 20;
            }
        }

        class Digger : Lemming
        {
            int Waiting;

            protected override int Move(Bitmap Popredi, Blocker[] Blockerz)
            {
                int Navrat = base.Move(Popredi, Blockerz);

                if (Navrat == 10 && Falling == 2)
                {
                    Waiting = 10;
                    return 0;
                }
                else if (Navrat == 10 && Falling != 2)
                    return 3;
                else
                    return Navrat;
            }

            protected override int Sideways(Bitmap Popredi, Blocker[] Blockerz)
            {
                if (Waiting > 0)
                {
                    Waiting--;
                    return 0;
                }
                else if (Waiting == 0)
                {
                    Waiting--;
                    return 9;
                }
                else
                    return 3;
            }

            public Digger(Lemming Zdroj)
            {
                Typ = 7;
                Pozice = Zdroj.Pozice;
                Falling = Zdroj.Falling;
                Smer = Zdroj.Smer;
                detonate = Zdroj.detonate;
                TicksToDetonation = Zdroj.TicksToDetonation;

                Waiting = 10;
            }
        }

        Lemming[] Lemmingove;
        Blocker[] Blockerz; 
        Spawn[] Spawny;
        Bitmap Popredi;
        int[] ZbyvajiciItemy;
        public int Selected; //Zvoleny button
        int AktualniPocetZivichLemmingu;
        int PocetSpawnutych;
        int AktRychlostSpawnu, MaxRychlostSpawnu, MinRychlostSpawnu;
        int BOOOOM,BOOOOMTimer; //Pro GlobalBOOM
        int AktPocetBlockeru;

        public DrawInfoTransfer Tick()
        {
            DrawInfoTransfer navrat = new DrawInfoTransfer();
            DrawLemmings AktualniLemming = navrat.Lemmings;
            DrawStairs AktualniSchod = navrat.Stairs;
            DrawSpace AktualniSpace = navrat.Spaces;
            
            //Move
            int Cyklus = 0; //v podstate vlastni for cyklus s promenlivym koncem a vice cyklech na jednom i
            while (Cyklus < AktualniPocetZivichLemmingu)
            {
                AktualniLemming.Dalsi = new DrawLemmings(Lemmingove[Cyklus].Pozice,
                            Lemmingove[Cyklus].Typ, Lemmingove[Cyklus].Smer, false, Lemmingove[Cyklus].TicksToDetonation);

                switch (Lemmingove[Cyklus].Tick(Popredi, Blockerz))
                // 0 - zije, 1 - spadnul, 2 - detonate, 3 - zmena na walkera, 4 - odraz od steny, 5 - odraz od blockera
                // 6 - build stair, 7 - kop horizontalne, 8 - kop diagonalne, 9 - kop dolu, 10 - pada
                {
                   
                    case 1: //Spadnul
                        AktualniLemming.Death = true;
                        if (Lemmingove[Cyklus] is Blocker)
                            (Lemmingove[Cyklus] as Blocker).OdstranSe(Blockerz, AktPocetBlockeru--);

                        Lemmingove[Cyklus] = Lemmingove[--AktualniPocetZivichLemmingu];//DEATH
                        Lemmingove[AktualniPocetZivichLemmingu] = null;
                        continue;//znovu projde to same misto v poli, protoze sem tam presnunul noveho

                    case 2: //Detonate
                        AktualniLemming.Death = true;

                        AktualniSpace.Dalsi = new DrawSpace(0, Lemmingove[Cyklus].Pozice, 0);
                        AktualniSpace = AktualniSpace.Dalsi;

                        if (Lemmingove[Cyklus] is Blocker)
                            (Lemmingove[Cyklus] as Blocker).OdstranSe(Blockerz, AktPocetBlockeru--);

                        Lemmingove[Cyklus] = Lemmingove[--AktualniPocetZivichLemmingu];//DEATH
                        Lemmingove[AktualniPocetZivichLemmingu] = null;
                        continue;//znovu projde to same misto v poli, protoze sem tam presnunul noveho

                    case 3:  //Zmena zpet na Walkera  
                        Lemmingove[Cyklus] = new Walker(Lemmingove[Cyklus]);
                        AktualniLemming.Typ = 0;
                        break;

                    case 6://Build stair
                        AktualniSchod.Dalsi = new DrawStairs(Lemmingove[Cyklus].Pozice, Lemmingove[Cyklus].Smer);
                        AktualniSchod = AktualniSchod.Dalsi;
                        break;
                    case 7://Kop horizontalne
                        AktualniSpace.Dalsi = new DrawSpace(1, Lemmingove[Cyklus].Pozice, Lemmingove[Cyklus].Smer);
                        AktualniSpace = AktualniSpace.Dalsi;
                        break;
                    case 8://Kop diagonalne
                        AktualniSpace.Dalsi = new DrawSpace(2, Lemmingove[Cyklus].Pozice, Lemmingove[Cyklus].Smer);
                        AktualniSpace = AktualniSpace.Dalsi;
                        break;
                    case 9://Kop dolu
                        AktualniSpace.Dalsi = new DrawSpace(3, Lemmingove[Cyklus].Pozice, Lemmingove[Cyklus].Smer);
                        AktualniSpace = AktualniSpace.Dalsi;
                        break;
                }
                AktualniLemming = AktualniLemming.Dalsi;
                Cyklus++;
            }


            //Spawn
            Lemming TempLemming;
            //projde vsechny spawnpojnty, pokud uz nejsou vsichni naspawnovani
            for (int i = 0; (PocetSpawnutych < Lemmingove.Length) && (i < Spawny.Length) && 
                (BOOOOMTimer + BOOOOM > 0); //Aby se nespawnovali po global boomu se zapornym poctem ticku
                i++) 
            {
                TempLemming = Spawny[i].Tick();
                if (TempLemming != null) //Lemming se spawnul
                {
                    Lemmingove[AktualniPocetZivichLemmingu] = TempLemming;
                    PocetSpawnutych++;
                    AktualniPocetZivichLemmingu++;
                    AktualniLemming.Dalsi = new DrawLemmings(TempLemming.Pozice, TempLemming.Typ, TempLemming.Smer, false, Lemmingove[Cyklus].TicksToDetonation);
                    AktualniLemming = AktualniLemming.Dalsi;
                }
            }

            //Global BOOM 
            
            if (BOOOOM >= 0)
            {
                Cyklus = 0;
                while (Cyklus < AktualniPocetZivichLemmingu)
                {
                    if (Lemmingove[Cyklus].Detonate(BOOOOMTimer + BOOOOM))
                    {
                        BOOOOM++;
                        break;
                    }
                    Cyklus++;
                }
                BOOOOMTimer--;
            }


            return navrat;
        }

        public string TypAPocetLemminguVKurzoru(Point StredKurzoru, out int Pocet) //Lze jednoduse predelat, aby spocitalo pocet 
                                                                                    //daneho typu
        {
            int VKurzoru = LemmingoveVKurzoru(StredKurzoru,out Pocet);
            if (VKurzoru != -1)
            {
                if (Lemmingove[VKurzoru] is Walker)
                    return "Walker";
                else if (Lemmingove[VKurzoru] is Climber)
                    return "Climber";
                else if (Lemmingove[VKurzoru] is Floater)
                    return "Floater";
                else if (Lemmingove[VKurzoru] is Blocker)
                    return "Blocker";
                else if (Lemmingove[VKurzoru] is Builder)
                    return "Builder";
                else if (Lemmingove[VKurzoru] is Basher)
                    return "Bahser";
                else if (Lemmingove[VKurzoru] is Miner)
                    return "Miner";
                else if (Lemmingove[VKurzoru] is Digger)
                    return "Digger";
                else
                    return "";
            }
            else
                return "";      
        }

        private int LemmingoveVKurzoru(Point StredKurzoru, out int Pocet) //Vraci indexi lemingu v poli Lemmingove
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

            Pocet = TempInt - 1;
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
            

            return VKurzoru[0];
        }

        public void LemmingsClick(Point PoziceMysivMape)
        {
            int ThrowAway;
            int KliknutyLemming = LemmingoveVKurzoru(PoziceMysivMape, out ThrowAway); ; //index v poli lemingu
            
            //int VKurzoru = 

            /* pokud bych cchtel vracet zase pole
            if (VKurzoru != -1)
            {
                KliknutyLemming = VKurzoru;
            }
            */

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
                        case 3://Blocker
                            if (!(Lemmingove[KliknutyLemming] is Blocker))
                            {
                                Lemmingove[KliknutyLemming] = new Blocker(Lemmingove[KliknutyLemming], Blockerz, AktPocetBlockeru++);
                                ZbyvajiciItemy[Selected]--;
                            }
                            break;
                        case 4://Builder
                            if (!(Lemmingove[KliknutyLemming] is Builder))
                            {
                                Lemmingove[KliknutyLemming] = new Builder(Lemmingove[KliknutyLemming]);
                                ZbyvajiciItemy[Selected]--;
                            }
                            break;
                        case 5://Basher
                            if (!(Lemmingove[KliknutyLemming] is Basher))
                            {
                                Lemmingove[KliknutyLemming] = new Basher(Lemmingove[KliknutyLemming]);
                                ZbyvajiciItemy[Selected]--;
                            }
                            break;
                        case 6://Miner
                            if (!(Lemmingove[KliknutyLemming] is Miner))
                            {
                                Lemmingove[KliknutyLemming] = new Miner(Lemmingove[KliknutyLemming]);
                                ZbyvajiciItemy[Selected]--;
                            }
                            break;
                        case 7://Digger
                            if (!(Lemmingove[KliknutyLemming] is Digger))
                            {
                                Lemmingove[KliknutyLemming] = new Digger(Lemmingove[KliknutyLemming]);
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
            if (BOOOOM < 0)
            {
                BOOOOM = 0;
            }
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
            Lemmingove = new Lemming[100];
            Selected = 0;
            AktualniPocetZivichLemmingu = 0;
            PocetSpawnutych = 0;
            MaxRychlostSpawnu = 100;
            MinRychlostSpawnu = 20;
            AktRychlostSpawnu = 100;
            ZbyvajiciItemy = new int[8];
            BOOOOM = -1;
            BOOOOMTimer = 5 * 1000 / Konstanty.Rychlosthry; //chci aby to bylo 5 sekund
            AktPocetBlockeru = 0;
            
            //FORTESTING
            for (int i = 0; i < ZbyvajiciItemy.Length; i++)
            {
                ZbyvajiciItemy[i] = 50;
            }

            Blockerz = new Blocker[Math.Min(Lemmingove.Length, ZbyvajiciItemy[3])];


            //FORTESTING
            Spawny = new Spawn[2];
            Spawny[0] = new Spawn(30, new Point(400, 600));
            Spawny[1] = new Spawn(25, new Point(200, 600));
        }




    }
}

using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

/// @author Ari Ikonen
/// @version 21.11.2019
///
/// <summary>
/// Breakout-peli, jossa mailaa ohjaamalla tuhotaan pallolla tiiliä.
/// 
/// Puutteita:
///     .tiilille painovoima tms. gravity, jotta pysyvät paikoillaan
///     -laskuri tms. joka laskee kun kaikki tiilet tuhottu -> peli päättyy
///     -kun pallo osuu alalaidan tekstiin -> peli päättyy
/// </summary>

public class Attack_of_8bit : PhysicsGame
{
    Image tausta = LoadImage("background");

    /// <summary>
    /// Kenttä taulukkona
    /// </summary> 

    private static String[] lines = {
                      " x   y   z   y   z   x  " ,
                      "                        ",
                      " z   x   y   z   x   y  ",
                      "                        ",
                      " y   z   x   y   z   x  ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "                        ",
                      "            t           ",
                      };

    private TileMap tiles = TileMap.FromStringArray(lines);

    private static int tileWidth = 1000 / lines[0].Length;
    private static int tileHeight = 800 / lines.Length;

    private Vector nopeusVasemmalle = new Vector(-400, 0);
    private Vector nopeusOikealle = new Vector(400, 0);

    private PhysicsObject pallo; 
    private PhysicsObject maila;

    private PhysicsObject vasenReuna;
    private PhysicsObject oikeaReuna;

    public override void Begin()
    {
        Luokentta();
        AsetaOhjaimet();
        AloitaPeli();
    }


    /// <summary>
    /// Luodaan kenttä, maila ja pallo.
    /// </summary>
   private void Luokentta()
    {
        tiles.SetTileMethod('x', LuoTiili01, Color.Wheat); 
        tiles.SetTileMethod('y', LuoTiili02, Color.Wheat); 
        tiles.SetTileMethod('z', LuoTiili03, Color.Wheat);
        tiles.SetTileMethod('t', LuoTeksti, Color.Wheat);

        tiles.Execute(tileWidth, tileHeight);
        Level.Background.Image = tausta;
        pallo = new PhysicsObject(60, 20); //
        pallo.Restitution = 1.0;
        pallo.Tag = "pallo";
        pallo.Image = LoadImage("pallo");
        Add(pallo);

        maila = LuoMaila(0.0, Level.Bottom + 60.0);

        vasenReuna = Level.CreateLeftBorder();
        vasenReuna.Restitution = 1.0;
        vasenReuna.KineticFriction = 0.0;
        vasenReuna.IsVisible = false;

        oikeaReuna = Level.CreateRightBorder();
        oikeaReuna.Restitution = 1.0;
        oikeaReuna.KineticFriction = 0.0;
        oikeaReuna.IsVisible = false;

        PhysicsObject ylaReuna = Level.CreateTopBorder();
        ylaReuna.Restitution = 1.0;
        ylaReuna.KineticFriction = 0.0;
        ylaReuna.IsVisible = false;

        PhysicsObject alaReuna = Level.CreateBottomBorder();
        alaReuna.Restitution = 1.0;
        alaReuna.IsVisible = false;
        alaReuna.KineticFriction = 0.0;

        Camera.ZoomToLevel();
    }


     /// <summary>
	 /// Luodaan tiili 01, joka hajoaa pallon osuessa
     /// </summary>
    private void LuoTiili01(Vector paikka, double leveys, double korkeus, Color vari)
    {
        PhysicsObject tiili01 = new PhysicsObject(leveys = 120, korkeus = 30);
        tiili01.Position = paikka;
        tiili01.Color = vari;
        tiili01.Tag = "tiili";
        //tiili01.KineticFriction = 1.0;
        AddCollisionHandler(tiili01, "pallo", TiileenOsui);
        tiili01.Image = LoadImage("tiili01");
        Add(tiili01);
    }


    /// <summary>
    /// Luodaan tiili 02, joka hajoaa pallon osuessa
    /// </summary>
    private void LuoTiili02(Vector paikka, double leveys, double korkeus, Color vari)
    {
        PhysicsObject tiili02 = new PhysicsObject(leveys = 120, korkeus = 30);
        tiili02.Position = paikka;
        tiili02.Color = vari;
        tiili02.Tag = "tiili";
        //tiili02.KineticFriction = 1.0;
        AddCollisionHandler(tiili02, "pallo", TiileenOsui);
        tiili02.Image = LoadImage("tiili02");
        Add(tiili02);
    }


    /// </summary>
    /// Luodaan tiili 03, joka hajoaa pallon osuessa
    /// </summary>
    private void LuoTiili03(Vector paikka, double leveys, double korkeus, Color vari)
    {
        PhysicsObject tiili03 = new PhysicsObject(leveys = 120, korkeus = 30);
        tiili03.Position = paikka;
        tiili03.Color = vari;
        tiili03.Tag = "tiili";
        //tiili03.KineticFriction = 1.0;
        AddCollisionHandler(tiili03, "pallo", TiileenOsui);
        tiili03.Image = LoadImage("tiili03");
        Add(tiili03);
    }


    /// </summary>
    /// Luodaan teksti alareunaan, hajoaa pallon osuessa
    /// </summary>
    private void LuoTeksti(Vector paikka, double leveys, double korkeus, Color vari)
    {
        PhysicsObject teksti = new PhysicsObject(leveys = 970, korkeus = 35);
        teksti.Position = paikka;
        teksti.Color = vari;
        teksti.Tag = "teksti";
        AddCollisionHandler(teksti, "pallo", TiileenOsui);
        teksti.Image = LoadImage("teksti");
        Add(teksti);
    }

    /// <summary>
    /// Kun pallo osuu tiileen
    /// </summary>
    /// <param name="pallo"> joka törmäsi</param>
    /// <param name="tiili"> johon osui</param>
    private void TiileenOsui(PhysicsObject tiili, PhysicsObject pallo)
    {
        RajaytaTiili(tiili);
    }


   	/// <summary>
	/// Aliohjelma tiilen räjäyttämiseksi ja poistamiseksi
	/// </summary>
	/// <param name="tiili"></param> 
    private void RajaytaTiili(IPhysicsObject tiili)
    {
        Explosion rajahdys = new Explosion(tiili.Width * 1);
        rajahdys.Position = tiili.Position;
        rajahdys.UseShockWave = false;
        Add(rajahdys);
        Timer.CreateAndStart(0.05, tiili.Destroy);
        // tiili.Destroy();
        //Remove(tiili); 
    }


    /// <summary>
    /// Luodaan maila
    /// </summary>
    private PhysicsObject LuoMaila(double x, double y)
    {
        PhysicsObject maila = PhysicsObject.CreateStaticObject(200.0, 40.0);
        maila.Shape = Shape.Rectangle;
        maila.X = x;
        maila.Y = y;
        maila.Restitution = 1.0;
        maila.Image = LoadImage("maila");
        Add(maila);
        return maila;
    }


    /// <summary>
    /// Luodaan näppäimet, joilla ohjataan peliä
    /// </summary>
    private void AsetaOhjaimet()
    {
        Keyboard.Listen(Key.Left, ButtonState.Down, AsetaNopeus, "Liikuta mailaa vasemmalle", maila, nopeusVasemmalle);
        Keyboard.Listen(Key.Left, ButtonState.Released, AsetaNopeus, null, maila, Vector.Zero);
        Keyboard.Listen(Key.Right, ButtonState.Down, AsetaNopeus, "Liikuta mailaa oikealle", maila, nopeusOikealle);
        Keyboard.Listen(Key.Right, ButtonState.Released, AsetaNopeus, null, maila, Vector.Zero);

        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    private void AsetaNopeus(PhysicsObject maila, Vector nopeus)
    {
        if ((nopeus.X > 0 && maila.Right > Level.Right) || (nopeus.X < 0 && maila.Left < Level.Left))
        {
            maila.Velocity = Vector.Zero;
            return;
        }
        maila.Velocity = nopeus;
    }

    const double PALLON_MIN_NOPEUS = 500;
    /// <summary>
    /// Jypeli-bugi fix: pallo ei hidastu
    /// </summary>
 
    protected override void Update(Time time)
    {
        if (pallo != null && Math.Abs(pallo.Velocity.X) < PALLON_MIN_NOPEUS)
        {
            pallo.Velocity = new Vector(pallo.Velocity.X * 1.1, pallo.Velocity.Y);
        }
        base.Update(time);
    }

    /// <summary>
    /// Pelin aloitus
    /// </summary>
    private void AloitaPeli()
    {
        Vector impulssi = new Vector(500.0, -100.0);
        pallo.Hit(impulssi);
    }
}
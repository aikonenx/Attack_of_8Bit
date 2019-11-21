using System;
using Jypeli;
using Jypeli.Controls;
using Jypeli.Widgets;
/// @author Ari Ikonen
/// @version 09.11.2019
///
/// <summary>
/// Breakout-peli, jossa tuhotaan tiiliä mailan avulla
/// </summary>

/*
Ongelmia

*/

public class BrickPong : PhysicsGame
{
    private static String[] lines = {
                "    **        *",      
                "*   *   *  * * ",
                "               ",
                "*            * ",
                "***************",
                "               ",
                "               ",
                "               ",
                "               ", 
                "               ",
                "               ",
                "               ",
                "               ",
                "               ",
                "               ", 
                };


    private static int tileWidth = 1000 / lines[0].Length;
    private static int tileHeight = 730 / lines.Length;
    private TileMap tiles = TileMap.FromStringArray(lines);

    private Vector nopeusOikea = new Vector(400, 0);
    private Vector nopeusVasen = new Vector(-400, 0);

    private PhysicsObject pallo;
    private PhysicsObject maila;

    const double PALLON_MIN_NOPEUS = 250;
    /// <summary>
    /// pallo ei hidastu
    /// </summary>
    /// <param name="time"></param>
    protected override void Update(Time time)
    {
        if (pallo != null && Math.Abs(pallo.Velocity.X) < PALLON_MIN_NOPEUS)
        {
            pallo.Velocity = new Vector(pallo.Velocity.X * 1.1, pallo.Velocity.Y);
        }

        if (pallo != null && Math.Abs(pallo.Velocity.Y) < PALLON_MIN_NOPEUS)
        {
            pallo.Velocity = new Vector(pallo.Velocity.X, pallo.Velocity.Y * 1.1);
        }
        base.Update(time);
    }

    public override void Begin()
    {
        LuoKentta();
        AsetaOhjaimet();
        AloitaPeli();

        tiles.SetTileMethod('*', LuoBrick, Color.Green);

        tiles.Execute(tileWidth, tileHeight);

        AddCollisionHandler(pallo, "brick", PalloOsui);
    }

    /// <summary>
    /// Luodaan pelikenttä, pallo ja maila
    /// </summary>
    private void LuoKentta()
    {
        pallo = new PhysicsObject(35.0, 35.0);
        pallo.Shape = Shape.Circle;
        pallo.X = 0.0;
        pallo.Y = 0.0;
        pallo.Restitution = 1.0;
        pallo.Tag = "pallo";
        Add(pallo);

        maila = LuoMaila(0.0, Level.Bottom + 50.0);

        Level.CreateBorders(1.0, true);
        Level.BackgroundColor = Color.Black;

        Camera.ZoomToLevel();
    }

    /// <summary>
    /// Pelin aloittava aliohjelma
    /// </summary>
    private void AloitaPeli()
    {
        Vector impulssi = new Vector(0.0, -400.0);
        pallo.Hit(impulssi);
    }

    /// <summary>
    /// Luodaan maila
    /// </summary>
    /// <param name="x">mailan leveys</param>
    /// <param name="y">mailan korkeus</param>
    /// <returns>maila</returns>
    private PhysicsObject LuoMaila(double x, double y)
    {
        PhysicsObject maila = PhysicsObject.CreateStaticObject(100.0, 20.0);
        maila.Shape = Shape.Rectangle;
        maila.X = x;
        maila.Y = y;
        maila.Restitution = 1.0;
        Add(maila);
        return maila;
    }

    /// <summary>
    /// Luodaan näppäimet, joilla ohjataan peliä
    /// </summary>
    private void AsetaOhjaimet()
    {
        Keyboard.Listen(Key.Right, ButtonState.Down, AsetaNopeus, "Liikuta mailaa oikealle", maila, nopeusOikea);
        Keyboard.Listen(Key.Right, ButtonState.Released, AsetaNopeus, null, maila, Vector.Zero);
        Keyboard.Listen(Key.Left, ButtonState.Down, AsetaNopeus, "Liikuta mailaa vasemmalle", maila, nopeusVasen);
        Keyboard.Listen(Key.Left, ButtonState.Released, AsetaNopeus, null, maila, Vector.Zero);

        Keyboard.Listen(Key.H, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    /// <summary>
    /// Luodaan mailan nopeus/liikkuvuus
    /// </summary>
    /// <param name="maila">minkä nopeus</param>
    /// <param name="nopeus">kuinka nopea</param>
    private void AsetaNopeus(PhysicsObject maila, Vector nopeus)
    {
        if ((nopeus.X < 0) && (maila.Left < Level.Left))
        {
            maila.Velocity = Vector.Zero;
            return;
        }
        if ((nopeus.X > 0) && (maila.Right > Level.Right))
        {
            maila.Velocity = Vector.Zero;
            return;
        }

        maila.Velocity = nopeus;
    }

    /// <summary>
    /// Luodaan kohteet eli "brickit"
    /// </summary>
    /// <param name="paikka">mihin brick menee, sijoitetaan taulukkoon</param>
    /// <param name="leveys">brickin leveys</param>
    /// <param name="korkeus">brickin korkeus</param>
    /// <param name="vari">brickin väri</param>
    private void LuoBrick(Vector paikka, double leveys, double korkeus, Color vari)
    {
        PhysicsObject brick = PhysicsObject.CreateStaticObject(leveys * 0.9, korkeus * 0.9, Shape.Rectangle);
        brick.Position = paikka;
        brick.Color = vari;
        brick.Restitution = 1.0;
        brick.Tag = "brick";
        Add(brick);
    }

    /// <summary>
    /// Aliohjelma pallon osumiselle kohteeseen
    /// </summary>
    /// <param name="pallo">mikä osuu</param>
    /// <param name="brick">mihin osuu</param>
    private void PalloOsui(PhysicsObject pallo, PhysicsObject brick)
    {
        brick.Destroy();
    }
}

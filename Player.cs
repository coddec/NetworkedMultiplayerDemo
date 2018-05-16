using System;
using SplashKitSDK;

public class Player
{
    public string Name { get; set; }
    public double X { get; private set; }
    public double Y { get; private set; }
    public double Radius { get; set; } = 10;
    public Circle c;
    private Color PlayerColor;

    private int Speed = 5;

    public Player(Window gameWindow, string name)
    {
        X = gameWindow.Width / 2;
        Y = gameWindow.Height / 2;
        Name = name;

        PlayerColor = SplashKit.RandomRGBColor(200);
    }


    public void Draw()
    {
        SplashKit.FillCircle(PlayerColor, X, Y, Radius);
        c = SplashKit.CircleAt(X, Y, Radius);

        SplashKit.DrawText(Name, Color.Black, X - Radius, Y - Radius - 10);

    }

    public bool Quit { get; private set; }

    public void HandleInput()
    {
        if (SplashKit.KeyDown(SplashKitSDK.KeyCode.EscapeKey))
        {
            Quit = true;
        }

        if (SplashKit.QuitRequested())
        {
            Quit = true;
        }

        if (SplashKit.KeyDown(SplashKitSDK.KeyCode.LeftKey))
        {
            X -= Speed;
        }

        if (SplashKit.KeyDown(SplashKitSDK.KeyCode.RightKey))
        {
            X += Speed;
        }

        if (SplashKit.KeyDown(SplashKitSDK.KeyCode.UpKey))
        {
            Y -= Speed;
        }

        if (SplashKit.KeyDown(SplashKitSDK.KeyCode.DownKey))
        {
            Y += Speed;
        }
    }

    public bool CollidedWithPlayer(OtherPlayer op)
    {
        return SplashKit.CirclesIntersect(c, op.CollisionCircle);
    }
}

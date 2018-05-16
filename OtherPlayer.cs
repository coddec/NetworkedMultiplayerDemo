using System;
using SplashKitSDK;

public class OtherPlayer
{
    public string Name { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Radius { get; set; } = 10;
    private Color PlayerColor;

    public OtherPlayer(string name, double x, double y, double radius)
    {
        Name = name;
        X = x;
        Y = y;
        Radius = radius;
        PlayerColor = SplashKit.ColorGray();
    }

    public void Draw()
    {
        SplashKit.FillCircle(PlayerColor, X, Y, Radius);
        SplashKit.DrawText(Name, Color.Red, X - Radius, Y - Radius - 10);
    }

    public Circle CollisionCircle
    {
        get
        {
            return SplashKit.CircleAt(X, Y, Radius);
        }
    }
}

using Microsoft.Xna.Framework;

namespace SharedLib;

public class AndroidGameScreen : IGameScreen
{
    public Point resolution {get;set;} = Point.Zero;

    public Point GetResolution()
    {
        return resolution;
    }

    public void SetResolution(Point resolution)
    {
        this.resolution = resolution;
    }
}
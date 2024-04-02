using Microsoft.Xna.Framework;

namespace SharedLib;

public class DesktopGameScreen : IGameScreen
{
    public Point GetResolution()
    {
        //return new Point(144 * 4, 128 * 4);
        return new Point(480, 640); // 4.445 x 3.75
    }

    public void SetResolution(Point resolution)
    {
        // Do nothing
    }
}
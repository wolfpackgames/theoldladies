using Microsoft.Xna.Framework;

public interface IGameScreen
{
  Point GetResolution();
  void SetResolution(Point resolution);
}
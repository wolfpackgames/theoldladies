using Microsoft.Xna.Framework;
using MonoGame.Aseprite;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;

namespace SharedLib
{
  /// <summary>
  /// Provides extension methods for game-related functionality.
  /// </summary>
  public static class GameExtensions
  {
    /// <summary>
    /// Gets the global scale used for game screens.
    /// </summary>
    public static Point GlobalScale { get { return new Point(128, 144); } }

    /// <summary>
    /// Determines whether the specified game screen has a resolution of zero.
    /// </summary>
    /// <param name="screen">The game screen to check.</param>
    /// <returns><c>true</c> if the game screen has a resolution of zero; otherwise, <c>false</c>.</returns>
    public static bool IsZero(this IGameScreen screen)
    {
      return screen.GetResolution() == Point.Zero;
    }

    /// <summary>
    /// Scales the game screen using the global scale.
    /// </summary>
    /// <param name="screen">The game screen to scale.</param>
    /// <returns>The scaled vector.</returns>
    public static Vector2 Scale(this IGameScreen screen)
    {
      return screen.Scale(GlobalScale);
    }

    /// <summary>
    /// Scales the game screen using the specified factor.
    /// </summary>
    /// <param name="screen">The game screen to scale.</param>
    /// <param name="factor">The scaling factor.</param>
    /// <returns>The scaled vector.</returns>
    public static Vector2 Scale(this IGameScreen screen, Point factor)
    {
      return new Vector2((float)screen.GetResolution().X / factor.X, (float)screen.GetResolution().Y / factor.Y);
    }

    /// <summary>
    /// Converts the specified position to screen coordinates.
    /// </summary>
    /// <param name="screen">The game screen.</param>
    /// <param name="position">The position to convert.</param>
    /// <returns>The converted position.</returns>
    public static Vector2 Coordinate(this IGameScreen screen, Vector2 position)
    {
      return screen.Scale() * position;
    }

    /// <summary>
    /// Converts the specified point to a vector2.
    /// </summary>
    /// <param name="point">The point to convert.</param>
    /// <returns>The converted vector2.</returns>
    public static Vector2 AsVector2(this Point point)
    {
      return new Vector2(point.X, point.Y);
    }
  }

}
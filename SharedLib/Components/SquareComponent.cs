using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SharedLib;

public class SquareComponent
{
  public string Name { get; private set; }
  public int SquareWidth { get; private set; }
  public int SquareHeight { get; private set; }
  public int SquareBorder { get; private set; }
  public Rectangle Position { get; private set; }
  public bool IsClicked { get { return _squareColor == _clickColor; } }
  public Point BoardPosition { get; private set; }
  private Texture2D _squareTexture;
  private Color _squareColor = Color.Black;
  private Color _clickColor = Color.Red;


  public SquareComponent(
    Texture2D texture,
    Point position,
    Vector2 gridPosition,
    int width = 100,
    int height = 100,
    int scale = 1    
    )
  {
    _squareTexture = texture;
    SquareWidth = width;
    SquareHeight = height;
    BoardPosition = position;

    Name = $"Square at {position.X}, {position.Y}";

    Position = new Rectangle(
        (position.X * SquareWidth  * scale) + (int)gridPosition.X,
        (position.Y * SquareHeight * scale) + (int)gridPosition.Y,
        SquareWidth * scale,
        SquareHeight * scale
      );
  }

  public void Clicked(Color color)
  {
    _squareColor = color;
  }

  public void Reset()
  {
    _squareColor = Color.Black;
  }

  public void Draw(SpriteBatch spriteBatch)
  {
    // Draw the square that will be used as border
    spriteBatch.Draw(
      _squareTexture, 
      Position, 
      _squareColor
      );
  }
}
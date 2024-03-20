using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SharedLib;

public class BoardGrid
{
  public bool IsClicked { get; set; }

  private Texture2D _texture;
  private int _scale = 1;
  private Rectangle[,] _gridTexturePosition = new Rectangle[3, 3];
  private PlayerEnum[,] _grid = new PlayerEnum[3, 3];
  private const int _imageSize = 64;
  private Point _xPointPosition = new Point(64 * 3, 0);
  private Point _oPointPosition = new Point(64 * 3, 64);
  private Point _nonePointSize = new Point(64 * 3, 64 * 2);

  public BoardGrid(Texture2D texture, int scale = 1)
  {
    _texture = texture;
    _scale = scale;
    Reset();
  }

  public void Reset()
  {
    for (int row = 0; row < 3; row++)
    {
      for (int column = 0; column < 3; column++)
      {
        _grid[row, column] = PlayerEnum.None;
        _gridTexturePosition[row, column] =
          new Rectangle(
            _imageSize * row * _scale, // Scaled X position on screen
            _imageSize * column * _scale, // Scaled Y position on screen
            _imageSize * _scale, // Scaled size
            _imageSize * _scale  // Scaled size
          );
      }
    }
  }

  public (PlayerEnum, Point) Clicked(Point clickedPoint, PlayerEnum player)
  {
    for (int row = 0; row < 3; row++)
    {
      for (int column = 0; column < 3; column++)
      {
        if (_gridTexturePosition[row, column].Contains(clickedPoint) && _grid[row, column] == PlayerEnum.None)
        {
          _grid[row, column] = player;
          return (_grid[row, column], new Point(row, column));
        }
      }
    }
    return (PlayerEnum.None, new Point(-1, -1));
  }

  public void Draw(SpriteBatch spriteBatch)
  {
    // Draws the board grid
    for (int row = 0; row < 3; row++)
    {
      for (int column = 0; column < 3; column++)
      {
        spriteBatch.Draw(
          _texture,
          _gridTexturePosition[row, column],
          new Rectangle(
            GetPointPosition(_grid[row, column]).X,
            GetPointPosition(_grid[row, column]).Y,
            _imageSize,
            _imageSize
          ), // This defines the position on the texture atlas
          Color.Black
        );

        spriteBatch.Draw(
          _texture,
          _gridTexturePosition[row, column],
          new Rectangle(_imageSize * row, _imageSize * column, _imageSize, _imageSize), // This defines the position on the texture atlas
          Color.Black
        );
      }
    }
  }

  private Point GetPointPosition(PlayerEnum player)
  {
    return player switch
    {
      PlayerEnum.X => _xPointPosition,
      PlayerEnum.O => _oPointPosition,
      _ => _nonePointSize
    };
  }
}
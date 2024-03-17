using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SharedLib
{
  /// <summary>
  /// Represents a component for drawing text using a custom font texture.
  /// </summary>
  public class GameFontComponent
  {
    private Texture2D _fontTexture;
    private int _charWidth;
    private int _charHeight;
    private char[][] _characters;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameFontComponent"/> class.
    /// </summary>
    /// <param name="fontTexture">The font texture used for drawing.</param>
    /// <param name="charWidth">The width of each character in the font texture.</param>
    /// <param name="charHeight">The height of each character in the font texture.</param>
    /// <param name="characters">The matrix of characters in the font texture.</param>
    public GameFontComponent(Texture2D fontTexture, int charWidth, int charHeight, char[][] characters)
    {
      this._fontTexture = fontTexture;
      this._charWidth = charWidth;
      this._charHeight = charHeight;
      this._characters = characters;
    }

    /// <summary>
    /// Draws a string of text using the specified font texture and position.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used for drawing.</param>
    /// <param name="text">The text to be drawn.</param>
    /// <param name="position">The position where the text should be drawn.</param>
    /// <param name="color">The color of the text.</param>
    public void Draw(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
    {
      Vector2 drawPosition = position;

      foreach (char textCharacter in text)
      {
        // Find the position of the character in the matrix
        (int row, int column) = FindCharacter(_characters, textCharacter);
        // If the character is found in the matrix
        if (row >= 0 && column >= 0)
        {
          // Calculate the source rectangle for the character
          Rectangle sourceRect = new Rectangle(column * _charWidth, row * _charHeight, _charWidth, _charHeight);
          // Draw the character
          spriteBatch.Draw(_fontTexture, drawPosition, sourceRect, color);
          // Advance draw position to the right for the next character
          drawPosition.X += _charWidth;
        }
      }
    }

    /// <summary>
    /// Finds the position of a target character in a matrix.
    /// </summary>
    /// <param name="matrix">The matrix to search in.</param>
    /// <param name="target">The target character to find.</param>
    /// <returns>
    /// The position of the target character as a tuple of row and column indices.
    /// If the target character is not found, (-1, -1) is returned.
    /// </returns>
    private (int, int) FindCharacter(char[][] matrix, char target)
    {
      for (int i = 0; i < matrix.Length; i++)
      {
        for (int j = 0; j < matrix[i].Length; j++)
        {
          if (matrix[i][j] == target)
          {
            return (i, j);
          }
        }
      }

      return (-1, -1);
    }
  }

}

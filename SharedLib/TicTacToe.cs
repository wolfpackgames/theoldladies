using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SharedLib
{
  public class TicTacToe
  {

    public delegate void PlayerXWonHandler();
    public delegate void PlayerOWonHandler();
    public delegate void GameDrawHandler();

    public event PlayerXWonHandler PlayerXWon;
    public event PlayerOWonHandler PlayerOWon;
    public event GameDrawHandler GameDraw;

    private enum GameState
    {
      Playing,
      Draw,
      XWon,
      OWon,
      Won
    }
    private List<SquareComponent> _squares = new();
    private GameFontComponent _gameFontComponent;
    private bool _xPlayer = true;
    private int[,] _gameBoard = new int[3, 3];
    private bool _gameOver = false;

    private Rectangle _textBarArea;

    public TicTacToe()
    {
      Reset();
    }

    public void Initialize(ContentManager content,Vector2 gridPosition,int squareSize,Rectangle textBarArea)
    {
      Texture2D _squareTexture = content.Load<Texture2D>("Square");
      _textBarArea = textBarArea;

      for (int column = 0; column < 3; column++)
      {
        for (int row = 0; row < 3; row++)
        {
          _squares.Add(new SquareComponent(_squareTexture, new Point(column, row),gridPosition, squareSize, squareSize));
        }
      }

      _gameFontComponent = new GameFontComponent(
                content.Load<Texture2D>("Font"),
                16,
                16,
                new char[][]
                {
                    "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(),
                    "abcdefghijklmnopqrstuvwxyz".ToCharArray(),
                    "0123456789,<.>/?;:'\"[{]}`~".ToCharArray(),
                    "!@#$%^&*()_-=+\\| ".ToCharArray(),
                }
            );

    }

    public void Draw(SpriteBatch spriteBatch)
    {
      _squares.ForEach(square => square.Draw(spriteBatch));

      string text = CheckWin(_xPlayer ? 1 : 2) switch
      {
        GameState.Draw => "Draw",
        GameState.XWon => "X Won",
        GameState.OWon => "O Won",
        GameState.Playing => $"{(_xPlayer ? 'X' : 'O')} turn",
        _ => throw new NotImplementedException()
      };

      var textSize = text.Length * 16;
      var restartTextSize = "Click to restart".Length * 16;


      _gameFontComponent.Draw(spriteBatch, text, new Vector2((_textBarArea.Width / 2)-textSize / 2,_textBarArea.Y), Color.Black);

      if(_gameOver)
        _gameFontComponent.Draw(spriteBatch, "Click to restart", new Vector2((_textBarArea.Width / 2)-restartTextSize / 2,_textBarArea.Y + 16), Color.Red);
    }

    public void Reset()
    {
      if (_gameOver)
      {
        _gameOver = false;
        _xPlayer = true;
        _gameBoard = new int[3, 3];
        _squares.ForEach(square => square.Reset());
      }
    }

    public void SelectSquare(Point selectedPosition)
    {
      if (_gameOver)
        return;

      if (selectedPosition != Point.Zero)
      {
        // Iterate over the list of squares
        foreach (SquareComponent square in _squares)
        {
          // Check if the mouse click occurred within the square
          if (selectedPosition.X >= square.Position.X && selectedPosition.X <= square.Position.X + square.Position.Width &&
              selectedPosition.Y >= square.Position.Y && selectedPosition.Y <= square.Position.Y + square.Position.Height &&
              !square.IsClicked
              )
          {
            square.Clicked(_xPlayer ? Color.Red : Color.Yellow);

            // Mark the square with the current player's symbol if it's not already marked
            if (_gameBoard[square.BoardPosition.X, square.BoardPosition.Y] == 0)
            {
              _gameBoard[square.BoardPosition.X, square.BoardPosition.Y] = _xPlayer ? 1 : 2;

              var gameState = CheckWin(_xPlayer ? 1 : 2);
              if (gameState == GameState.Playing)
              {
                _xPlayer = !_xPlayer;
              }
              else if (gameState == GameState.Draw)
              {
                _gameOver = true;
                GameDraw?.Invoke();
              }
              else
              {
                _gameOver = true;
                if (_xPlayer)
                {
                  PlayerXWon?.Invoke();
                }
                else
                {
                  PlayerOWon?.Invoke();
                }
              }

              break;
            }
          }

        }
      }
    }

    private GameState CheckWin(int player)
    {
      for (int i = 0; i < 3; i++)
      {
        if ((_gameBoard[i, 0] == player && _gameBoard[i, 1] == player && _gameBoard[i, 2] == player) ||
            (_gameBoard[0, i] == player && _gameBoard[1, i] == player && _gameBoard[2, i] == player))
        {
          return player == 1 ? GameState.XWon : GameState.OWon;
        }
      }

      if ((_gameBoard[0, 0] == player && _gameBoard[1, 1] == player && _gameBoard[2, 2] == player) ||
          (_gameBoard[0, 2] == player && _gameBoard[1, 1] == player && _gameBoard[2, 0] == player))
      {
        return player == 1 ? GameState.XWon : GameState.OWon;
      }

      bool isBoardFull = true;
      for (int i = 0; i < 3; i++)
      {
        for (int j = 0; j < 3; j++)
        {
          if (_gameBoard[i, j] == 0)
          {
            isBoardFull = false;
            break;
          }
        }
      }

      if (isBoardFull)
      {
        return GameState.Draw;
      }

      return GameState.Playing;
    }

  }
}
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
    public bool GamePlaying => !_gameOver;

    private enum GameState
    {
      Playing,
      Draw,
      XWon,
      OWon,
      Won
    }
    private BoardGrid _grid;
    private GameFontComponent _gameFontComponent;
    private bool _xPlayer = true;
    private int[,] _gameBoard = new int[3, 3];
    private bool _gameOver = false;

    private Texture2D _gameTexture;

    int _height = 0;


    public TicTacToe()
    {
      Reset();
    }

    public void Initialize(ContentManager content, int scale)
    {

      _gameTexture = content.Load<Texture2D>("Tic-Tac-Toe");      

      _grid = new BoardGrid(_gameTexture,scale / (64 * 3) );

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
                },
                 scale / 16
            );

            _height = scale;

    }

    public void Draw(SpriteBatch spriteBatch)
    {
      _grid.Draw(spriteBatch);
      string text = CheckWin(_xPlayer ? 1 : 2) switch
      {
        GameState.Draw => "Draw",
        GameState.XWon => "X Won",
        GameState.OWon => "O Won",
        GameState.Playing => $"{(_xPlayer ? 'X' : 'O')} turn",
        _ => throw new NotImplementedException()
      };

      var textSize = text.Length * (_height / 16);
      var restartTextSize = "Click to restart".Length * (_height / 16);

      _gameFontComponent.Draw(spriteBatch, text, new Vector2(textSize / 2, 64 * 3 * (_height / (64 * 3))), Color.Black);

      if (_gameOver)
        _gameFontComponent.Draw(spriteBatch, "Click to restart", new Vector2(0, 64 * 3 * (_height / (64 * 3)) + (_height / 16)), Color.Red);
    }

    public void Reset()
    {
      if (_gameOver)
      {
        _gameOver = false;
        _xPlayer = true;
        _gameBoard = new int[3, 3];
        _grid.Reset();
      }
    }

    public void SelectSquare(Point selectedPosition)
    {
      if (_gameOver)
        return;

      if (selectedPosition != Point.Zero)
      {
        var (player, position) = _grid.Clicked(selectedPosition, _xPlayer ? PlayerEnum.X : PlayerEnum.O);
        if (position == new Point(-1, -1))
          return;

        _gameBoard[position.X, position.Y] = _xPlayer ? 1 : 2;

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
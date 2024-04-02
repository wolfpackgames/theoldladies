using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;

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
    };

    private GameFontComponent _gameFontComponent;
    private bool _xPlayer = true;
    private int[,] _gameBoard = new int[3, 3];
    private bool _gameOver = false;
    int _height = 0;
    private Dictionary<string,Sprite> _sprites = new();
 
    private IGameScreen _gameScreen;

    public TicTacToe()
    {
      Reset();
    }

    public void Initialize(ContentManager content, IGameScreen gameScreen, GraphicsDevice graphics)
    { 
      _gameScreen = gameScreen;
      AsepriteFile aseFile = content.Load<AsepriteFile>("SpriteSheet");
         
      TextureAtlas atlas = aseFile.CreateTextureAtlas(graphics);
      atlas.CreateRegion("Screen",0,0,128,144);
      atlas.CreateRegion("Board",128,0,32 * 3,32 * 3);
      atlas.CreateRegion("Xplayer",0,0,128,144);
      atlas.CreateRegion("Oplayer",0,0,128,144);
      atlas.CreateRegion("Banner",0,0,128,144);
      atlas.CreateRegion("Button",0,0,128,144);
      
      // Load all atlases into the dictionary
      var regions = atlas.GetEnumerator();
      regions.MoveNext(); //This is to skip the first one
      while(regions.MoveNext())
      {
        // Add the region as a sprite to the dictionary
        _sprites.Add(regions.Current.Name,atlas.CreateSprite(regions.Current.Name));
        // Set the correct scale
        _sprites[regions.Current.Name].Scale = _gameScreen.Scale();
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
                },
                 16 * (int)_gameScreen.Scale().X
            );

            _height = (int)_gameScreen.Scale().X;//gameScreen.Scale();

            ;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      _sprites["Screen"].Draw(spriteBatch, Vector2.Zero);      
      _sprites["Board"].Draw(spriteBatch,_gameScreen.Coordinate(new Vector2(16,32)));

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

      //_gameFontComponent.Draw(spriteBatch, text, new Vector2(textSize / 2, 64 * 3 * (_height / (64 * 3))), Color.Black);

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
      }
    }

    public void SelectSquare(Point selectedPosition)
    {
      if (_gameOver || !_gameOver)
        return;
/*
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
      }*/
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
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SharedLib
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private InputManager _inputManager;
        private TicTacToe _ticTacToe;
        private IGameScreen _gameScreen;

        public Game1(IGameScreen gameScreen)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            if (!gameScreen.IsZero())
            {
                _graphics.PreferredBackBufferWidth = gameScreen.GetResolution().X;
                _graphics.PreferredBackBufferHeight = gameScreen.GetResolution().Y;
                _graphics.ApplyChanges();
            }
            else
            {
                gameScreen.SetResolution(new Point(_graphics.PreferredBackBufferHeight,_graphics.PreferredBackBufferWidth));
            }


            _ticTacToe = new();
            _inputManager = new();
            _inputManager.PointSelected += OnPointSelected;
            _gameScreen = gameScreen;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _ticTacToe.Initialize(Content, _gameScreen, GraphicsDevice);
            _inputManager.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            _inputManager.Update();
            base.Update(gameTime);
        }

        private void OnPointSelected(Point selectedPosition)
        {
            Console.WriteLine($"Selected position {selectedPosition}");
            if (!_ticTacToe.GamePlaying)
                _ticTacToe.Reset();
            else
                _ticTacToe.SelectSquare(selectedPosition);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _ticTacToe.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

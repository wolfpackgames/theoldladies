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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _ticTacToe = new();
            _inputManager = new();
            _inputManager.PointSelected += OnPointSelected;
        }

        protected override void Initialize()
        {
            base.Initialize();
            //2028 - 1014

            float screenHeight = GraphicsDevice.Viewport.Height;
            float screenWidth = GraphicsDevice.Viewport.Width;

            float smallerReference = Math.Min(screenHeight, screenWidth);


            Console.WriteLine($"Screen Height: {screenHeight}, Screen Width: {screenWidth}, Bigger Reference: {smallerReference}");
            Console.WriteLine($"Scale Grid: {smallerReference / (64 * 3)}, Scale Font: {smallerReference / 16}");
          
            
            _ticTacToe.Initialize(Content, (int)smallerReference);
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

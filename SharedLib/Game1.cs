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
           

            float desiredGridHeightPercentage = 0.8f; // 80% for the grid
            float screenHeight = GraphicsDevice.Viewport.Height;
            float screenWidth = GraphicsDevice.Viewport.Width;
            Console.WriteLine($"Screen Height: {screenHeight}, Screen Width: {screenWidth}");

            // Calculate text bar area
            float textBarHeight = screenHeight * 0.2f; // 20% for the text bar
            var textBarArea = new Rectangle(0, (int)(screenHeight - textBarHeight) + 32, GraphicsDevice.Viewport.Width, (int)textBarHeight);

            Console.WriteLine($"Text Bar Area: {textBarArea}");

             var gridSizeH = textBarArea.Y / 3;

            Console.WriteLine($"Grid Size H: {gridSizeH}");


            // Calculate grid scale based on screen height
            var gridScale = (screenWidth * desiredGridHeightPercentage) / textBarArea.Y;
            var gridPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - (textBarArea.Y/2), 0);

            Console.WriteLine($"Grid Scale: {gridScale}, Grid Position: {gridPosition}");



             _ticTacToe.Initialize(Content,gridPosition,textBarArea.Y/3,textBarArea);
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
            _ticTacToe.Reset();
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

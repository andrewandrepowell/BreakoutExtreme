using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using BreakoutExtreme.Components;
using System;
using System.Collections.Generic;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BreakoutExtremeGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Controller _controller;
        Runner _runner;
        Texter _testTexter;
        
        public BreakoutExtremeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
#if DEBUG
            try
            {
#endif
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                _controller = new Controller();
                _runner = new Runner();

                Globals.Initialize(
                    spriteBatch: _spriteBatch, 
                    contentManager: Content,
                    controlState: _controller.GetControlState(),
                    game: this,
                    runner: _runner);

                _runner.CreateGameWindow();

                Texter.Load();
                Animater.Load();

                _testTexter = new() { Color = Color.Yellow };
#if DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
#endif
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
#if DEBUG
            try
            {
#endif
                Globals.Update(gameTime);
                _controller.Update();
                _runner.Update();

                var controllerState = _controller.GetControlState();
                var windowSize = _spriteBatch.GraphicsDevice.Viewport.Bounds.Size;
                _testTexter.Message = $"Cursor Position: {controllerState.CursorPosition}. Cursor State: {controllerState.CursorSelectState}"; 
                _testTexter.Position = _testTexter.Size / 2;

                base.Update(gameTime);
#if DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
#endif
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
#if DEBUG
            try
            {
#endif
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _runner.Draw();

                _spriteBatch.Begin();
                _testTexter.Draw();
                _spriteBatch.End();
                base.Draw(gameTime);
#if DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
#endif
        }
    }
}

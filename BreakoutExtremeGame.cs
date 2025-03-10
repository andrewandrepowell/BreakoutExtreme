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
using Microsoft.Extensions.Logging;

namespace BreakoutExtreme
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BreakoutExtremeGame : Game
    {
#pragma warning disable IDE0052
        private readonly GraphicsDeviceManager _graphics;
#pragma warning restore IDE0052
        private SpriteBatch _spriteBatch;
        private Controller _controller;
        private Runner _runner;
        private Texter _logger;
        private Loader _loader;
        
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
                _loader = new Loader(messageAction: (string message) => _logger.Message = message);
                _loader.Add(
                    action: () => _runner.Initialize(),
                    message: "Initializing Runner");
                _loader.Add(
                    action: () => _runner.CreateGameWindow(),
                    message: "Creating Game Window");
                _loader.Add(
                    action: () => Texter.Load(),
                    message: "Loading Texter Fonts...");
                _loader.Add(
                    action: () => Animater.Load(),
                    message: "Loading Animations...");
                _loader.Add(
                    action: () => Particler.Load(),
                    message: "Loading Particles...");
                _loader.Add(
                    action: () => Sounder.Load(),
                    message: "Loading Sounds...");
                _loader.Start();

                _spriteBatch = new SpriteBatch(GraphicsDevice);
                _controller = new Controller();
                _runner = new Runner();
                Globals.Initialize(
                    spriteBatch: _spriteBatch,
                    contentManager: Content,
                    controlState: _controller.GetControlState(),
                    runner: _runner);

                _logger = new() { Color = Color.White };
                Globals.Initialize(_logger);
#if DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
#pragma warning disable CA2200
                throw e;
#pragma warning restore CA2200
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
                if (_loader.Loaded)
                {
                    Globals.Update(gameTime);
                    _controller.Update();
                    _runner.Update();
                }
                else
                {
                    _loader.Update();
                }
                _logger.Position = _logger.Size / 2;
                base.Update(gameTime);
#if DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
#pragma warning disable CA2200
                throw e;
#pragma warning restore CA2200
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
                GraphicsDevice.Clear(Color.Black);

                if (_loader.Loaded)
                    _runner.Draw();

                _spriteBatch.Begin();
                _logger.Draw();
                _spriteBatch.End();
                base.Draw(gameTime);
#if DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
#pragma warning disable CA2200
                throw e;
#pragma warning restore CA2200
            }
#endif
        }
    }
}

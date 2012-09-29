using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using Microsoft.Phone.Controls;
using Microsoft.Devices;

using System.Diagnostics;

//using WindowsPhoneGame1;


namespace WindowsPhoneGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D whitePixel;

        Braile braileData;
        Controls controls;
        VibrateController vibrate;

        string[] braileText;
        string testText = "#";

        int wordPos = 0;
        int letterPos = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            TouchPanel.EnabledGestures = GestureType.HorizontalDrag | GestureType.DragComplete;
            vibrate = VibrateController.Default;

            braileData = new Braile(graphics);
            controls = new Controls(graphics);

            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            whitePixel.SetData(new[] { Color.White });

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            Braile.setBraileState(braileData.braileLetter1, '#');
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);

            foreach (TouchLocation location in TouchPanel.GetState())
            {
                // Letter control
                for (int i = 0; i < Braile.cBraileDotsNum; i++)
                {
                    if (braileData.braileLetter1[i].rect.Contains((int)location.Position.X, (int)location.Position.Y))
                    {
                        if (i == 1)
                        {
                            //vibrate.Start(TimeSpan.FromMilliseconds(20));
                            vibrate.Start(TimeSpan.FromMilliseconds(5));
                            System.Threading.Thread.Sleep(20);
                            vibrate.Start(TimeSpan.FromMilliseconds(10));
                            System.Threading.Thread.Sleep(10);
                            vibrate.Start(TimeSpan.FromMilliseconds(5));
                        }

                        if (i == 3)
                        {
                            //vibrate.Start(TimeSpan.FromMilliseconds(1));
                            vibrate.Start(TimeSpan.FromMilliseconds(20));
                            System.Threading.Thread.Sleep(40);
                            vibrate.Start(TimeSpan.FromMilliseconds(20));
                            System.Threading.Thread.Sleep(20);
                            vibrate.Start(TimeSpan.FromMilliseconds(10));
                        }

                        if (i == 5)
                        {
                            vibrate.Start(TimeSpan.FromMilliseconds(100));
                        }

                        if (i == 7)
                        {
                            vibrate.Start(TimeSpan.FromMilliseconds(50));
                            //vibrate.Start(TimeSpan.FromMilliseconds(70));
                            //System.Threading.Thread.Sleep(120);
                            //vibrate.Start(TimeSpan.FromMilliseconds(70));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            foreach (Braile.braileDot br in braileData.braileLetter1)
            {
                if (br.state)
                {
                    spriteBatch.Draw(whitePixel, br.rect, Color.Tomato);
                }

                spriteBatch.Draw(whitePixel, new Rectangle(br.rect.Left, br.rect.Top, 2, br.rect.Height), Color.Black);
                spriteBatch.Draw(whitePixel, new Rectangle(br.rect.Right, br.rect.Top, 2, br.rect.Height), Color.Black);
                spriteBatch.Draw(whitePixel, new Rectangle(br.rect.Left, br.rect.Top, br.rect.Width, 2), Color.Black);
                spriteBatch.Draw(whitePixel, new Rectangle(br.rect.Left, br.rect.Bottom, br.rect.Width, 2), Color.Black);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

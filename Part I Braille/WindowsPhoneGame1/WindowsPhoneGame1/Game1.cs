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
        string testText = "a b c d e f g h i j k l m n o p q r s t u v w x y z";

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
            braileText = Braile.prepareText(testText);
            Debug.WriteLine(braileText[0][0]);

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

        void proceedText()
        {
            Braile.setBraileState(braileData.braileLetter1, braileText[wordPos][letterPos]);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            proceedText();

            base.Update(gameTime);

            bool nextLetter = true;
            foreach (TouchLocation location in TouchPanel.GetState())
            {
                // Letter control
                for (int i = 0; i < Braile.cBraileDotsNum; i++)
                {
                    if (braileData.braileLetter1[i].rect.Contains((int)location.Position.X, (int)location.Position.Y))
                    {
                        if (braileData.braileLetter1[i].state)
                        {
                            // start vibe
                            Debug.WriteLine("Yes vibe! {0}", i);
                            vibrate.Start(TimeSpan.FromMilliseconds(200));
                        }
                        else
                        {
                            // stop vibe
                            Debug.WriteLine("No vibe! {0}", i);
                            vibrate.Stop();
                        }

                        braileData.braileLetter1[i].touched = true;
                        nextLetter = false;
                    }
                }

                // Controls
                if (controls.letterControl.Contains((int)location.Position.X, (int)location.Position.Y))
                {
                    //Debug.WriteLine("Start controling letter");

                    while (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample gs = TouchPanel.ReadGesture();
                        switch (gs.GestureType)
                        {
                            case GestureType.HorizontalDrag:
                                controls.letterControlDeltaX += gs.Delta.X;
                                Debug.WriteLine("VerticalDrag {0}", gs.Delta.X);
                                break;

                            case GestureType.DragComplete:
                                if (controls.letterControlDeltaX > 10.0)
                                {
                                    if (wordPos + 1 < braileText.Length)
                                    {
                                        wordPos++;
                                        letterPos = 0;
                                    }
                                    else
                                    {
                                        Debug.WriteLine("End of text");
                                    }

                                    Debug.WriteLine("Next word!");
                                }
                                else if (controls.letterControlDeltaX < -10.0) 
                                {
                                    if (letterPos == 0 && wordPos != 0)
                                    {
                                        wordPos--;
                                        Debug.WriteLine("Previous word");
                                    }

                                    letterPos = 0;
                                    Debug.WriteLine("Begining of the word");
                                }

                                Debug.WriteLine("{0}", controls.letterControlDeltaX);

                                controls.letterControlDeltaX = 0;
                                break;
                        }
                    }
                }
            }

            if (braileData.braileLetter1.All(x => x.touched) && nextLetter)
            {
                Debug.WriteLine("Next letter!");
                if (letterPos + 1 < braileText[wordPos].Length)
                {
                    letterPos++;
                }
                else
                {
                    letterPos = 0;

                    if (wordPos + 1 < braileText.Length)
                    {
                        vibrate.Start(TimeSpan.FromMilliseconds(100));
                        System.Threading.Thread.Sleep(200);
                        vibrate.Start(TimeSpan.FromMilliseconds(100));

                        wordPos++;
                    }
                    else
                    {
                        Debug.WriteLine("End of text!");
                    }
                }

                for (int i = 0; i < braileData.braileLetter1.Length; i++)
                {
                    braileData.braileLetter1[i].touched = false;
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


            // Letter swipe
            spriteBatch.Draw(whitePixel, new Rectangle(controls.letterControl.Left, controls.letterControl.Top, 2, controls.letterControl.Height), Color.Black);
            spriteBatch.Draw(whitePixel, new Rectangle(controls.letterControl.Right, controls.letterControl.Top, 2, controls.letterControl.Height), Color.Black);
            spriteBatch.Draw(whitePixel, new Rectangle(controls.letterControl.Left, controls.letterControl.Top, controls.letterControl.Width, 2), Color.Black);
            spriteBatch.Draw(whitePixel, new Rectangle(controls.letterControl.Left, controls.letterControl.Bottom, controls.letterControl.Width, 2), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

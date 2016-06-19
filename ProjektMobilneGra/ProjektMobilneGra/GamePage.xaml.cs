using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjektMobilneGra.GameEngine;
using ProjektMobilneGra.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;

namespace ProjektMobilneGra
{
    public partial class GamePage : PhoneApplicationPage
    {
        private ContentManager contentManager;
        private GameTimer timer;
        private SpriteBatch spriteBatch;
        private Engine engine;

        private bool end;

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: use this.content to load your game content here
            engine = new Engine();
            engine.init(this.contentManager);
            
            //Viewport viewport, int worldWidth, int worldHeight, float initialZoom

            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            //SoundPlayer.stopMusic();
            engine.stop();



            base.OnNavigatedFrom(e);
        }


        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            if (!end)
            {
                if (!engine.isGameEnd())
                    engine.logic();
                else
                {
                    MessageBox.Show("Przegrałeś");
                    NavigationService.GoBack();
                    end = true;
                    saveScore(engine.getPlayer().getPoints());
                }
            }
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
                SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

                engine.getCamera().setPos(new Vector2(MathHelper.Lerp(engine.getCamera().Pos.X, engine.getPlayer().getPosition().X + engine.getPlayer().getVelocity().X + engine.getPlayer().getSize().X / 2, 0.1f),
                    MathHelper.Lerp(engine.getCamera().Pos.Y, engine.getPlayer().getPosition().Y + engine.getPlayer().getVelocity().Y + engine.getPlayer().getSize().Y / 2, 0.1f)));

                spriteBatch.Begin(SpriteSortMode.Deferred,
                            BlendState.AlphaBlend,
                            null,
                            null,
                            null,
                            null,
                            engine.getCamera().GetTransformation());
                
                engine.render(spriteBatch, this.contentManager);

                spriteBatch.End();
            
        }

        public void saveScore(int points)
        {
            IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();


            // open isolated storage, and write the savefile.
            IsolatedStorageFileStream fs = null;
            using (fs = savegameStorage.CreateFile("highscore"))
            {
                if (fs != null)
                {
                    // just overwrite the existing info for this example.
                    byte[] bytes = System.BitConverter.GetBytes(points);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }
}
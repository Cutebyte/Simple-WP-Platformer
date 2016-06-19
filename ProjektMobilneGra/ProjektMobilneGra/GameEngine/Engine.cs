using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Collections.Generic;
using ProjektMobilneGra.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ProjektMobilneGra.GameEngine.LevelSystem;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input.Touch; 

namespace ProjektMobilneGra.GameEngine
{
    public class Engine
    {
        private Physics physics;
        private Renderer renderer;
        private Player player;
        private Camera2D cam;
        private GUI gui;

        private List<Entity> entityList;
        private List<Collectible> collectibleList;
        private Level level;

        private SoundEffect musicLoop;
        private SoundEffectInstance musicLoopInstance;

        private bool jumpSndLock;

        private bool gameEnd;

        public Engine()
        {
            entityList = new List<Entity>();
            collectibleList = new List<Collectible>();
            physics = new Physics();
            renderer = new Renderer();
            gameEnd = false;
        }

        public Player getPlayer()
        {
            return player;
        }

        public void init(ContentManager manager)
        {
            player = new Player(manager);
            jumpSndLock = false;

            gui = new GUI(manager);

            player.setTexture(manager.Load<Texture2D>("Player/player"));
            this.addEntity((Entity)player);

            SoundPlayer.loadJump(manager);
            SoundPlayer.loadPoint(manager);

            musicLoop = manager.Load<SoundEffect>("Sounds/xedoxTheme");
            musicLoopInstance = musicLoop.CreateInstance();
            musicLoopInstance.IsLooped = true;

            cam = new Camera2D(SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport, 1000, 1000, 1.0f);
            //load level
            level = new Level(); 
            level.load(manager, level,collectibleList);
            musicLoopInstance.Play();
        }

        public void logic()
        {
            TouchCollection collection = TouchPanel.GetState();
            foreach (TouchLocation point in collection)
            {
                System.Diagnostics.Debug.WriteLine(point.Position);
                if (Math.Pow(point.Position.X - 704, 2) + Math.Pow(point.Position.Y - 382, 2) < Math.Pow(80,2))
                {
                    if (!player.isJumpLocked())
                    {
                        if (!jumpSndLock)
                            SoundPlayer.playJump();
                        player.jump();
                        jumpSndLock = true;
                    }
                }
                else
                {
                    jumpSndLock = false;
                }
                if (Math.Pow(point.Position.X - 67, 2) + Math.Pow(point.Position.Y - 391, 2) < Math.Pow(70, 2))
                {
                    player.mvLeft();
                }
                if (Math.Pow(point.Position.X - 169, 2) + Math.Pow(point.Position.Y - 391, 2) < Math.Pow(70, 2))
                {
                    player.mvRight();
                }
            }
            //Keyboard support 
            KeyboardState keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.O) && !jumpSndLock)
            {
                if (!player.isJumpLocked())
                {
                    if(!jumpSndLock)
                        SoundPlayer.playJump();
                    player.jump();
                    jumpSndLock = true;
                }
            }
            else if (keyboardState.IsKeyUp(Keys.O))
            {
                jumpSndLock = false;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                player.mvDown();
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                player.mvLeft();
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                player.mvRight();
            }
            //physics
            physics.joinLists(entityList);
        
            physics.joinBlockList(level.getBlockList(player,0));
            level.searchCollision(player);
            physics.update();

            if (player.getPosition().Y > level.getSize().Y * 32 + 64 || player.isDead())
            {
                end();
            }
        }

        public void render(SpriteBatch spriteBatch, ContentManager manager)
        {
            //renderer
            renderer.prepareList(entityList);
            renderer.prepareList(collectibleList);
            renderer.prepareList(level.getVisibleBlockList(cam));
            renderer.render(spriteBatch);
            gui.render(spriteBatch, player.getPoints(), cam);
        }

        public void addBlock(int x, int y, int type, Texture2D texture)
        {
            level.addBlock(x, y, type, texture);
        }
         

        public void addEntity(Entity entity)
        {
            entityList.Add(entity);
        }

        public Level getLevel()
        {
            return level;
        }

        public Camera2D getCamera()
        {
            return cam;
        }

        public void stop()
        {
            musicLoopInstance.Stop();
        }

        public bool isGameEnd()
        {
            return gameEnd;
        }

        public void end()
        {
            gameEnd = true;
            stop();
        }
    }
}

using System;
using System.Net;
using System.Windows;
using ProjektMobilneGra.Entities;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace ProjektMobilneGra.GameEngine.LevelSystem
{
    public class Level
    {
        private Entity[,] level;
        private int blockSize;
        private Vector2 size;

        public Level(Vector2 size, int blockSize)
        {
            this.blockSize = blockSize;
        }

        public Level()
        {
            blockSize = 32;
        }

        private void init(Vector2 lvlSize)
        {
            level = new Entity[(int)lvlSize.X, (int)lvlSize.Y];
        }

        public Vector2 getSize()
        {
            return size;
        }

        public void load(ContentManager manager, Level level, List<Collectible> list )
        {
            Texture2D levelFile = manager.Load<Texture2D>("Level/level");
            Vector2 size = new Vector2((float)levelFile.Width, (float)levelFile.Height);
            this.size = size;
            Color[] array = new Color[(int)size.X * (int)size.Y];
            init(size);
            levelFile.GetData<Color>(array);
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    if (array[(int)(i + (j * size.X))] == new Color(237, 28, 36))
                    {
                        level.addBlock(i, j, 0, manager.Load<Texture2D>("Level/inside"));
                    }
                    if (array[(int)(i + (j * size.X))] == new Color(255, 242, 0))
                    {
                        level.addBlock(i, j, 0, manager.Load<Texture2D>("Level/upper"));
                    }
                    if (array[(int)(i + (j * size.X))] == new Color(0, 0, 255))
                    {
                        level.addCollectible(i, j, manager.Load<Texture2D>("Level/collect"));
                    }
                    if (array[(int)(i + (j * size.X))] == new Color(0, 0, 0))
                    {
                        level.addTrap(i, j, manager.Load<Texture2D>("Level/trap"));
                    }
                }
            }
        }

        public void addBlock(int x, int y, int type, Texture2D texture)
        {
            level[x, y] = new Block(new Vector2(x*blockSize,y*blockSize), new Vector2(blockSize,blockSize), texture);
        }

        public void addCollectible(int x, int y, Texture2D texture)
        {
            level[x, y] = new Collectible(new Vector2(x * blockSize, y * blockSize), new Vector2(blockSize, blockSize), texture);
        }

        public void addTrap(int x, int y, Texture2D texture)
        {
            level[x, y] = new Trap(new Vector2(x * blockSize, y * blockSize), new Vector2(blockSize, blockSize), texture);
        }

        public List<Entity> getBlockList()
        {
            List<Entity> list = new List<Entity>();

            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    if(level[i,j] != null)
                        list.Add(level[i, j]);
                }
            }
            return list;
        }

        public void searchCollision(Entity collider)
        {
            int x = (int)((collider.getPosition().X) / blockSize) - 1;
            int y = (int)((collider.getPosition().Y) / blockSize) - 1;

            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x + 5 > size.X) x = (int)size.X - 5;
            if (y + 5 > size.Y) y = (int)size.Y - 5;

            for (int i = x; i < x + 5; i++)
            {
                for (int j = y; j < y + 5; j++)
                {
                    if (level[i, j] != null && level[i, j].GetType().Name.ToLower().Equals("collectible"))
                    {
                        if (collider.getRect().Intersects(level[i, j].getRect()))
                        {
                            SoundPlayer.playPoint();
                            ((Player)collider).addPoints(((Collectible)level[i, j]).getValue());
                            level[i, j] = null;
                        }
                    }
                    if (level[i, j] != null && level[i, j].GetType().Name.ToLower().Equals("trap"))
                    {
                        if (collider.getRect().Intersects(level[i, j].getRect()))
                        {
                            ((Player)collider).die();
                        }
                    }
                }
            }
        }

        public List<Entity> getBlockList(Entity collider, int type)
        {
            List<Entity> list = new List<Entity>();
            
            int x = (int)((collider.getPosition().X) / blockSize) - 1;
            int y = (int)((collider.getPosition().Y) / blockSize) - 1;

            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x + 5 > size.X) x = (int)size.X - 5;
            if (y + 5 > size.Y) y = (int)size.Y - 5;

            for (int i = x; i < x+5; i++)
            {
                for (int j = y; j < y+5; j++)
                {
                    if (type == 0)
                    {
                        if (level[i, j] != null && level[i, j].GetType().Name.ToLower().Equals("block"))
                            list.Add(level[i, j]);
                    }
                    if (type == 1)
                    {
                        if (level[i, j] != null && level[i, j].GetType().Name.ToLower().Equals("collectible"))
                            list.Add(level[i, j]);
                    }
                }
            }

            return list;
        }

        public List<Entity> getVisibleBlockList(Camera2D cam)
        {
            List<Entity> list = new List<Entity>();

            int x = (int)(cam.Pos.X / blockSize) - (int)(400 / 32) - 1;
            int y = (int)(cam.Pos.Y / blockSize) - (int)(240 / 32) - 1;

            if (x < 0) x = 0;
            if (x + (int)(800 / 32) + 2 > size.X) x = (int)(size.X) - (int)(800 / 32) - 2;
            if (y < 0) y = 0;
            if (y + (int)(480 / 32) + 2 > size.Y) y = (int)(size.Y) - (int)(480 / 32) - 2;

            for (int i = x; i < x + (int)(800 / 32) + 2; i++)
            {
                for (int j = y; j < y + (int)(480 / 32) + 2; j++)
                {
                        if (level[i, j] != null)
                        {
                            list.Add(level[i, j]);
                        }
                }
            }
            return list;
        }
    }
}

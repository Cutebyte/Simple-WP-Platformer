using System;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ProjektMobilneGra.GameEngine
{
    public class GUI
    {
        private Texture2D texture;
        private SpriteFont font;

        public GUI(ContentManager manager)
        {
            texture = manager.Load<Texture2D>("GUI/GUI");
            font = manager.Load<SpriteFont>("Font1");
        }

        public void render(SpriteBatch spriteBatch, int points, Camera2D cam)
        {
            spriteBatch.Draw(texture, new Rectangle((int)cam.Pos.X - 400, (int)cam.Pos.Y - 240, 800, 480), Color.White);
            spriteBatch.DrawString(font,points.ToString(), new Vector2((int)cam.Pos.X-300, (int)cam.Pos.Y-210), Color.White);
        }
    }
}

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
//using System.Windows.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektMobilneGra.Entities
{
    public class Collectible:Entity
    {
        private int value;
        private double x;
        

        public Collectible(Vector2 position, ContentManager manager)
        {
            value = 1;
            this.position = position;
            texture = manager.Load<Texture2D>("Level/collect");
            size = new Vector2(32, 32);
            
        }

        public Collectible(Vector2 position, Vector2 size, Texture2D texture)
        {
            value = 1;
            this.position = position;
            this.texture = texture;
            this.size = size;
        }

        public int getValue()
        {
            return value;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            //System.Diagnostics.Debug.WriteLine(x* Math.PI);
            rectangle = new Rectangle((int)this.getPosition().X, (int)this.getPosition().Y + (int)(10*(float)Math.Sin(x)), (int)this.getSize().X, (int)this.getSize().Y);
            x += 0.2;
            if (x >= 2*Math.PI)
                x = 0;

            if (facing == 0)
                spriteBatch.Draw(getTexture(), rectangle, color);
            if (facing > 0)
                spriteBatch.Draw(getTexture(), rectangle, null, color, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
            if (facing < 0)
                spriteBatch.Draw(getTexture(), rectangle, null, color, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.0f);
        }
    }
}

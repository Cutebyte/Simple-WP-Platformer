using System;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektMobilneGra.Entities
{
    public class Block:Entity
    {
        public Block(Vector2 position, Vector2 size, Texture2D texture)
        {
            this.position = position;
            this.size = size;
            rectangle = new Rectangle((int)this.getPosition().X, (int)this.getPosition().Y, (int)this.getSize().X, (int)this.getSize().Y);
            this.texture = texture;
            color = Color.White;
            facing = 0;
        }
    }
}

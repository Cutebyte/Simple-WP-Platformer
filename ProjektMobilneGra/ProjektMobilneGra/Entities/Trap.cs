using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektMobilneGra.Entities
{
    public class Trap:Entity
    {
        public Trap(Vector2 position, Vector2 size, Texture2D texture)
        {
            this.position = position;
            this.size = size;
            rectangle = new Rectangle((int)this.getPosition().X, (int)this.getPosition().Y, (int)this.getSize().X, (int)this.getSize().Y);
            color = Color.White;
            facing = 0;
            this.texture = texture;
        }
    }
}

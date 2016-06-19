using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjektMobilneGra.Entities
{
    public class Entity
    {
        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 size;
        protected Texture2D texture;

        protected Rectangle rectangle;

        protected Color color;

        protected bool jumpLocked;
        protected bool accelerateLocked;

        protected int facing;

        public Entity()
        {
            color = Color.White;
        }

        public Entity(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
            rectangle = new Rectangle((int)this.getPosition().X, (int)this.getPosition().Y, (int)this.getSize().X, (int)this.getSize().Y);
            color = Color.White;
            facing = 0;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public void updatePosition()
        {
            if (Math.Abs(velocity.X) < 0.3)
                velocity.X = 0;
            position += velocity;
            if(!accelerateLocked)
                velocity.X *= 0.7f;
            accelerateLocked = false;
            if (velocity.X == 0)
                facing = 0;
        }

        public void setFacing(int facing)
        {
            this.facing = facing;
        }

        public void lockAcceleration()
        {
            accelerateLocked = true;
        }

        public void setForce(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        public void speedup(float velocity, float limit)
        {
            accelerateLocked = true;
            if (Math.Abs(this.velocity.X) < limit || Math.Sign(this.velocity.X) != Math.Sign(velocity))
                this.velocity.X += velocity;
        }

        public void applyForce(Vector2 velocity)
        {
            this.velocity += velocity;
        }

        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public Vector2 getSize()
        {
            return size;
        }

        public Vector2 getVelocity()
        {
            return velocity;
        }

        public Texture2D getTexture()
        {
            return texture;
        }

        public bool isJumpLocked()
        {
            return jumpLocked;
        }

        public void jumpLock()
        {
            jumpLocked = true;
        }

        public void unlockJump()
        {
            jumpLocked = false;
        }

        public Rectangle getRect()
        {
            return rectangle;
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            rectangle = new Rectangle((int)this.getPosition().X, (int)this.getPosition().Y, (int)this.getSize().X, (int)this.getSize().Y);
            if (facing == 0)
                spriteBatch.Draw(getTexture(), rectangle, color);
            if (facing > 0)
                spriteBatch.Draw(getTexture(), rectangle, null, color, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
            if (facing < 0)
                spriteBatch.Draw(getTexture(), rectangle, null, color, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.0f);
        }
    }
}

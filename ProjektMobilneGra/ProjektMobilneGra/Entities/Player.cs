using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjektMobilneGra.Entities
{
    public class Player:Entity
    {
        private float speed;
        private float jumpForce;
        private int points;
        private Texture2D jumpTexture;
        private Texture2D jumpCtTexture;
        private Texture2D standingTexture;
        private Texture2D runTexture;
        private bool dead;

        public Player(ContentManager manager)
        {
            position = new Vector2(80, 10); // test
            velocity = new Vector2(0, 0);
            size = new Vector2(64, 64);
            jumpForce = 1;
            speed = 10;
            rectangle = new Rectangle(0, 0, (int)this.getSize().X, (int)this.getSize().Y);
            points = 0;
            facing = 0;

            jumpTexture = manager.Load<Texture2D>("player/playerJump");
            standingTexture = manager.Load<Texture2D>("player/playerSt");
            jumpCtTexture = manager.Load<Texture2D>("player/playerJumpCt");
            runTexture = manager.Load<Texture2D>("player/playerRun");
            dead = false;
        }

        public void addPoints(int points)
        {
            this.points += points;
        }

        public int getPoints()
        {
            return points;
        }

        public void setSpeed(float speed)
        {
            this.speed = speed;
        }

        public void jump()
        {
            setForce(new Vector2(velocity.X, -12*jumpForce));
            jumpLock();
        }

        public void mvDown()
        {
            setForce(new Vector2(velocity.X, -speed));
        }

        public void mvRight()
        {
            speedup(1,speed);
            setFacing(1);
        }

        public void mvLeft()
        {
            speedup(-1, speed);
            setFacing(-1);
        }
        
        public Rectangle getRunAnimation(int x)
        {
            Rectangle result = new Rectangle((int)(size.X * Math.Abs(x)), 0, (int)size.X, (int)size.Y);
            return result;
        }
        
        public Texture2D getJumpTexture()
        {
            return jumpTexture;
        }

        public Texture2D getRunTexture()
        {
            return runTexture;
        }

        public Texture2D getStandingTexture()
        {
            return standingTexture;
        }

        public Texture2D getJumpCtTexture()
        {
            return jumpCtTexture;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            rectangle = new Rectangle((int)this.getPosition().X, (int)this.getPosition().Y, (int)this.getSize().X, (int)this.getSize().Y);
            if (isJumpLocked())
            {
                if (facing > 0)
                    spriteBatch.Draw(getJumpTexture(), rectangle, null, color, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.0f);
                if (facing < 0)
                    spriteBatch.Draw(getJumpTexture(), rectangle, null, color, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
                if (facing == 0)
                    spriteBatch.Draw(getJumpCtTexture(), rectangle, null, color, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.0f);
            }
            else
            {
                if (facing < 0)
                        spriteBatch.Draw(getRunTexture(), rectangle, getRunAnimation((int)(position.X/32)%3), color, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
                if (facing > 0)
                {
                    spriteBatch.Draw(getRunTexture(), rectangle, getRunAnimation((int)(position.X / 32) % 3), color, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.0f);
                }
                if (facing == 0)
                    spriteBatch.Draw(getStandingTexture(), rectangle, null, color, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.0f);
            }

        }

        public void die()
        {
            dead = true;
        }

        public bool isDead()
        {
            return dead;
        }
    }
}

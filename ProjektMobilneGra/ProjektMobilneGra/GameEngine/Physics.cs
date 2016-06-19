using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using ProjektMobilneGra.Entities;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ProjektMobilneGra.GameEngine
{
    public class Physics
    {
        List<Entity> physList;
        List<Entity> blockList; //temp

        public Physics()
        {
            //constructor
            physList = new List<Entity>();
            blockList = new List<Entity>();
        }

        public void update()
        {
            // detecting collisions | applying gravity
            foreach(Entity entity in physList)
            {
                applyGravity(entity);
                entity.jumpLock();
                detectCollisionsWithWorld(entity, blockList);
                entity.updatePosition();
            }
            physList.Clear();
            blockList.Clear();
        }

        public void detectCollisionsWithWorld(Entity entity, List<Entity> list)
        {
            foreach (Entity ent in list)
            {
                dealWithCollisions(entity, ent);
            }
        }

        public static Boolean areColliding(Entity ent1, Entity ent2)
        {
            if (ent1.getRect().Intersects(ent2.getRect()))
                return true;
            else
                return false;
        }

        public void dealWithCollisions(Entity entity, Entity block)
        {
            /* theory
             * ent.pos.x+ent.vel.x+ent.siz.x<block.pos.x
             * 
             * 
             */
            int pos = -1;
            if(    entity.getPosition().X + entity.getVelocity().X + entity.getSize().X > block.getPosition().X  //lewo
                && entity.getPosition().X + entity.getVelocity().X < block.getPosition().X + block.getSize().X  //prawo
                && entity.getPosition().Y + entity.getVelocity().Y < block.getPosition().Y + block.getSize().Y //down
                && entity.getPosition().Y + entity.getVelocity().Y + entity.getSize().Y > block.getPosition().Y //up
                )
            {
                pos = detPos(entity, block);
                if (pos == 0)
                {
                    entity.setPosition(new Vector2(block.getPosition().X - entity.getSize().X, entity.getPosition().Y));
                    entity.setForce(new Vector2(0, entity.getVelocity().Y));
                    Debug.WriteLine("lewo");
                }
                if (pos == 1)
                {
                    entity.setPosition(new Vector2(block.getPosition().X + block.getSize().X, entity.getPosition().Y));
                    entity.setForce(new Vector2(0, entity.getVelocity().Y));
                    Debug.WriteLine("prawo");
                }
                if (pos == 2)
                {
                    entity.setPosition(new Vector2(entity.getPosition().X, block.getPosition().Y + block.getSize().Y));
                    entity.setForce(new Vector2(entity.getVelocity().X, 0));
                }
                if (pos == 3) //from up - STANDING
                {
                    entity.setPosition(new Vector2(entity.getPosition().X, block.getPosition().Y - entity.getSize().Y));
                    entity.setForce(new Vector2(entity.getVelocity().X, 0));
                    if (entity.isJumpLocked() && entity.getVelocity().Y == 0)
                    {
                        entity.unlockJump();
                    }
                }
            }
        }

        public int detPos(Entity entity, Entity dstEntity)
        {
            if (entity.getPosition().X + entity.getSize().X < dstEntity.getPosition().X + 1
                && entity.getPosition().Y + entity.getSize().Y > dstEntity.getPosition().Y
                && entity.getPosition().Y < dstEntity.getPosition().Y + dstEntity.getSize().Y)
            {
                return 0; // 0 -> left
            }
            if (entity.getPosition().X > dstEntity.getPosition().X + dstEntity.getSize().X - 1 
                && entity.getPosition().Y + entity.getSize().Y > dstEntity.getPosition().Y
                && entity.getPosition().Y < dstEntity.getPosition().Y + dstEntity.getSize().Y)
            {
                return 1; // 1 -> right
            }
            if (entity.getPosition().Y + entity.getSize().Y > dstEntity.getPosition().Y + dstEntity.getSize().Y
                && entity.getPosition().X + entity.getSize().X > dstEntity.getPosition().X
                && entity.getPosition().X < dstEntity.getPosition().X + dstEntity.getSize().X)
            {
                return 2; // 2 -> up
            }
            if (entity.getPosition().Y < dstEntity.getPosition().Y + dstEntity.getSize().Y )
            {
                return 3; // 3 -> down
            }

            return -1; //-1 nothing just in case
        }

        public void addToBlockList(Entity entity)
        {
            blockList.Add(entity);
        }

        public void joinBlockList(List<Entity> list)
        {
            blockList.AddRange(list);
        }

        public void addToList(Entity entity)
        {
            physList.Add(entity);
        }

        public void joinLists(List<Entity> list)
        {
            physList.AddRange(list);
        }

        public void applyGravity(Entity entity)
        {
            if (entity.getVelocity().Y < 10)
            {
                entity.applyForce(new Microsoft.Xna.Framework.Vector2(0, 0.8f));
            }
        }
    }
}

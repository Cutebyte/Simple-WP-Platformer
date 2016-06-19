using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using ProjektMobilneGra.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektMobilneGra.GameEngine
{
    public class Renderer
    {
        List<Entity> renderList;

        public Renderer()
        {
            renderList = new List<Entity>();
        }

        public void render(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in renderList)
            {
                entity.draw(spriteBatch);
            }
            renderList.Clear();
        }

        public void prepareList(List<Entity> list)
        {
            //render only those objects that are visible

            //STUB - render everything - TODO: dont render hidden entities functionality - DONE > physics, level
            //foreach (Entity entity in renderList)
            //{
                //renderList = list;
            renderList.AddRange(list);
            //}
        }

        public void prepareList(List<Collectible> list)
        {
            //renderList.AddRange(list);
            foreach (Collectible coll in list)
            {
                addToList((Entity)coll);
            }
        }

        public void addToList(Entity entity)
        {
            //Add to list anyway
            //for example explosions, bullets and every temporary entity
            renderList.Add(entity);
        }
    }
}

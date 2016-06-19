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

namespace ProjektMobilneGra.GameEngine
{
    public class Camera2D
    {
        private const float zoomUpperLimit = 1.5f;
   private const float zoomLowerLimit = .5f;

   private float zoom;
   private Matrix transform;
   private Vector2 position;
   private float rotation;
   private int viewportWidth;
   private int viewportHeight;
   private int worldWidth;
   private int worldHeight;        

   public Camera2D(Viewport viewport, int worldWidth, 
      int worldHeight, float initialZoom)
   {
      zoom = initialZoom;
      rotation = 0.0f;
      position = Vector2.Zero;
      viewportWidth = viewport.Width;
      viewportHeight = viewport.Height;
      this.worldWidth = worldWidth;
      this.worldHeight = worldHeight;
   }

   #region Properties

   public float Zoom
   {
       get { return zoom; }
       set
       {
           zoom = value;
           if (zoom < zoomLowerLimit)
              zoom = zoomLowerLimit;
           if (zoom > zoomUpperLimit)
              zoom = zoomUpperLimit;
       }
   }

   public float Rotation
   {
       get { return rotation; }
       set { rotation = value; }
   }

   public void setPos(Vector2 position)
   {
       this.position = position;
   }

   public void Move(Vector2 amount)
   {
       position += amount;
   }

   public Vector2 Pos
   {
       get { return position; }
       set
       {
           float leftBarrier = (float)viewportWidth *
                  .5f / zoom;
           float rightBarrier = worldWidth -
                  (float)viewportWidth * .5f / zoom;
           float topBarrier = worldHeight -
                  (float)viewportHeight * .5f / zoom;
           float bottomBarrier = (float)viewportHeight *
                  .5f / zoom;
           position = value;
           if (position.X < leftBarrier)
               position.X = leftBarrier;
           if (position.X > rightBarrier)
               position.X = rightBarrier;
           if (position.Y > topBarrier)
               position.Y = topBarrier;
           if (position.Y < bottomBarrier)
               position.Y = bottomBarrier;
        }
   }

   #endregion

   public Matrix GetTransformation()
   {
     transform = 
        Matrix.CreateTranslation(new Vector3(-(int)position.X, -(int)position.Y, 0)) *
        Matrix.CreateRotationZ(Rotation) *
        Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
        Matrix.CreateTranslation(new Vector3(viewportWidth * 0.5f,
            viewportHeight * 0.5f, 0));

       return transform;
   }
    }
}

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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace ProjektMobilneGra.GameEngine
{
    public class SoundPlayer
    {
        static SoundEffect jumpSound;
        static SoundEffect collectSound;
        
        public static void loadPoint(ContentManager manager)
        {
            collectSound = manager.Load<SoundEffect>("Sounds/point");
        }
        
        public static void loadJump(ContentManager manager)
        {
            jumpSound = manager.Load<SoundEffect>("Sounds/jump");
        }

        public static void playJump()
        {
            jumpSound.Play();
        }

        public static void playPoint()
        {
            collectSound.Play();
        }
    }
}

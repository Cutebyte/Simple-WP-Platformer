using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using ProjektMobilneGra.GameEngine;
using Microsoft.Xna.Framework.Content;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;

namespace ProjektMobilneGra
{
    public partial class MainPage : PhoneApplicationPage
    {

        private SoundEffect menuLoop;
        private SoundEffectInstance menuLoopInstance;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            init();
            //SoundPlayer.playMenu();
        }

        public void init()
        {
            ContentManager contentManager = (Application.Current as App).Content;
            //SoundPlayer.loadMenu(contentManager);
            menuLoop = contentManager.Load<SoundEffect>("Sounds/xedoxMenu");
            menuLoopInstance = menuLoop.CreateInstance();
            menuLoopInstance.IsLooped = true;

            loadHighscore();
        }

        // Simple button Click event handler to take us to the second page
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Info.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            //SoundPlayer.stopMenu();
            menuLoopInstance.Stop();
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            menuLoopInstance.Play();
            loadHighscore();
            base.OnNavigatedTo(e);
        }

        private void loadHighscore()
        {
            using (IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (savegameStorage.FileExists("highscore"))
                {
                    using (IsolatedStorageFileStream fs = savegameStorage.OpenFile("highscore", System.IO.FileMode.Open))
                    {
                        if (fs != null)
                        {
                            // Reload the saved high-score data.
                            byte[] saveBytes = new byte[4];
                            int count = fs.Read(saveBytes, 0, 4);
                            if (count > 0)
                            {
                                if(int.Parse(highscore.Text) < System.BitConverter.ToInt32(saveBytes, 0))
                                    highscore.Text = System.BitConverter.ToInt32(saveBytes, 0).ToString();
                            }
                        }
                    }
                }
            }
        }
    }
}
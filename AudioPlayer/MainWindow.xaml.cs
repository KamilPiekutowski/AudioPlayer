using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Controls.Primitives;

namespace AudioPlayer
{
    // Audio Player App
    public partial class MainWindow : Window
    {
        Dictionary<string, string> playListCollection; //stores filenames and their corresponding paths
        private string mediaName = "media"; //alias used in MCI API
        public bool isPlaying; // Player state variable
        bool isMuted; // Volume State variable

        [DllImport("winmm.dll")] // Using winmm API
        static extern int mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Audio Player";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            isPlaying = false;
            isMuted = false;

            playListCollection = new Dictionary<string, string>();

            Thread thread = new Thread(Run);
            thread.Start();
        }

        // Allows to select and load WAV files.
        private void LoadFiles_Click(object sender, RoutedEventArgs e)
        {
            // Select files to load.
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = true;
            playListCollection.Clear();
            openFileDialog.Filter = "WAV files (*.wav)|*.wav|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                Playlist.Items.Clear();
                int idx = 1; //file indexd on the list

                foreach (string filename in openFileDialog.FileNames)
                {
                    Playlist.Items.Add(System.IO.Path.GetFileName(filename));
                    playListCollection[System.IO.Path.GetFileName(filename)] = filename; //populating dictionary with the filenames and their paths
                    idx++;
                }

                // Always first item in the ListView is selected
                Playlist.SelectedIndex = 0;

                // Loading first song from hte list
                int ret = -1;
                string playCommand;

                playCommand = "Close " + mediaName;
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
                var filePath = playListCollection.First().Value;
                playCommand = "Open \"" + filePath + "\" type mpegvideo alias " + mediaName;
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);

                isPlaying = false;
            }

            
        }

        // Enebles dragging the player accross the desktop.
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // TODO: This will be used when implementing the jump of the progress slider to a location
            //       of the mouse click. 
        }

        // When dragging this slider completes, the new postion of the song is set.
        private void ProgressSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            int ret = -1;
            int length = 0;
            string playCommand;

            StringBuilder buff = new StringBuilder(32);

            ret = mciSendString("status " + mediaName + " length", buff, buff.Capacity, IntPtr.Zero);
            length = int.Parse(buff.ToString());

            long millisecs = (long)((ProgressSlider.Value / 100) * (double)length);

            if (isPlaying)
            {
                playCommand = string.Format("Play " + mediaName + " from {0}", millisecs);
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
            }
            else
            {
                playCommand = string.Format("Seek " + mediaName + " to {0}", millisecs);
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
            }
        }

        // Playes previous song.
        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            Int32 selectedIndex = Playlist.SelectedIndex;
            Int32 playlistCount = Playlist.Items.Count - 1;

            selectedIndex--;

            if (selectedIndex < 0)
            {
                selectedIndex = 0;
            }

            PrevNextHelper(selectedIndex);
        }

        // Plays or pauses the song.
        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            int ret = -1;
            string playCommand;
            // Opening first file from the list.

            if (!isPlaying && playListCollection.Count != 0)
            {
                Image img = (Image) PlayPause.Content;
                img.Source = new BitmapImage(new Uri("PauseButton.png", UriKind.RelativeOrAbsolute));
                PlayPause.Content = img;
                
                playCommand = "Play " + mediaName + " notify";
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
            }
            else
            {
                Image img = (Image) PlayPause.Content;
                img.Source = new BitmapImage(new Uri("PlayButton.png", UriKind.RelativeOrAbsolute));
                PlayPause.Content = img;

                playCommand = "Pause " + mediaName + " notify";
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
            }

            isPlaying = !isPlaying;

        }

        // Plays next song.
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Int32 selectedIndex = Playlist.SelectedIndex;
            Int32 playlistCount = Playlist.Items.Count-1;

            selectedIndex++;

            if(selectedIndex > playlistCount)
            {
                selectedIndex = playlistCount;
            }

            PrevNextHelper(selectedIndex);
        }

        // Encapsulates common code from Prev_Click() and Next_Click() events.
        private void PrevNextHelper(Int32 selectedIndex) 
        {
            if (playListCollection.Count != 0)
            {
                Playlist.SelectedIndex = selectedIndex; // new selected index

                String fileName = Playlist.Items[selectedIndex].ToString();

                String filePath = playListCollection[fileName];

                int ret = -1;
                string playCommand;

                playCommand = "Close " + mediaName;
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
                playCommand = "Open \"" + filePath + "\" type mpegvideo alias " + mediaName;
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);

                if (isPlaying)
                {
                    ProgressSlider.Value = 0;
                    playCommand = "Play " + mediaName + " notify";
                    ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
                }
            }
        }

        // A thread function that runs every second and updates the progress slider.
        // Progress slider does not get updated when song is not playing.
        private void Run()
        {
            while (true) // Infinate loop in another thread.
            {
                if (isPlaying && playListCollection.Count != 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        int ret = -1;
                        int length = 0;
                        int position = 0;
                        StringBuilder buff = new StringBuilder(32);

                        ret = mciSendString("Status " + mediaName + " length", buff, buff.Capacity, IntPtr.Zero);

                        try
                        {
                            length = int.Parse(buff.ToString());
                        }
                        catch (InvalidCastException e)
                        {
                            //quick fix, if there is nothing in the buffer then length stays 0
                        }

                        ret = mciSendString("status " + mediaName + " position", buff, buff.Capacity, IntPtr.Zero);

                        try
                        {
                            position = int.Parse(buff.ToString());
                        }
                        catch (InvalidCastException e)
                        {
                            //quick fix, if there is nothing in the buffer then position stays 0
                        }

                        ProgressSlider.Value = ((float) position / (float) length) * 100;
                    });
                }

                Thread.Sleep(1000);
            }

        }

        // Mutes and umutes the player
        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            int ret = -1;
            string playCommand;
            string audioState = "on";

            if(!isMuted)
            {

                Image img = (Image) VolumeButton.Content;
                img.Source = new BitmapImage(new Uri("MuteButton.png", UriKind.RelativeOrAbsolute));
                VolumeButton.Content = img;
           
                audioState = "off";
            }
            else
            {
                Image img = (Image) VolumeButton.Content;
                img.Source = new BitmapImage(new Uri("VolumeButton.png", UriKind.RelativeOrAbsolute));
                VolumeButton.Content = img;
            }

            playCommand = string.Format("setaudio " + mediaName + " right {0}", audioState);
            ret = mciSendString(playCommand, null, 0, IntPtr.Zero);

            playCommand = string.Format("setaudio " + mediaName + " left {0}", audioState);
            ret = mciSendString(playCommand, null, 0, IntPtr.Zero);

            isMuted = !isMuted;
        }

        // Changes the volume of the player.
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int ret = -1;
            int volumeLevel;

            StringBuilder buff = new StringBuilder(32);

            volumeLevel = (int) ((VolumeSlider.Value / (double)100) * (double) 1000);

            ret = mciSendString("Status " + mediaName + " left volume", buff, buff.Capacity, IntPtr.Zero);

            string playCommand;

            playCommand = string.Format("setaudio " + mediaName + " left volume to {0}", volumeLevel);
            ret = mciSendString(playCommand, null, 0, IntPtr.Zero);

            playCommand = string.Format("setaudio " + mediaName + " right volume to {0}", volumeLevel);
            ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
        }

        // Shuts down the app.
        private void Poweroff_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Playlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Threading;
using ListViewItem = System.Windows.Controls.ListViewItem;
using System.Windows.Controls.Primitives;

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, string> playListCollection; //stores filenames and their corresponding paths
        private string mediaName = "media"; //alias used in MCI API
        bool isPlaying; // Player state variable
        bool isMuted; // Volume State variable
        bool isUserTriggered; // defines if event is cause by the user

        [DllImport("winmm.dll")]
        static extern int mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Audio Player";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            bool isPlaying = false;
            bool isMuted = false;
            bool isUserTriggered = false;

            Thread thread = new Thread(Run);
            thread.Start();
        }

        void OnLoadPlayList(object sender, RoutedEventArgs e)
        {
            playListCollection = new Dictionary<string, string>();

            for (int i = 1; i < 22; i++)
            {
                TextBlock textBlock = new TextBlock();
                Playlist.Items.Add(textBlock);
            }
        }

        private void LoadFiles_Click(object sender, RoutedEventArgs e)
        {
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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

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

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            int ret = -1;
            string playCommand;
            // Opening first file from the list.

            if (!isPlaying && playListCollection.Count != 0)
            {
                playCommand = "Play " + mediaName + " notify";
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
            }
            else
            {
                playCommand = "Pause " + mediaName + " notify";
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
            }

            isPlaying = !isPlaying;

        }

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

        private void PrevNextHelper(Int32 selectedIndex) //it encapsulates common code from Prev_Click and Next_Click
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
                playCommand = "Play " + mediaName + " notify";
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
            }
        }

        private void Run()
        {
            while (true)
            {


                if (isPlaying)
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
                            //quick fix, if there is nothing in the buffer then value is 0
                        }

                        ret = mciSendString("status " + mediaName + " position", buff, buff.Capacity, IntPtr.Zero);

                        try
                        {
                            position = int.Parse(buff.ToString());
                        }
                        catch (InvalidCastException e)
                        {
                            //quick fix, if there is nothing in the buffer then value is 0
                        }

                        ProgressSlider.Value = ((float) position / (float) length) * 100;
                    });
                }

                Thread.Sleep(1000);
            }

        }

        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            int ret = -1;
            string playCommand;
            string audioState = "on";

            if(!isMuted)
            {
                audioState = "off";
            }

            playCommand = string.Format("setaudio " + mediaName + " right {0}", audioState);
            ret = mciSendString(playCommand, null, 0, IntPtr.Zero);

            playCommand = string.Format("setaudio " + mediaName + " left {0}", audioState);
            ret = mciSendString(playCommand, null, 0, IntPtr.Zero);

            isMuted = !isMuted;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int ret = -1;
            int length = 0;
            int position = 0;
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

        private void Playlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

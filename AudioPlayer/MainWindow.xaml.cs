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
        Dictionary<String, String> playListCollection; //stores filenames and their corresponding paths
        private string mediaName = "media"; //alias used in MCI API
        bool isPlaying; //Player state variable
        bool isUserTriggered; // defines if event is cause by the user

        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Audio Player";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            bool isPlaying = false;
            bool isUserTriggered = false;

            Thread thread = new Thread(test);
            thread.Start();
        }

        void OnLoadPlayList(object sender, RoutedEventArgs e)
        {
            playListCollection = new Dictionary<String, String>();

            for (int i = 1; i < 22; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = (i.ToString() + ".");
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
                    Playlist.Items.Add(idx.ToString() + "." + System.IO.Path.GetFileName(filename));
                    playListCollection[System.IO.Path.GetFileName(filename)] = filename; //populating dictionary with the filenames and their paths
                    idx++;
                }     
            }

            Playlist.SelectedIndex = 0;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ProgressSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            Int32 ret = -1;
            Int32 length = 0;

            StringBuilder buff = new StringBuilder(32);

            ret = mciSendString("status " + mediaName + " length", buff, buff.Capacity, IntPtr.Zero);
            length = Int32.Parse(buff.ToString());

            long millisecs = (long)((ProgressSlider.Value / 100) * (double)length);
            String playCommand = String.Format("Play " + mediaName + " from {0}", millisecs);
            long status = mciSendString(playCommand, null, 0, IntPtr.Zero);
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            Int32 ret = -1;
            String playCommand;
            // Opening first file from the list.

            if (!isPlaying && playListCollection.Count != 0)
            {
                playCommand = "Close " + mediaName;
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
                var filepath = playListCollection.First().Value;
                playCommand = "Open \"" + filepath + "\" type waveaudio alias " + mediaName;
                ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
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

            
        }

        private void test()
        {
            while (true)
            {


                if (isPlaying)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Int32 ret = -1;
                        Int32 length = 0;
                        Int32 position = 0;
                        StringBuilder buff = new StringBuilder(32);

                        ret = mciSendString("status " + mediaName + " length", buff, buff.Capacity, IntPtr.Zero);

                        try
                        {
                            length = Int32.Parse(buff.ToString());
                        }
                        catch (InvalidCastException e)
                        {
                            //quick fix, if there is nothing in the buffer then value is 0
                        }

                        ret = mciSendString("status " + mediaName + " position", buff, buff.Capacity, IntPtr.Zero);

                        try
                        {
                            position = Int32.Parse(buff.ToString());
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

        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Playlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

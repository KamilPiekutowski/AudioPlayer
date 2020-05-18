using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double PlaylistWidth;
        public double PlaylistHeight {get;set;}

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Audio Player";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void LoadFiles_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

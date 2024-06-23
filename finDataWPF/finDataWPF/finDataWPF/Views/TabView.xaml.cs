using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace finDataWPF.Views
{
    /// <summary>
    /// Interaction logic for TabView.xaml
    /// </summary>
    public partial class TabView : UserControl
    {
        private bool _isPlaying = false;
        public TabView()
        {
            InitializeComponent();
        }
        private void MediaElement_Loaded(object sender, RoutedEventArgs e)
        {
            var mediaElement = sender as MediaElement;
            if (mediaElement != null && mediaElement.Source != null)
            {
                mediaElement.Play();
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            var mediaElement = sender as MediaElement;
            if (mediaElement != null)
            {
                mediaElement.Position = TimeSpan.Zero;
                mediaElement.Pause();
                _isPlaying = true;
            }
        }
        private void MediaElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is MediaElement mediaElement)
            {
                if (_isPlaying)
                {
                    mediaElement.Pause();
                }
                else
                {
                    mediaElement.Play();
                }
                _isPlaying = !_isPlaying;
            }
        }
    }
}

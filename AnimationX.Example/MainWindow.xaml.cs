using System;
using System.Windows;
using System.Windows.Shapes;
using AnimationX.Class.Model.Animations;
using AnimationX.Class.Model.EasingFunctions;

namespace AnimationX.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var ani = new ThicknessAnimation
            {
                AnimateObject = Rect,
                AnimateProperty = Rectangle.MarginProperty,
                From = new Thickness(0),
                To = new Thickness(400, 0, 0, 0),
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                EasingFunction = new BounceEase()
            };

            ani.Begin();
        }
    }
}

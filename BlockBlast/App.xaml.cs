
using Microsoft.Maui.Controls;

namespace BlockBlast
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(
            IActivationState? activationState)
        {
            return new Window(
                new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.FromArgb("#10182E"),
                    BarTextColor = Colors.White
                });
        }
    }
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListViewChat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var tabs = new TabbedPage();
            MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

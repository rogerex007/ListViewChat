using ListViewChat.Bass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListViewChat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Config : ContentPage
    {
        public Config()
        {
            InitializeComponent();
        }

        private async void btnSetIp_Onclicked(object sender, EventArgs e)
        {
            Room.IPs = etIP.Text;
            App.Current.MainPage = new NavigationPage(new Login());
        }
    }
}
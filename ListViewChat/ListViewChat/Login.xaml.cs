using ListViewChat.Bass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListViewChat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem
            {
                Name = ":",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(() => Navigation.PushAsync(new Config()))
            });
        }

        private async void btnLogin_Onclicked(object sender, EventArgs e)
        {
            if (etUser.Text != null && etPass.Text != null)
            {
                var client = new HttpClient();
                var uri = new Uri(string.Format("http://" + Room.IPs + "/Chat/checkuserLogin/" + etUser.Text + "&" + etPass.Text));
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody != "[]")
                {
                    App.Current.MainPage = new NavigationPage(new Member(etUser.Text));
                }
                else
                {
                    await DisplayAlert("Alerta", "Usuario y/o Contraseña incorrecto", "Entiendo");
                }
            }
            else
            {
                await DisplayAlert("Alerta", "Debes ingresar datos en los campos..", "Entiendo");
            }
        }

        private async void btnRegister_Onclicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registration());
        }
    }

  
}
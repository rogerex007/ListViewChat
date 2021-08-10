using ListViewChat.Bass;
using Newtonsoft.Json;
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
    public partial class Registration : ContentPage
    {
        public Registration()
        {
            InitializeComponent();
        }

        private async void btnCreateAccount_OnClicked(object sender, EventArgs e)
        {
            if(etUser.Text != null && etPass.Text != null && etName.Text != null && etEmail.Text != null && pickerAge.SelectedIndex != -1)
            {
                var clientC = new HttpClient();
                var uriC = new Uri(string.Format("http://" + Room.IPs + "/Chat/checkuserRegister/" + etUser.Text));
                var responseC = await clientC.GetAsync(uriC);
                responseC.EnsureSuccessStatusCode();
                string responseBodyC = await responseC.Content.ReadAsStringAsync();
                if(responseBodyC == "[]")
                {
                    Random r = new Random();
                    string picc = "http://"+ "192.168.0.6" + "/userPick/" + r.Next(1, 50).ToString() + ".png";
                    users uss = new users();
                    uss.userID = etUser.Text;
                    uss.password = etPass.Text;
                    uss.name = etName.Text;
                    uss.email = etEmail.Text;
                    uss.image = picc;
                    uss.sex = pickerAge.Items[pickerAge.SelectedIndex];
                    uss.RequestF = new List<RequestF>
                    {

                    };

                    uss.friend = new List<Friend>
                    {

                    };

                    using (var client = new HttpClient())
                    {
                        var response = client.PostAsync("http://" + Room.IPs + "/Chat/adduser",
                            new StringContent(JsonConvert.SerializeObject(uss).ToString(),
                            Encoding.UTF8, "application/json"))
                            .Result;

                        if (response.IsSuccessStatusCode)
                        {
                            dynamic content = JsonConvert.DeserializeObject(
                                response.Content.ReadAsStringAsync()
                                .Result);
                            await DisplayAlert("Alerta", "Success !!", "Entiendo");
                            App.Current.MainPage = new NavigationPage(new Login());
                        }
                    }

                }
                else
                {
                    await DisplayAlert("Alert", "Usuario ya existe", "Entiendo");
                    etUser.Text = null;
                }
            }
            else
            {
                await DisplayAlert("Alerta", "Debes ingresar datos en los campos..", "Entiendo");
            }

        }
    }
}
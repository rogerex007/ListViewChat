using ListViewChat.Bass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class SearchFriend : ContentPage
    {
        public string _param { get; set; }
        Profile p = new Profile();
        public SearchFriend(string paramm, Profile pp)
        {
            InitializeComponent();
            p = pp;
            _param = paramm;
            GetFindFriend();

        }
        HttpClient client;
        HttpResponseMessage response;
        string[] find = { "", "", "" };

        async void GetFindFriend()
        {
            client = new HttpClient();
            var uri = new Uri(string.Format("http://" + Room.IPs + "/Chat/findaddfriend/" + _param));
            response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            if(responseBody != "[]")
            {
                dynamic blogPosts = JArray.Parse(responseBody);
                dynamic blogPost = blogPosts[0];
                find[0] = blogPost.userID;
                find[1] = blogPost.name;
                find[2] = blogPost.image;
            }
            else
            {
                lbuser.Text = "Desconocido";
            }
            lbuser.Text = find[1];
            imgPic.Source = find[2];
        }

        private async void btnAdd_Onclicked(object sender, EventArgs e)
        {
            client = new HttpClient();
            var uri = new Uri(string.Format("http://" + Room.IPs + "/Chat/findRequestF/" + _param + "&" + p.userID));
            response = await client.GetAsync(uri);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (responseBody.Length > 37)
            {
                await DisplayAlert("Alerta", "Upss a ocurrido un error", "Entiendo");
            }
            else
            {
                RequestF rf = new RequestF();
                rf.RequestID = p.userID;
                rf.name = p.name;
                rf.imageR = p.image;

                using (var clientq = new HttpClient())
                {
                    var responseq = clientq.PutAsync("http://" + Room.IPs + "/Chat/addrequestF/" + find[0],
                        new StringContent(JsonConvert.SerializeObject(rf).ToString(),
                        Encoding.UTF8, "application/json"))
                        .Result;
                    if (responseq.IsSuccessStatusCode)
                    {
                        dynamic content = JsonConvert.DeserializeObject(
                            responseq.Content.ReadAsStringAsync()
                            .Result);
                        await DisplayAlert("Alerta", "Solicitud enviada", "Entiendo");
                        App.Current.MainPage = new NavigationPage(new Member(p.userID));
                    }
                }
            }

        }
    }
}
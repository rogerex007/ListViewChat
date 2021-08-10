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
    public partial class Member : ContentPage
    {
        public string _param { get; set; }
        public Member(string paramU)
        {
            InitializeComponent();
            _param = paramU;
            GetFriend();
            GetProfile();

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem != null)
            {
                var selection = e.SelectedItem as Friend;

                #region ttt
                var client = new HttpClient();
                var uri = new Uri(string.Format("http://" + Room.IPs + "/Chat/findrooms/" + "ch_" + p.userID + "_" + selection.friendID));
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                if(responseBody != "[]")
                {
                    await Navigation.PushAsync(new Chatroom("ch_" + p.userID + "_" + selection.friendID,p));
                }
                else
                {
                    uri = new Uri(string.Format("http://" + Room.IPs + "/Chat/findrooms/" + "ch_" + selection.friendID + "_" + p.userID));
                    response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                    if(responseBody != "[]")
                    {
                        await Navigation.PushAsync(new Chatroom("ch_" + selection.friendID + "_" + p.userID,p));
                    }
                    else
                    {
                        Charroom css = new Charroom();
                        css.chatroomID = "ch_" + p.userID + "_" + selection.friendID;
                        css.chatroomName = "ch_" + p.userID + "_" + selection.friendID;
                        css.guest = new List<Guest>
                        {
                            new Guest {userID=p.userID,name=p.name},
                            new Guest {userID=selection.friendID,name=selection.name}
                        };
                        css.Message = new List<Message>
                        {

                        };

                        using(var clientP = new HttpClient())
                        {
                            var responseP = clientP.PostAsync("http://" + Room.IPs + "/Chat/addroom",
                                new StringContent(JsonConvert.SerializeObject(css).ToString(),
                                Encoding.UTF8, "application/json"))
                                .Result;
                            if (responseP.IsSuccessStatusCode)
                            {
                                await Navigation.PushAsync(new Chatroom("ch_" + p.userID + "_" + selection.friendID, p));
                            }
                        }
                    }
                }
                #endregion

                #region
                ((ListView)sender).SelectedItem = null;
                #endregion
            }
        }

        private void LvRefreshing(object sender, EventArgs e)
        {
            if (listView.IsRefreshing)
            {
                GetProfile();
                GetFriend();

                listView.EndRefresh();
            }
        }

        HttpClient client;
        HttpResponseMessage response;
        JArray jr;
        JArray a;
        IList<Friend> Fr;
        async void GetFriend()
        {
            var tclient = new HttpClient();
            var turi = new Uri(string.Format("http://" + Room.IPs + "/Chat/findfriend/" + _param));
            var tresponse = await tclient.GetAsync(turi);
            tresponse.EnsureSuccessStatusCode();
            string responseBody = await tresponse.Content.ReadAsStringAsync();
            jr = JArray.Parse(responseBody) as dynamic;
            a = (JArray)jr[0]["friend"];
            Fr = a.ToObject <IList<Friend>>();
            listView.ItemsSource = Fr;
        }

        string[] profile = { "", "", "", "", "", "" };
        Profile p = new Profile();
        async void GetProfile()
        {
            client = new HttpClient();
            var uri = new Uri(string.Format("http://" + Room.IPs + "/Chat/findProfile/" + _param));
            response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string rresponseBody = await response.Content.ReadAsStringAsync();

            dynamic blogPosts = JArray.Parse(rresponseBody);
            dynamic blogPost = blogPosts[0];
            profile[0] = blogPost.UserID;
            profile[1] = blogPost.UserID;
            profile[2] = blogPost.name;
            profile[3] = blogPost.email;
            profile[4] = blogPost.image;
            profile[5] = blogPost.sex;

            p.userID = blogPost.userID;
            p.password = blogPost.password;
            p.name = blogPost.name;
            p.email = blogPost.email;
            p.image = blogPost.image;
            p.sex = blogPost.sex;
        }

        private async void btnSearch_Onclicked(object sender, EventArgs e)
        {
            if(etSearchUser.Text != null && etSearchUser.Text != p.userID)
            {
                 await Navigation.PushAsync(new SearchFriend(etSearchUser.Text, p));
            }
            else
            {
                await DisplayAlert("Alerta", "Por favor ingresa el ID de un usuario", "Entiendo");
            }

        }

        private async void btnRF_Onclicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShowRequestFriend(p.userID,p));
        }

        private async void btnRE_OnClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Member(p.userID));
        }

        private async void btnLO_OnClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Login());
        }
    }
}
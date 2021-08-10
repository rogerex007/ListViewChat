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
    public partial class ShowRequestFriend : ContentPage
    {
        public string _param { get; set; }
        Profile p = new Profile();
        public ShowRequestFriend(string paramm, Profile pp)
        {
            InitializeComponent();
            _param = paramm;
            p = pp;
            GetRequestFriend();
        }
        HttpClient client;
        HttpResponseMessage response;
        JArray jr;
        JArray a;
        IList<RequestF> Rf;
        string[] profile = { "", "", "", "", "", "" };
        string[] accept = { "", "", "" };

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        void OnTap(object sender, ItemTappedEventArgs e)
        {

        }

        private void LvRefreshing(object sender, EventArgs e)
        {
            if (listView.IsRefreshing)
            {
                listView.EndRefresh();
            }

        }

        public void OnAccept(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            AcceptF(mi.CommandParameter);
        }

        public void OnDeAccept(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            OnDeAcceptF(mi.CommandParameter);
        }

        async void GetRequestFriend()
        {
            client = new HttpClient();
            var uri = new Uri(string.Format("http://" + Room.IPs + "/Chat/findfriend/" + _param));
            response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            jr = JArray.Parse(responseBody) as dynamic;
            a = (JArray)jr[0]["RequestF"];
            Rf = a.ToObject<IList<RequestF>>();
            listView.ItemsSource = Rf;
        }

        async void AcceptF(object Fid)
        {
            string[] find = { "", "", "" };

            client = new HttpClient();
            var uri = new Uri(string.Format("http://" + Room.IPs + "/Chat/findaddfriend/" + Fid));
            response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            dynamic blogPosts = JArray.Parse(responseBody);
            dynamic blogPost = blogPosts[0];
            find[0] = blogPost.userID;
            find[1] = blogPost.name;
            find[2] = blogPost.image;


            Friend frf = new Friend();
            frf.friendID = find[0];
            frf.name = find[1];
            frf.imageFr = find[2];

            using (var clientq = new HttpClient())
            {
                var responseq = clientq.PutAsync("http://" + Room.IPs + "/Chat/addfriendbyRequest/" + _param,
                    new StringContent(JsonConvert.SerializeObject(frf).ToString(),
                    Encoding.UTF8, "application/json"))
                    .Result;
                if (responseq.IsSuccessStatusCode)
                {
                    dynamic content = JsonConvert.DeserializeObject(
                        responseq.Content.ReadAsStringAsync()
                        .Result);
                }
            }

            Friend frmm = new Friend();
            frmm.friendID = p.userID;
            frmm.name = p.name;
            frmm.imageFr = p.image;

            using (var clientq = new HttpClient())
            {
                var responseq = clientq.PutAsync("http://" + Room.IPs + "/Chat/addfriendbyRequest/" + find[0],
                    new StringContent(JsonConvert.SerializeObject(frmm).ToString(),
                    Encoding.UTF8, "application/json"))
                    .Result;
                if (responseq.IsSuccessStatusCode)
                {
                    dynamic content = JsonConvert.DeserializeObject(
                        responseq.Content.ReadAsStringAsync()
                        .Result);
                }
            }


            //Elimar solicitud aceptada
            using (var clientqq = new HttpClient())
            {
                var responseqq = clientqq.PutAsync("http://" + Room.IPs + "/Chat/updaterequestF/" +_param + "&" + find[0],
                    new StringContent(JsonConvert.SerializeObject(frf).ToString(),
                    Encoding.UTF8, "application/json"))
                    .Result;
                if (responseqq.IsSuccessStatusCode)
                {
                    dynamic content = JsonConvert.DeserializeObject(
                        responseqq.Content.ReadAsStringAsync()
                        .Result);
                    GetRequestFriend();
                }
            }

        }

        async void OnDeAcceptF(object Fid)
        {
            string[] find = { "", "", "" };
            client = new HttpClient();
            var uri = new Uri(string.Format("http://" + Room.IPs + "/Chat/findaddfriend/" + Fid));
            response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            dynamic blogPosts = JArray.Parse(responseBody);
            dynamic blogPost = blogPosts[0];
            find[0] = blogPost.userID;
            find[1] = blogPost.name;
            find[2] = blogPost.image;

            Friend frf = new Friend();
            frf.friendID = find[0];
            frf.name = find[1];
            frf.imageFr = find[2];

            using (var clientqq = new HttpClient())
            {
                var responseqq = clientqq.PutAsync("http://" + Room.IPs + "/Chat/updaterequestF/" + _param + "&" + find[0],
                    new StringContent(JsonConvert.SerializeObject(frf).ToString(),
                    Encoding.UTF8, "application/json"))
                    .Result;
                if (responseqq.IsSuccessStatusCode)
                {
                    dynamic content = JsonConvert.DeserializeObject(
                        responseqq.Content.ReadAsStringAsync()
                        .Result);
                    GetRequestFriend();
                }
            }

        }



 
       
    }
}
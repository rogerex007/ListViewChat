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
    public partial class Chatroom : ContentPage
    {
        public string _param { get; set; }
        Profile p = new Profile();
        public Chatroom(string param, Profile pp)
        {
            InitializeComponent();
            _param = param;
            p = pp;
            GetCon();
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
        HttpClient client;
        HttpResponseMessage response;
        JArray jr;
        JArray a;
        IList<Message> Ms;

        async void GetCon()
        {
            client = new HttpClient();
            var uri = new Uri(string.Format("http://" + Room.IPs + "/Chat/findrooms/" + _param));
            response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            jr = JArray.Parse(responseBody) as dynamic;
            a = (JArray)jr[0]["Message"];
            Ms = a.ToObject<IList<Message>>();

            listView.ItemsSource = Ms;
        }

        private void LvRefreshing(object sender, EventArgs e)
        {
            if (listView.IsRefreshing)
            {
                GetCon();
                listView.EndRefresh();
            }
        }

        private async void btnSend_OnClicked(object sender, EventArgs e)
        {
            if(etMessage.Text != null && etMessage.Text != "")
            {
                GetCon();
                Message mss = new Message();
                mss.userID = p.userID;
                mss.name = p.name;
                mss.imageM = p.image;
                mss.message = etMessage.Text;
                mss.time_state = "";

                using (var clientq = new HttpClient())
                {
                    var responseq = clientq.PutAsync("http://" + Room.IPs + "/Chat/addmessage/" + _param,
                        new StringContent(JsonConvert.SerializeObject(mss).ToString(),
                        Encoding.UTF8, "application/json"))
                        .Result;
                    if (responseq.IsSuccessStatusCode)
                    {
                        dynamic content = JsonConvert.DeserializeObject(
                            responseq.Content.ReadAsStringAsync()
                            .Result);

                        GetCon();
                        etMessage.Text = "";
                    }
                }

            }
        }

        private async void btnRefresh_OnClicked(object sender, EventArgs e)
        {
            GetCon();
        }
    }
}
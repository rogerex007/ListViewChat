using System;
using System.Collections.Generic;
using System.Text;

namespace ListViewChat.Bass
{
    public class Room
    {
        public static string IPs = "192.168.0.6:3000";

        public Room()
        {

        }
    }

    public class users
    {
        public string userID { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string image { get; set; }
        public string sex { get; set; }
        public List<RequestF> RequestF { get; set; }
        public List<Friend> friend { get; set; }

    }

    public class Profile
    {
        public string userID { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string image { get; set; }
        public string sex { get; set; }
    }

    public class RequestF
    {
        public string RequestID { get; set; }
        public string name { get; set; }
        public string imageR { get; set; }
    }

    public class Friend
    {
        public string friendID { get; set; }
        public string imageFr { get; set; }
        public string name { get; set; }
    }

    public class Charroom
    {
        public string chatroomID { get; set; }
        public string chatroomName { get; set; }
        public List<Guest> guest { get; set; }
        public List<Message> Message { get; set; }
    }

    public class Guest
    {
        public string userID { get; set; }
        public string name { get; set; }
    }

    public class Message
    {
        public string userID { get; set; }
        public string name { get; set; }
        public string imageM { get; set; }
        public string message { get; set; }
        public string time_state { get; set; }
    }
}

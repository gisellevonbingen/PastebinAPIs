using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PastebinAPIs;

namespace PastebinAPIs.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var user = new UserConsole();
            var apiKey = user.ReadInput("Enter API Key").AsString;
            var name = user.ReadInput("Enter User Name").AsString;
            var password = user.ReadInput("Enter User Password").AsString;

            var api = new PastbinAPI();
            api.APIKey = apiKey;

            string userKey = null;

            if (string.IsNullOrWhiteSpace(name) == false && string.IsNullOrWhiteSpace(password) == false)
            {
                userKey = api.Login(name, password);
            }

            user.SendMessage("User Key = " + userKey);

            var data = new PasteData();
            data.UserKey = userKey;
            data.Code = "Just Test";
            data.Private = PastePrivate.Public;
            data.Name = "TEST";
            data.Format = "text";
            data.ExpireDate = PasteExpireDate.Never;
            var uri = api.CreateNewPaste(data);

            user.SendMessage();
            user.SendMessage(uri);
        }

    }

}

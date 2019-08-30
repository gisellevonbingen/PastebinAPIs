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
            var createUser = ParseArgs(args, user);
            var (api, userKey) = Create(createUser);

            user.SendMessage("API Key = " + api.APIKey);
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

        public static UserAbstract ParseArgs(string[] args, UserConsole defaultUser)
        {
            if (args.Length == 1)
            {
                return new UserFile(args[0]);
            }

            return defaultUser;
        }

        public static (PastbinAPI api, string userKey) Create(UserAbstract user)
        {
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

            return (api, userKey);
        }

    }

}

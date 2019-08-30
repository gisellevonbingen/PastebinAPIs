using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giselle.Commons;
using PastebinAPIs;

namespace PastebinAPIs.Test
{
    public class Program
    {
        public delegate void TestDelegate(UserAbstract user, PastbinAPI api, string userKey);

        public static void Main(string[] args)
        {
            var user = new UserConsole();
            var createUser = ParseArgs(args, user);
            var (api, userKey) = Create(createUser);

            user.SendMessage("API Key = " + api.APIKey);
            user.SendMessage("User Key = " + userKey);

            var tests = new Dictionary<string, TestDelegate>();
            tests["psate"] = (TestPaste);

            while (true)
            {
                user.SendMessage();
                user.SendMessage();
                var testQuery = user.QueryInput("Enter Test", tests, pair => pair.Key, true);

                if (testQuery.Breaked == true)
                {
                    continue;
                }

                var test = testQuery.Value.Value;
                test(user, api, userKey);
            }

        }

        public static void TestPaste(UserAbstract user, PastbinAPI api, string userKey)
        {
            var data = new PasteData();
            data.UserKey = userKey;
            data.Name = user.ReadInput("Enter Paste Name").AsString;
            data.Code = string.Join(Environment.NewLine, user.ReadInputWhileBreak("Enter Paste Text While Break"));
            data.Private = user.QueryInput("Enter Private", EnumUtils.GetValues<PastePrivate>(), v => v.ToString()).Value;
            data.ExpireDate = user.QueryInput("Enter Expire Date", PasteExpireDate.Values, v => v.Name).Value;

            var uri = api.CreateNewPaste(data);
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

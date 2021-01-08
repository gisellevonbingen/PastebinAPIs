using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giselle.Commons;
using Giselle.Commons.Enums;
using Giselle.Commons.Users;
using PastebinAPIs;

namespace PastebinAPIs.Test
{
    public class Program
    {
        public delegate void TestDelegate(UserAbstract user, PastebinAPI api, string userKey);

        public static void Main(string[] args)
        {
            var user = new UserConsole();
            var createUser = ParseArgs(args, user);
            var (api, userKey) = Create(createUser);

            user.SendMessage("API Key = " + api.APIKey);
            user.SendMessage("User Key = " + userKey);

            var tests = new Dictionary<string, TestDelegate>();
            tests["psate"] = TestPaste;
            tests["list"] = TestList;
            tests["delete"] = TestDelete;
            tests["user"] = TestUser;
            tests["raw"] = TestRaw;

            while (true)
            {
                try
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
                catch (UserInputReturnException)
                {

                }
                catch (Exception e)
                {
                    user.SendMessage(string.Concat(e));
                }

            }

        }

        public static void TestPaste(UserAbstract user, PastebinAPI api, string userKey)
        {
            var request = new PasteCreateRequest();
            request.UserKey = userKey;
            request.Name = user.ReadInput("Enter Paste Name").AsString;
            request.Code = string.Join(Environment.NewLine, user.ReadInputWhileBreak("Enter Paste Text While Break"));
            request.Private = user.QueryInput("Enter Private", EnumUtils.Values<PastePrivate>(), v => v.ToString()).Value;
            request.ExpireDate = user.QueryInput("Enter Expire Date", PasteExpireDate.Values, v => v.Name).Value;

            var url = api.CreatePaste(request);
            user.SendMessage(url);
        }

        public static void TestList(UserAbstract user, PastebinAPI api, string userKey)
        {
            var request = new PasteListRequest();
            request.UserKey = userKey;
            request.ResultsLimit = user.ReadInput("Enter Results Limit, Nullable").AsInt;

            var array = api.ListPastes(request);
            user.SendMessageAsReflection("ListPastes", array);
        }

        public static void TestDelete(UserAbstract user, PastebinAPI api, string userKey)
        {
            var request = new PasteDeleteRequest();
            request.UserKey = userKey;
            request.PasteKey = user.ReadInput("Enter Paste Key").AsString;

            var result = api.DeletePaste(request);
            user.SendMessage("Result : " + result);
        }

        public static void TestUser(UserAbstract user, PastebinAPI api, string userKey)
        {
            var puser = api.GetUser(userKey);
            user.SendMessageAsReflection("GetUser", puser);
        }

        public static void TestRaw(UserAbstract user, PastebinAPI api, string userKey)
        {
            var request = new PasteGetRawRequest();
            request.UserKey = userKey;
            request.PasteKey = user.ReadInput("Enter Paste Key").AsString;

            var raw = api.GetPasteRaw(request);
            user.SendMessage("=== RAW ===");
            user.SendMessage(raw);
        }

        public static UserAbstract ParseArgs(string[] args, UserConsole defaultUser)
        {
            if (args.Length == 1)
            {
                return new UserFile(args[0]);
            }

            return defaultUser;
        }

        public static (PastebinAPI api, string userKey) Create(UserAbstract user)
        {
            var apiKey = user.ReadInput("Enter API Key").AsString;
            var name = user.ReadInput("Enter User Name").AsString;
            var password = user.ReadInput("Enter User Password").AsString;

            var api = new PastebinAPI();
            api.APIKey = apiKey;

            string userKey = null;

            if (string.IsNullOrWhiteSpace(name) == false && string.IsNullOrWhiteSpace(password) == false)
            {
                userKey = api.Login(new PasteLoginRequest() { Name = name, Password = password });
            }

            return (api, userKey);
        }

    }

}

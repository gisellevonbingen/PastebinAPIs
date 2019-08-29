using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs.Test
{
    public static class UserExtensions
    {
        public static List<string> ReadInputWhileBreak(this UserAbstract user, string message)
        {
            return ReadInputWhileBreak(user, message, (u, ir) => ir.AsString);
        }

        public static List<T> ReadInputWhileBreak<T>(this UserAbstract user, string message, Func<UserAbstract, InputResult, T> func)
        {
            var list = new List<T>();

            while (true)
            {
                user.SendMessage(message);
                user.SendMessage(user.GetBreakMessage());

                var input = user.ReadInput();

                if (user.IsBreak(input.AsString) == true)
                {
                    break;
                }
                else
                {
                    var v = func(user, input);
                    list.Add(v);
                }

            }

            return list;
        }

        public static void SendMessageAsReflection<T>(this UserAbstract user, string name, T value)
        {
            var lines = ObjectUtils2.ToPrintableLines(value);

            user.SendMessage();
            user.SendMessage($"== {name} ==");

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var prefix = new string(' ', (line.Level + 1) * 4);
                user.SendMessage($"{prefix}{line.Message}");
            }

        }

    }

}

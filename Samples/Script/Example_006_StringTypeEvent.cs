using System;
using Aya.Events;

namespace Aya.Sample
{
    public static class StringEventDefine
    {
        public const string Event01 = "Event01";
        public const string Event02 = "Event02";
    }

    public class UserCSharpClass006 : ObjectListener
    {
        // Auto Listen

        public void Test01()
        {
            UEvent.Dispatch(StringEventDefine.Event01, "Message 01");
        }

        [Listen(StringEventDefine.Event01)]
        public void Receive01(string message)
        {
            Console.WriteLine(message);
        }

        // Manual Listen

        public void Test02()
        {
            UEvent.Listen(StringEventDefine.Event02, Receive02);
            UEvent.Dispatch(StringEventDefine.Event02, "Message 02");
            UEvent.Remove(StringEventDefine.Event02, Receive02);
        }

        public void Receive02(string message)
        {
            Console.WriteLine(message);
        }
    }
}

using System;
using Aya.Events;

namespace Aya.Sample
{
    public struct StructEventDefine
    {
        public string Message;
    }

    public class UserCSharpClass008 : ObjectListener
    {
        // Auto Listen

        public void Test01()
        {
            var evt = new StructEventDefine()
            {
                Message = "Message 01"
            };

            UEvent.Dispatch(evt);
        }

        [Listen(typeof(StructEventDefine))]
        public void Receive01(StructEventDefine evt)
        {
            Console.WriteLine(evt.Message);
        }

        // Manual Listen

        public void Test02()
        {
            UEvent.Listen<StructEventDefine>(Receive02);

            UEvent.Dispatch(new StructEventDefine()
            {
                Message = "Message 02"
            });

            UEvent.Remove<StructEventDefine>(Receive02);
        }

        public void Receive02(StructEventDefine evt)
        {
            Console.WriteLine(evt.Message);
        }
    }
}
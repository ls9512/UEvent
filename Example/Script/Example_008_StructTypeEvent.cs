using System;
using Aya.Events;

namespace Aya.Example
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

            UEvent<StructEventDefine>.Dispatch(evt);
        }

        [Listen(typeof(StructEventDefine))]
        public void Receive01(StructEventDefine evt)
        {
            Console.WriteLine(evt.Message);
        }

        // Manual Listen

        public void Test02()
        {
            UEvent<StructEventDefine>.Listen(Receive02);

            UEvent<StructEventDefine>.Dispatch(new StructEventDefine()
            {
                Message = "Message 02"
            });

            UEvent<StructEventDefine>.Remove(Receive02);
        }

        public void Receive02(StructEventDefine evt)
        {
            Console.WriteLine(evt.Message);
        }
    }
}
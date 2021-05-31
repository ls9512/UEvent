using System;
using Aya.Events;

namespace Aya.Sample
{
    public class ClassEventDefine
    {
        public string Message;
    }

    public class UserCSharpClass007 : ObjectListener
    {
        // Auto Listen

        public void Test01()
        {
            var evt = new ClassEventDefine()
            {
                Message = "Message 01"
            };

            UEvent.Dispatch(evt);
        }

        [Listen(typeof(ClassEventDefine))]
        public void Receive01(ClassEventDefine evt)
        {
            Console.WriteLine(evt.Message);
        }

        // Manual Listen

        public void Test02()
        {
            UEvent.Listen<ClassEventDefine>(Receive02);

            UEvent.Dispatch(new ClassEventDefine()
            {
                Message = "Message 02"
            });

            UEvent.Remove<ClassEventDefine>(Receive02);
        }

        public void Receive02(ClassEventDefine evt)
        {
            Console.WriteLine(evt.Message);
        }
    }
}
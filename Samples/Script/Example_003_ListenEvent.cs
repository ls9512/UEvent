using Aya.Events;

namespace Aya.Sample
{
    [EventEnum]
    public enum EventType003
    {
        Event01,
        Event02,
    }

    public class UserCSharpClass003 : ObjectListener
    {
        [Listen(EventType003.Event01)]
        public void ListenMethod01()
        {

        }

        [Listen(EventType003.Event01, 9, true)]
        public void ListenMethod02(object eventType)
        {

        }

        [Listen(EventType003.Event01, 5)]
        public void ListenMethod03()
        {

        }

        [Listen(EventType003.Event01, EventType003.Event02)]
        public void ListenMethod04()
        {

        }

        [ListenType(typeof(EventType003))]
        public void ListenMethod05()
        {

        }


        [ListenGroup(EventType003.Event01, "Group01")]
        public void ListenMethod06()
        {

        }
    }
}

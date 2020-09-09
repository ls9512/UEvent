using Aya.Events;

namespace Aya.Example
{
    [EventEnum]
    public enum EventType005
    {
        Event01,
    }

    public class UserCSharpClass00501 : ObjectListener
    {
        public void Test()
        {
            Dispatch(EventType005.Event01);
            DispatchTo(EventType005.Event01, null);
            DispatchGroup(EventType005.Event01, "Group01");
        }
    }

    public class UserCSharpClass00502
    {
        public void Test()
        {
            EventManager.GetDispatcher<EventType005>().Dispatch(EventType005.Event01);
            EventManager.GetDispatcher<EventType005>().DispatchTo(EventType005.Event01, null);
            EventManager.GetDispatcher<EventType005>().DispatchGroup(EventType005.Event01, "Group01");
        }
    }

    public class UserUnityClass005 : MonoListener
    {
        public void Test()
        {
            DispatchSafe(EventType005.Event01);
        }
    }
}

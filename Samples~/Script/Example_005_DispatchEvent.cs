using Aya.Events;

namespace Aya.Sample
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
            UEvent.Dispatch(EventType005.Event01);
            UEvent.DispatchTo(EventType005.Event01, null);
            UEvent.DispatchGroup(EventType005.Event01, "Group01");
        }
    }

    public class UserCSharpClass00502
    {
        public void Test()
        {
            UEvent.Dispatch(EventType005.Event01);
            UEvent.DispatchTo(EventType005.Event01, null);
            UEvent.DispatchGroup(EventType005.Event01, "Group01");
        }
    }

    public class UserUnityClass005 : MonoListener
    {
        public void Test()
        {
            UEvent.DispatchSafe(EventType005.Event01);
        }
    }
}

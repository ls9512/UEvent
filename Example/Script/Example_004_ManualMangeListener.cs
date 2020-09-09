using Aya.Events;

namespace Aya.Example
{
    [EventEnum]
    public enum EventType004
    {
        Event01,
    }

    public class UserCSharpClass00401 : ObjectListener
    {
        public void Test()
        {
            // Add listen method
            var listenMethod = this.GetType().GetMethod("ListenMethod");
            AddListener(EventType004.Event01, listenMethod);

            // Add listen action
            AddListener(EventType004.Event01, ListenAction);

            // Check listener exists
            HasListener(EventType004.Event01);

            // Get listeners
            var listeners = GetListeners(EventType004.Event01);

            // Remove listen method
            RemoveListener(EventType004.Event01, listenMethod);

            // Remove listen action
            RemoveListener(EventType004.Event01, ListenAction);
        }

        public void ListenMethod()
        {

        }

        public void ListenAction(EventType004 eventType, params object[] args)
        {

        }
    }

    public class UserCSharpClass00402
    {
        public void Test()
        {
            // Add listen method
            var listenMethod = this.GetType().GetMethod("ListenMethod");
            EventManager.GetDispatcher<EventType004>().AddListener(EventType004.Event01, this, listenMethod);

            // Add listen action
            EventManager.GetDispatcher<EventType004>().AddListener(EventType004.Event01, ListenAction);

            // Check listener exists
            EventManager.GetDispatcher<EventType004>().HasListener(EventType004.Event01);

            // Get listeners
            var listeners = EventManager.GetDispatcher<EventType004>().GetListeners(EventType004.Event01, this);

            // Remove listen method
            EventManager.GetDispatcher<EventType004>().RemoveListener(EventType004.Event01, this, listenMethod);

            // Remove listen action
            EventManager.GetDispatcher<EventType004>().RemoveListener(EventType004.Event01, ListenAction);
        }

        public void ListenMethod()
        {

        }

        public void ListenAction(EventType004 eventType, params object[] args)
        {

        }
    }
}

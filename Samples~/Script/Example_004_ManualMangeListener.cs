using Aya.Events;

namespace Aya.Sample
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
            // Quick API

            // Add listen method
            var listenMethod = this.GetType().GetMethod("ListenMethod");
            UEvent.Listen(EventType004.Event01, this, listenMethod);

            // Add listen action
            UEvent.Listen(EventType004.Event01, ListenAction);

            // Check listener exists
            UEvent.Contains(EventType004.Event01);
            
            // Get listeners
            var listeners = UEvent.Get(EventType004.Event01);
            
            // Remove listen method
            UEvent.Remove(EventType004.Event01, this, listenMethod);

            // Remove listen action
            UEvent.Remove(EventType004.Event01, ListenAction);
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
            // Full API

            // Add listen method
            var listenMethod = this.GetType().GetMethod("ListenMethod");
            EventManager.GetDispatcher<EventType004>().AddListener(EventType004.Event01, this, listenMethod);

            // Add listen action
            EventManager.GetDispatcher<EventType004>().AddListener(EventType004.Event01, ListenAction);

            // Check listener exists
            EventManager.GetDispatcher<EventType004>().ContainsListener(EventType004.Event01);

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

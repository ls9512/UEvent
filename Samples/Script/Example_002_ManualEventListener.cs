using UnityEngine;
using Aya.Events;

namespace Aya.Sample
{
    public class UserCSharpClass002
    {
        public EventListener Listener;

        public UserCSharpClass002()
        {
            Listener = new EventListener(this);
            Listener.Register();
        }

        ~UserCSharpClass002()
        {
            Listener.DeRegister();
        }
    }

    public class UserUnityClass002 : MonoBehaviour
    {
        public EventListener Listener;

        public void Awake()
        {
            Listener = new EventListener(this);
        }

        public void OnEnable()
        {
            Listener.Register();
        }

        public void OnDisable()
        {
            Listener.DeRegister();
        }
    }
}

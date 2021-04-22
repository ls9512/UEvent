/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MonoListener.cs
//  Info     : MonoBehaviour 事件处理器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace Aya.Events
{
    public abstract class MonoListener : MonoBehaviour
    {
        #region Property
        
        /// <summary>
        /// 事件监听器
        /// </summary>
        public EventListener EventListener { get; protected set; }

        /// <summary>
        /// 是否仅激活状态接收事件
        /// </summary>
        public virtual bool ListenOnlyActive => true;

        /// <summary>
        /// 监听分组
        /// </summary>
        public object ListenGroup { get; set; } = null;

        #endregion

        #region MonoBehaviour

        protected virtual void Awake()
        {
            EventListener = new EventListener(this);
            if (!ListenOnlyActive)
            {
                EventListener?.Register();
            }
        }

        protected virtual void OnEnable()
        {
            if (ListenOnlyActive)
            {
                EventListener?.Register();
            }
        }

        protected virtual void OnDisable()
        {
            if (ListenOnlyActive)
            {
                EventListener?.DeRegister();
            }
        }

        protected virtual void OnDestroy()
        {
            EventListener?.DeRegister();
        }
       
        #endregion
    }
}

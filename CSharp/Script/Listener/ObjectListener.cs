/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ObjectListener.cs
//  Info     : 常规类型事件处理器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////

namespace Aya.Events
{
    public abstract partial class ObjectListener
    {
        #region Property

        /// <summary>
        /// 事件监听器
        /// </summary>
        public EventListener EventListener { get; protected set; }

        /// <summary>
        /// 监听分组
        /// </summary>
        public object ListenGroup { get; set; } = null;

        #endregion

        #region Construct

        protected ObjectListener()
        {
            EventListener = new EventListener(this);
            EventListener.Register();
        }

        protected ObjectListener(object listener)
        {
            EventListener = new EventListener(listener);
            EventListener.Register();
        }

        ~ObjectListener()
        {
            EventListener.DeRegister();
        }

        #endregion
    }
}
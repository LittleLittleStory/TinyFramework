using System;

namespace TinyFramework
{
    /// <summary>
    /// 游戏框架中包含事件数据的类的基类。
    /// </summary>
    public abstract class TinyFrameworkEventArgs : EventArgs, IReference
    {
        /// <summary>
        /// 初始化游戏框架中包含事件数据的类的新实例。
        /// </summary>
        public TinyFrameworkEventArgs(){}

        /// <summary>
        /// 清理引用。
        /// </summary>
        public abstract void Clear();
    }
}

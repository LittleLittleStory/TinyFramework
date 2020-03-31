﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://TinyFramework.cn/
// Feedback: mailto:ellan@TinyFramework.cn
//------------------------------------------------------------

namespace TinyFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器异步将资源文件转换为加载对象完成事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperReadFileCompleteEventArgs : TinyFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载资源代理辅助器异步将资源文件转换为加载对象完成事件的新实例。
        /// </summary>
        public LoadResourceAgentHelperReadFileCompleteEventArgs()
        {
            Resource = null;
        }

        /// <summary>
        /// 获取加载对象。
        /// </summary>
        public object Resource
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载资源代理辅助器异步将资源文件转换为加载对象完成事件。
        /// </summary>
        /// <param name="resource">资源对象。</param>
        /// <returns>创建的加载资源代理辅助器异步将资源文件转换为加载对象完成事件。</returns>
        public static LoadResourceAgentHelperReadFileCompleteEventArgs Create(object resource)
        {
            LoadResourceAgentHelperReadFileCompleteEventArgs loadResourceAgentHelperReadFileCompleteEventArgs = ReferencePool.Acquire<LoadResourceAgentHelperReadFileCompleteEventArgs>();
            loadResourceAgentHelperReadFileCompleteEventArgs.Resource = resource;
            return loadResourceAgentHelperReadFileCompleteEventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器异步将资源文件转换为加载对象完成事件。
        /// </summary>
        public override void Clear()
        {
            Resource = null;
        }
    }
}

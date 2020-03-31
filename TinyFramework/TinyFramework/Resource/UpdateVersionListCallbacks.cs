//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://TinyFramework.cn/
// Feedback: mailto:ellan@TinyFramework.cn
//------------------------------------------------------------

namespace TinyFramework.Resource
{
    /// <summary>
    /// 版本资源列表更新回调函数集。
    /// </summary>
    public sealed class UpdateVersionListCallbacks
    {
        private readonly UpdateVersionListSuccessCallback m_UpdateVersionListSuccessCallback;
        private readonly UpdateVersionListFailureCallback m_UpdateVersionListFailureCallback;

        /// <summary>
        /// 初始化版本资源列表更新回调函数集的新实例。
        /// </summary>
        /// <param name="updateVersionListSuccessCallback">版本资源列表更新成功回调函数。</param>
        public UpdateVersionListCallbacks(UpdateVersionListSuccessCallback updateVersionListSuccessCallback)
            : this(updateVersionListSuccessCallback, null)
        {
        }

        /// <summary>
        /// 初始化版本资源列表更新回调函数集的新实例。
        /// </summary>
        /// <param name="updateVersionListSuccessCallback">版本资源列表更新成功回调函数。</param>
        /// <param name="updateVersionListFailureCallback">版本资源列表更新失败回调函数。</param>
        public UpdateVersionListCallbacks(UpdateVersionListSuccessCallback updateVersionListSuccessCallback, UpdateVersionListFailureCallback updateVersionListFailureCallback)
        {
            if (updateVersionListSuccessCallback == null)
            {
                throw new Exception("Update version list success callback is invalid.");
            }

            m_UpdateVersionListSuccessCallback = updateVersionListSuccessCallback;
            m_UpdateVersionListFailureCallback = updateVersionListFailureCallback;
        }

        /// <summary>
        /// 获取版本资源列表更新成功回调函数。
        /// </summary>
        public UpdateVersionListSuccessCallback UpdateVersionListSuccessCallback
        {
            get
            {
                return m_UpdateVersionListSuccessCallback;
            }
        }

        /// <summary>
        /// 获取版本资源列表更新失败回调函数。
        /// </summary>
        public UpdateVersionListFailureCallback UpdateVersionListFailureCallback
        {
            get
            {
                return m_UpdateVersionListFailureCallback;
            }
        }
    }
}

//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://TinyFramework.cn/
// Feedback: mailto:ellan@TinyFramework.cn
//------------------------------------------------------------

namespace TinyFramework.Resource
{
    internal sealed partial class ResourceManager : TinyFrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源加载方式类型。
        /// </summary>
        private enum LoadType
        {
            /// <summary>
            /// 从文件加载。
            /// </summary>
            LoadFromFile = 0,

            /// <summary>
            /// 从内存加载。
            /// </summary>
            LoadFromMemory,

            /// <summary>
            /// 从内存快速解密加载。
            /// </summary>
            LoadFromMemoryAndQuickDecrypt,

            /// <summary>
            /// 从内存解密加载。
            /// </summary>
            LoadFromMemoryAndDecrypt
        }
    }
}

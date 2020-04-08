using TinyFramework.Download;
using TinyFramework.ObjectPool;
using System;

namespace TinyFramework.Resource
{
    /// <summary>
    /// 资源管理器接口。
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        string ReadOnlyPath
        {
            get;
        }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        string ReadWritePath
        {
            get;
        }

        /// <summary>
        /// 获取当前变体。
        /// </summary>
        string CurrentVariant
        {
            get;
        }

        /// <summary>
        /// 获取已准备完毕资源数量。
        /// </summary>
        int AssetCount
        {
            get;
        }

        /// <summary>
        /// 获取已准备完毕资源数量。
        /// </summary>
        int ResourceCount
        {
            get;
        }

        /// <summary>
        /// 获取加载资源代理总数量。
        /// </summary>
        int LoadTotalAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取可用加载资源代理数量。
        /// </summary>
        int LoadFreeAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取工作中加载资源代理数量。
        /// </summary>
        int LoadWorkingAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取等待加载资源任务数量。
        /// </summary>
        int LoadWaitingTaskCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        float AssetAutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        int AssetCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        float AssetExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        int AssetPriority
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        float ResourceAutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        int ResourceCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        float ResourceExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        int ResourcePriority
        {
            get;
            set;
        }

        /// <summary>
        /// 设置资源只读区路径。
        /// </summary>
        /// <param name="readOnlyPath">资源只读区路径。</param>
        void SetReadOnlyPath(string readOnlyPath);

        /// <summary>
        /// 设置资源读写区路径。
        /// </summary>
        /// <param name="readWritePath">资源读写区路径。</param>
        void SetReadWritePath(string readWritePath);

        /// <summary>
        /// 设置当前变体。
        /// </summary>
        /// <param name="currentVariant">当前变体。</param>
        void SetCurrentVariant(string currentVariant);

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        void SetObjectPoolManager(IObjectPoolManager objectPoolManager);

        /// <summary>
        /// 设置下载管理器。
        /// </summary>
        /// <param name="downloadManager">下载管理器。</param>
        void SetDownloadManager(IDownloadManager downloadManager);

        /// <summary>
        /// 设置解密资源回调函数。
        /// </summary>
        /// <param name="decryptResourceCallback">要设置的解密资源回调函数。</param>
        /// <remarks>如果不设置，将使用默认的解密资源回调函数。</remarks>
        void SetDecryptResourceCallback(DecryptResourceCallback decryptResourceCallback);

        /// <summary>
        /// 设置资源辅助器。
        /// </summary>
        /// <param name="resourceHelper">资源辅助器。</param>
        void SetResourceHelper(IResourceHelper resourceHelper);

        /// <summary>
        /// 增加加载资源代理辅助器。
        /// </summary>
        /// <param name="loadResourceAgentHelper">要增加的加载资源代理辅助器。</param>
        void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper);

        /// <summary>
        /// 使用单机模式并初始化资源。
        /// </summary>
        /// <param name="initResourcesCompleteCallback">使用单机模式并初始化资源完成时的回调函数。</param>
        void InitResources(InitResourcesCompleteCallback initResourcesCompleteCallback);

        /// <summary>
        /// 检查资源是否存在。
        /// </summary>
        /// <param name="assetName">要检查资源的名称。</param>
        /// <returns>资源是否存在。</returns>
        bool HasAsset(string assetName);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        void UnloadAsset(object asset);

        /// <summary>
        /// 获取所有加载资源任务的信息。
        /// </summary>
        /// <returns>所有加载资源任务的信息。</returns>
        TaskInfo[] GetAllLoadAssetInfos();
    }
}

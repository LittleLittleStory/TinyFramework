namespace TinyFramework.Resource
{
    /// <summary>
    /// 资源辅助器接口。
    /// </summary>
    public interface IResourceHelper
    {
        /// <summary>
        /// 直接从指定文件路径读取数据流。
        /// </summary>
        /// <param name="fileUri">文件路径。</param>
        /// <param name="loadBytesCallback">读取数据流回调函数。</param>
        void LoadBytes(string fileUri, LoadBytesCallback loadBytesCallback);

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="objectToRelease">要释放的资源。</param>
        void Release(object objectToRelease);
    }
}

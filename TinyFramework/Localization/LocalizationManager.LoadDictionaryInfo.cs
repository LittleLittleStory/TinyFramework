namespace TinyFramework.Localization
{
    internal sealed partial class LocalizationManager : TinyFrameworkModule, ILocalizationManager
    {
        private sealed class LoadDictionaryInfo : IReference
        {
            private LoadType m_LoadType;
            private object m_UserData;

            public LoadDictionaryInfo()
            {
                m_LoadType = LoadType.Text;
                m_UserData = null;
            }

            public LoadType LoadType
            {
                get
                {
                    return m_LoadType;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }

            public static LoadDictionaryInfo Create(LoadType loadType, object userData)
            {
                LoadDictionaryInfo loadDictionaryInfo = ReferencePool.Acquire<LoadDictionaryInfo>();
                loadDictionaryInfo.m_LoadType = loadType;
                loadDictionaryInfo.m_UserData = userData;
                return loadDictionaryInfo;
            }

            public void Clear()
            {
                m_LoadType = LoadType.Text;
                m_UserData = null;
            }
        }
    }
}

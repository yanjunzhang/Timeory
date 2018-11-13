using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MgrBundle : MonoBehaviour {

    private Dictionary<string , AssetBundle> m_Bundlers = new Dictionary<string , AssetBundle>( );
    private const string m_FileSuffix = ".unity3d";
    private string m_URL;

    void Awake() {
        m_URL = "file://" + Application.streamingAssetsPath + "/";
    }

    void Start ( ) {      
        switch ( Application.platform ) {
            case RuntimePlatform.Android:
                m_URL = Application.streamingAssetsPath + "/";
                break;
        }
    }

    public bool Contains ( string bundleName ) {
        return m_Bundlers.ContainsKey( bundleName );
    }

    public void Clear ( bool force = true ) {
        foreach ( var item in m_Bundlers ) {
            item.Value.Unload( force );
        }
        m_Bundlers.Clear( );
    }

    public void Unload ( string bundleName , bool force = false ) {
        if ( m_Bundlers.ContainsKey( bundleName ) ) {
            m_Bundlers[bundleName].Unload( force );
            m_Bundlers.Remove( bundleName );
        }
    }

    public IEnumerator IE_LoadBundle ( string bundleName ) {
        if ( !m_Bundlers.ContainsKey( bundleName ) ) {
            string url = m_URL + bundleName + m_FileSuffix;
            using ( WWW www = new WWW( url ) ) {
                yield return www;
                if ( string.IsNullOrEmpty( www.error ) ) {
                    if ( !m_Bundlers.ContainsKey( bundleName ) ) {
                        m_Bundlers.Add( bundleName , www.assetBundle );
                    }
                } else {
                    Debug.LogError( www.error );
                }
            }
        }
    }

    public IEnumerator IE_LoadCacheBundle ( string bundleName ) {
        if ( !m_Bundlers.ContainsKey( bundleName ) ) {
            string url = m_URL + bundleName + m_FileSuffix;
            using ( WWW www = WWW.LoadFromCacheOrDownload( url , 0 ) ) {
                yield return www;
                if ( string.IsNullOrEmpty( www.error ) ) {
                    if ( !m_Bundlers.ContainsKey( bundleName ) ) {
                        m_Bundlers.Add( bundleName , www.assetBundle );
                    }
                } else {
                    Debug.LogError( www.error );
                }
            }
        }
    }

    public IEnumerator IE_LoadAsset ( string bundleName , string objectName , System.Action<Object> initHandler , bool autoUnload = true ) {
        if ( m_Bundlers.ContainsKey( bundleName ) ) {
            AssetBundle ab = m_Bundlers[bundleName];
            Object ob = ab.LoadAsset( objectName );
            if ( ob != null && initHandler != null ) {
                initHandler( ob );
            }
            if ( autoUnload ) {
                Unload( bundleName );
            }
        } else {
            yield return StartCoroutine( IE_LoadBundle( bundleName ) );
            if ( m_Bundlers.ContainsKey( bundleName ) ) {
                yield return StartCoroutine( IE_LoadAsset( bundleName , objectName , initHandler , autoUnload ) );
            }
        }
    }

    public IEnumerator IE_LoadCacheAsset ( string bundleName , string objectName , System.Action<Object> initHandler , bool autoUnload = true ) {
        if ( m_Bundlers.ContainsKey( bundleName ) ) {
            AssetBundle ab = m_Bundlers[bundleName];
            Object ob = ab.LoadAsset(objectName);
            if ( ob != null && initHandler != null ) {
                initHandler( ob );
            }
            if ( autoUnload ) {
                Unload( bundleName );
            }
        } else {
            yield return StartCoroutine( IE_LoadCacheBundle( bundleName ) );
            if ( m_Bundlers.ContainsKey( bundleName ) ) {
                yield return StartCoroutine( IE_LoadAsset( bundleName , objectName , initHandler , autoUnload ) );
            }
        }
    }


    public IEnumerator IE_LoadAsset<T> ( string bundleName , string objectName , System.Action<Object> initHandler , bool autoUnload = true ) {
        if ( m_Bundlers.ContainsKey( bundleName ) ) {
            AssetBundle ab = m_Bundlers[bundleName];
            Object ob = ab.LoadAsset(objectName, typeof(T));
            if ( ob != null && initHandler != null ) {
                initHandler( ob );
            }
            if ( autoUnload ) {
                Unload( bundleName );
            }
        } else {
            yield return StartCoroutine( IE_LoadBundle( bundleName ) );
            if ( m_Bundlers.ContainsKey( bundleName ) ) {
                yield return StartCoroutine( IE_LoadAsset<T>( bundleName , objectName , initHandler , autoUnload ) );
            }
        }
    }

    public IEnumerator IE_LoadCacheAsset<T> ( string bundleName , string objectName , System.Action<Object> initHandler , bool autoUnload = true ) {
        if ( m_Bundlers.ContainsKey( bundleName ) ) {
            AssetBundle ab = m_Bundlers[bundleName];
            Object ob = ab.LoadAsset(objectName, typeof(T));
            if ( ob != null && initHandler != null ) {
                initHandler( ob );
            }
            if ( autoUnload ) {
                Unload( bundleName );
            }
        } else {
            yield return StartCoroutine( IE_LoadCacheBundle( bundleName ) );
            if ( m_Bundlers.ContainsKey( bundleName ) ) {
                yield return StartCoroutine( IE_LoadAsset<T>( bundleName , objectName , initHandler , autoUnload ) );
            }
        }
    }

    public IEnumerator IE_LoadAssetAsync ( string bundleName , string objectName , System.Action<Object> initHandler , bool autoUnload = true ) {
        if ( m_Bundlers.ContainsKey( bundleName ) ) {
            AssetBundle ab = m_Bundlers[bundleName];
            AssetBundleRequest abr = ab.LoadAssetAsync( objectName , typeof( Object ) );
            yield return abr;
            Object ob = abr.asset;
            if ( ob != null && initHandler != null ) {
                initHandler( ob );
            }
            if ( autoUnload ) {
                Unload( bundleName );
            }
        } else {
            yield return StartCoroutine( IE_LoadBundle( bundleName ) );
            if ( m_Bundlers.ContainsKey( bundleName ) ) {
                yield return StartCoroutine( IE_LoadAssetAsync( bundleName , objectName , initHandler , autoUnload ) );
            }
        }
    }

    public IEnumerator IE_LoadCacheAssetAsync ( string bundleName , string objectName , System.Action<Object> initHandler , bool autoUnload = true ) {
        if ( m_Bundlers.ContainsKey( bundleName ) ) {
            AssetBundle ab = m_Bundlers[bundleName];
            AssetBundleRequest abr = ab.LoadAssetAsync(objectName, typeof(Object));
            yield return abr;
            Object ob = abr.asset;
            if ( ob != null && initHandler != null ) {
                initHandler( ob );
            }
            if ( autoUnload ) {
                Unload( bundleName );
            }
        } else {
            yield return StartCoroutine( IE_LoadCacheBundle( bundleName ) );
            if ( m_Bundlers.ContainsKey( bundleName ) ) {
                yield return StartCoroutine( IE_LoadAssetAsync( bundleName , objectName , initHandler , autoUnload ) );
            }
        }
    }

    public IEnumerator IE_LoadAssetAsync<T> ( string bundleName , string objectName , System.Action<Object> initHandler , bool autoUnload = true ) {
        if ( m_Bundlers.ContainsKey( bundleName ) ) {
            AssetBundle ab = m_Bundlers[bundleName];
            AssetBundleRequest abr = ab.LoadAssetAsync(objectName, typeof(T));
            yield return abr;
            Object ob = abr.asset;
            if ( ob != null && initHandler != null ) {
                initHandler( ob );
            }
            if ( autoUnload ) {
                Unload( bundleName );
            }
        } else {
            yield return StartCoroutine( IE_LoadBundle( bundleName ) );
            if ( m_Bundlers.ContainsKey( bundleName ) ) {
                yield return StartCoroutine( IE_LoadAssetAsync<T>( bundleName , objectName , initHandler , autoUnload ) );
            }
        }
    }

    public IEnumerator IE_LoadCacheAssetAsync<T> ( string bundleName , string objectName , System.Action<Object> initHandler , bool autoUnload = true ) {
        if ( m_Bundlers.ContainsKey( bundleName ) ) {
            AssetBundle ab = m_Bundlers[bundleName];
            AssetBundleRequest abr = ab.LoadAssetAsync(objectName, typeof(T));
            yield return abr;
            Object ob = abr.asset;
            if ( ob != null && initHandler != null ) {
                initHandler( ob );
            }
            if ( autoUnload ) {
                Unload( bundleName );
            }
        } else {
            yield return StartCoroutine( IE_LoadCacheBundle( bundleName ) );
            if ( m_Bundlers.ContainsKey( bundleName ) ) {
                yield return StartCoroutine( IE_LoadAssetAsync<T>( bundleName , objectName , initHandler , autoUnload ) );
            }
        }
    }

}

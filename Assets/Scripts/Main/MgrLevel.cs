using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum LoadType {
    Default ,
    FadeAsync ,
    StreamAsync ,
    StreamFadeAsync ,
    LoadingFadeAsync ,
    LoadingStreamFadeAsync ,
    LoadingCacheStreamFadeAsync ,
    LoadingCacheStreamFadeInAsync ,
}

public class MgrLevel : MonoBehaviour {

    public event System.Action<string> Event_PreLoad;
    public event System.Action<string> Event_BeforeLoad;

    private string m_URL;
    private bool m_Loading = false;
    private WWW m_Stream;
    private AsyncOperation m_Asy;
    private string m_LevelName;

    void Awake ( ) {
        m_URL = "file://" + Application.streamingAssetsPath + "/";
        if ( Application.platform == RuntimePlatform.Android )
            m_URL = Application.streamingAssetsPath + "/";
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        App.MgrTool.YieldMethod( 0.3f , ( ) => { m_Loading = false; } );
        Caching.ClearCache();
        Resources.UnloadUnusedAssets( );
        System.GC.Collect( );
    }

    public bool Load ( string levelName , float delay = 0 , LoadType levelType = LoadType.Default ) {
        if ( m_Loading ) return false;
        m_Loading = true;
        m_LevelName = levelName;
        if ( Event_BeforeLoad != null ) Event_BeforeLoad( m_LevelName );
        switch ( App.MgrConfig._AssetType ) {
            case AssetType.Bundle:
                switch ( levelType ) {
                    case LoadType.Default:
                        StartCoroutine( IE_Default( levelName , delay ) );
                        break;
                    case LoadType.FadeAsync:
                        StartCoroutine( IE_FadeAsync( levelName , delay ) );
                        break;
                    case LoadType.StreamAsync:
                        StartCoroutine( IE_StreamAsync( levelName , delay ) );
                        break;
                    case LoadType.StreamFadeAsync:
                        StartCoroutine( IE_StreamFadeAsync( levelName , delay ) );
                        break;
                    case LoadType.LoadingFadeAsync:
                        StartCoroutine( IE_LoadingFadeAsync( levelName , delay ) );
                        break;
                    case LoadType.LoadingStreamFadeAsync:
                        StartCoroutine( IE_LoadingStreamFadeAsync( levelName , delay ) );
                        break;
                    case LoadType.LoadingCacheStreamFadeAsync:
                        StartCoroutine( IE_LoadingCacheStreamFadeAsync( levelName , delay ) );
                        break;
                    case LoadType.LoadingCacheStreamFadeInAsync:
                        StartCoroutine( IE_LoadingCacheStreamFadeAsync( levelName , delay , false ) );
                        break;
                }
                break;
            case AssetType.Resource:
                switch ( levelType ) {
                    case LoadType.Default:
                        StartCoroutine( IE_Default( levelName , delay ) );
                        break;
                    case LoadType.FadeAsync:
                        StartCoroutine( IE_FadeAsync( levelName , delay ) );
                        break;
                    case LoadType.StreamAsync:
                        StartCoroutine( IE_Default( levelName , delay ) );
                        break;
                    case LoadType.StreamFadeAsync:
                        StartCoroutine( IE_FadeAsync( levelName , delay ) );
                        break;
                    case LoadType.LoadingFadeAsync:
                        StartCoroutine( IE_LoadingFadeAsync( levelName , delay ) );
                        break;
                    case LoadType.LoadingStreamFadeAsync:
                        StartCoroutine( IE_LoadingFadeAsync( levelName , delay ) );
                        break;
                    case LoadType.LoadingCacheStreamFadeAsync:
                        StartCoroutine( IE_LoadingFadeAsync( levelName , delay ) );
                        break;
                    case LoadType.LoadingCacheStreamFadeInAsync:
                        StartCoroutine( IE_LoadingFadeAsync( levelName , delay , false ) );
                        break;
                }
                break;
        }

        return true;
    }

    private IEnumerator IE_Default ( string levelName , float delay ) {
        yield return new WaitForSeconds( delay );
        Deallocate( );
        SceneManager.LoadScene(levelName);
    }

    private IEnumerator IE_FadeAsync ( string levelName , float delay ) {
        yield return new WaitForSeconds( delay );
        FadeScene.FadeOut( );
        yield return new WaitForSeconds( 0.3f );
        Deallocate( );
        yield return SceneManager.LoadSceneAsync(levelName);
        FadeScene.FadeIn( );
    }

    private IEnumerator IE_StreamAsync ( string levelName , float waitTime ) {
        string url = m_URL + levelName + ".unity3d";
        m_Stream = new WWW( url );
        yield return new WaitForSeconds( waitTime );
        yield return m_Stream;
        Deallocate( );
        if ( string.IsNullOrEmpty( m_Stream.error ) ) {
            AssetBundle bundle = m_Stream.assetBundle;
            yield return SceneManager.LoadSceneAsync(levelName);
            bundle.Unload( false );
        } else {
            Debug.LogError( m_Stream.error );
        }
        m_Stream.Dispose( );
    }

    private IEnumerator IE_StreamFadeAsync ( string levelName , float waitTime ) {
        string url = m_URL + levelName + ".unity3d";
        m_Stream = new WWW( url );
        yield return new WaitForSeconds( waitTime );
        FadeScene.FadeOut( );
        yield return new WaitForSeconds( 0.3f );
        Deallocate( );
        yield return m_Stream;
        if ( string.IsNullOrEmpty( m_Stream.error ) ) {
            AssetBundle bundle = m_Stream.assetBundle;
            yield return SceneManager.LoadSceneAsync(levelName);
            FadeScene.FadeIn( );
            bundle.Unload( false );
        } else {
            Debug.LogError( m_Stream.error );
        }
        m_Stream.Dispose( );
    }

    private IEnumerator IE_LoadingFadeAsync ( string levelName , float delay , bool autoFadeIn = true ) {
        yield return new WaitForSeconds( delay );
        FadeScene.FadeOut( );
        yield return new WaitForSeconds( 0.3f );
        Deallocate( );
        yield return SceneManager.LoadSceneAsync("Loading");
        FadeScene.FadeIn( );
        yield return new WaitForSeconds( 1f );
        FadeScene.FadeOut( );
        yield return new WaitForSeconds( 0.3f );
        yield return SceneManager.LoadSceneAsync(levelName);
        if ( autoFadeIn ) FadeScene.FadeIn( );
        Resources.UnloadUnusedAssets( );
        System.GC.Collect( );
    }

    private IEnumerator IE_LoadingStreamFadeAsync ( string levelName , float waitTime ) {
        string url = m_URL + levelName + ".unity3d";
        m_Stream = new WWW( url );
        yield return new WaitForSeconds( waitTime );
        FadeScene.FadeOut( );
        yield return new WaitForSeconds( 0.3f );
        Deallocate( );
        yield return SceneManager.LoadSceneAsync("Loading");
        FadeScene.FadeIn( );
        yield return m_Stream;
        if ( string.IsNullOrEmpty( m_Stream.error ) ) {
            AssetBundle bundle = m_Stream.assetBundle;
            FadeScene.FadeOut( );
            yield return SceneManager.LoadSceneAsync(levelName);
            FadeScene.FadeIn( );
            bundle.Unload( false );
        } else {
            Debug.LogError( m_Stream.error );
        }
        m_Stream.Dispose( );
    }

    private IEnumerator IE_LoadingCacheStreamFadeAsync ( string levelName , float waitTime , bool autoFadeIn = true ) {
        string url = m_URL + levelName + ".unity3d";
        m_Stream = WWW.LoadFromCacheOrDownload( url , 0 );
        yield return new WaitForSeconds( waitTime );
        FadeScene.FadeOut( );
        yield return new WaitForSeconds( 0.3f );
        Deallocate( );
        yield return SceneManager.LoadSceneAsync("Loading");
        FadeScene.FadeIn( );
        yield return new WaitForSeconds(0.3f);
        FadeScene.FadeOut();
        yield return new WaitForSeconds(0.3f);
        yield return m_Stream;
        if ( string.IsNullOrEmpty( m_Stream.error ) ) {
            AssetBundle bundle = m_Stream.assetBundle;
            yield return SceneManager.LoadSceneAsync(levelName);
            if ( autoFadeIn ) {
                FadeScene.FadeIn( );
            }
            bundle.Unload( false );
        } else {
            Debug.LogError( m_Stream.error );
        }
        m_Stream.Dispose( );
    }

    private void Deallocate ( ) {

        App.MgrAudio.Clear( );
        App.MgrBundle.Clear( true );
        App.MgrTool.Clear( );

        if ( !string.IsNullOrEmpty( m_LevelName ) && Event_PreLoad != null ) {
            Event_PreLoad( m_LevelName );
        }

        /*
		UISprite[ ] UISpritsInScene = FindObjectsOfType<UISprite>( );
        UITexture[ ] TextureInScene = FindObjectsOfType<UITexture>( );
        UIEventListener[ ] UIEventListenerScene = FindObjectsOfType<UIEventListener>( );

        for ( int i = 0; i < UISpritsInScene.Length; i++ ) {
            DestroyImmediate( UISpritsInScene[i] );
            UISpritsInScene[i] = null;
        }

        for ( int i = 0; i < TextureInScene.Length; i++ ) {
            DestroyImmediate( TextureInScene[i] );
            TextureInScene[i] = null;
        }

        for ( int i = 0; i < UIEventListenerScene.Length; i++ ) {
            DestroyImmediate( UIEventListenerScene[i] );
            UIEventListenerScene[i] = null;
        }
		*/

        Resources.UnloadUnusedAssets( );
        System.GC.Collect( );

    }

}
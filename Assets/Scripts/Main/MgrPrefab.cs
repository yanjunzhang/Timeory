using UnityEngine;
using System.Collections;

public enum PrefabFrom {
    Auto,
    Resources,
    Bundle,
}

public partial class MgrPrefab : MonoBehaviour {

    public void Create ( GameObject parent , string name , System.Action<GameObject> handle , PrefabFrom from = PrefabFrom.Auto ) {
        Create( parent , name , Vector3.zero , handle , from );
    }

    public void Create ( string name , Vector3 pos , System.Action<GameObject> handle , PrefabFrom from = PrefabFrom.Auto ) {
        Create( null , name , pos , handle , from );
    }

    public void Create ( GameObject parent , string name , Vector3 localPos , System.Action<GameObject> handle , PrefabFrom from = PrefabFrom.Auto ) {
        GameObject go;
        string bundleName = name;
        switch ( from ) {
            case PrefabFrom.Auto:
                if ( App.MgrConfig._AssetType == AssetType.Resource ) {
                    go = Instantiate( App.MgrResource.Load( name , ResourceRoot.PrefabRoot ) ) as GameObject;
                    if ( parent != null ) {
                        go.transform.parent = parent.transform;
                        go.transform.localPosition = localPos;
                    } else {
                        go.transform.position = localPos;
                    }
                    if ( handle != null ) handle( go );
                } else {
                    bundleName = WWW.EscapeURL( name ).Replace( "%" , "" );
                    StartCoroutine( App.MgrBundle.IE_LoadAsset<GameObject>( bundleName , name , ( ob ) => {
                        go = Instantiate( ob ) as GameObject;
                        if ( parent != null ) {
                            go.transform.parent = parent.transform;
                            go.transform.localPosition = localPos;
                        } else {
                            go.transform.position = localPos;
                        }
                        if ( handle != null ) handle( go );
                    } ) );
                }
                break;
            case PrefabFrom.Resources:
                go = Instantiate( App.MgrResource.Load( name , ResourceRoot.PrefabRoot ) ) as GameObject;
                if ( parent != null ) {
                    go.transform.parent = parent.transform;
                    go.transform.localPosition = localPos;
                } else {
                    go.transform.position = localPos;
                }
                if ( handle != null ) handle( go );
                break;
            case PrefabFrom.Bundle:
                bundleName = WWW.EscapeURL( name ).Replace( "%" , "" );
                StartCoroutine( App.MgrBundle.IE_LoadAsset<GameObject>( bundleName , name , ( ob ) => {
                    go = Instantiate( ob ) as GameObject;
                    if ( parent != null ) {
                        go.transform.parent = parent.transform;
                        go.transform.localPosition = localPos;
                    } else {
                        go.transform.position = localPos;
                    }
                    if ( handle != null ) handle( go );
                } ) );
                break;
        }
    }

    public void CreateSprite ( string name , System.Action<Sprite> handle , PrefabFrom from = PrefabFrom.Auto ) {
        switch ( from ) {
            case PrefabFrom.Auto:
                if ( App.MgrConfig._AssetType == AssetType.Resource ) {
                    GameObject go = Instantiate( App.MgrResource.Load( name , ResourceRoot.PrefabRoot ) ) as GameObject;
                    go.SetActive( false );
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>( );
                    if ( handle != null && sr != null ) handle( sr.sprite );
                    Destroy( go );
                } else {
                    string bundleName = WWW.EscapeURL( name );
                    bundleName = bundleName.Replace( "%" , "" );
                    StartCoroutine( App.MgrBundle.IE_LoadAsset<GameObject>( bundleName , name , ( ob ) => {
                        GameObject go = ob as GameObject;
                        SpriteRenderer sr = go.GetComponent<SpriteRenderer>( );
                        if ( handle != null ) handle( sr.sprite );
                    } ) );
                }
                break;
            case PrefabFrom.Bundle:
                string bundleName2 = WWW.EscapeURL( name );
                bundleName2 = bundleName2.Replace( "%" , "" );
                StartCoroutine( App.MgrBundle.IE_LoadAsset<GameObject>( bundleName2 , name , ( ob ) => {
                    GameObject go = ob as GameObject;
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>( );
                    if ( handle != null ) handle( sr.sprite );
                } ) );
                break;
            case PrefabFrom.Resources:
                GameObject go2 = Instantiate( App.MgrResource.Load( name , ResourceRoot.PrefabRoot ) ) as GameObject;
                go2.SetActive( false );
                SpriteRenderer sr2 = go2.GetComponent<SpriteRenderer>( );
                if ( handle != null && sr2 != null ) handle( sr2.sprite );
                Destroy( go2 );
                break;
        }

    }

    public void PreLoad ( string name , System.Action<Object> handle , PrefabFrom from = PrefabFrom.Auto ) {
        string bundleName = name;
        switch ( from ) {
            case PrefabFrom.Auto:
                if ( App.MgrConfig._AssetType == AssetType.Resource ) {
                    if ( handle != null ) handle( App.MgrResource.Load( name , ResourceRoot.PrefabRoot ) );
                } else {
                    bundleName = WWW.EscapeURL( name ).Replace( "%" , "" );
                    StartCoroutine( App.MgrBundle.IE_LoadAsset<GameObject>( bundleName , name , ( ob ) => {
                        if ( handle != null ) handle( ob );
                    } ) );
                }
                break;
            case PrefabFrom.Resources:
                if ( handle != null ) handle( App.MgrResource.Load( name , ResourceRoot.PrefabRoot ) );
                break;
            case PrefabFrom.Bundle:
                bundleName = WWW.EscapeURL( name ).Replace( "%" , "" );
                StartCoroutine( App.MgrBundle.IE_LoadAsset<GameObject>( bundleName , name , ( ob ) => {
                    if ( handle != null ) handle( ob );
                } ) );
                break;
        }
    }

    public void UnloadImmediate ( GameObject go , float delay = 0 ) {
        if ( delay == 0 ) {
            DestroyImmediate( go );
            Resources.UnloadUnusedAssets( );
            System.GC.Collect( );
        } else {
            App.MgrTool.YieldMethod( delay , ( ) => {
                DestroyImmediate( go );
                Resources.UnloadUnusedAssets( );
                System.GC.Collect( );
            } );
        }
    }

    public void Unload ( GameObject go , float delay = 0 ) {
        if ( delay == 0 ) {
            Destroy( go );
            Resources.UnloadUnusedAssets( );
            System.GC.Collect( );
        } else {
            App.MgrTool.YieldMethod( delay , ( ) => {
                Destroy( go );
                Resources.UnloadUnusedAssets( );
                System.GC.Collect( );
            } );
        }
    }

}

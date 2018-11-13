using UnityEngine;
using System.Collections;

public enum ResourceRoot {
    Root ,
    JsonRoot ,
    PrefabRoot ,
    AnimationRoot ,
    TextureRoot ,
    AudioRoot ,
}


public class MgrResource : MonoBehaviour {

    public Object Load ( string objectName , ResourceRoot type = ResourceRoot.Root ) {
        Object ob = null;
        switch ( type ) {
            case ResourceRoot.Root:
                ob = Resources.Load( objectName );
                break;
            case ResourceRoot.PrefabRoot:
                ob = Resources.Load( "Prefabs/" + objectName );
                break;
            case ResourceRoot.JsonRoot:
                ob = Resources.Load( "Jsons/" + objectName );
                break;
            case ResourceRoot.TextureRoot:
                ob = Resources.Load( "Textures/" + objectName );
                break;
            case ResourceRoot.AnimationRoot:
                ob = Resources.Load( "Animations/" + objectName );
                break;
            case ResourceRoot.AudioRoot:
                ob = Resources.Load( "Audios/" + objectName );
                break;
        }
        return ob;
    }

    public T Load<T> ( string objectName , ResourceRoot type = ResourceRoot.Root ) where T : Object {
        T ob = default( T );
        switch ( type ) {
            case ResourceRoot.Root:
                ob = Resources.Load<T>( objectName );
                break;
            case ResourceRoot.PrefabRoot:
                ob = Resources.Load<T>( "Prefabs/" + objectName );
                break;
            case ResourceRoot.JsonRoot:
                ob = Resources.Load<T>( "Jsons/" + objectName );
                break;
            case ResourceRoot.TextureRoot:
                ob = Resources.Load<T>( "Textures/" + objectName );
                break;
            case ResourceRoot.AnimationRoot:
                ob = Resources.Load<T>( "Animations/" + objectName );
                break;
            case ResourceRoot.AudioRoot:
                ob = Resources.Load<T>( "Audios/" + objectName );
                break;
        }
        return ob;
    }

}
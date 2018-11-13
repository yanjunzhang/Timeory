using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EditorTools : EditorWindow {

    [MenuItem( "Quibos/Editor Tools" )]
    public static void OpenWindows ( ) {
        EditorWindow window = GetWindow( typeof( EditorTools ) );
        window.Show( );
    }

    [MenuItem( "Quibos/Select App &A" , false , 0 )]
    public static void SelectApp ( ) {
        Object app = AssetDatabase.LoadAssetAtPath( "Assets/Resources/App.prefab" , typeof( GameObject ) );
        AssetDatabase.OpenAsset( app );
    }

    [MenuItem( "Quibos/Select MgrData &D" , false , 0 )]
    public static void SelectMgrData ( ) {
        Object app = AssetDatabase.LoadAssetAtPath( "Assets/Resources/App.prefab" , typeof( GameObject ) );
        GameObject data = ( app as GameObject ).transform.GetChild( 0 ).gameObject;
        AssetDatabase.OpenAsset( data );
    }

    

    [MenuItem( "Quibos/Help" , false , 100000 )]
    public static void Help ( ) {
        Debug.Log( "Help" );
    }

    void OnGUI ( ) {
        if ( GUILayout.Button( "Test" ) ) {
            Test( );
        }
    }

    void Test ( ) {
        Debug.Log( "Editor Tools Test!" );
    }
}

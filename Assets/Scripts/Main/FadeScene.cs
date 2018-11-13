using UnityEngine;
using System.Collections;

public class FadeScene : MonoBehaviour {

    Texture2D m_Tex;
    Color m_Color;
    float m_Dir;

    static FadeScene _Fade;
    public static FadeScene Fade {
        get {
            if ( _Fade ) {
                return _Fade;
            } else {
                GameObject go = new GameObject( );
                go.name = "Fade Scene";
                _Fade = go.AddComponent<FadeScene>( );
                return _Fade;
            }
        }
    }

    void Init ( float startA , float dir ) {
        DontDestroyOnLoad( gameObject );
        m_Color = Color.white;
        m_Color.a = startA;
        m_Dir = dir;
        m_Tex = new Texture2D( 1 , 1 );
        m_Tex.SetPixel( 0 , 0 , Color.white );
        m_Tex.Apply( );
    }

    void OnGUI ( ) {
        m_Color.a += m_Dir * 3f * Time.deltaTime;
        m_Color.a = Mathf.Clamp01( m_Color.a );
        GUI.color = m_Color;
        GUI.DrawTexture( new Rect( 0 , 0 , Screen.width , Screen.height ) , m_Tex );
    }

    public static void FadeIn ( ) {
        FadeScene.Fade.Init( 1 , -1f );
    }

    public static void FadeOut ( ) {
        FadeScene.Fade.Init( 0 , 1f );
    }

}
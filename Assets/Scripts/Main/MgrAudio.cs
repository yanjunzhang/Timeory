using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MgrAudio : MonoBehaviour {

    public static Transform AudioRoot;

    Dictionary<string , AudioSource> m_Audios = new Dictionary<string , AudioSource>( );

    public AudioSource Play ( GameObject go , string clipName , float volume ) {
        return Play( go , clipName , false , volume );
    }

    public AudioSource Play ( GameObject go , string clipName , bool loop = false , float volume = 1f , float pitch = 1 ) {

        AudioClip audioClip = App.MgrResource.Load( clipName , ResourceRoot.AudioRoot ) as AudioClip;

        if ( !audioClip ) {
            Debug.LogWarning( "There is no this audio clip : " + clipName );
            return null;
        }

        AudioSource audioSource = go.GetComponent<AudioSource>( );
        if ( audioSource == null ) {
            audioSource = go.AddComponent<AudioSource>( );
        }

        if ( !m_Audios.ContainsKey( clipName ) ) {
            m_Audios.Add( clipName , audioSource );
        }

        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.playOnAwake = false;
        audioSource.Play( );

        if ( !loop ) {
            DestroyAudio( clipName , audioClip.length );
        }

        return audioSource;

    }

    public AudioSource Play ( string clipName , float volume ) {
        return Play( clipName , false , volume );
    }

    public AudioSource Play ( string clipName , bool loop = false , float volume = 1f , float pitch = 1 ) {

        AudioSource audioSource;
        if ( m_Audios.TryGetValue( clipName , out audioSource ) ) {
            if ( audioSource ) {
                if ( !audioSource.isPlaying )
                    audioSource.Play( );
            } else {
                m_Audios.Remove( clipName );
                return Play( clipName , loop , volume , pitch );
            }
        } else {

            AudioClip audioClip = App.MgrResource.Load( clipName , ResourceRoot.AudioRoot ) as AudioClip;

            if ( !audioClip ) {
                Debug.LogWarning( "There is no this audio clip : " + clipName );
                return null;
            }

            GameObject go = new GameObject( "Audio:" + clipName );
            if ( !AudioRoot ) AudioRoot = new GameObject( "Audio Root" ).transform;
            go.transform.parent = AudioRoot;
            audioSource = go.AddComponent<AudioSource>( );

            m_Audios.Add( clipName , audioSource );

            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.playOnAwake = false;
            audioSource.Play( );

            if ( !loop ) {
                DestroyAudio( clipName , audioClip.length );
            }
        }

        return audioSource;

    }

    public AudioSource MultPlay ( string clipName , float volume = 1f , float pitch = 1 ) {

        AudioClip audioClip = App.MgrResource.Load( clipName , ResourceRoot.AudioRoot ) as AudioClip;

        if ( !audioClip ) {
            Debug.LogWarning( "There is no this audio clip : " + clipName );
            return null;
        }

        GameObject go = new GameObject( "Audio:" + clipName );
        if ( !AudioRoot ) AudioRoot = new GameObject( "Audio Root" ).transform;
        go.transform.parent = AudioRoot;
        AudioSource audioSource = go.AddComponent<AudioSource>( );

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.playOnAwake = false;
        audioSource.Play( );

        Destroy( go , audioClip.length );

        return audioSource;
    }

    public AudioSource RandomPlay ( string[ ] clipNames , bool loop = false ) {
        if ( loop ) {
            int i = Random.Range( 0 , clipNames.Length );
            return App.MgrAudio.Play( clipNames[i] , true );
        } else {
            int i = Random.Range( 0 , clipNames.Length );
            return App.MgrAudio.MultPlay( clipNames[i] );
        }
    }

    public AudioSource GetAudio ( string clipName ) {
        AudioSource audioSource;
        if ( m_Audios.TryGetValue( clipName , out audioSource ) ) {
            audioSource.Pause( );
        }
        return audioSource;
    }

    public void Pause ( string clipName ) {
        AudioSource audioSource;
        if ( m_Audios.TryGetValue( clipName , out audioSource ) ) {
            audioSource.Pause( );
        }
    }

    public void Stop ( string clipName ) {
        AudioSource audioSource;
        if ( m_Audios.TryGetValue( clipName , out audioSource ) ) {
            audioSource.Stop( );
        }
    }

    public bool IsPlaying ( string clipName ) {
        AudioSource audioSource;
        if ( m_Audios.TryGetValue( clipName , out audioSource ) ) {
            if ( audioSource.isPlaying ) {
                return true;
            } else {
                return false;
            }
        }
        return false;
    }

    public void DestroyAudio ( string clipName , float delay = 0 ) {
        App.MgrTool.YieldMethod( delay , ( ) => {
            AudioSource audioSource;
            if ( m_Audios.TryGetValue( clipName , out audioSource ) ) {
                if ( audioSource ) {
                    if ( audioSource.gameObject.name.Substring( 0 , 6 ) == "Audio:" ) {
                        Destroy( audioSource.gameObject );
                    } else {
                        Destroy( audioSource );
                    }
                }
                m_Audios.Remove( clipName );
            }
        } );
    }

    public void Clear ( ) {
        m_Audios.Clear( );
    }

}

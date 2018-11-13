using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MgrTool : MonoBehaviour {


    /// <summary>
    /// 循环随机执行方法结构
    /// </summary>
    public class RM {
        public GameObject Go;
        public System.Action Method;
        public System.Action<float> DurMethod;
        public float LastTime;
        public float Duration;
        public float Min;
        public float Max;
    }

    private List<RM> m_RMPool = new List<RM>( );


    //*********************************************************************************************************************************//

    /// <summary>
    /// 延时执行方法
    /// </summary>
    /// <param name="yieldTime"></param>
    /// <param name="method"></param>
    public void YieldMethod ( float yieldTime , System.Action method ) {
        StartCoroutine( IE_YieldMethod( yieldTime , method ) );
    }

    public void YieldMethod ( System.Action method ) {
        StartCoroutine( IE_YieldMethod( method ) );
    }

    private IEnumerator IE_YieldMethod ( float yieldTime , System.Action method ) {
        yield return new WaitForSeconds( yieldTime );
        if ( method != null ) {
            method( );
        }
    }

    private IEnumerator IE_YieldMethod ( System.Action method ) {
        yield return new WaitForEndOfFrame( );
        if ( method != null ) {
            method( );
        }
    }

    /// <summary>
    /// 循环随机执行方法
    /// </summary>
    /// <param name="method"></param>
    /// <param name="minDuration"></param>
    /// <param name="maxDuration"></param>
    public void RandomMethod ( GameObject gameObject , System.Action method , float minDuration , float maxDuration ) {
        RM rlm = new RM( );
        rlm.Go = gameObject;
        rlm.Method = method;
        rlm.Min = minDuration;
        rlm.Max = maxDuration;
        rlm.Duration = Random.Range( minDuration , maxDuration );
        rlm.LastTime = Random.Range( 0.0f , minDuration );
        m_RMPool.Add( rlm );
    }

    /// <summary>
    /// 循环随机执行方法
    /// </summary>
    /// <param name="method"></param>
    /// <param name="minDuration"></param>
    /// <param name="maxDuration"></param>
    public void RandomMethod ( GameObject gameObject , System.Action<float> method , float minDuration , float maxDuration ) {
        RM rlm = new RM( );
        rlm.Go = gameObject;
        rlm.DurMethod = method;
        rlm.Min = minDuration;
        rlm.Max = maxDuration;
        rlm.Duration = Random.Range( minDuration , maxDuration );
        rlm.LastTime = Random.Range( 0.0f , minDuration );
        m_RMPool.Add( rlm );
    }

    void RandomMethodListener ( ) {
        if ( m_RMPool.Count > 0 ) {
            for ( int i = 0; i < m_RMPool.Count; i++ ) {
                if ( !m_RMPool[i].Go ) {
                    m_RMPool.Remove( m_RMPool[i] );
                    continue;
                }
                if ( m_RMPool[i].LastTime > m_RMPool[i].Duration ) {
                    if ( m_RMPool[i].Method != null ) {
                        m_RMPool[i].Method( );
                    }
                    if ( m_RMPool[i].DurMethod != null ) {
                        m_RMPool[i].DurMethod( m_RMPool[i].Duration );
                    }
                    m_RMPool[i].Duration = Random.Range( m_RMPool[i].Min , m_RMPool[i].Max );
                    m_RMPool[i].LastTime = 0;
                } else {
                    m_RMPool[i].LastTime += Time.deltaTime;
                }
            }
        }
    }



    /// <summary>
    /// 限制随机整数
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="except"></param>
    /// <returns></returns>
    public int RandomExcept ( int from , int to , int except ) {
        int num = Random.Range( from , to );
        if ( num == except )
            return RandomExcept( from , to , except );
        else
            return num;
    }

    public void Clear ( ) {
        m_RMPool.Clear( );
        StopAllCoroutines( );
    }


    /// <summary>
    /// 应用内打开
    /// </summary>
    /// <param name="id"></param>
    public void OpenInApplication(string id)
    {
#if UNITY_IOS
        Prime31.StoreKitBinding.displayStoreWithProductId(id);
#endif
    }

    

    //*****************************************************************************************************************************//

    void Update ( ) {
        RandomMethodListener( );
    }

}

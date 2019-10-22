////18년 7월 10일 황재석
////카메라 매니져. 현재는 통로쪽을 지나가면 카메라가 정면을 응시하도록 바뀌면서 이동이 안되는 기능 구현

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CamMng : MonoBehaviour
//{
//    private ThirdPersonCamera thirdPersonCamera = null;
//    private EntranceCollider[] entranceColliders = null;//입구쪽 트리거 콜라이더
//    private EntranceColliderCtrl[] entranceColliderCtrls = null;//입구 다음부터 트리거 콜라이더
//    private EnemySpawnTrapTrigger[] enemySpawnTrapTriggers = null;/*출구쪽 트리거 콜라이더, 
//                                                                     충돌처리는 값이 많이 들어가기에 커플링 허용*/

//    // Use this for initialization
//    void Start()
//    {
//        thirdPersonCamera = this.gameObject.GetComponentInChildren<ThirdPersonCamera>();
//        if (thirdPersonCamera == null)
//        {
//            Debug.LogError("CamMng의 thirdPersonCamera is Null");
//        }

//        entranceColliders = this.gameObject.GetComponentsInChildren<EntranceCollider>();
//        for (int i = 0; i < entranceColliders.Length; ++i)
//        {
//            if (entranceColliders[i] == null)
//            {
//                Debug.LogError("CamMng의 entranceColliders[" + i + "] is Null");
//            }
//        }

//        entranceColliderCtrls = this.gameObject.GetComponentsInChildren<EntranceColliderCtrl>();
//        for(int i=0;i<entranceColliderCtrls.Length; ++i)
//        {
//            if (entranceColliderCtrls[i] == null)
//            {
//                Debug.LogError("CamMng의 entranceColliderCtrls[" + i + "] is Null");
//            }
//        }

//        enemySpawnTrapTriggers = this.gameObject.GetComponentsInChildren<EnemySpawnTrapTrigger>();
//        for (int i = 0; i < enemySpawnTrapTriggers.Length; ++i)
//        {
//            if (enemySpawnTrapTriggers[i] == null)
//            {
//                Debug.LogError("CamMng의 enemySpawnTrapTriggers[" + i + "] is Null");
//            }
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        foreach (EntranceCollider EC in entranceColliders)
//        {
//            if (EC.HasPlayer)
//            {
//                thirdPersonCamera.CanMove = false;
//            }
//            else
//            {
//                thirdPersonCamera.CanMove = true;
//            }
//        }

//        foreach(EntranceColliderCtrl EC_Ctrl in entranceColliderCtrls)
//        {
//            if (EC_Ctrl.HasPlayer)
//            {
//                thirdPersonCamera.CanMove = false;

//                Debug.Log("CamMng EC_Ctrl.HasPlayer 실행");
//            }
//        }

//        foreach (EnemySpawnTrapTrigger enemySpawnTrapTrigger in enemySpawnTrapTriggers)
//        {
//            if (enemySpawnTrapTrigger.HasPlayer)
//            {
//                thirdPersonCamera.CanMove = true;
//            }
//        }
//    }
//}

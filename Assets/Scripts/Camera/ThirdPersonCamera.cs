//18년 6월 25일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    //카메라가 바닥을 뚫거나 너무 올라가지 않도록 설정한 변수
    private const float Y_ANGLE_MIN = 0f;
    private const float Y_ANGLE_MAX = 60f;

    public Transform lookAt = null;//카메라가 쳐다볼 타겟(플레이어)
    private Transform camTransform = null;
    private PlayerController player = null;

    [Header("카메라와 플레이어와의 거리(초기 값)")]
    public float distance = 13f;

    [Header("카메라 회전 속도")]
    public float xSpeed = 220f;
    public float ySpeed = 100f;

    //마우스의 입력 값을 getAxis 계산 후 나온 각도를 저장하는 변수
    private float currentX = 0f;
    private float currentY = 50f;

    private float defaultDist = 0f;

    private void Start()
    {
        camTransform = this.gameObject.transform;
        player = lookAt.gameObject.GetComponent<PlayerController>();
        if (player == null)
        {
            Debug.LogError("ThirdPersonCamera의 player is Null");
        }
        defaultDist = distance;
    }

    private void Update()
    {
        //canMove는 플레이어가 통로에 들어가면 값이 조정됨.
        if (!player.IsInPassage)
        {
            //마우스 스크롤과의 거리계산
            distance -= 1 * Input.mouseScrollDelta.y;

            //마우스 스크롤했을경우 카메라 거리의 Min과Max
            //일정 거리가 넘거나 값이 너무 작으면 작동하지 않도록 만듬.
            if (distance < 3)
            {
                distance = 3;
            }

            if (distance >= 15)
            {
                distance = 15;
            }

            //마우스 오른쪽 버튼을 눌렀을때 작동.
            if (Input.GetMouseButton(1))
            {
                currentX += Input.GetAxis("Mouse X") * xSpeed * 0.015f;
                currentY -= Input.GetAxis("Mouse Y") * ySpeed * 0.015f;

                currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
            }
        }
        else
        {
            currentX = 0f;
            currentY = 20f;
            distance = 7f;
        }

        if (player.CanChangeCamSettingWithDefault)
        {
            ChangeCamPosWithDefault();
            player.CanChangeCamSettingWithDefault = false;
        }
    }

    private void LateUpdate()
    {
        //값이 없으면 실행 안됨. 플레이어가 죽었다던지, 인스펙터에서 설정을 안했다던지
        if (!lookAt)
            return;

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }

    public void ChangeCamPosWithDefault()
    {
        currentX = 0f;
        currentY = 50f;
        distance = defaultDist;
    }
}

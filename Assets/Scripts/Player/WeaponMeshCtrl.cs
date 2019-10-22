//18년 5월 9일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeshCtrl : NeedWeaponThingsForSystem
{
    private const float PIECE_ANGLE = 5f;  // 1폴리곤의 각도(원의 원만한 정도)

    private Mesh mesh = null;
    private MeshFilter meshFilter = null;
    private MeshCollider meshCollider = null;

    private bool isInitializationSuccess = false;

    public void DestroyWeaponMesh()
    {
        Destroy(this.gameObject);
    }

    public void MakeFanShape(float[] angle, float atkRangeDist)
    {
        Initialization();

        float startAngle; //원의 시작 각도.
        float endAngle;   //원의 종료 각도.
        float pieceAngle = PIECE_ANGLE; // 1폴러긴의 각도(원의 완만함).
        float radius = atkRangeDist; // 원의 반지름

        startAngle = angle[0];
        endAngle = angle[1];

        //준비
        //
        //

        if (Mathf.Abs(startAngle - endAngle) > 180f)
        {
            //0도 <-> 359도를 초과한다고 간주해 +360도 한다.
            if (startAngle < 180f)
            {
                startAngle += 360f;
            }
            if (endAngle < 180f)
            {
                endAngle += 360f;
            }
        }

        Vector3[] circleVertices;   // 원을 구성하는 각 폴리건 항목 좌표.
        int[] circleTriangles; //폴리건 면정보(정점 접속 정보).

        //시작 > 종료되면 교체한다.
        if (startAngle > endAngle)
        {
            float tmp = startAngle;
            startAngle = endAngle;
            endAngle = tmp;
        }

        //삼각형의 수.(ceil = 크거나 같은 가장 작은 정수로 바꿈.)
        int triangleNum = (int)Mathf.Ceil((endAngle - startAngle) / pieceAngle);

        //배열 확보.
        circleVertices = new Vector3[triangleNum + 1 + 1];
        circleTriangles = new int[triangleNum * 3];

        //폴리곤 작성.
        //
        //

        //정범 좌표를 계산.

        circleVertices[0] = new Vector3(0f, 0.5f, 0f);

        Vector3 objForwardPlusVector3Up = this.gameObject.transform.forward;
        objForwardPlusVector3Up.y = 1f;
        objForwardPlusVector3Up.z = 0f;

        for (int i = 0; i < triangleNum + 1; i++)
        {
            float currentAngle = startAngle + (float)i * pieceAngle;

            //지정값을 초과하지 않도록.
            currentAngle = Mathf.Min(currentAngle, endAngle);
            //angleAxis = 축 axis 주위를 angle 만큼 회전한 rotation을 생성합니다.
            circleVertices[1 + i] = Quaternion.AngleAxis(currentAngle, Vector3.up) * Vector3.forward * radius;
            circleVertices[1 + i] += circleVertices[0];
        }

        //인덱스

        for (int i = 0; i < triangleNum; i++)
        {
            circleTriangles[i * 3 + 0] = 0;
            circleTriangles[i * 3 + 1] = i + 1;
            circleTriangles[i * 3 + 2] = i + 2;
        }

        //메시 작성.
        //
        //


        mesh.Clear();

        mesh.vertices = circleVertices;
        mesh.triangles = circleTriangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        //mesh를 변경한 후 false-> true로 설정해야 반영된다.
        meshCollider.enabled = false;
        meshCollider.enabled = true;
    }

    private void Initialization()
    {
        if (!isInitializationSuccess)
        {
            mesh = new Mesh();
            if (!mesh)
            {
                Debug.LogError("웨폰메쉬컨트롤의 mesh Null");
                Debug.Break();
            }

            meshFilter = this.gameObject.GetComponent<MeshFilter>();
            if (!meshFilter)
            {
                Debug.LogError("웨폰메쉬컨트롤의 MeshFilter Null");
            }

            meshCollider = this.gameObject.GetComponent<MeshCollider>();
            if (!meshCollider)
            {
                Debug.LogError("웨폰메쉬컨트롤의 MeshCollider Null");
            }

            meshCollider.sharedMesh = mesh;

            isInitializationSuccess = true;
        }
    }
}
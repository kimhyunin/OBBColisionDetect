using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OBBColisionCheck : MonoBehaviour
{
    private GameObject target_obj;
    private GameObject player;
    private Bounds playerBounds;
    private Bounds target_objBound;

    private Vector3 target_objCenter;

    private Vector3 distance;
    public Vector3[] playerAxis;
    public Vector3[] target_objAxis;

    private float playerX, playerY, playerZ;

    private float target_objX, target_objY, target_objZ;
    public void Init(GameObject pPlayer, GameObject pTarget_obj)
    {
        player = pPlayer;
        target_obj = pTarget_obj;
        playerBounds = player.GetComponent<Renderer>().bounds;
        target_objBound = target_obj.GetComponent<Renderer>().bounds;

        target_objCenter = pTarget_obj.transform.position;

        target_objX = target_objBound.size.x * 0.5f;
        target_objY = target_objBound.size.y * 0.5f;
        target_objZ = target_objBound.size.z * 0.5f;
        Debug.Log(target_objBound.size);
    }

    public bool CheckStart()
    {
        // 플레이어 사이즈
        playerX = playerBounds.size.x * 0.5f;
        playerY = playerBounds.size.y * 0.5f;
        playerZ = playerBounds.size.z * 0.5f;
        // player와 타겟 사이의 거리
        distance = player.transform.position - target_objCenter;

        playerAxis = new Vector3[]{
                player.transform.right,
                player.transform.up,
                player.transform.forward
        };
        target_objAxis = new Vector3[]{
                target_obj.transform.right,
                target_obj.transform.up,
                target_obj.transform.forward
        };

        // player 기준 체크
        for (byte i = 0; i < playerAxis.Length; i++)
        {
            if (!IsCollisionCheck(playerAxis[i]))
            {
                return false;
            }
        }

        // target 기준 체크
        for (byte i = 0; i < target_objAxis.Length; i++)
        {
            if (!IsCollisionCheck(target_objAxis[i]))
            {
                return false;
            }
        }

        // player와 target 조합
        for (byte i = 0; i < playerAxis.Length; i++)
        {
            for (byte j = 0; j < target_objAxis.Length; j++)
            {
                // player와 target의 외적
                if (!IsCollisionCheck(Vector3.Cross(playerAxis[i], target_objAxis[j])))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsCollisionCheck(Vector3 pAxis)
    {
        // pAxis축에 대한 player 투영
        float RP = Mathf.Abs(Vector3.Dot((playerX * player.transform.right), (pAxis))) +
                    Mathf.Abs(Vector3.Dot((playerY * player.transform.up), (pAxis))) +
                    Mathf.Abs(Vector3.Dot((playerZ * player.transform.forward), (pAxis)));

        // pAxis축에 대한 target 투영
        float RC = Mathf.Abs(Vector3.Dot((target_objX * target_obj.transform.right), (pAxis))) +
                    Mathf.Abs(Vector3.Dot((target_objY * target_obj.transform.up), (pAxis))) +
                    Mathf.Abs(Vector3.Dot((target_objZ * target_obj.transform.forward), (pAxis)));

        // 두 중점 사이의 거리에 pAxis 투영
        if (Mathf.Abs(Vector3.Dot(pAxis, distance)) > RP + RC)
        {
            return false;
        }
        return true;
    }


}

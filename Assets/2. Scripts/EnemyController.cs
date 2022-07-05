using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    PlayerController player;
    Transform backMap;
    Detector detector;

    float moveSpeed = 7;
    Vector3 movePoint;

    void Awake()
    {
        backMap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Transform>();
        movePoint = new Vector3(transform.position.x, transform.position.y);
        detector = transform.GetChild(0).GetComponent<Detector>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //최초 목적지를 자신의 위치로 설정
    }

    void Update()
    {
        float moveX = backMap.transform.localScale.x / 2;
        float moveY = backMap.transform.localScale.y / 2;

        // 플레이어 감지시 플레이어를 쫓아감
        if (detector._isDetected)
        {
            movePoint = player.transform.position;
            MoveToPos(movePoint);
        }

        else
        {
            // 좌표 이동, 목적지 도착 시 새로운 목적지 랜덤으로 생성  
            if (Vector3.Distance(transform.position, movePoint) > 0.1f)
                MoveToPos(movePoint);
            else
                movePoint = new Vector3(Random.Range(-moveX, moveX), Random.Range(-moveY, moveY));
        }
    }

    void MoveToPos(Vector3 targetPos)
    {
        transform.position += (targetPos - transform.position).normalized * moveSpeed * Time.deltaTime;
    }
}

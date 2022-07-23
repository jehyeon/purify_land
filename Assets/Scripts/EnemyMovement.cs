using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    Animator animator;
    PlayerController player;
    Transform backMap;
    Detector detector;
    Spawner spawner;

    private float[] speeds = { 7, 5, 5, 3 };
    private float moveSpeed = 5;
    Vector3 spawnPoint;
    Vector3 movePoint;
    int index;

    void Awake()
    {
        animator = transform.GetComponentInChildren<Animator>();
        backMap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Transform>();
        movePoint = new Vector3(transform.position.x, transform.position.y);
        detector = GameObject.FindGameObjectWithTag("Detector").GetComponent<Detector>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spawnPoint = transform.position;
    }

    private void Start()
    {
        animator.SetBool("Run", true);
        animator.SetFloat("RunState", 0.5f);
        // 스포너의 생성 순서에 따라 속도를 변경.
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        index = spawner.Idx - 1;
        moveSpeed = speeds[index];
    }

    void Update()
    {
        float moveX = backMap.transform.localScale.x / 2;
        float moveY = backMap.transform.localScale.y / 2;

        // 플레이어 감지 부분
        if (detector.IsDetected)
        {
            movePoint = player.transform.position;
            MoveToPos(movePoint);
        }

        else
        {
            if (Vector3.Distance(transform.position, movePoint) > 0.1f)
                MoveToPos(movePoint);
            // 플레이어를 감지하지 못했을 경우, 다음 MovePoint로 이동.
            else
            {
                // 생성위치 주변에서 배회.
                float maxMoveX = spawnPoint.x + 5;
                float maxMoveY = spawnPoint.y + 3;
                movePoint = new Vector3(Random.Range(-maxMoveX, maxMoveX), Random.Range(-maxMoveY, maxMoveY));
                //movePoint = new Vector3(Random.Range(-moveX, moveX), Random.Range(-moveY, moveY));
            }
        }
    }

    void MoveToPos(Vector3 targetPos)
    {
        transform.position += (targetPos - transform.position).normalized * moveSpeed * Time.deltaTime;
    }
}

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
        //���� �������� �ڽ��� ��ġ�� ����
    }

    void Update()
    {
        float moveX = backMap.transform.localScale.x / 2;
        float moveY = backMap.transform.localScale.y / 2;

        // �÷��̾� ������ �÷��̾ �Ѿư�
        if (detector.IsDetected)
        {
            movePoint = player.transform.position;
            MoveToPos(movePoint);
        }

        else
        {
            // ��ǥ �̵�, ������ ���� �� ���ο� ������ �������� ����  
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

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform backMap;

    float moveSpeed = 10;
    Vector3 movePoint;

    void Awake()
    {
        backMap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Transform>();
        movePoint = new Vector3(transform.position.x, transform.position.y);
    }

    void Update()
    {
        float moveX = backMap.transform.localScale.x / 2;
        float moveY = backMap.transform.localScale.y / 2;


        // 좌표 이동, 목적지 도착 시 새로운 목적지 랜덤으로 생성  
        if (Vector3.Distance(transform.position, movePoint) > 0.1f)
            MoveToPos(movePoint);
        else
            movePoint = new Vector3(Random.Range(-moveX, moveX), Random.Range(-moveY, moveY));

    }

    void MoveToPos(Vector3 targetPos)
    {
        transform.position += (targetPos - transform.position).normalized * moveSpeed * Time.deltaTime;
    }
}

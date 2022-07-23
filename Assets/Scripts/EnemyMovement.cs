using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    Stat stat;
    Animator animator;
    ActionController player;
    Detector detector;
    Spawner spawner;

    [SerializeField]
    UnitCode unitcode;

    Transform backMap;
    Collider2D collider;
    Rigidbody2D rgb;

    private float[] speeds = { 3, 2, 2, 3 };
    private float moveSpeed = 5;
    Vector3 spawnPoint;
    Vector3 movePoint;
    int index;
    float time;
    bool damageOn = false;

    void Awake()
    {
        animator = transform.GetComponentInChildren<Animator>();
        backMap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Transform>();
        movePoint = new Vector3(transform.position.x, transform.position.y);
        detector = GameObject.FindGameObjectWithTag("Detector").GetComponent<Detector>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionController>();
        spawnPoint = transform.position;
        collider = GetComponent<Collider2D>();
        rgb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        animator.SetBool("Run", true);
        animator.SetFloat("RunState", 0.5f);
        // 스포너의 생성 순서에 따라 속도를 변경.
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        index = spawner.Idx - 1;
        moveSpeed = speeds[index];
        stat = new Stat();
        stat = stat.SetUnitStat(unitcode);
    }

    void Update()
    {
        if(damageOn)
            time += Time.deltaTime;

        rgb.velocity = Vector3.zero;
        rgb.angularVelocity = 0;

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

        if(stat.hp <= 0)
        {
            stat.hp = 0;
            moveSpeed = 0;
            collider.enabled = false;

            animator.SetFloat("RunState", 0);

            Destroy(gameObject, 1);
        }
    }

    void MoveToPos(Vector3 targetPos)
    {
        transform.position += (targetPos - transform.position).normalized * moveSpeed * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        stat.hp -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        damageOn = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (damageOn)
        {
            if (time > 1)
            {
                player.stat.hp -= stat.attack;
                time = 0;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        damageOn = false;
    }
}

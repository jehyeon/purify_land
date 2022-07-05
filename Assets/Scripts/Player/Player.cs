using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float h;
    private float v;
    private bool isHorizonMove;
    public Stat stat;
    public UnitCode unitCode;
    
    private float curTime;
    private float coolTime = 0.25f;
    public Vector2 boxSize; // 공격 판정 박스
    public Transform pos;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        stat = new Stat();
        stat = stat.SetUnitStat(unitCode);
    }

    // Update is called once per frame

    void Update()
    {
        Move();
        Attack();
    }

    private void FixedUpdate()
    {
        Vector2 moveVec = isHorizonMove 
            ? new Vector2(h, 0) 
            : new Vector2(0, v);

        rigid.velocity = moveVec * stat.speed;
        if(h > 0) transform.Rotate(0,0,90);
        if(h < 0) transform.Rotate(0, 0, -90);
        if(v > 0) transform.Rotate(0, 0, 0);
        if(v < 0) transform.Rotate(0, 0, 180);
    }

    private void Move()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonUp("Horizontal");
        bool vUp = Input.GetButtonUp("Vertical");

        if (hDown || vUp) isHorizonMove = true;
        else if (vDown || hUp) isHorizonMove = false;
    }
    
    private void Attack()
    {

        if (curTime <= 0)
        {
            // 공격
            if (Input.GetKey(KeyCode.K))
            {
                GameObject.Find("WoodBlock").GetComponent<WoodBlock>().TakeDamage(1);
                
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Enemy")
                    {
                        collider.GetComponent<WoodBlock>().TakeDamage(stat.attack);
                    }
                }
                curTime = coolTime; 
            }
            
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}

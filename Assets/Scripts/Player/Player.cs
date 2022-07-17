using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private InventoryUI inventory;
    
    private Rigidbody2D rigid;
    private float _h;
    private float _v;
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
        Attack();
    }

    private void FixedUpdate()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");
        Vector2 moveVec = new Vector2(_h, _v);

        rigid.velocity = moveVec * stat.speed;
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("Item"))
        {
            // collider.gameObject.SetActive(false);
            Debug.Log(collider.transform.GetComponent<ItemPickUp>().item.itemName + "획득");
            inventory.AcquireItem(collider.transform.GetComponent<ItemPickUp>().item);
            Destroy(collider.gameObject);
        }
    }
}

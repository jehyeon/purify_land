using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    //[SerializeField]
    //private InventoryUI inventory;
    private Rigidbody2D rigid;
    private float _h;
    private float _v;

    private Collider2D collider;
    private Collider2D _clickedEnemy;
    private Collider2D _clickedItem;
    private Collider2D[] _nearEnemies;
    private Collider2D[] _nearItems;
    private LayerMask _enemyLayerMask;
    private LayerMask _itemLayerMask;
    private Vector2 _mousePos;
    private bool _isKeyboardInput; // Ű����� ���콺 ���� ������ (�̱���)
    private GameObject _targetObject;
    public Stat stat;
    public UnitCode unitCode;
    public Animator animator;

    bool isDeath = false;
    float time;
    bool canAttack = true;

    void Start()
    {
        _enemyLayerMask = LayerMask.GetMask("Enemy");
        _itemLayerMask = LayerMask.GetMask("Item");
        stat = new Stat();
        stat = stat.SetUnitStat(unitCode);
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        GetMouseCommand();
        GetKeyboardCommand();
    }

    private void FixedUpdate()
    {
        if (!isDeath)
        {
            _h = Input.GetAxisRaw("Horizontal");
            _v = Input.GetAxisRaw("Vertical");
            Vector2 moveVec = new Vector2(_h, _v);
            bool isMove = moveVec.sqrMagnitude > 0.1f;
            animator.SetBool("isMove", isMove);
            if (_h > 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (_h < 0)
                transform.localScale = new Vector3(1, 1, 1);

            rigid.velocity = moveVec * stat.speed * 2;
        }

        if(stat.hp <= 0)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = 0;
            isDeath = true;
            stat.hp = 0;
            stat.speed = 0;
            collider.enabled = false;

            animator.SetBool("isDeath", true);
        }
    }

    GameObject GetClickedObject(Vector2 pos)
    {
        _clickedEnemy = Physics2D.OverlapPoint(pos, _enemyLayerMask); // layerMask �� �����Ͽ� enemy�� �����ϰų� interactive�� ����
        _clickedItem = Physics2D.OverlapPoint(pos, _itemLayerMask); // OverlapPoint(����Ʈ ��ġ, �������̾�)

        if (_clickedEnemy)
        {
            Debug.Log(_clickedEnemy.ToString());
            return _clickedEnemy.gameObject;
        }
        if (_clickedItem)
        {
            Debug.Log(_clickedItem.ToString());
            return _clickedItem.gameObject;
        }

        return null;
    }

    // ���� ����� ������Ʈ(��, ������) ��ȯ
    GameObject GetNearestObject(int keyCode) // keyCode: ���ϴ� ���̾��� ������Ʈ�� ��ȯ�ϱ� ���� ���
    {
        // �ֺ� ��, �������� ����Ʈȭ (������ġ, ��������, �������̾�)
        _nearEnemies = Physics2D.OverlapCircleAll(transform.position, 2.0f, _enemyLayerMask);
        _nearItems = Physics2D.OverlapCircleAll(transform.position, 2.0f, _itemLayerMask);

        if (_nearEnemies.Length > 0 && keyCode == 0) // �ֺ��� ���� �ִٸ�
        {
            _nearEnemies.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position)); // �Ÿ��� ������
            return _nearEnemies[0].gameObject; // ���� ������Ʈ�� ��ȯ
        }
        if (_nearItems.Length > 0 && keyCode == 1) // �ֺ��� �������� �ִٸ�
        {
            _nearItems.OrderBy(item => Vector2.Distance(transform.position, item.transform.position)); // �Ÿ��� ������
            return _nearItems[0].gameObject; // �������� ������Ʈ�� ��ȯ
        }
        return null; // �� �� ���ٸ� null ��ȯ
    }

    void GetMouseCommand()
    {
        //// ���콺 ��Ŭ��: �ݱ�, ����
        //if (Input.GetMouseButtonDown(0))
        //{
        //    _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Ŭ�� ��ġ
        //    _targetObject = GetClickedObject(_mousePos); // Ŭ���� ������Ʈ, ������ null ��ȯ

        //    // �̵�

        //    // �ݱ�

        //    // ����
        //}
        // ���콺 ��Ŭ��: �̵�


        // if(_targetObject is null && !_isKeyboardInput)
        //     transform.position = Vector2.MoveTowards(transform.position, _mousePos, Time.deltaTime * 2);     
    }

    void GetKeyboardCommand()
    {
        // F: ����� ���� ����
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _targetObject = GetNearestObject(0);
            if (_targetObject is null) return;
            if (_targetObject.CompareTag("Enemy")) //&& !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                if (_targetObject.transform.position.x - transform.position.x > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);
                if (canAttack)
                {
                    animator.SetTrigger("attack");
                    _targetObject.GetComponent<EnemyMovement>().TakeDamage(stat.attack);
                    Debug.Log("����");
                    canAttack = false;
                }
                //animator.SetBool("Attack", true);

            }
        }
        // �����̽���: ����� ������ ȹ�� 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _targetObject = GetNearestObject(1);
            if (_targetObject is null) return;
            if (_targetObject.CompareTag("Item"))
            {
                //inventory.AcquireItem(_targetObject.GetComponent<ItemPickUp>().item);
                Destroy(_targetObject);
            }
        }
    }
    void SetTriggerOff()
    {
        canAttack = true;
    }
}

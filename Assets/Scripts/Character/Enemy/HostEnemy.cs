using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostEnemy : MonoBehaviour
{
    private NetworkManager _network;
    private Enemy _enemy;
    private Detector _detector;
    private bool isPatrol = false;

    private void Start()
    {
        NetworkManager _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        _enemy = this.GetComponent<Enemy>();
    }

    private void Update()
    {
        if ((_enemy.StartPos - _enemy.transform.position).sqrMagnitude > _enemy.BackRange)
        {
            // 거리가 초과되는 경우
            _enemy.SetState((int)State.Back);
            SendStatePacket(State.Back);
        }

        if (_enemy.State == State.Idle)
        {
            if (!isPatrol)
            {
                StartPatrol();
            }
        }

        if (_enemy.State == State.Back)
        {
            if ((_enemy.transform.position - _enemy.StartPos).sqrMagnitude < 0.01)
            {
                _enemy.SetState((int)State.Idle);
                SendStatePacket(State.Idle);

                StartPatrol();
                return;
            }
        }
        //if ((_startPos - this.transform.position).sqrMagnitude > _startDiff)
        //{
        //    // 시작 지점에서 일정 거리 떨어지면 Back 모드
        //    BackMode();
        //}

        //if (_state == State.Back)
        //{
        //    // Back 모드일 때 처음 위치로 돌아가면 Idle 모드
        //    if ((_startPos - this.transform.position).sqrMagnitude < 0.02f)
        //    {
        //        IdleMode();
        //    }

        //    return;
        //}

        ////if (_state == State.Idle)
        ////{
        ////    // !!! 배회 (HostEnemy에서 처리해야 할듯)
        ////}

        //if (_state == State.Detect)
        //{
        //    if (_target == null)
        //    {
        //        IdleMode();
        //    }

        //    this.DestinationPos = _target.DestinationPos;

        //    if ((_startPos - this.transform.position).sqrMagnitude < _attackRange)
        //    {

        //    }
        //}


    }

    public void SetDetector(Detector detector)
    {
        _detector = detector;
        _detector.SetParent(this);
    }

    // -------------------------------------------------------------------------
    // Detect, Target
    // -------------------------------------------------------------------------
    public void DetectPlayer(Player target)
    {
        if (_enemy.State != State.Idle)
        {
            // Idle 상태일때만 감지
            return;
        }

        StopPatrol();
        //_enemy.DetectMode();
        _enemy.State = State.Detect;
        _enemy.Target = target;

        SendTargetPacket(target.PlayerId);
    }

    // -------------------------------------------------------------------------
    // 순찰
    // -------------------------------------------------------------------------
    private void StartPatrol()
    {
        StopPatrol();
        isPatrol = true;
        StartCoroutine("Patrol");
    }

    private void StopPatrol()
    {
        isPatrol = false;
        StopCoroutine("Patrol");
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            // start position을 기준으로 주변을 순찰
            float patrolX = _enemy.StartPos.x + Random.Range(-1f, 1f);
            float patrolY = _enemy.StartPos.y + Random.Range(-1f, 1f);
            _enemy.DestinationPos = new Vector3(patrolX, patrolY, 0f);

            SendMovePacket();

            yield return new WaitForSeconds(Random.Range(4f, 6f));
        }
    }

    // -------------------------------------------------------------------------
    // 서버 패킷 전송
    // -------------------------------------------------------------------------
    private void SendMovePacket()
    {
        C_EnemyMove movePacket = new C_EnemyMove();

        movePacket.id = _enemy.Id;
        movePacket.posX = _enemy.DestinationPos.x;
        movePacket.posY = _enemy.DestinationPos.y;

        if (_network == null)
        {
            _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        }

        _network.Send(movePacket.Write());
    }

    private void SendTargetPacket(int playerId)
    {
        C_EnemyTarget targetPacket = new C_EnemyTarget();

        targetPacket.id = _enemy.Id;
        targetPacket.playerId = playerId;

        if (_network == null)
        {
            _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        }

        _network.Send(targetPacket.Write());
    }

    private void SendStatePacket(State state)
    {
        C_EnemyState statePacket = new C_EnemyState();

        statePacket.id = _enemy.Id;
        statePacket.state = (int)state;

        if (_network == null)
        {
            _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        }

        _network.Send(statePacket.Write());
    }

    //IEnumerator DetectPlayer()
    //{
    //    while (true)
    //    {
    //        if (_enemy.Target == null)
    //        {
    //            IdleMode();
    //        }

    //        SendMovePacket();
    //        yield return new WaitForSeconds(0.25f);
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    protected override void Start()
    {
        base.Start();
        // !!! TEMP
        _backRange = 10f;
        _attackRange = .75f;
    }
}

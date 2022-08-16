using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private HostEnemy _parent;

    public void SetParent(HostEnemy parent)
    {
        _parent = parent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _parent.DetectPlayer(other.GetComponent<Player>());
        }
    }
}

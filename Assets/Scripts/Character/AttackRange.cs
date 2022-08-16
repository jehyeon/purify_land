using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private BoxCollider2D _collider;
    private List<Collider2D> _targets = new List<Collider2D>();
    public List<Collider2D> Targets { get { return _targets; } }

    private void Awake()
    {
        _collider = this.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        if (other.CompareTag("Enemy"))
        {
            _targets.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        if (other.CompareTag("Enemy"))
        {
            _targets.Remove(other);
        }
    }

    public void Activate()
    {
        _collider.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    Vector3 playerPos;
    bool isDetected = false;

    public bool _isDetected
    {
        get { return isDetected; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isDetected = true;
            Debug.Log("플레이어 감지됨.");
        }
    }
}

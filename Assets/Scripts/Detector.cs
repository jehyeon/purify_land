using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    Vector3 playerPos;
    private bool isDetected = false;

    public bool IsDetected
    {
        get { return isDetected; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isDetected = true;
            Debug.Log("�÷��̾� ������.");
        }
    }
}

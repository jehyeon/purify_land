using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
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
            Debug.Log("감지됨.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isDetected = false;
            Debug.Log("시야를 벗어남.");
        }
    }
}

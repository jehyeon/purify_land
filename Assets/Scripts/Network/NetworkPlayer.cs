using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    // !!! 파일명을 임시로 NetworkPlayer
    public int PlayerId { get; set; }
    // public int DestinationPos { get; set; }

    private void Update()
    {
        Debug.Log(this.transform.position);
    }

}

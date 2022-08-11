using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform temp = this.transform.GetChild(0);
        Debug.Log(temp);
        Zombie b = temp.GetComponent<Zombie>();
        Enemy a = temp.GetComponent<Enemy>();

        Debug.Log(b);
        Debug.Log(a);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

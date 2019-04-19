using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathTrigger : MonoBehaviour
{
    public bool isP1;
    // Start is called before the first frame update
    void Start()
    {
        isP1 = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            isP1 = true;
        }
    }
}

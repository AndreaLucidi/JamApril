using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float speed;
    private float XA;
    private float YA;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag == "P1")
        {
            XA = Input.GetAxis("HorizontalP1");
            YA = Input.GetAxis("VerticalP1");
        }
        if (this.gameObject.tag == "P2")
        {
            XA = Input.GetAxis("HorizontalP2");
            YA = Input.GetAxis("VerticalP2");
        }
        Vector3 pos = new Vector3(XA, 0, YA);
        transform.position += Vector3.ClampMagnitude(pos * speed, speed) * Time.deltaTime;
    }
}

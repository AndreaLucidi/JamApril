using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterwithPlayers : MonoBehaviour
{
    //public GameObject GameManager;
    public Material Interacted1;
    public Material Interacted2;
    public GameObject Opposite;
    public GameObject OtherG;
    // Start is called before the first frame update
    void Start()
    {
        //GameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<Renderer>().material = Interacted1;
                //GameManager.GetComponent<GameMan>().XofP1 = GetComponent<Tile>().x;
                //GameManager.GetComponent<GameMan>().YofP1 = GetComponent<Tile>().y;
                OtherG = GameObject.Find("Grid_2");
                Opposite = OtherG.gameObject.transform.Find(this.gameObject.name).gameObject;
                Opposite.GetComponent<Renderer>().material = Interacted1;
            }
        }
        if (other.CompareTag("P2"))
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                GetComponent<Renderer>().material = Interacted2;
                OtherG = GameObject.Find("Grid_1");
                Opposite = OtherG.gameObject.transform.Find(this.gameObject.name).gameObject;
                Opposite.GetComponent<Renderer>().material = Interacted2;
            }
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyDeath>().Tile = this.gameObject;
        }
    }
}

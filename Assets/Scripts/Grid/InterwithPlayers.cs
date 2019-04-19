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
    public Material startMat;
    public float RepairTime;
    // Start is called before the first frame update
    void Start()
    {
        //GameManager = GameObject.FindGameObjectWithTag("GameController");
        startMat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            /*if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<Renderer>().material = Interacted1;
                //GameManager.GetComponent<GameMan>().XofP1 = GetComponent<Tile>().x;
                //GameManager.GetComponent<GameMan>().YofP1 = GetComponent<Tile>().y;
                OtherG = GameObject.Find("Grid_2");
                Opposite = OtherG.gameObject.transform.Find(this.gameObject.name).gameObject;
                Opposite.GetComponent<Renderer>().material = Interacted1;
            }*/
            if (GetComponent<Renderer>().material.name == "BoxDes (Instance)")
            {
                Debug.Log("Morto");
            }
        }
        if (other.CompareTag("P2"))
        {
            if (GetComponent<Renderer>().material.name == "BoxDes (Instance)")
            {
                if (Input.GetKey(KeyCode.Keypad1)||Input.GetButton("ActionP2"))
                {
                    StartCoroutine("Repairing");
                }
                else
                {
                    StopCoroutine("Repairing");
                }
            }
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyDeath>().OrTile = this.gameObject;
        }
    }
    IEnumerator Repairing()
    {
        yield return new WaitForSeconds(RepairTime);
        GetComponent<Renderer>().material = startMat;
        OtherG = GameObject.Find("Grid_1");
        Opposite = OtherG.gameObject.transform.Find(this.gameObject.name).gameObject;
        Opposite.GetComponent<Renderer>().material = startMat;
    }
}

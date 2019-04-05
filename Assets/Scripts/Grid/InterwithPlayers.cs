using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterwithPlayers : MonoBehaviour
{
    public GameObject GameManager;
    public Material Interacted;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameController");
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
                GetComponent<Renderer>().material = Interacted;
                GameManager.GetComponent<GameMan>().XofP1 = GetComponent<Tile>().x;
                GameManager.GetComponent<GameMan>().YofP1 = GetComponent<Tile>().y;
            }
        }
    }
}

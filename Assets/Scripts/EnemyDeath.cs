using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public GameObject Tile;
    public GameObject Trigger;
    public GameObject OppTile;
    public GameObject otherG;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Trigger.GetComponent<EnemyDeathTrigger>().isP2)
        {
            Destroy(this.gameObject);
        }
        otherG = GameObject.Find("Grid_2");
        OppTile = otherG.gameObject.transform.Find(Tile.gameObject.name).gameObject;
        Trigger.transform.position = OppTile.transform.position;
    }
}

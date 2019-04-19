using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public GameObject OrTile;
    public GameObject Trigger;
    public GameObject OppTile;
    public GameObject otherG;
    public Material Destroyed;
    public Material Exploding;
    public float ExplosionTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Trigger.GetComponent<EnemyDeathTrigger>().isP1)
        {
            StartCoroutine(Explosion());
        }
        otherG = GameObject.Find("Grid_1");
        OppTile = otherG.gameObject.transform.Find(OrTile.gameObject.name).gameObject;
        Trigger.transform.position = OppTile.transform.position;
    }
    IEnumerator Explosion()
    {
        gameObject.GetComponent<Renderer>().material = Exploding;
        yield return new WaitForSeconds(ExplosionTime);
        OppTile.GetComponent<Renderer>().material = Destroyed;
        OrTile.GetComponent<Renderer>().material = Destroyed;
        Destroy(this.gameObject);
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("P2"))
        {
            Debug.Log("Morto 2");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Properties
    // PUBLIC
    public int x;
    public int y;
    // PRIVATE
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCoordinates(int x_, int y_)
    {
        x = x_;
        y = y_;
    }
}

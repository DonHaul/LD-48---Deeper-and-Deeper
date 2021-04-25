using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePart : MonoBehaviour
{

    public enum PartType { Head, Tail, None };

    public PartType parttype=PartType.None;
    //0 is up
    //1 is right 
    //2 is down
    //3 is left
    public bool[] attachable = new bool[4];

    // Start is called before the first frame update
    public void Shift()
    {
        //janky shift right

        bool aux0 = attachable[0];
        bool aux1 = attachable[1];
        bool aux2 = attachable[2];
        bool aux3 = attachable[3];

        attachable[1] = aux0;
        attachable[2] = aux1;
        attachable[3] = aux2;
        attachable[0] = aux3;





    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

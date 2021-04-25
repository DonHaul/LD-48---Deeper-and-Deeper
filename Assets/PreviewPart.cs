using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPart : MonoBehaviour
{

    public bool isColliding = false;

    public static PreviewPart instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("I started Colliding");

        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("I stopped Colliding");

        isColliding = false;
    }

}

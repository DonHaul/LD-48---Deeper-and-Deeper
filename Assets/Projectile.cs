using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public bool seenByPlayer;

    public float destroytime = 0f;

    void Start()
    {
        if(destroytime>0)
        {
            //Debug.Log("Destroying");
        Destroy(gameObject, destroytime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="MainCamera")
        {
            seenByPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MainCamera")
        {
            Destroy(gameObject);
        }
    }
}

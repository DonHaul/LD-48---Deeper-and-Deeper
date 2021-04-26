using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform[] nextChekpoint;

    float radius=3f;
    // Start is called before the first frame update
    void Start()
    {
        radius = GetComponent<CircleCollider2D>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

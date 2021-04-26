using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{

    public float destroytime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroytime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

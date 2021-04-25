using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnAwake : MonoBehaviour
{

    public float angularVelocity = 10f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().angularVelocity = angularVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Vehicle")
        {
            GameManager.instance.Win();
        }
    }
}

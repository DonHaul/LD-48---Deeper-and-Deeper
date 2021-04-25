using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public float interval = 5f;
    public GameObject[] shot;

    public bool active = false;

    public Vector2 horzLimits = new Vector2(-1, 1);
    public Vector2 vertLimits = new Vector2(-1, -1);


    public float radius = 30f;
    public LayerMask mask;

      

    // Start is called before the first frame update
    void Start()
    {

        active = true;
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if(Physics2D.OverlapCircle(transform.position,radius,mask) && active==true)
        {
            active = true;
         
            StartCoroutine(Shoot());
        }*/
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(Random.Range(0f, 5f));

        while(active)
        {
            

            GameObject go = Instantiate(shot[Random.Range(0,shot.Length)],transform.position,Quaternion.Euler(0,0,Random.Range(0,360f)));



            float x = Random.Range(horzLimits.x, horzLimits.y);
            float y = Random.Range(vertLimits.x, vertLimits.y);

            go.GetComponent<Rigidbody2D>().velocity= new Vector2(x, y);

            yield return new WaitForSeconds(interval);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="MainCamera")
        {
            active = false;
        }
    }
}

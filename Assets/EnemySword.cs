using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public GameObject target;
    public float speed = 5f;

    public float fireRate;


    public GameObject spawnpoint;

    public float interval = 5f;
    public GameObject[] shot;

    public bool active = true;


    public Vector2 horzLimits = new Vector2(-1, 1);
    public Vector2 vertLimits = new Vector2(-1, -1);

    public float projectileSpeed = 2f;

    public float radius = 5f;

    public LayerMask mask;

    Rigidbody2D rb = null;

    Quaternion q;

    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(horzLimits.x, horzLimits.y);
        float y = Random.Range(vertLimits.x, vertLimits.y);

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y);



        rb= GetComponent<Rigidbody2D>(); 

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //roatate toweard player

        if(Physics2D.OverlapCircle(transform.position,radius,mask))
        {

            Vector3 vectorToTarget = target.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = q;//Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

            rb.velocity = transform.up * speed* Mathf.Min(Vector3.Dot(transform.up,target.transform.position-transform.position),0.3f);
        }


    }


    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

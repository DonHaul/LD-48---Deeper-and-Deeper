using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : Enemy
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

    public float radius = 30f;
    public LayerMask mask;


    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(horzLimits.x, horzLimits.y);
        float y = Random.Range(vertLimits.x, vertLimits.y);

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y);

        

        active = false;

    }

    // Update is called once per frame
    void Update()
    {
        //roatate toweard player

        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Physics2D.OverlapCircle(transform.position, radius, mask) && active == false)
        {
            active = true;
            //Vector3 vectorToTarget = target.transform.position - transform.position;
            //float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            //Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            //transform.rotation = q;//Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

            //rb.velocity =transform.up * speed*Vector3.Dot(transform.up,target.transform.position-transform.position);

            StartCoroutine(Shoot());
        }
    }


    IEnumerator Shoot()
    {
        while (active)
        {
            

            GameObject go = Instantiate(shot[Random.Range(0, shot.Length)], spawnpoint.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));



            float x = Random.Range(horzLimits.x, horzLimits.y);
            float y = Random.Range(vertLimits.x, vertLimits.y);

            go.GetComponent<Rigidbody2D>().velocity = transform.up* projectileSpeed;

            yield return new WaitForSeconds(interval);

            AudioManager.instance.PlaySound("enemydeath1");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MainCamera")
        {
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MainCamera")
        {
            active = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

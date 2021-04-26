using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health;
    public float damage;

    public GameObject DeathParticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySound("enemydeath2");
            Instantiate(DeathParticle, transform.position,Quaternion.Euler(0,0,Random.Range(0,360f)));
        }
    }
}

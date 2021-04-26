using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;  

    public enum State { Edit, Play, Pause };

    public State gameState;

    public float tailSpeed=200f;
    public float headRotationSpeed=30f;

    public float totalSpeed;
    public float totalRotationSpeed;

    public float currentSpeed;

    public float speedIncrement;

    public GameObject vehicle;
    Rigidbody2D rb;

    public float initialForce=200f;

    public GameObject deathParticle;

    public Animator screenAnim;


    public Image black;

    int tryCount;

    public Text CounterText;

    public Text ZoneText;

    public float baseLife = 100;

    public float life;

    public int  deaths;

    public float calculatedLife;


    public Text lifeText;
    public Image lifeBar;


    public GameObject[] competitors;

    private void Awake()
    {
        instance = this;

        black.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {

        competitors = GameObject.FindGameObjectsWithTag("Competitor");

        gameState = State.Edit;

        rb = vehicle.GetComponent<Rigidbody2D>();

        tryCount = PlayerPrefs.GetInt("Try", 1);

        deaths = PlayerPrefs.GetInt("Deaths", 1);

        CounterText.text = "Day #" + tryCount.ToString();

        PlayerPrefs.SetInt("Try", tryCount + 1);
        PlayerPrefs.Save();

        //life increases with tries
        life = baseLife + deaths * 5;

        calculatedLife = life;

        ChangeLife(0);
    }

    public void SetZone(string zone)
    {
        ZoneText.text = zone;
    }

    public void ChangeLife(float add)
    {

       life += add;
        lifeText.text = Mathf.RoundToInt(life).ToString();
   lifeBar.fillAmount=life/ calculatedLife;

        if(life<=0)
        {
            Die();
        }

}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) )//|| (Input.GetKeyDown(KeyCode.Space) && gameState == State.Play))
        {
            StartCoroutine(FadeOut("Game"));
        }

        


    }
    // Update is called once per frame
    void FixedUpdate()
    {
   

        if (Input.GetKeyDown(KeyCode.Space) && gameState!= State.Play)
        {
            gameState = State.Play;
            CalculateStats();
            PrepareVehicle();


            rb.velocity = vehicle.transform.up * initialForce;

            AudioManager.instance.PlaySound("launch" + Random.Range(1, 4).ToString());


            //launch competitors
            for (int i = 0; i < competitors.Length; i++)
            {

                competitors[i].GetComponent<Rigidbody2D>().velocity = competitors[i].transform.up * (initialForce + Random.Range(18f, 22f));

                competitors[i].GetComponent<Competitor>().active = true;

            }

            //rb.AddForce(vehicle.transform.up * initialForce, ForceMode2D.Impulse);

        }

        if(gameState == State.Play)
        {
            if(Input.GetKey(KeyCode.W))
            {
                /* if(rb.velocity.magnitude<totalSpeed)
                 { 
                 rb.AddForce(v);
                 }*/
                rb.AddForce(vehicle.transform.up* totalSpeed);
                //Debug.Log("Accelerating");
            }

            if (Input.GetKey(KeyCode.S))
            {
                /* if(rb.velocity.magnitude<totalSpeed)
                 { 
                 rb.AddForce(v);
                 }*/
                rb.AddForce(-vehicle.transform.up * totalSpeed*0.5f);
                //Debug.Log("Accelerating");
            }



            float rotation = Input.GetAxisRaw("Horizontal");
            if(rotation!=0)
            { 
            rb.AddTorque(totalRotationSpeed*rotation*-1 );
            }
            /*if (rb.velocity.magnitude < totalSpeed)
              {
                  rb.AddForce(vehicle.transform.up * speedIncrement);
              }*/

        }
    }
    //magic is done here
    void CalculateStats()
    {
        Debug.Log("Processing Vehicle Stats");
        Debug.Log(VehicleEditor.instance.VehicleParts.Count);

        for (int i = 0; i < VehicleEditor.instance.VehicleParts.Count; i++)
        {

            GameObject part = VehicleEditor.instance.VehicleParts[i];

            switch (part.GetComponent<VehiclePart>().parttype)
            {
                case VehiclePart.PartType.Head:

                    totalRotationSpeed += headRotationSpeed;
                    // code block
                    break;
                case VehiclePart.PartType.Tail:
                    Debug.Log("Tail");
                    totalSpeed += tailSpeed;
                    Debug.Log(totalSpeed);
                    // code block
                    break;
                default:
                    Debug.LogWarning("It seems that this part is not attached to anything");
                    break;
            }

            

        }
    }

    public void Die()
    {
        Instantiate(deathParticle, vehicle.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
        StartCoroutine(FadeOut("DeathScene"));
    }

    public void Win()
    {

        StartCoroutine(FadeOut("WinScene"));
    }

    IEnumerator FadeOut(string sceneName)
    {
        screenAnim.SetTrigger("FadeOut");
        yield return new WaitUntil(() => black.color == Color.black);

        SceneManager.LoadScene(sceneName);

    }




    //magic is done here
    void PrepareVehicle()
    {
        Debug.Log("Destroying Child RigidBodies");

        for (int i = 0; i < VehicleEditor.instance.VehicleParts.Count; i++)
        {

            GameObject part = VehicleEditor.instance.VehicleParts[i];


            Destroy(part.GetComponentInChildren<Rigidbody2D>());

         }
    }
}

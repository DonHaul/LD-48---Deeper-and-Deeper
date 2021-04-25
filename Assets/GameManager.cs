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

    public Animator screenAnim;


    public Image black;

    int tryCount;

    public Text CounterText;

    public Text ZoneText;

    public float baseLife = 100;

    public float life;



    private void Awake()
    {
        instance = this;

        black.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = State.Edit;

        rb = vehicle.GetComponent<Rigidbody2D>();

        tryCount = PlayerPrefs.GetInt("Try", 1);


        CounterText.text = "Day #" + tryCount.ToString();

        PlayerPrefs.SetInt("Try", tryCount + 1);
        PlayerPrefs.Save();

        //life increases with tries
        life = baseLife + tryCount * 5;
    }

    public void SetZone(string zone)
    {
        ZoneText.text = zone;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(FadeOut("Game"));
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameState = State.Play;
            CalculateStats();
            PrepareVehicle();


            rb.velocity = vehicle.transform.up * initialForce;

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
        Debug.Log("Processing Vehicle Stats");

        for (int i = 0; i < VehicleEditor.instance.VehicleParts.Count; i++)
        {

            GameObject part = VehicleEditor.instance.VehicleParts[i];


            Destroy(part.GetComponentInChildren<Rigidbody2D>());

         }
    }
}

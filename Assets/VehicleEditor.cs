using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleEditor : MonoBehaviour
{

    public static VehicleEditor instance;  

    public GameObject[] VehiclePartsPrefabs;

    public int selectedId;

    public GameObject partPreview;

    public GameObject vehicleParent;

    public float partRotation = 0;
    public Vector3 partPosition = Vector3.zero;

    Vector3 anchorOffset = Vector3.zero;

    public List<GameObject> VehicleParts = new List<GameObject>();

    public LayerMask mask;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.gameState!=GameManager.State.Edit)
        {
            if(partPreview!=null)
            {
                Destroy(partPreview);
            }
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectPart(0);



        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectPart(1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotatePart();
        }

        if(Input.GetMouseButtonDown(0))
        {

            CheckAndPlaceItem();

        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;


            DeletePartAtPos(mousePos);
        }


        //make part preview foolow mouse
        partPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        partPosition.z = 0;
        //snap to grid
        partPosition.x = Mathf.Round(partPosition.x);
        partPosition.y = Mathf.Round(partPosition.y);

        if (partPreview!=null)
        {
            //relative to anchor
           // Debug.Log("Mouse");
           // Debug.Log(partPosition);
            partPreview.transform.position = partPosition;// - anchorOffset;
        }

    }

    public void RotatePart()
    {
        partRotation -= 90;
        partPreview.transform.Rotate(0, 0, -90);

        partPreview.GetComponent<VehiclePart>().Shift();
    }

    public void SelectPart(int id)
    {
        selectedId = id;
        if(partPreview!=null)
        {
            Destroy(partPreview);
        }

        partPreview = Instantiate(VehiclePartsPrefabs[id]);

        partPreview.transform.GetChild(0).gameObject.AddComponent<PreviewPart>();
        partPreview.transform.GetComponentInChildren<BoxCollider2D>().isTrigger = true;
        //reduce collider slightly so it dont collides wit just in the ege with neighoboring parts
        partPreview.transform.GetComponentInChildren<BoxCollider2D>().size = partPreview.transform.GetComponentInChildren<BoxCollider2D>().size * 0.9f;

        //set layer mask to not affect certain collisiosn
        partPreview.gameObject.layer = 9;
        partPreview.transform.GetChild(0).gameObject.layer = 9;


        anchorOffset =  partPreview.transform.GetChild(0).transform.position- partPreview.transform.position;
    }

    public void DeletePartAtPos(Vector3 pos)
    {
        int layerID = LayerMask.NameToLayer("Vehicle");  // 0-31
        int layerMask = 1 << layerID;                    // 2^layerID

        Collider2D col = Physics2D.OverlapPoint(pos, layerMask);

        if(col!=null)
        {
            GameObject part = col.transform.parent.gameObject;
            int i = VehicleParts.IndexOf(part);

            Debug.Log(i);
            Debug.Log(part.name);

            if(i==0)
            {
                Debug.Log("Cant Delete the Root Head");
            }
            else
            {
                Debug.Log("Removing");
                VehicleParts.RemoveAt(i);
                Destroy(part);

            }
        }else
        {
            Debug.Log("No Object Found");
        }
       
    }

    public void CheckAndPlaceItem()
    {

        VehiclePart part = partPreview.GetComponent<VehiclePart>();

        float rotation = 0;

        bool canbeattached = false;
        Debug.Log("Checking");

        //check if it is being attached somewhere
        for (int i = 0; i < part.attachable.Length; i++)
        {

            
            //if can be attached in this direction
            if (part.attachable[i]==true)
            {
                Debug.Log(i);
                Debug.Log(Quaternion.Euler(0, 0, rotation) * Vector2.up);
                Debug.Log(partPreview.transform.position + Quaternion.Euler(0, 0, rotation) * Vector2.up);

                Collider2D col = Physics2D.OverlapPoint(partPreview.transform.position + Quaternion.Euler(0, 0, rotation) * Vector2.up);
                if (col!=null)
                    {
                    Debug.Log("There is an object Here!");
                    Debug.Log(rotation);

                    Debug.Log((i + 2) % 4);
                    Debug.Log(col.gameObject.name);
                    Debug.Log(col.gameObject.GetComponentInParent<VehiclePart>().attachable.Length);
                    if(col.gameObject.GetComponentInParent<VehiclePart>().attachable[(i + 2) % 4] == true)
                    {
                        canbeattached = true;
                        Debug.Log("Can Be Placed!");
                        break;
                    }
                    
                   

                }
                else
                                { Debug.Log("NO Object!"); }
            }
            rotation -= 90;
        }

        //if parts isnt not hitting anythin, place item
        if(PreviewPart.instance.isColliding==false && canbeattached)
        {
            GameObject GO = Instantiate(VehiclePartsPrefabs[selectedId], partPosition, Quaternion.Euler(0, 0, partRotation), vehicleParent.transform);
            VehicleParts.Add(GO);
        }


    }
}

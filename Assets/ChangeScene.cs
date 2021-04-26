using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Animator screenAnim;

    public bool space2play = false;


    public Image black;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(FadeOut("Game"));
        }

        if(Input.GetKeyDown(KeyCode.Space) && space2play)
        {
            StartCoroutine(FadeOut("Game"));
        }
    }

    IEnumerator FadeOut(string sceneName)
    {
        screenAnim.SetTrigger("FadeOut");
        yield return new WaitUntil(() => black.color == Color.black);

        SceneManager.LoadScene(sceneName);

    }
}

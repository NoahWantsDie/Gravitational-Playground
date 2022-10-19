using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    public CanvasGroup cg;
    // Start is called before the first frame update
    void Start()
    {
        cg.alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            GameReset();
        }
        if(cg.alpha > 0)
        {
            cg.alpha -= 2f * Time.deltaTime;
            //print("faded");
        }
        
    }
    public void GameReset()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //print("reset");
    }
}

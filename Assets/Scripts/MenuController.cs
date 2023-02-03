using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MenuController : MonoBehaviour
{
    public Image loadingModel;
    // Start is called before the first frame update
    void Awake() {
        loadingModel.gameObject.SetActive(false);
    }
    public void GetMainMap() {
        loadingModel.gameObject.SetActive(true);

        StartCoroutine(LoadRandomMapScene(loadingModel.gameObject.GetComponentInChildren<TMP_Text>(), "Map"));
    }

    public void GetRandomMap() {
        loadingModel.gameObject.SetActive(true);

        StartCoroutine(LoadRandomMapScene(loadingModel.gameObject.GetComponentInChildren<TMP_Text>(), "RandomMap"));
    }

    public void DoOptions() {

    }

    public void Quit() {
        Application.Quit();
    }

    IEnumerator LoadRandomMapScene(TMP_Text text, string map)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(map, LoadSceneMode.Single);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                text.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public void GetMapSelection() {
        SceneManager.LoadScene("MapSelection", LoadSceneMode.Single);   
    }

    public void GetMap1() {
        SceneManager.LoadScene("Map1", LoadSceneMode.Single);   
    }
    public void GetTutorialMap() {
        SceneManager.LoadScene("TutorialMap", LoadSceneMode.Single);   
    }

    public void GetMainMenu() {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);   
    }

    public void DoOptions() {

    }

    public void Quit() {
        Application.Quit();
    }
}

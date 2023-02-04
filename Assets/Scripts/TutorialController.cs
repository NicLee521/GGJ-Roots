using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class TutorialController : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text body;
    public TMP_Text title;
    public GameObject tutorialUIParent;
    private string[] tutorialSteps = new string[]{"Introduction", "Roots", "Tiles", "Obstacles", "Resources", "Font"};
    public int step = 0;


    void Start()
    {
        tutorialUIParent.SetActive(false);
        title.text = "Welcome!";
        body.text = "Welcome to Roots of Life, a game created for the 2023 global game jam." +
            "The following will be a breif tutorial of how the game works! Press Continue to move on.";
        tutorialUIParent.SetActive(true); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceStep() {
        if(step == 0) {
            title.text = "Roots of Life";
            body.text = "Roots of life is a hex based resource management game where you play as a magical tree trying to replenish its life force by connecting to the corresponding magical fonts of power." +
                " As with anything there will be obstacles along your path and you only have so much of each magical resource to build your roots, so use them sparingly." +
                " Lets start with the \"root\" of the game. Go on and build your first root. The only stipulations are that a root can only be built connecting to another root and roots can not be built through rocks." +
                " You can also change the color of the root your going to place by the menu in the lower right";
            step += 1;
            return;
        }
        if(step == 5) {
            SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);   
        }
        step += 1;
        tutorialUIParent.SetActive(false);
    }

    public void PlayStep() {
        switch(tutorialSteps[step]) {
            case "Tiles":
                title.text = "Tiles";
                body.text = "In roots of life there are a couple different tiles. The swirly ones are resource tiles, when a root of the matching color connects to a resource tile you get a random amount of resources applied to your total. They are depleted after gathering their resources so be careful" +
                " Next are rock tiles, these block your path so that you have to go around them" +
                " The last tile type is the font entrance, these are the stone path looking tiles by the fonts. In order to connect to a font you must have at least 2 roots of the same color connected to the font tile and over 60% of the path to get there be the corresponding color."
                + " Go on and gather that resource there.";
                break;
            case "Obstacles":
                title.text = "Obstacles";
                body.text = "Slow down there! You'll notice some new rocks are ahead of you now. Be careful of the paths you choose because there could be hidden consequences anywhere";
                break;
            case "Resources":
                title.text = "Resources";
                body.text = "If you haven't already noticed there are some numbers at the bottom of your screen. These numbers correspond to the colors of roots you have in your aresnal" +
                " Each root costs 10 resources to make and the only way to get more resources is to connect to the resource tiles. I.e the one you just connected to. These resource tiles give you anywhere between 30 and 70 resources." +
                " All your resources are finite so be careful with the colors you build and where you build them" + 
                " We gave you an extra 30 resources to make sure you can get to the end. Now try to connect to the font";
                break;
            case "Font":
                title.text = "Fonts";
                body.text = "Congrats you've connected to the blue font of power! When playing normally you would have to connect to all 4 fonts before winning but since this is the tutorial we will leave it here." +
                "To play the real map just select Map 1 in the map selection screen! Good Luck!";
                break;
        }
        tutorialUIParent.SetActive(true);

    }
}

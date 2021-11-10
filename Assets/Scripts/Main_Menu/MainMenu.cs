using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Ye script main menu scene mein canvas naam kae object pae laga rakhi hai.
public class MainMenu : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This method is new game button and to link this method to new game button.
    //Just go to the new game button inspector screen under which there is a onCLick section in which you need to add the GameObject who has this code which is Canvas then in Canvas you need to select the Script File and then select this method make sure to make the method to public.
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

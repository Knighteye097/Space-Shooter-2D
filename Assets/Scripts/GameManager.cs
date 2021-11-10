using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;// This is a additional library whenever we want to do something regarding scenes we will use this

//Ye script jo hai woh humnae game scene mein ek empty GameObject Game_Manager pae laga rakhi hai.
//Look getting inputs in UI is a bad call so we create A GAME MANAGER to take the input and to make options available we make UI
public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;//This variable is used to stop the restart while playing the game and if player is alive
    private bool _isGameOn = false;
    private int _pauseMenuFlag = 1;//Ye variable isliyae liya hai kyonki humlog escape sae pause menu ko activate aur deactivate dono krna chahte hai. 1 ka mtlb false aur 0 ka mtlb true hai.

    private UIManager _uiManager;
    
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager in Game Manager Script is NULL!");
        }

    }
    
    void Update()
    {
        //Before Loading new scenes add them to build settings just go to file section Build Settings and add open scenes. After doing this you will also get a index number of the scene. Using index number of the scene to load it is faster than using Strings.
        if(Input.GetKeyDown("r") && _isGameOver == true)// If R key is pressed and then our game will restart.
        {
            SceneManager.LoadScene(1);// 1 index is for Game Scene, restart here means that we will move to the first scene which is Game in our Project. To find the name of the scene just go to the Project View(Folder management) section in unity which is just below Hierarchy section and look for a folder named scene there we will have all scenes with their names.
        }

        if (Input.GetKeyDown("escape") && _isGameOn == true && _pauseMenuFlag == 1)
        {
            Time.timeScale = 0;//(https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/) Refer to this link for better explanation  The scale at which time passes. This can be used for slow motion effects or to speed up your application.When timeScale is 1.0, time passes as fast as real time.When timeScale is 0.5 time passes 2x slower than realtime. When timeScale is set to zero your application acts as if paused if all your functions are frame rate independent. Negative values are ignored.
            _pauseMenuFlag = 0;// yha pae hum pause menu ko enable kr rhe hai toh uskae flag ko true(0) pae set kr rhe hai.
            _uiManager.enablePauseMenu();
        } 

        else if(Input.GetKeyDown("escape") && _isGameOn == true && _pauseMenuFlag == 0)
        {
            Time.timeScale = 1;
            _pauseMenuFlag = 1;// Yha pae hum pause menu ko disable kr rhe hai toh uskae flag ko false(1) pae set kr rhe hai.
            _uiManager.disablePauseMenu();
        }
    }

    //Now if the player is dead then to activate restart we are setting this variable to true we are calling this method in player's damage method.
    public void GameOver()
    {
        _isGameOver = true;
        _isGameOn = false;
    }

    public void GameOn()
    {
        _isGameOn = true;
    }
}

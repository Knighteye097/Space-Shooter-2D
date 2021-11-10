using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;// This is a additonal library whenever we will work on User Interface in Unity we need to add this.

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private Text _scoreText;
    
    [SerializeField]
    private GameObject _GameOverText;// 1)UI Text ko GameObject ki tarah bhi declare kr sakte ho

    [SerializeField]
    private Text _restartText;// 2)UI Text ko Text type ki tarah bhi declare kr sakte ho

    [SerializeField]
    private GameObject _instructionText;

    [SerializeField]
    private GameObject _pauseMenuPanel;
    private PauseMenuPanel _pauseMenuScript;
    
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprite; // Sprite jo hai 2D image hoti hai with graphics. Aur ye Sprite type ka array hai

    // Start is called before the first frame update
    void Start()
    {
        _pauseMenuScript = _pauseMenuPanel.GetComponent<PauseMenuPanel>();

        if(_pauseMenuScript == null)
        {
            Debug.LogError("The Pause Menu Script in UI manager is NULL!");
        }
        
        _scoreText.text = "Score: " + 0;// This is a way how you can write String and Int type values together.
        _GameOverText.SetActive(false);// Agar text ko GameObject ki tarah declare kiya hai toh aise access kr sakte hai
        _restartText.gameObject.SetActive(false);// Agar text ko text ko Text ki tarah declare kiya hai toh aise access krna hai!
        _instructionText.SetActive(true);//Dekho yae how to play game screen pae ayengae 
        _pauseMenuPanel.SetActive(false);// Ye isliyae kyonki hum nhi chahte ki galti sae bhi hamara pause menu game start honae pae active ho jaye ye baad mein humlog manually on krengae.
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    
    //This method is written to update score of the player on the game screen using UI text.
    public void scoreUpdate(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore; // or we can also use playerScore.ToString();
    }

    //This method is written to update the sprite of lives on the game screen.
    public void livesUpdate(int curr_live)
    {
        _livesImg.sprite = _livesSprite[curr_live];
    }

    //This method is to Enable GameOver Text after the player is dead.
    public void enableGameOver()
    {
        _GameOverText.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine("GameOverFlickerRoutine");
    }

    //The Game Over Flicker Routine is creater here
    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            _GameOverText.SetActive(false);
            _restartText.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);
            _GameOverText.SetActive(true);
            _restartText.gameObject.SetActive(true);
        }    
    }
    
    //ye funtion tab call hoga jaise hi hamara player pehli baar laser fire krega tab hamara instruction text disable ho jaye. 
    public void disableInstructionText()
    {
        _instructionText.SetActive(false);
    }

    public void enablePauseMenu()
    {
        _pauseMenuPanel.SetActive(true);
        _pauseMenuScript.enablePauseMenuRoutine();
    }

    public void disablePauseMenu()
    {
        _pauseMenuPanel.SetActive(false);
    }
}

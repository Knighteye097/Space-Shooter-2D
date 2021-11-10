using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI Manager ki script mein reasin hai iska
using UnityEngine.SceneManagement; 

public class PauseMenuPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenuText;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }

    public void mainMenuReturn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void enablePauseMenuRoutine()
    {
        StartCoroutine("pauseMenuTextRoutine");
    }

    IEnumerator pauseMenuTextRoutine()
    {
        while (true)
        {
            _pauseMenuText.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            _pauseMenuText.SetActive(false);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}

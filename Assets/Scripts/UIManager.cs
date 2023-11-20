using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private bool gameIsPaused;

    [Header("Options")]
    [Space]
    private bool inSettings;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject settingsButton;

    [Header("Credits")]
    [Space]
    private bool InCredits;
    [SerializeField] private GameObject creditMenuCanvas;
    [SerializeField] private GameObject creditMenuButton;
    [SerializeField] private GameObject quitButton;

    [Header("Restart")]
    [Space]
    [SerializeField] private GameObject restartButton;

    [Header("Next & Previous Level")]
    [Space]
    [SerializeField] private GameObject NextLevel;
    [SerializeField] private GameObject PreviousLevel;

    [Header("In-Game Menu")]
    [Space]
    [SerializeField] private GameObject InGameMenu;
    public TextMeshProUGUI SpeedoMeter;

    public UnityEngine.UI.Image speedOMeter;
    public UnityEngine.UI.Image map;
    public TextMeshProUGUI countdownText;
    private Color transparent;


    private void Start()
    {
        transparent= new Color(255,255,255,0);
        MainMenuButtonStates(settingsButton, creditMenuButton, quitButton, restartButton, NextLevel, PreviousLevel, false);
        speedOMeter.color = transparent;
        map.color = transparent;
    }

    private void Update()
    {
        PauseManagement();
    }

    /// <summary>
    /// This function gets activated every frame in Update(), and checks for "ESC" input.
    /// </summary>
    private void PauseManagement()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)//this bool gets controllerd in the Resume and Pause Functions
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }

        BackToMainMenu(settings, inSettings, true);
        BackToMainMenu(creditMenuCanvas, InCredits, true);
    }

    /// <summary>
    /// This function gets used to "Resume" the flow of the game.
    /// </summary>
    private void Resume()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        MainMenuButtonStates(settingsButton, creditMenuButton, quitButton, restartButton, NextLevel, PreviousLevel, false);
    }

    /// <summary>
    /// This function gets used to "Pause" the flow of the game.
    /// </summary>
    private void Pause()
    {
        Time.timeScale = 0f;
        gameIsPaused = true;
        MainMenuButtonStates(settingsButton, creditMenuButton, quitButton, restartButton, NextLevel, PreviousLevel, true);
    }
    public void UIQuit()
    {
        GameManager.Instance.Quit();
    }
    
    #region Options & Credits functionality
    /// <summary>
    /// This function controls the behaviour of the option screen
    /// </summary>
    public void Options()//wordt alleen gebruikt zodra je op de button drukt
    {
        if (inSettings == false)
        {
            settings.SetActive(true);
            inSettings = true;
            MainMenuButtonStates(settingsButton, creditMenuButton, quitButton, restartButton, NextLevel, PreviousLevel, false);
        }
        else
        {
            settings.SetActive(false);
            inSettings = false;
            settingsButton.SetActive(true);
        }
    }

    /// <summary>
    /// This function controls the behaviour of the credit screen
    /// </summary>
    public void Credits()//wordt alleen gebruikt zodra je op de button drukt (nog met UIManager implementen)
    {
        if (InCredits == false)
        {
            creditMenuCanvas.SetActive(true);
            InCredits = true;
            MainMenuButtonStates(settingsButton, creditMenuButton, quitButton, restartButton, NextLevel, PreviousLevel, true);
        }
        else
        {
            creditMenuCanvas.SetActive(false);
            InCredits = false;
            creditMenuButton.SetActive(true);
        }
    }
    #endregion

    /// <summary>
    /// Fade's in the UI using the "fadeIn" as a bool to check and the image to interpolate the color.a from 0 to 255
    /// </summary>
    /// <param name="fadeIn"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    public IEnumerator FadeInUI(bool fadeIn, UnityEngine.UI.Image image)
    {
        if (fadeIn)
        {
            for (float i = 0; i <= 1; i += 0.33f * Time.deltaTime)
            {
                image.color = new Color(255, 255, 255, i);
                yield return null;
            }
        }
    }

    /// <summary>
    /// This function counts down at the start of the game.
    /// </summary>
    /// <returns></returns>
    public IEnumerator countDown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownText.text = "GO!";
        //needs slower fade out
        GameManager.Instance.playerMovementScript.enabled = true;
        yield return new WaitForSeconds(1);
        countdownText.text = "";
        countdownText.gameObject.active = false;//fuck you imma use this even tho is obsolete
    }

    /// <summary>
    /// This function call the restartlevel function from the gamemanager
    /// </summary>
    public void ResetButton()
    {
        GameManager.Instance.RestartLevel();
        MainMenuButtonStates(settingsButton, creditMenuButton, quitButton, restartButton, NextLevel, PreviousLevel, false);
    }

    /// <summary>
    /// This Function gets called when going to the next level/the next level button gets clicked
    /// </summary>
    public void GoToPreviousLevel()
    {
        GameManager.Instance.PreviousLevel();
    }

    /// <summary>
    /// This Function gets called when going to the previous level/the previous level button gets clicked
    /// </summary>
    public void GoToPreviousNext()
    {
        GameManager.Instance.NextLevel();
    }

    #region Code cleanup functions
    /// <summary>
    /// This function serves to clean up code
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="state"></param>
    /// <param name="gameObject1"></param>
    /// <param name="state1"></param>
    /// <param name="gameObject2"></param>
    /// <param name="state2"></param>
    /// <param name="gameObject3"></param>
    /// <param name="state3"></param>
    private void MainMenuButtonStates(GameObject gameObject, GameObject gameObject1, GameObject gameObject2, GameObject gameObject3, GameObject gameObject4,GameObject gameObject5, bool state)
    {
        gameObject.SetActive(state);
        gameObject1.SetActive(state);
        gameObject2.SetActive(state);
        gameObject3.SetActive(state);
        gameObject4.SetActive(state);
        gameObject5.SetActive(state);
    }

    /// <summary>
    /// this function gets called instead of all the individual menu's
    /// </summary>
    /// <param name="currentMenu"></param>
    /// <param name="InXMenu"></param>
    /// <param name="Enable"></param>
    private void BackToMainMenu(GameObject currentMenu, bool InXMenu, bool Enable)
    {
        if (InXMenu && Input.GetKeyDown(KeyCode.Escape))//go back to pausemenu
        {
            currentMenu.SetActive(false);
            InXMenu = false;
            creditMenuButton.SetActive(Enable);
            settingsButton.SetActive(Enable);
            quitButton.SetActive(Enable);
            restartButton.SetActive(Enable);
            NextLevel.SetActive(Enable);
            PreviousLevel.SetActive(Enable);
            Time.timeScale = 0;
            gameIsPaused = Enable;
        }
    }
    #endregion
}

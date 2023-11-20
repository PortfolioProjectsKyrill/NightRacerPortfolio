using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> Checkpoints;
    private UIManager UIManager;
    
    [HideInInspector] public Vector3 StartCheckpointPos;
    public Vector3 FinishCheckpointPos;

    private CheckPointScript[] checkPointScript;

    [HideInInspector] public PlayerMovement playerMovementScript;

    [Header("Respawning")]
    [Space]
    [SerializeField] private float RespawnTimer;
    [SerializeField] private bool MayRespawn;

    [SerializeField] private Transform Player;

    [Header("EndGame/Track")]
    [Space]
    public float DimmerTimer;
    [SerializeField] private GameObject dimImage;

    private DimmerScript dimmerScript;
    private UIManager uIManager;

    private void Awake()
    {
        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else{
            Instance = this;
        }

        playerMovementScript = FindObjectOfType<PlayerMovement>();
        UIManager = FindObjectOfType<UIManager>();
        checkPointScript = FindObjectsOfType<CheckPointScript>();//multiple objects (works as normal array with indeces and such)
        uIManager = FindObjectOfType<UIManager>();
    }
    void Start() {
        StartCoroutine(StartGame());
        dimmerScript = FindObjectOfType<DimmerScript>();
        MayRespawn = true;
    }

    void Update()
    {
        Respawn();
        CalculateSpeed();
    }

    /// <summary>
    /// This function controls the respawn of the player (Gets called every frame)
    /// </summary>
    private void Respawn()
    {
        if (Input.GetKey(KeyCode.R) && MayRespawn)
        {
            dimmerScript.StartDimmingAnim();
            for (int i = 0; i < checkPointScript.Length; i++)
            {
                #if (UNITY_EDITOR)
                if (checkPointScript[i].isStart)//only necessary for testing/show the start checkpoint x & y pos
                    print(checkPointScript[i].name + "cords are: " + StartCheckpointPos);
                #endif
            }

            RespawnTimer += Time.deltaTime;
            DimmerTimer += Time.deltaTime;
            //screen dimming needed
            if (RespawnTimer >= 3)
            {
                MayRespawn = false;
                if (Checkpoints.Count != 0)//voor als er meerdere checkpoints zijn
                {
                    playerMovementScript.transform.position = Checkpoints[Checkpoints.Count - 1].transform.position;//count -1 bc of indexing
                    playerMovementScript.transform.rotation = Checkpoints[Checkpoints.Count - 1].transform.rotation;
                    RespawnTimer = 0;
                    DimmerTimer = 0;
                }
                else//voor als er alleen maar een start checkpoint
                {
                    playerMovementScript.transform.position = Checkpoints[0].transform.position;
                    playerMovementScript.transform.rotation = Checkpoints[0].transform.rotation;
                    RespawnTimer = 0;
                    DimmerTimer = 0;
                }
            }
        }
        else if (!Input.GetKey(KeyCode.R))//if there is not inputs
        {
            MayRespawn= true;
            RespawnTimer = 0;
            DimmerTimer = 0;
            if (RespawnTimer <= 2.5f)//als respawntimer niet kleiner is dan 2.5 (FUNCTIONALITY NEEDS TO BE MOVED)
            {
                dimmerScript.SetToZero();
            }
            else if (RespawnTimer > 2.5f)
            {
                dimmerScript.StartDimmingAnim();
            }
        }
    }

    /// <summary>
    /// Fade's in all the UI Components (NEEDS REIMPLEMENTATION since real objects won't be images)
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartGame()
    {
        playerMovementScript.enabled = false;
        StartCoroutine(UIManager.FadeInUI(true, UIManager.speedOMeter));//true asks if you wanna fade in or not
        StartCoroutine(UIManager.FadeInUI(true, UIManager.map));
        StartCoroutine(UIManager.countDown());
        yield return null;
    }

    /// <summary>
    /// NEESD IMPLEMENTATION
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndTrack()
    {
        yield return new WaitForSeconds(3);
        //set all vars to default states/values
        //deactivate UI Elements lke speed, timers etc.
        //activate End/or/Death screen
    }

    /// <summary>
    /// Enables all the checkpoint hitboxes & clears list
    /// </summary>
    private void ResetAllCheckpoints()
    {
        foreach (GameObject checkpoint in Checkpoints)
        {
            checkpoint.GetComponent<BoxCollider>().enabled = true;
        }
        Checkpoints.Clear();
    }

    /// <summary>
    /// Quits the application when clicked on
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 
    /// </summary>
    public void RestartLevel()//to call from UIManager
    {
        Time.timeScale = 1;
        dimmerScript.StartDimmingAnim();//from dimmerscript
        playerMovementScript.enabled = false;
        SpawnPlayerAtStart();
        //whole canvas gets turned off in UIManager.cs
    }

    /// <summary>
    /// Spawns player at the "Start checkpoint" & clears the checkpoint list using ResetAllCheckpoints();
    /// </summary>
    public void SpawnPlayerAtStart()
    {
        Player.transform.position = Checkpoints[0].transform.position;
        Player.transform.rotation = Checkpoints[0].transform.rotation;
        ResetAllCheckpoints();
        RespawnTimer = 0;
        playerMovementScript.enabled = true;
    }

    /// <summary>
    /// Switches the scene to the next one as shown in the buildsettings of Unity
    /// </summary>
    public void NextLevel()
    {
        SceneManagerScript.instance.SwitchScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// If you aren't already on the first index the scene before current scene gets loaded
    /// </summary>
    public void PreviousLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManagerScript.instance.SwitchScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            Debug.LogError("No Previous Scene Could Be Loaded Because You Are On The First Scene");//Throws error
        }
    }

    public void CalculateSpeed()
    {
        float speed = (playerMovementScript.currentSpeed * 3.6f);
        uIManager.SpeedoMeter.text = " " + string.Format("{0:0.0}", speed) + "\n" + "Km/H";
    }
}

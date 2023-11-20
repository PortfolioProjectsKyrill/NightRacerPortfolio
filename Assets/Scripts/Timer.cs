using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI[] lapTimes;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private GameObject[] lapOne;
    [SerializeField] private GameObject[] lapTwo;
    [SerializeField] private GameObject[] lapThree;
    private float startTime;

    public void GameHasEnded()
    {
        // t = the time - starttime
        float t = Time.time - startTime;
        // if t is smaller then getfloat, get the fastets time
        if (t < PlayerPrefs.GetFloat("Fastest Time:"))
        {
            PlayerPrefs.SetFloat("Fastest Time:", t);
            highScore.text = t.ToString();
        }
    }

    private void Start()
    {
        lapTwo[2].SetActive(false);
        lapThree[2].SetActive(false);

        startTime = Time.time;
        // Highscore text = get the float from fastest time
        highScore.text = PlayerPrefs.GetFloat("Fastest Time: ", startTime).ToString();
        Debug.Log(highScore.text);
    }

    public void Update()
    {
        float t = Time.time - startTime;
        // Minute timer
        string minutes = ((int)t / 60).ToString();
        // Second timer
        string seconds = (t % 60).ToString("f2");
        // Set the text to Time: Mins, Secs
        timerText.text = "Time: " + minutes + " : " + seconds;
    }

    public void OnTriggerEnter(Collider other)
    {
        GameHasEnded();
        if (other.gameObject.tag == "Lap One")
        {
            lapTimes[0].text = Time.time.ToString("Lap 1 : 0.00");
            highScore.text.ToString();
            lapOne[2].SetActive(false);
            lapTwo[2].SetActive(true);
        }
        else if (other.gameObject.tag == "Lap Two")
        {
            lapTimes[1].text = Time.time.ToString("Lap 2 : 0.00");
            lapTwo[2].SetActive(false);
            lapThree[2].SetActive(true);
        }
        else if (other.gameObject.tag == "Lap Three")
        {
            lapTimes[2].text = Time.time.ToString("Lap 3 : 0.00");
            lapThree[2].SetActive(false);
        }
    }
}

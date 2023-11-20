using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    [Header("Start & Finish Bools")]
    [Space]
    [Tooltip("Enable isStart bool if you want to have the reset point be this checkpoint")]
    public bool isStart;
    [Tooltip("Enable isFinish if you want this checkpoint to be the finish")]
    public bool isFinish;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(!isStart || !isFinish)
            gameObject.name = ("Checkpoint " + GameManager.Instance.Checkpoints.Count);
            GameManager.Instance.Checkpoints.Add(gameObject);

            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void Start()
    {
        if (isStart || isFinish)
        {
            GameManager.Instance.Checkpoints.Add(gameObject);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        if (isStart)//stuur positie naar de gamemanager
        {
            gameObject.name = ("Start Checkpoint");
            GameManager.Instance.StartCheckpointPos = StartPos();
        }
        else if (isFinish)
        {
            gameObject.name = ("Finish Checkpoint");
            GameManager.Instance.FinishCheckpointPos = FinishPos();
            //send finish cords to var in gamemanager for cutscene purposes
        }

    }

    public Vector3 StartPos()
    {
        return transform.position;
    }

    public Vector3 FinishPos()
    {
        return transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    bool inRangeOfDoor = false;
    public bool InRangeOfDoorSignal { get { return inRangeOfDoor; } }
    public AudioClip doorOpenSound;

    public string doorCode;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inRangeOfDoor = true;
        UIHandler.instance.DisplayKeypad();
        // Set this exit door as exit door UI Handler will call CheckDoorCode on
        UIHandler.instance.exitDoor = gameObject.GetComponent<ExitDoor>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        inRangeOfDoor = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inRangeOfDoor = false;
        UIHandler.instance.DisplayKeypad();
    }

    public void CheckDoorCode(string enteredCode)
    {
        if (enteredCode == doorCode)
        {   
            UIHandler.instance.keypadScreenText = "";
            Debug.Log("Correct Code");
            // todo play correct alert sound
            // todo play door opening sound
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Wrong Code");
            StartCoroutine(UIHandler.instance.WrongCodeAlert());
        }
    }
}

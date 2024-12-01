using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stairway : MonoBehaviour
{
    public string nextLevel;
    public string currentLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameController.instance.WinLevel(nextLevel, currentLevel);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Move camera with player position while restricting to within boundary of map
        float playerPosX = player.transform.position.x;
        float playerPosY = player.transform.position.y;
        float cameraPosX = Mathf.Clamp(playerPosX, -5, 5);
        float cameraPosY = Mathf.Clamp(playerPosY, -5, 5);
        transform.position = new Vector3(cameraPosX, cameraPosY, -10);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    AudioSource audioSource;

    Vector2 move;
    public float moveSpeed = 2.0f;
    float buttonPressBuffer = .25f;

    bool canPressTab = true;
    bool canPressShift = true;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.player = gameObject.GetComponent<PlayerController>();
        UIHandler.instance.player = gameObject.GetComponent<PlayerController>();
        UIHandler.instance.playerInventory = gameObject.GetComponent<Inventory>();
        audioSource = GetComponent<AudioSource>();


        if (GameController.instance.upgradedSpeed)
        {
            moveSpeed = GameController.instance.upgradeSpeed;
            Debug.Log("speed upgrade carryover");
        }

        if (GameController.instance.upgradedSight)
        {
            GameObject.Find("Player Light 2D").GetComponent<Light2D>().pointLightOuterRadius = GameController.instance.upgradeSightRadius;
        }

        rigidbody2d = GetComponent<Rigidbody2D>();
        MoveAction.Enable();
    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * moveSpeed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        if (canPressTab)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                UIHandler.instance.DisplayInventory();
                StartCoroutine(ButtonBufferTimer(buttonPressBuffer));
            }
  
        }

        if (canPressShift)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                UIHandler.instance.DisplayInputField();
            }
        }

        if (UIHandler.instance.inputFieldIsDisplayed)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                UIHandler.instance.UpdateInputFieldText();
                GameController.instance.CheckCheatCodes(UIHandler.instance.userInputText);
            }
        }

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    IEnumerator ButtonBufferTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canPressTab = true;

    }

}

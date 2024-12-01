using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance { get; private set; }

    public PlayerController player;
    public ExitDoor exitDoor;
    public Inventory playerInventory;

    public AudioClip timeRanOutSound;
    public AudioClip wrongCodeSound;
    public AudioClip buttonClick;

    public Sprite noTrophy;
    public Sprite trophy;

    Button restartButton;
    VisualElement loseScreen;
    Label youLoseScreenLabel;

    VisualElement winScreen;
    Label winScreenLabel;
    Button nextLevelButton;

    VisualElement instructions;
    Button acceptMission;
    Button abortMission;

    VisualElement endGameScreen;
    Label endGameText;
    Button replayMissionButton;
    Button endMissionButton;

    VisualElement trophyFiveImage;
    Label trophyFiveLabel;
    VisualElement trophyThreeImage;
    Label trophyThreeLabel;

    VisualElement timer;
    Label timerLabel;
    public float timerLength = 600;
    public float timeRemaining;
    public bool timerIsRunning = false;

    public float fastRunTrophyTimeCap = 300;
    public float ultraFastRunTropyTimeCap = 420;
  
    VisualElement intelCounterVE;
    Label intelCounterLabel;

    VisualElement inventory;
    public Label inventoryMessageLabel;
    bool inventoryIsDisplayed = false;

    // Input Field
    VisualElement inputFieldVE;
    TextField inputField;
    public Label inputFeedbackLabel;
    public bool inputFieldIsDisplayed = false;
    public string userInputText;

    // Secret Found Banner
    VisualElement secretFoundVE;

    //Instruction Banner
    VisualElement instructionBanner;

    // KeyPad
    Color warningRed = new Color(1, .3f, 0, 1);
    bool keyPadIsDisplayed = false;
    public string keypadScreenText;
    VisualElement doorKeyPadVE;
    Label keypadScreen;
    Button keyOne;
    Button keyTwo;
    Button keyThree;
    Button keyFour;
    Button keyFive;
    Button keySix;
    Button keySeven;
    Button keyEight;
    Button keyNine;
    Button keyZero;
    Button keyClear;
    Button keyEnter;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = timerLength;

        //Find and assign elements to variables
        //Register callbacks for click events on buttons
        //Disable visual elements that should not show at start of game
        UIDocument uiDocument = GetComponent<UIDocument>();

        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.RegisterCallback<ClickEvent>(RestartGame);

        nextLevelButton = uiDocument.rootVisualElement.Q<Button>("NextLevelButton");
        nextLevelButton.RegisterCallback<ClickEvent>(NextLevelLoad);

        loseScreen = uiDocument.rootVisualElement.Q<VisualElement>("LoseScreen");
        loseScreen.SetEnabled(false);
        loseScreen.style.display = DisplayStyle.None;

        youLoseScreenLabel = uiDocument.rootVisualElement.Q<Label>("YouLoseLabel");

        winScreen = uiDocument.rootVisualElement.Q<VisualElement>("WinScreen");
        winScreenLabel = uiDocument.rootVisualElement.Q<Label>("YouWinLabel");
        winScreen.SetEnabled(false);
        winScreen.style.display = DisplayStyle.None;

        instructions = uiDocument.rootVisualElement.Q<VisualElement>("Instructions");
        ShowInstructions();
        acceptMission = uiDocument.rootVisualElement.Q<Button>("AcceptMissionButton");
        acceptMission.RegisterCallback<ClickEvent>(StartGame);
        abortMission = uiDocument.rootVisualElement.Q<Button>("AbortMissionButton");
        abortMission.RegisterCallback<ClickEvent>(QuitGame);

        endGameScreen = uiDocument.rootVisualElement.Q<VisualElement>("EndGameScreen");
        endGameText = uiDocument.rootVisualElement.Q<Label>("EndGameLabel");
        replayMissionButton = uiDocument.rootVisualElement.Q<Button>("ReplayMissionButton");
        replayMissionButton.RegisterCallback<ClickEvent>(RestartGame);
        endMissionButton = uiDocument.rootVisualElement.Q<Button>("EndMissionButton");
        endMissionButton.RegisterCallback<ClickEvent>(QuitGame);

        trophyFiveImage = uiDocument.rootVisualElement.Q<VisualElement>("Trophy5Image");
        trophyFiveLabel = uiDocument.rootVisualElement.Q<Label>("Trophy5Label");
        trophyThreeImage = uiDocument.rootVisualElement.Q<VisualElement>("Trophy3Image");
        trophyThreeLabel = uiDocument.rootVisualElement.Q<Label>("Trophy3Label");
        HideEndGameScreen();

        timer = uiDocument.rootVisualElement.Q<VisualElement>("Timer");
        timerLabel = uiDocument.rootVisualElement.Q<Label>("TimerLabel");

        intelCounterVE = uiDocument.rootVisualElement.Q<VisualElement>("IntelCounter");
        intelCounterLabel = uiDocument.rootVisualElement.Q<Label>("IntelCounterLabel");

        inventory = uiDocument.rootVisualElement.Q<VisualElement>("Inventory");
        inventoryMessageLabel = uiDocument.rootVisualElement.Q<Label>("InventoryMessageLabel");
        inventory.style.display = DisplayStyle.None;

        inputFieldVE = uiDocument.rootVisualElement.Q<VisualElement>("InputFieldVE");
        inputFieldVE.style.display = DisplayStyle.None;
        inputField = uiDocument.rootVisualElement.Q<TextField>("InputField");
        inputFeedbackLabel = uiDocument.rootVisualElement.Q<Label>("InputFeedbackLabel");

        secretFoundVE = uiDocument.rootVisualElement.Q<VisualElement>("SecretFound");
        secretFoundVE.style.display = DisplayStyle.None;

        instructionBanner = uiDocument.rootVisualElement.Q<VisualElement>("InstructionBanner");
        HideInstructionBanner();

        // Assign Keypad objects
        doorKeyPadVE = uiDocument.rootVisualElement.Q<VisualElement>("DoorKeyPad");
        doorKeyPadVE.SetEnabled(false);
        doorKeyPadVE.style.display = DisplayStyle.None;
        keypadScreen = uiDocument.rootVisualElement.Q<Label>("KeyPadScreen");
        keyOne = uiDocument.rootVisualElement.Q<Button>("KeyOne");
        keyTwo = uiDocument.rootVisualElement.Q<Button>("KeyTwo");
        keyThree = uiDocument.rootVisualElement.Q<Button>("KeyThree");
        keyFour = uiDocument.rootVisualElement.Q<Button>("KeyFour");
        keyFive = uiDocument.rootVisualElement.Q<Button>("KeyFive");
        keySix = uiDocument.rootVisualElement.Q<Button>("KeySix");
        keySeven = uiDocument.rootVisualElement.Q<Button>("KeySeven");
        keyEight = uiDocument.rootVisualElement.Q<Button>("KeyEight");
        keyNine = uiDocument.rootVisualElement.Q<Button>("KeyNine");
        keyZero = uiDocument.rootVisualElement.Q<Button>("KeyZero");
        keyClear = uiDocument.rootVisualElement.Q<Button>("KeyClear");
        keyEnter = uiDocument.rootVisualElement.Q<Button>("KeyEnter");
        keyOne.RegisterCallback<ClickEvent>(ClickKeyOne);
        keyTwo.RegisterCallback<ClickEvent>(ClickKeyTwo);
        keyThree.RegisterCallback<ClickEvent>(ClickKeyThree);
        keyFour.RegisterCallback<ClickEvent>(ClickKeyFour);
        keyFive.RegisterCallback<ClickEvent>(ClickKeyFive);
        keySix.RegisterCallback<ClickEvent>(ClickKeySix);
        keySeven.RegisterCallback<ClickEvent>(ClickKeySeven);
        keyEight.RegisterCallback<ClickEvent>(ClickKeyEight);
        keyNine.RegisterCallback<ClickEvent>(ClickKeyNine);
        keyZero.RegisterCallback<ClickEvent>(ClickKeyZero);
        keyClear.RegisterCallback<ClickEvent>(ClickKeyClear);
        keyEnter.RegisterCallback<ClickEvent>(ClickKeyEnter);
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                GameController.instance.LoseGame("Time has run out!");
            }
        }
    }

    private void RestartGame(ClickEvent evt)
    {
        GameController.instance.PlaySound(buttonClick);
        timeRemaining = timerLength;
        DisplayTime(timeRemaining);
        GameController.instance.RestartGame();
    }

    private void NextLevelLoad(ClickEvent evt)
    {
        GameController.instance.PlaySound(buttonClick);
        GameController.instance.LoadNextLevel();
        winScreen.SetEnabled(false);
        winScreen.style.display = DisplayStyle.None;
        inventoryMessageLabel.text = "";
    }

    private void StartGame(ClickEvent evt)
    {
        GameController.instance.PlaySound(buttonClick);
        GameController.instance.StartGame();
    }

    private void QuitGame(ClickEvent evt)
    {
        GameController.instance.PlaySound(buttonClick);
        GameController.instance.QuitGame();
    }

    //Enable visual elements when called for
    public void ShowWinScreen(string currentLevel)
    {
        winScreen.SetEnabled(true);
        winScreenLabel.text = currentLevel + " Cleared!";
        winScreen.style.display = DisplayStyle.Flex;
    }

    public void ShowLoseScreen(string displayText)
    {
        loseScreen.SetEnabled(true);
        loseScreen.style.display = DisplayStyle.Flex;
        youLoseScreenLabel.text = displayText;

    }

    public void HideLoseScreen()
    {
        loseScreen.SetEnabled(false);
        loseScreen.style.display = DisplayStyle.None;
    }

    public void ShowInstructions()
    {
        instructions.SetEnabled(true);
        instructions.style.display = DisplayStyle.Flex;
    }

    public void HideInstructions()
    {
        instructions.SetEnabled(false);
        instructions.style.display = DisplayStyle.None;
    }

    public void ShowInstructionBanner()
    {
        instructionBanner.SetEnabled(true);
        instructionBanner.style.display = DisplayStyle.Flex;
    }

    public void HideInstructionBanner()
    {
        instructionBanner.SetEnabled(false);
        instructionBanner.style.display = DisplayStyle.None;
    }

    public void ShowEndGameScreen()
    {
        endGameScreen.SetEnabled(true);
        if(GameController.instance.intelCount == 3)
        {
            endGameText.text = "MISSION SUCCESS\nYou have successfully navigated the building without detection. You have collected all of the needed intel, and done so in under the time limit.\nCongratulations, and great work.";

            if (timeRemaining > fastRunTrophyTimeCap)
            {
                Debug.Log("Sub 5 Trohpy");
                trophyFiveImage.style.backgroundImage = new StyleBackground(trophy);
                trophyFiveImage.style.unityBackgroundImageTintColor = Color.white;
                trophyFiveLabel.style.color = Color.green;


                if (timeRemaining > ultraFastRunTropyTimeCap)
                {
                    Debug.Log("Sub 3 Trophy");
                    trophyThreeImage.style.backgroundImage = new StyleBackground(trophy);
                    trophyThreeImage.style.unityBackgroundImageTintColor = Color.white;
                    trophyThreeLabel.style.color = Color.green;
                }
            }
        }
        else
        {
            endGameText.text = $"MISSION FAILURE\nYou have navigated the building undetected, but you only found {GameController.instance.intelCount} of 3 pieces of intel.";
        }



        endGameScreen.style.display = DisplayStyle.Flex;
    }

    public void HideEndGameScreen()
    {
        endGameScreen.SetEnabled(false);
        endGameScreen.style.display = DisplayStyle.None;
    }

    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerLabel.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateIntelCounter()
    {
        intelCounterLabel.text = GameController.instance.intelCount + "/3 Intel Found";
    }

    public void DisplayInventory()
    {  
        if (inventoryIsDisplayed)
        {
            inventory.style.display = DisplayStyle.None;
            inventoryIsDisplayed = false;
            player.MoveAction.Enable();
        }
        else
        {
            inventory.style.display = DisplayStyle.Flex;
            inventoryIsDisplayed = true;
            player.MoveAction.Disable();
        }

    }

    public void UpdateInventoryListLabel(string newMessage)
    {
        inventoryMessageLabel.text += "- " + newMessage + "\n";
    }

    public void DisplayInputField()
    {
        if (inputFieldIsDisplayed)
        {
            inputFieldVE.style.display = DisplayStyle.None;
            inputFieldIsDisplayed = false;
            player.MoveAction.Enable();
        }
        else
        {
            inputFieldVE.style.display = DisplayStyle.Flex;
            inputFieldIsDisplayed = true;
            player.MoveAction.Disable();
        }
    }

    public void UpdateInputFieldText()
    {
        userInputText = inputField.text;   
    }

    public IEnumerator FlashNewSecretBanner()
    {
        secretFoundVE.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(.5f);
        secretFoundVE.style.display = DisplayStyle.None;
        yield return new WaitForSeconds(.5f);
        secretFoundVE.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(.5f);
        secretFoundVE.style.display = DisplayStyle.None;
        yield return new WaitForSeconds(.5f);
        secretFoundVE.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(5);
        secretFoundVE.style.display = DisplayStyle.None;
    }

    // Keypad Functions

    //Display
    public void DisplayKeypad()
    {
        if (keyPadIsDisplayed)
        {
            doorKeyPadVE.SetEnabled(false);
            doorKeyPadVE.style.display = DisplayStyle.None;
            keyPadIsDisplayed = false;
        }
        else
        {
            doorKeyPadVE.SetEnabled(true);
            doorKeyPadVE.style.display = DisplayStyle.Flex;
            keyPadIsDisplayed = true;
        }
    }

    void UpdateKeypadScreen()
    {
        keypadScreen.text = keypadScreenText;
    }

    // Buttons
    private void ClickKeyOne(ClickEvent evt){ GameController.instance.PlaySound(buttonClick); keypadScreenText += "1"; UpdateKeypadScreen(); }
    private void ClickKeyTwo(ClickEvent evt) { GameController.instance.PlaySound(buttonClick); keypadScreenText += "2"; UpdateKeypadScreen(); }
    private void ClickKeyThree(ClickEvent evt) { GameController.instance.PlaySound(buttonClick); keypadScreenText += "3"; UpdateKeypadScreen(); }
    private void ClickKeyFour(ClickEvent evt) { GameController.instance.PlaySound(buttonClick); keypadScreenText += "4"; UpdateKeypadScreen(); }
    private void ClickKeyFive(ClickEvent evt) { GameController.instance.PlaySound(buttonClick); keypadScreenText += "5"; UpdateKeypadScreen(); }
    private void ClickKeySix(ClickEvent evt) { GameController.instance.PlaySound(buttonClick); keypadScreenText += "6"; UpdateKeypadScreen(); }
    private void ClickKeySeven(ClickEvent evt) { GameController.instance.PlaySound(buttonClick); keypadScreenText += "7"; UpdateKeypadScreen(); }
    private void ClickKeyEight(ClickEvent evt) { GameController.instance.PlaySound(buttonClick); keypadScreenText += "8"; UpdateKeypadScreen(); }
    private void ClickKeyNine(ClickEvent evt) { GameController.instance.PlaySound(buttonClick); keypadScreenText += "9"; UpdateKeypadScreen(); }
    private void ClickKeyZero(ClickEvent evt) { GameController.instance.PlaySound(buttonClick); keypadScreenText += "0"; UpdateKeypadScreen(); }   
    private void ClickKeyClear(ClickEvent evt) { GameController.instance.PlaySound(buttonClick); keypadScreenText = ""; UpdateKeypadScreen(); }
    private void ClickKeyEnter(ClickEvent evt) { GameController.instance.PlaySound(buttonClick); exitDoor.CheckDoorCode(keypadScreenText); UpdateKeypadScreen(); }

    // Wrong Code Warning
    public IEnumerator WrongCodeAlert()
    {
        keypadScreen.style.color = warningRed;
        GameController.instance.PlaySound(wrongCodeSound);
        yield return new WaitForSeconds(1f);
        keypadScreen.style.color = Color.green;
        keypadScreenText = "";
    }

}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField]
    private float AIThinkTime = 0.5f;
    [SerializeField]
    private Textbox Textbox;
    [SerializeField]
    private TMP_InputField InputField;
    [SerializeField]
    private TextMeshProUGUI ButtonText;
    [SerializeField]
    private TextMeshProUGUI ButtonPlaceHolderText;
    [SerializeField]
    private TextMeshProUGUI PlayerNumber;
    [SerializeField]
    private Button InputButton;
    [SerializeField]
    private AI AI;

    private const string PLAY_GAME = "Play";
    private const string SET_COWS = "Set Cows";
    private const string SET_BULLS = "Set Bulls";
    private const string PLAYER_NUMBER = "Your Number: {0}";
    
    private bool inputBullsAndCows = false;
    private int clickState = 0;
    private int actualCows = 0;
    private int actualBulls = 0;
    private TextInfo textInfo = new TextInfo();
    private char[] secretNumber = new char[0];
    
    private void Awake()
    {
        CheckProperties();
        AI.Init();
        InputField.gameObject.SetActive(false);
        InputButton.gameObject.SetActive(false);
        ButtonText.SetText(PLAY_GAME);
    }

    private void Start()
    {
        textInfo.Type = MessageType.Welcome;
        Textbox.UpdateText(textInfo);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            ChangeGameState();
        }
    }

    private void ChangeGameState()
    {
        switch (textInfo.Type)
        {
            case MessageType.Welcome:
                textInfo.Type = MessageType.Input;
                Textbox.UpdateText(textInfo);
                InputButton.gameObject.SetActive(false);
                InputField.gameObject.SetActive(true);
                break;
            case MessageType.Input:
            case MessageType.Error:
                InputButton.gameObject.SetActive(InputField.text.Length == 4);
                break;
            case MessageType.Cheating:
                textInfo.Type = MessageType.ShowResult;
                Textbox.UpdateText(textInfo);
                InputField.gameObject.SetActive(true);
                break;
            case MessageType.ShowResult:
                InputButton.gameObject.SetActive(InputField.text.Length == 1);
                InputField.gameObject.SetActive(true);
                break;
            case MessageType.CorrectResult:
                textInfo.Reset();
                Textbox.UpdateText(textInfo);
                break;
            default:
                break;
        }
    }

    public void OnClick()
    {
        if (clickState == 0)
        {
            StartGame();
        }
        else if (clickState == 1)
        {
            SetCows();
        }
        else if (clickState == 2)
        {
            SetBulls();
        }
    }
    
    private void StartGame()
    {
        if (InputField.text.Length != 4)
        {
            Debug.LogError($"Tried to play game with invalid number {InputField.text}.");
            return;
        }
        if (!MasterMind.IsValidNumber(InputField.text))
        {
            textInfo.Type = MessageType.Error;
            Textbox.UpdateText(textInfo);
            return;
        }
        secretNumber = new char[4];
        for (int i = 0; i < InputField.text.Length; i++)
        {
            secretNumber[i] = InputField.text[i];
        }
        textInfo.Type = MessageType.Thinking;
        Textbox.UpdateText(textInfo);
        AI.Reset();
        AI.SetSecretNumber(secretNumber);
        PlayerNumber.SetText(string.Format(PLAYER_NUMBER, new string(secretNumber)));
        InputField.text = string.Empty;
        ButtonPlaceHolderText.SetText(SET_COWS);
        InputField.gameObject.SetActive(false);
        InputButton.gameObject.SetActive(false);
        ButtonText.text = SET_COWS;
        StartCoroutine(GuessNumber());
    }

    private IEnumerator GuessNumber()
    {
        clickState = 1;
        inputBullsAndCows = false;
        textInfo.Type = MessageType.ShowResult;
        AI.TakeFirstGuess(ref textInfo);
        (actualCows, actualBulls) = MasterMind.CheckResult(secretNumber, textInfo.CurrentGuess);
        yield return new WaitForSeconds(AIThinkTime);
        while (textInfo.Bulls < 4)
        {
            Textbox.UpdateText(textInfo);
            InputField.gameObject.SetActive(true);
            yield return new WaitUntil(BullsAndCowsAreCorrect);
            textInfo.Type = MessageType.Thinking;
            Textbox.UpdateText(textInfo);
            yield return new WaitForSeconds(AIThinkTime);
            AI.GuessNumber(ref textInfo);
            (actualCows, actualBulls) = MasterMind.CheckResult(secretNumber, textInfo.CurrentGuess);
            textInfo.Type = MessageType.ShowResult;
            inputBullsAndCows = false;
        }
        textInfo.Type = MessageType.CorrectResult;
        Textbox.UpdateText(textInfo);
        clickState = 0;
        ButtonText.text = PLAY_GAME;
    }

    private bool BullsAndCowsAreCorrect()
    {
        if (!inputBullsAndCows)
        {
            return false;
        }
        if (textInfo.Cows != actualCows || textInfo.Bulls != actualBulls)
        {
            textInfo.Type = MessageType.Cheating;
            inputBullsAndCows = false;
            Textbox.UpdateText(textInfo);
            InputField.gameObject.SetActive(false);
            inputBullsAndCows = false;
            return false;
        }
        return true;
    }
    private void SetCows()
    {
        if (InputField.text.Length != 1 || !char.IsDigit(InputField.text[0]))
        {
            return;
        }
        textInfo.Cows = InputField.text[0] - '0';
        clickState = 2;
        ButtonText.text = SET_BULLS;
        ButtonPlaceHolderText.SetText(SET_BULLS);
        InputField.text = string.Empty;
        InputButton.gameObject.SetActive(false);
    }
    
    private void SetBulls()
    {
        if (InputField.text.Length != 1 || !char.IsDigit(InputField.text[0]))
        {
            return;
        }
        textInfo.Bulls = InputField.text[0] - '0';
        clickState = 1;
        ButtonText.text = SET_COWS;
        inputBullsAndCows = true;
        ButtonPlaceHolderText.SetText(SET_COWS);
        InputField.text = string.Empty;
        InputField.gameObject.SetActive(false);
        InputButton.gameObject.SetActive(false);
    }
    
    private void CheckProperties()
    {
        if (Textbox == null)
        {
            Debug.LogError($"Textbox is null in GameLogic. Ending game.");
            EndGame();
        }
        if (AI == null)
        {
            Debug.LogError($"AI is null in GameLogic. Ending game.");
            EndGame();
        }
        if (InputField == null)
        {
            Debug.LogError($"InputField is null in GameLogic. Ending game.");
            EndGame();
        }
        if (InputButton == null)
        {
            Debug.LogError($"InputButton is null in GameLogic. Ending game.");
            EndGame();
        }
        if (ButtonText == null)
        {
            Debug.LogError($"ButtonText is null in GameLogic. Ending game.");
            EndGame();
        }
        if (PlayerNumber == null)
        {
            Debug.LogError($"PlayerNumber is null in GameLogic. Ending game.");
            EndGame();
        }
        if (ButtonPlaceHolderText == null)
        {
            Debug.LogError($"ButtonPlaceHolderText is null in GameLogic. Ending game.");
            EndGame();
        }
    }

    private void EndGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
        #if !UNITY_EDITOR
                    Application.Quit();
        #endif
    }
}

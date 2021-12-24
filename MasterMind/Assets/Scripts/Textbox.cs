using UnityEngine;
using TMPro;

public enum MessageType { Welcome, Input, Error, Thinking, Cheating, ShowResult, CorrectResult }

[System.Serializable]
public class TextInfo
{
    public int Cows = 0;
    public int Bulls = 0;
    public int NumberOfTurns = 0;
    public MessageType Type = MessageType.Welcome;
    public char[] CurrentGuess = { 'a', 'a', 'a', 'a'};

    public void Reset()
    {
        Cows = Bulls = NumberOfTurns = 0;
        Type = MessageType.Welcome;
        for (int i = 0; i < CurrentGuess.Length; i++)
        {
            CurrentGuess[i] = 'a';
        }
    }
}


public class Textbox : MonoBehaviour
{
        [SerializeField]
    private TextMeshProUGUI Text;
    
    private const string FIRST_MESSAGE = "Lets play a game of MasterMind called Bulls and Cows! Pick a 4 digit number" +
                                         "and I will try to guess it. The only rule is all 4 digits have to be unique." +
                                         " Press any button to begin. If you want to learn more about the game check here: " +
                                         "https://en.wikipedia.org/wiki/Bulls_and_Cows";
    private const string INPUT_NUMBER_MESSAGE = "Please input your number in the textbox. Remember it has to be a " +
                                                "4 digit number where every digit is unique. You can use 0-9.";
    private const string INVALID_NUMBER_MESSAGE = "It looks like that is an invalid number. Remember it has to be a " +
                                                  "4 digit number where every digit is unique. You can use 0-9.";
    private const string THINKING_MESSAGE = "Hmm, give me a second. I'm thinking.";
    private const string CHEATING_MESSAGE = "Hey no cheating! Make sure you enter the correct number of bulls and cows.";
    private const string GUESS_OUTPUT = "Turn Number: {0}.\nMy guess is {1}. Please enter the number of bulls and cows.";
    private const string GUESS_CORRECT_OUTPUT = "Your number is {0}.\nI guessed the word in {1} turns. Press any button to" +
                                                "play again.";
    
    private void Awake()
    {
        if (Text == null)
        {
            Debug.LogError("TextMeshProUGUI is null on textbox. Turning off.");
            gameObject.SetActive(false);
        }
    }

    public void UpdateText(TextInfo info)
    {
        switch (info.Type)
        {
            case MessageType.Welcome:
                Text.SetText(FIRST_MESSAGE);
                break;
            case MessageType.Input:
                Text.SetText(INPUT_NUMBER_MESSAGE);
                break;
            case MessageType.Error:
                Text.SetText(INVALID_NUMBER_MESSAGE);
                break;
            case MessageType.Thinking:
                Text.SetText(THINKING_MESSAGE);
                break;
            case MessageType.Cheating:
                Text.SetText(CHEATING_MESSAGE);
                break;
            case MessageType.ShowResult:
                var guessMessage = string.Format(GUESS_OUTPUT, info.NumberOfTurns, new string(info.CurrentGuess));
                Text.SetText(guessMessage);
                break;
            case MessageType.CorrectResult:
                var correctMessage = string.Format(GUESS_CORRECT_OUTPUT, new string(info.CurrentGuess), info.NumberOfTurns);
                Text.SetText(correctMessage);
                break;
            default:
                break;
        }
    }
}

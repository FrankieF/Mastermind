using System.Collections.Generic;
using UnityEngine;

class AI : MonoBehaviour
{
    public bool DebugMode;

    private char[] secretNumber = new char[0];
    private List<char[]> remainingGuesses = new List<char[]>();
    private List<char[]> possibleGuesses = new List<char[]>();

    public void Init()
    {
        possibleGuesses = MasterMind.GeneratePermutations();
    }

    public void Reset()
    {
        remainingGuesses.Clear();
        remainingGuesses.AddRange(possibleGuesses);
    }

    public void SetSecretNumber(char[] number)
    {
        secretNumber = number;
    }

    public void TakeFirstGuess(ref TextInfo info)
    {
        info.CurrentGuess = MasterMind.GetRandomGuess();
        (info.Cows, info.Bulls) = MasterMind.CheckResult(secretNumber, info.CurrentGuess);
        info.NumberOfTurns++;
        if (DebugMode)
        {
            Debug.Log($"Turn Number {info.NumberOfTurns}.Number is {new string(secretNumber)}, " +
                      $"Guess is {new string(info.CurrentGuess)}, Cows {info.Cows}, Bulls {info.Bulls}");
        }
    }

    public void GuessNumber(ref TextInfo info)
    {
        info.NumberOfTurns++;
        remainingGuesses = ReduceGuesses(info.Cows, info.Bulls, info.CurrentGuess);
        info.CurrentGuess = PickGuess();
        (info.Cows, info.Bulls) = MasterMind.CheckResult(secretNumber, info.CurrentGuess);
        if (DebugMode)
        {
            Debug.Log($"Turn Number {info.NumberOfTurns}.Number is {new string(secretNumber)}, " +
                      $"Guess is {new string(info.CurrentGuess)}, Cows {info.Cows}, Bulls {info.Bulls}");
        }
    }

    private List<char[]> ReduceGuesses(int cows, int bulls, char[] previousGuess)
    {
        int startingAmount = remainingGuesses.Count;
        var reducedGuesses = new List<char[]>();
        foreach (var guess in remainingGuesses)
        {
            (int _cows, int _bulls) = MasterMind.CheckResult(guess, previousGuess);
            if (cows == _cows && bulls == _bulls)
            {
                reducedGuesses.Add(guess);
            }
        }
        if (DebugMode)
        {
            int endingAmount = reducedGuesses.Count;
            Debug.Log($"Remaining guesses: {endingAmount}, removed {startingAmount - endingAmount} of guesses.");
        }
        return reducedGuesses;
    }

    private char[] PickGuess()
    {
        var remaining = new List<int>(remainingGuesses.Count);
        for (int i = 0; i < remaining.Count; i++)
        {
            var amount = remainingGuesses.Count - 1;
            int sample = amount > 100 ? Random.Range(0, 101) : Random.Range(0, amount);
            int remainder = 0;
            for (int j = 0; j < sample; j++)
            {
                var score = MasterMind.CheckResult(remainingGuesses[j], remainingGuesses[i]);
                for (int k = 0; k < sample; k++)
                {
                    var secondScore = MasterMind.CheckResult(remainingGuesses[k], remainingGuesses[i]);
                    if (score.Item1 == secondScore.Item1 && score.Item2 == secondScore.Item2)
                    {
                        remainder++;
                    }
                }
            }
            remaining[i] = remainder;
        }
        int min = int.MaxValue;
        int index = 0;
        for (int i = 0; i < remaining.Count; i++)
        {
            var value = remaining[i];
            if (value < min)
            {
                min = value;
                index = i;
            }
        }
        return remainingGuesses[index];
    }
}

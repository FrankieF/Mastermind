using System.Collections.Generic;
using UnityEngine;

public class MasterMind
{
    public static List<char[]> GeneratePermutations()
    {
        var list = new List<char[]>();
        for (char i = '0'; i <= '9'; i++)
        {
            for (char j = '0'; j <= '9'; j++)
            {
                if (j == i)
                {
                    j++;
                }
                for (char k = '0'; k <= '9'; k++)
                {
                    if (k == i)
                    {
                        k++;
                    }
                    if (k == j)
                    {
                        k++;
                    }
                    for (char l = '0'; l <= '9'; l++)
                    {
                        if (l == i)
                        {
                            l++;
                        }
                        if (l == j)
                        {
                            l++;
                        }
                        if (l == k)
                        {
                            l++;
                        }
                        if (j > '9' || k > '9' || l > '9' || i == k || i == l || j == k || j == l || k == l)
                        {
                            continue;
                        }
                        var chars = new char[] { i, j, k, l };
                        list.Add(chars);
                    }
                }
            }
        }
        return list;
    }
    
    public static (int, int) CheckResult(char[] word, char[] guess)
    {
        int cows = 0;
        int bulls = 0;
        for (int i = 0; i < word.Length; i++)
        {
            for (int j = 0; j < guess.Length; j++)
            {
                if (i == j && word[i] == guess[j])
                {
                    bulls++;
                }
                else if (word[i] == guess[j])
                {
                    cows++;
                }
            }
        }
        return (cows, bulls);
    }

    public static bool IsValidNumber(string number)
    {
        if (number == null || number.Length != 4)
        {
            return false;
        }
        for (int i = 0; i < number.Length; i++)
        {
            if (!char.IsDigit(number[i]))
            {
                return false;
            }
            for (int j = i + 1; j < number.Length; j++)
            {
                if (number[i] == number[j])
                {
                    return false;
                }
            }
        }
        return true;
    }
    
    public static char[] GetRandomGuess()
    {
        var values = new char[10] {'0','1','2','3','4','5','6','7','8','9'};
        var guess = new char[4] {'a','a','a','a'};
        int created = 0;
        while (created < guess.Length)
        {
            var number = Random.Range(0, 10);
            var character = values[number];
            bool unique = true;
            for (int i = 0; i < guess.Length; i++)
            {
                if (guess[i] == character)
                {
                    unique = false;
                }
            }
            if (unique)
            {
                guess[created] = character;
                created++;
            }
        }
        return guess;
    }
}
# Mastermind
 
I did research on google about Master Mind and Bulls and Cows and found this research paper by Alexy Slovesnov. I used his research to implement the algorithm.

The algorithm works by first generating all 5040 combinations and then picking a random number. The initial guess is compared to the selected number we are trying to guess. 
With the bulls and cows from our guess compared to the selected number, we compared everything in our set of possible guesses with our guess and only keep the numbers that 
produce the same number of bulls and cows. This reduces the set of possible guesses each turn and then we pick a number from our set. When picking a number after the first 
turn we compare it against other elements in the set to try and find a number which will remove the most elements from the set on the next turn. We then check our answer 
against the selected number and repeat if needed. The algorithm should solve every number in 7 turns.
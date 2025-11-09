namespace ConsoleApp1.Services;

public class WordCheck
{
    private readonly int[] _letterCount;
    private readonly char _firstletter;
    private readonly int _alphabetSize;

    public WordCheck(string firstword, char firstletter, int alphabetSize)
    {
        _firstletter = firstletter;
        _alphabetSize = alphabetSize;
        _letterCount = new int[alphabetSize];

        //Every used letter in the word adds 1 to  
        foreach (char c in firstword)
        {
            //_firstletter - the first leter of the alphabet (depends on a game localization)
            int index = c - _firstletter;

            //Adds 1 to the elements in the array
            if (index >= 0 && index < _alphabetSize)
                _letterCount[index]++;
        }
    }

    public bool Check(string secondword)
    {
        //Array for counting the amount of times when each letter of the alphabet is used in the word
        //'_alphabetSize' - alphabet size (depends on localization)
        int[] checkCount = new int[_alphabetSize];

        //'isValid' tells if the word is allowed or not
        bool isValid = true;

        //Counts the amount of times when each letter is used
        foreach (char c in secondword)
        {
            //'_firstletter' - the first letter of the alphabet (depends on localization)
            int index = c - _firstletter;

            //checks if the symbol is a correct letter and adds 1 to the array element
            if (index >= 0 && index < _alphabetSize)
                checkCount[index]++;
            else
            {
                isValid = false;
                break;
            }
        } 

        //Comparison between tho arrays to check how many letters are used

        //The number of used letters must be equal or lower than the number of the same letters in the first word
        for (int i = 0; i < _alphabetSize; i++)
        {
            if (_letterCount[i] < checkCount[i])
            {
                isValid = false;
                break;
            }
        }

        //'isValid' tells if the word is allowed or not
        return isValid;    
    }        
}
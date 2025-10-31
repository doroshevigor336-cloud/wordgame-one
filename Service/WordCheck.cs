namespace ConsoleApp1.Services;

public class WordCheck
{
    private readonly int[] arr;
    private readonly char firstletter;
    private readonly int mas;

    public WordCheck(string firstword, char firstletter, int mas)
    {
        this.firstletter = firstletter;
        this.mas = mas;
        arr = new int[mas];

        //Every used letter in the word adds 1 to  
        foreach (char c in firstword)
        {
            //firstletter - the first leter of the alphabet (depends on a game localization)
            int nomer = c - firstletter;

            //Adds 1 to the elements in the array
            if (nomer >= 0 && nomer < mas)
                arr[nomer]++;
        }
    }

    public bool Check(string secondword)
    {
        //Array for counting the amount of times when each letter of the alphabet is used in the word
        //'mas' - alphabet size (depends on localization)
        int[] arrcheck = new int[mas];

        //'rule' tells if the word is allowed or not
        bool rule = true;

        //Counts the amount of times when each letter is used
        foreach (char c in secondword)
        {
            //'firstletter' - the first letter of the alphabet (depends on localization)
            int nomer = c - firstletter;

            //checks if the symbol is a correct letter and adds 1 to the array element
            if (nomer >= 0 && nomer < mas)
                arrcheck[nomer]++;
            else
            {
                rule = false;
                break;
            }
        } 

        //Comparison between tho arrays to check how many letters are used

        //The number of used letters must be equal or lower than the number of the same letters in the first word
        for (int i = 0; i < mas; i++)
        {
            if (arr[i] < arrcheck[i])
            {
                rule = false;
                break;
            }
        }

        //'rule' tells if the word is allowed or not
        return rule;    
    }        
}
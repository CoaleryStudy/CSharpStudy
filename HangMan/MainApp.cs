using System;

namespace HangMan
{
    public static class ExtendMethods
    {
        // Add Methods for Extend Here
        public static string SetAt(this string target, int index, char charToChange)
        {
            return target.Substring(0, index) + charToChange + target.Substring(index + 1, target.Length - index - 1);
        }
    }

    class HangMan
    {
        static void Main(string[] args)
        {
            string[] wordslist = { "apple", "banana", "csharp", "java", "visual", "studio" };
            Random random = new Random();

            string answerWord = wordslist[random.Next(0, wordslist.Length)];
            string showWord = "";

            int correctCount = 0;
            int attemptCount = 0;

            for (int i = 0; i < answerWord.Length; i++)
                showWord += "_";
            
            // 비트 연산.
            while(correctCount != Math.Pow(2, answerWord.Length) - 1)
            {
                Console.WriteLine(showWord + " ( Attempt : " + attemptCount + " )");
                string input = Console.ReadLine();
                if (input.Length == 0)
                    continue;
                
                for(int ii=0; ii<input.Length; ii++)
                {
                    char charToCompare = input[ii];
                    for (int ai = 0; ai < answerWord.Length; ai++)
                    {
                        if (answerWord[ai] == charToCompare)
                        {
                            correctCount |= (int)Math.Pow(2, ai);
                            showWord = showWord.SetAt(ai, answerWord[ai]);
                        }
                    }
                }
                attemptCount++;
                Console.WriteLine();
            }
            Console.WriteLine("Successed!");
            Console.WriteLine("Attempt : " + attemptCount);
        }
    }
}
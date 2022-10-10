using System;
using System.Collections.Generic;
namespace MarkovEntry
{
    public class MarkovState
    {
        List<char> suffixList = new List<char>();

        private string state; // key
        public int count;

        public MarkovState(string state)
        {
            this.state = state;
            this.count = 0;
        }

        public void AddSuffix(char ch)
        {
            this.count++;
            suffixList.Add(ch); // add character to suffix list
            
        }

        public char RandomLetter()
        {
            Random rng = new Random(); // random number generator for random letters after states
            int num = rng.Next(suffixList.Count);
            return suffixList[num];
        }
        

        public override string ToString()
        {
            return $"{state}({count})";
        }
    }
}

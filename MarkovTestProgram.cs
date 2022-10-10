using System;
using MarkovEntry;
using ListSymbolTable;
using MyBinarySymbolTable;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace MarkovStory
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //--Validate Length--//
            if(args.Length < 3 || args.Length > 3)
            {
                Console.WriteLine("Error: incorrect number of arguments");
                Console.WriteLine("Usage: <filename.txt> <Markov Model number> <story length>");
                return;
            }

            string filename = args[0];

            //--Validate Parsing--//
            if (!int.TryParse(args[1], out int N)) // Markov model number K - if smaller than 5 bigger than 10 throw exception
            {
                Console.WriteLine("Error: invalid Markov Model number. Please enter an integer.");
                return;
            }

            if(!int.TryParse(args[2], out int L)) // story length
            {
                Console.WriteLine("Error: invalid story length. Please enter an integer.");
                return;
            }

            //--Validate Range--//
            if (N < 5 || N > 10)
            {
                Console.WriteLine("Error: Markov Model number is out of range");
            }

            if (L < 1 || L > 200)
            {
                Console.WriteLine("Error: story length is out of range");
            }

            //--Create Stopwatches for each Table--//
            Stopwatch symbolListTime = new Stopwatch();
            Stopwatch BSTTime = new Stopwatch();
            Stopwatch DotNetTime = new Stopwatch();

            symbolListTime.Start();
            BSTTime.Start();
            DotNetTime.Start();

            //--Read Source Text File--//
            StreamReader sr = new StreamReader(filename);

            while (!sr.EndOfStream)
            {
                string currentline = sr.ReadLine();
            }

            string[] lines = File.ReadAllLines(filename);

            //--Get length of Source Text File--//
            int textLength = 0;
            foreach (string line in lines)
            {
                foreach(char ch in line)
                {
                    textLength++;
                }
            }

            string story = lines[0].Substring(0, N); // initialize to first N letters of source text file
            sr.Close();

            BSTTime.Stop();
            DotNetTime.Stop();

            //--Symbol Table--//
            Console.WriteLine(" ");
            Console.WriteLine("Custom Linked List Symbol Table");


            // Call the symbol tree traversal here
            ListSymbolTable<string, MarkovState> db = ListSymbolTable<string, MarkovState>.LSTTraversal(N, lines);
            Console.WriteLine($"Text length: {textLength} chars");

            for (int i = 0; i < L; i++)
            {
                string state = story.Substring(story.Length - N, N);
                story += db[state].RandomLetter();
                // extra credit: make sure story continues to print until the end of a sentence
                if (i == L)
                {
                    while (!state.Contains("."))
                    {
                        string stateExtend = story.Substring(story.Length - N, N);
                        story += db[state].RandomLetter();
                    }
                    int stateCount = 0;
                    if (state.Contains("."))
                    {
                        stateCount++;
                    }
                    while (stateCount <= N)
                    {
                        string stateExtendFinal = story.Substring(story.Length - N, N);
                        story += db[state].RandomLetter();
                    }
                }
            }
            Console.WriteLine(story + ".");
            symbolListTime.Stop();
            Console.WriteLine($"Execution Time: {symbolListTime.Elapsed.ToString("ss\\:fff")} (sec:ms)");

            //--Binary Search Tree--//
            string story2 = lines[0].Substring(0, N); // initialize to first N letters of source text file
            BSTTime.Start();

            Console.WriteLine(" ");
            Console.WriteLine("Custom Binary Tree Symbol Table");

            MyTreeTable<string, MarkovState> db2 = MyTreeTable<string, MarkovState>.BSTMakeTree(N, lines);
            db2.BSTTraversal();

            Console.WriteLine($"Text length: {textLength} chars");

            for (int i = 0; i < L; i++)
            {
                string state = story2.Substring(story2.Length - N, N);
                story2 += db2[state].RandomLetter();
                // extra credit: make sure story continues to print until the end of a sentence
                if (i == L)
                {
                    while (!state.Contains("."))
                    {
                        string stateExtend = story2.Substring(story2.Length - N, N);
                        story2 += db2[state].RandomLetter();
                    }
                    int stateCount = 0;
                    if (state.Contains("."))
                    {
                        stateCount++; 
                    }
                    while(stateCount <= N)
                    {
                        string stateExtendFinal = story2.Substring(story2.Length - N, N);
                        story2 += db2[state].RandomLetter();
                    }
                }
            }

            Console.WriteLine(story2 + ".");
            BSTTime.Stop();
            Console.WriteLine($"Execution Time: {BSTTime.Elapsed.ToString("ss\\:fff")} (sec:ms)");

            //--.NET Sorted Dictionary--//
            string story3 = lines[0].Substring(0, N); // initialize to first N letters of source text file
            DotNetTime.Start();

            Console.WriteLine(" ");
            Console.WriteLine("Dot Net Sorted Dictionary");
            SortedDictionary<string, MarkovState> db3 = new SortedDictionary<string, MarkovState>();

            foreach (string line in lines)
            {
                for (int i = 0; i < line.Length - N; i++)
                {
                    string state = line.Substring(i, N);
                    char next = line[i + N];

                    //construct markov model

                    if (!db3.ContainsKey(state))
                    {
                        db3.Add(state, new MarkovState(state));
                    }
                    db3[state].AddSuffix(next);
                }
            }
            //Traverse Dot Net Tree
            foreach (KeyValuePair<string, MarkovState> key in db3)
            {
                string k = key.Key;
                MarkovState v = key.Value;
            }

            Console.WriteLine($"Text length: {textLength} chars");

            for (int i = 0; i < L; i++)
            {
                string state = story3.Substring(story3.Length - N, N);
                story3 += db3[state].RandomLetter();
                // extra credit: make sure story continues to print until the end of a sentence
                if (i == L)
                {
                    while (!state.Contains("."))
                    {
                        string stateExtend = story3.Substring(story3.Length - N, N);
                        story3 += db3[state].RandomLetter();
                    }
                    int stateCount = 0;
                    if (state.Contains("."))
                    {
                        stateCount++;
                    }
                    while (stateCount <= N)
                    {
                        string stateExtendFinal = story3.Substring(story3.Length - N, N);
                        story3 += db3[state].RandomLetter();
                    }
                }
            }
            Console.WriteLine(story3 + ".");
            DotNetTime.Stop();
            Console.WriteLine($"Execution Time: {DotNetTime.Elapsed.ToString("ss\\:fff")} (sec:ms)");
        }
    }
}

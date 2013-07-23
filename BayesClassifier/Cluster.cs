using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BayesClassifier
{
    public class Cluster
    {
        public Dictionary<string, int> Bag { get; set; }
        public long TotalWords { get; set; }
        public Cluster()
        {
            Bag = new Dictionary<string, int>();
            TotalWords = 0;
        }

        public void addTextToBag(string text)
        {
            string[] textWords = text.Split();
            for (int i = 0; i < textWords.Length; i++)
                if (textWords[i].Length > 0)
                {
                    if (Bag.ContainsKey(textWords[i]))
                        Bag[textWords[i]]++;
                    else
                        Bag.Add(textWords[i], 1);
                    TotalWords++;
                }
        }
    }
}

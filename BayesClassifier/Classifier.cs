using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BayesClassifier
{
    public class Classifier
    {
        Dictionary<string, Cluster> clusters;
        double z = 1;

        public Classifier()
        {
            clusters = new Dictionary<string, Cluster>();
        }

        public void learnText(string label, string text)
        {
            text = prepareText(text);
            string[] messageWords = text.Split();
            if (!clusters.ContainsKey(label))
                clusters.Add(label, new Cluster());

            clusters[label].addTextToBag(text);
        }

        public string classifyMessage(string message)
        {
            message = removeDigits(message);
            string[] words = message.Split();
            Dictionary<string, double> results = new Dictionary<string, double>();

            foreach (String eachClusterKey in clusters.Keys)
            {
                results.Add(eachClusterKey, 0);
                foreach (string eachWord in words)
                {
                    int wordsCount = clusters[eachClusterKey].Bag.ContainsKey(eachWord) ?
                        clusters[eachClusterKey].Bag[eachWord] : 0;
                    double val = (wordsCount + z) /
                        (clusters[eachClusterKey].TotalWords + z * clusters[eachClusterKey].Bag.Count);
                    results[eachClusterKey] += Math.Log(val);
                }
            }

            double maxResult = double.MinValue;
            string winnerClusterName = "";
            foreach (string eachResultKey in results.Keys)
            {
                if (results[eachResultKey] > maxResult)
                {
                    maxResult = results[eachResultKey];
                    winnerClusterName = eachResultKey;
                }
            }
            return winnerClusterName;
        }

        private string prepareText(string text)
        {
            text = removeDigits(text);
            text = text.ToLower();
            text = text.Replace(",", "");
            text = text.Replace(".", "");
            text = text.Replace("-", "");
            text = text.Replace("<", "");
            text = text.Replace(">", "");
            text = text.Replace("`", "");
            text = text.Replace("'", "");
            text = text.Replace("?", "");
            text = text.Replace("!", "");
            text = text.Replace(":", "");
            text = text.Replace(";", "");
            text = text.Replace("*", "");
            text = text.Replace("#", "");
            text = text.Replace("\"", "");
            return text;
        }

        private static string removeDigits(string key)
        {
            return Regex.Replace(key, @"\d", "1");
        }

        private void addMessageToCollection(Dictionary<string, int> collection, string[] messageWords)
        {
            for (int i = 1; i < messageWords.Length; i++)
                if (collection.ContainsKey(messageWords[i]))
                    collection[messageWords[i]]++;
                else
                    collection.Add(messageWords[i], 1);
        }
    }
}

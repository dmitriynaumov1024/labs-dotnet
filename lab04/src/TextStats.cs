namespace Lab04;

using System;
using System.Collections.Generic;
using System.Linq;

public class TextStats
{
    private TextStatsConfig config = new();

    private SortedDictionary<string, int> wordStats = new();
    private SortedDictionary<string, int> punctStats = new();

    public IEnumerable<KeyValuePair<string, int>> Stats => this.wordStats.Concat(this.punctStats);
    public IEnumerable<KeyValuePair<string, int>> WordStats => this.wordStats;
    public IEnumerable<KeyValuePair<string, int>> PunctStats => this.punctStats;

    protected TextStats (TextStatsConfig config) 
    {
        this.config = config;
    }

    public void AddWord (string word, int count = 1)
    {
        if (this.config.IgnoreCase) word = word.ToLower();
        if (!this.wordStats.ContainsKey(word)) this.wordStats[word] = count;
        else this.wordStats[word] += count;
    }

    public void AddPunct (string punct, int count = 1)
    {
        if (!this.punctStats.ContainsKey(punct)) this.punctStats[punct] = count;
        else this.punctStats[punct] += count;
    }

    public static TextStats For (string text, TextStatsConfig config = null)
    {
        var result = new TextStats(config ?? new());
        int wordStart = -1, wordLen = 0, end = text.Length;
        for (int i=0; i<end; i+=1) {
            if (Char.IsLetterOrDigit(text, i)) {
                if (wordStart < 0) wordStart = i;
                wordLen += 1;
            }
            else {
                if (Char.IsPunctuation(text, i)) {
                    result.AddPunct(text.Substring(i, 1));
                }
                if (wordLen > 0) {
                    result.AddWord(text.Substring(wordStart, wordLen));
                }
                wordStart = -1;
                wordLen = 0;
            }
        }
        if (wordLen > 0) {
            result.AddWord(text.Substring(wordStart, wordLen));
        }
        return result;
    }

}

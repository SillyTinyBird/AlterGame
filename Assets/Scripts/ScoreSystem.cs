using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    private static int _score;
    [SerializeField] TextMeshProUGUI _scoreText;
    private void Awake()
    {
        _score = 0;
        StartCoroutine(ScoreCoroutine());
    }
    public static int GetScore()
    {
        return _score;
    }
    public static void AddPoints(int amount)
    {
        if (amount > 0)
            _score += amount;
    }
    /// <summary>
    /// Returns <i>true</i> if New High Score, otherwise <i>false</i>.
    /// </summary>
    public static bool SaveScore()
    {
        string fileName = "scoreData.bin";
        int score = int.Parse(FileIO.ReadString(fileName));
        if (score < _score)
        {
            FileIO.WriteString(fileName, _score.ToString());
            return true;
        }
        return false;
    }
    IEnumerator ScoreCoroutine()
    {
        while (true)//waiting for 2147483647 to happen
        {
            _score++;
            _scoreText.SetText(_score.ToString("000000"));
            yield return new WaitForSeconds(0.05f);
        }
    }
}

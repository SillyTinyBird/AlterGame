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
        StartCoroutine("ScoreCoroutine");
    }
    public static void AddPoints(int amount)
    {
        if (amount > 0)
            _score += amount;
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

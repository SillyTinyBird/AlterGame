using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    private int _score;
    [SerializeField] TextMeshProUGUI _scoreText;
    private void Awake()
    {
        _score = 0;
        StartCoroutine("ScoreCoroutine");
    }
    IEnumerator ScoreCoroutine()
    {
        while (true)//waiting for 2147483647
        {
            _score++;
            _scoreText.SetText(_score.ToString("000000"));
            yield return new WaitForSeconds(0.05f);
        }
    }
}

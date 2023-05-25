using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    private int _score;
    private int _distance;
    private int _dougnuts;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _distanceText;
    [SerializeField] TextMeshProUGUI _dougnutText;
    public static ScoreSystem instance;//singletone-ish
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        _score = 0;
        _distance = 0;
        _dougnuts = 0;
        StartCoroutine(ScoreCoroutine());
        StartCoroutine(DistanceCoroutine());
    }
    public static ScoreSystem Instance => instance;
    public int Score => _score;
    public int Distance => _distance;
    public int Dougnuts => _dougnuts;
    public void AddPoints(int amount)
    {
        if (amount > 0)
            _score += amount;
    }
    public void AddDoughnut()
    {
        _dougnuts++;
        if (_dougnutText != null)
        {
            _dougnutText.text = _dougnuts.ToString("000");
        }
    }
    /// <summary>
    /// Returns <i>true</i> if New High Score, otherwise <i>false</i>.
    /// </summary>
    public bool SaveScore()
    {
        string fileName = "scoreData.bin";
        int score = int.Parse(FileIO.ReadString(fileName));
        if (score < _score)
        {
            if (PlayServicesLoginStatus.IsLoginSucceeded)
            {
                Leaderboard.PostScore(_score);
            }
            FileIO.WriteString(fileName, _score.ToString());
            return true;
        }
        return false;
    }
    IEnumerator DistanceCoroutine()
    {
        while (true)//waiting for 2147483647 to happen
        {
            _distance++;
            _distanceText.SetText(_distance.ToString("00000m"));
            yield return new WaitForSeconds(0.2f);
        }
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

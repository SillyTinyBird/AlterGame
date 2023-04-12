using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class PlaymodeInterfaceScript : MonoBehaviour
{
    [SerializeField] private GameObject _deathScreenGroup;
    [SerializeField] private GameObject _overlayGroup;
    [SerializeField] private GameObject _loadingGroup;
    [SerializeField] private TextMeshProUGUI _curScore;
    [SerializeField] private TextMeshProUGUI _newScore;
    public void SetPause(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    private void Start()
    {
        SetPause(true);
    }
    public void LoadingCompleetAction()
    {
        _loadingGroup.SetActive(false);
        SetPause(false);
    }
    public void FailActions()
    {
        if (_deathScreenGroup != null && _overlayGroup != null)
        {
            SetPause(true);
            if (ScoreSystem.SaveScore())
            {
                //do something about it
            }
            _curScore.SetText(ScoreSystem.GetScore().ToString("000000"));
            _newScore.SetText(FileIO.ReadInt("scoreData.bin").ToString("000000"));//yeah not so clean i guess
            _deathScreenGroup.SetActive(true);
            _overlayGroup.SetActive(false);
        }
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//async, for now
    }
    public void BackToManuButtonAction()
    {
        SceneManager.LoadScene(0);//async, for now
    }
}

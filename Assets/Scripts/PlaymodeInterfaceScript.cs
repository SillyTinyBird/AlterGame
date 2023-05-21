using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
[DefaultExecutionOrder(-1)]
public class PlaymodeInterfaceScript : MonoBehaviour
{
    [SerializeField] private GameObject _deathScreenGroup;
    [SerializeField] private GameObject _overlayGroup;
    [SerializeField] private GameObject _loadingGroup;
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _curScore;
    [SerializeField] private TextMeshProUGUI _newScore;
    [SerializeField] private TextMeshProUGUI _deathMessage;
    [SerializeField] private TextMeshProUGUI _statsText;
    [SerializeField] private DeathMessageScript _scriptWeGetDeathMessageFrom;
    [SerializeField] private FailSFX _scriptForDeathSFX;
    [SerializeField] private IronSourceAdsScript _ads;
    [SerializeField] private StatsMessage _stats;
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
    private void Awake()
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
            if (ScoreSystem.Instance.SaveScore())
            {
                //do something about it
            }
            _curScore.SetText(ScoreSystem.Instance.Score.ToString("000000"));
            _newScore.SetText(FileIO.ReadInt("scoreData.bin").ToString("000000"));//yeah not so clean i guess
            _deathMessage.SetText(_scriptWeGetDeathMessageFrom.GetDeathMessage());
            _statsText.SetText(_stats.GetStatsString());
            _scriptForDeathSFX.PlayDeathSFX();
            _deathScreenGroup.SetActive(true);
            _panel.SetActive(true);
            _overlayGroup.SetActive(false);
        }
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void PlayRewardedAdButton()
    {
        _ads.ShowRewardedAd();
    }
    public void RewardedAdCompleete()
    {
        _deathScreenGroup.SetActive(false);
        _panel.SetActive(false);
        _overlayGroup.SetActive(true);
        SetPause(false);
    }
    public void BackToManuButtonAction()
    {
        SceneManager.LoadScene(0);
    }
}

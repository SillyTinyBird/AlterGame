using UnityEngine;
using UnityEngine.SceneManagement;
public class PlaymodeInterfaceScript : MonoBehaviour
{
    [SerializeField] private GameObject _deathScreenGroup;
    [SerializeField] private GameObject _overlayGroup;
    [SerializeField] private AdsManager _adsManager;
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
    public void FailActions()
    {
        if (_deathScreenGroup != null && _overlayGroup != null)
        {
            SetPause(true);
            _deathScreenGroup.SetActive(true);
            _overlayGroup.SetActive(false);
        }
    }
    public void ReloadLevel()
    {
        if (_adsManager.IsAdLoaded)
        {
            //_adsManager.ShowAd();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//async, for now
    }
    public void BackToManuButtonAction()
    {
        SceneManager.LoadScene(0);//async, for now
    }
}

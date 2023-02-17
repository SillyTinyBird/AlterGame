using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    public void PlayButtonAction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//async, for now
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    public void PlayButtonAction()
    {
        SceneManager.LoadScene(1);
    }
}

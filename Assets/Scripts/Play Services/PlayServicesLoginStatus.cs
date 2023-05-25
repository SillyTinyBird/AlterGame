using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
public class PlayServicesLoginStatus : Singleton<PlayServicesLoginStatus>
{
    private static bool _IsLoginSucceeded;

    public static bool IsLoginSucceeded { get => _IsLoginSucceeded; set => _IsLoginSucceeded = value; }

    public void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            _IsLoginSucceeded = true;
        }
        else
        {
            _IsLoginSucceeded = false;
        }
    }
}

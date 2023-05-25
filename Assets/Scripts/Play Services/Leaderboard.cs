using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Leaderboard : MonoBehaviour
{
    public void ShowLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIo-XG1foEEAIQAg");
    }
    public static void PostScore(int score)
    {
        PlayGamesPlatform.Instance.ReportScore(score, "CgkIo-XG1foEEAIQAg", (bool success) => { } );
    }
}

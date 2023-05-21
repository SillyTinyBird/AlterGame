using UnityEngine;
using UnityEngine.Localization;

public class StatsMessage : MonoBehaviour
{
    [SerializeField] private LocalizedString _and;
    [SerializeField] private LocalizedString _dougnutsString;

    public string GetStatsString() => ScoreSystem.Instance.Distance.ToString("00000m") + _and.GetLocalizedString() + ScoreSystem.Instance.Dougnuts.ToString("000") + _dougnutsString.GetLocalizedString();
}

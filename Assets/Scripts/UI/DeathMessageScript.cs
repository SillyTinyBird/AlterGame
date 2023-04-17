using UnityEngine;
using UnityEngine.Localization;
using System;

public class DeathMessageScript : MonoBehaviour
{
    [Header("This script needs ObstacleScript attached to be working.")]
    [SerializeField] ObstacleScript _obstacleScript;
    [Header("Messages:")]
    //List<LocalizedString> _listOfString = new List<LocalizedString>(3);//it dosent want to serialize :(
    [SerializeField] private LocalizedString _fallUpper;
    [SerializeField] private LocalizedString _fallMiddle;
    [SerializeField] private LocalizedString _fallLower;
    [SerializeField] private LocalizedString _bonkUpper;
    [SerializeField] private LocalizedString _bonkMiddle;
    [SerializeField] private LocalizedString _bonkLower;

    public string GetDeathMessage()
    {
        string _deathMessage;
        Tuple<bool, int> deathData = _obstacleScript.GetDeathData;
        if(deathData.Item1)//fall
        {
            switch (deathData.Item2)
            {
                case 0:
                    _deathMessage = _fallLower.GetLocalizedString();
                    break;
                case 1:
                    _deathMessage = _fallMiddle.GetLocalizedString();
                    break;
                case 2:
                    _deathMessage = _fallUpper.GetLocalizedString();
                    break;
                default:
                    _deathMessage = "null";
                    break;
            }
        }
        else//hit
        {
            switch (deathData.Item2)
            {
                case 0:
                    _deathMessage = _bonkLower.GetLocalizedString();
                    break;
                case 1:
                    _deathMessage = _bonkMiddle.GetLocalizedString();
                    break;
                case 2:
                    _deathMessage = _bonkUpper.GetLocalizedString();
                    break;
                default:
                    _deathMessage = "null";
                    break;
            }
        }
        return _deathMessage;
    }
}

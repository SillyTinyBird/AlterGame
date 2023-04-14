using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
/// <summary>
/// I have not done justice to this script by calling it "Obstale Script" but
/// this script is basically for the logic behind hitting a wall with the player's face
/// It also contains all the death messages
/// </summary>
public class ObstacleScript : MonoBehaviour
{
    private Dictionary<string, int> _layers = new Dictionary<string, int>() {
        { "LowerLayer", 0 }, { "MiddleLayer", 1 }, { "UpperLayer", 2 },
        { "LowerLayerDrop", 0 }, { "MiddleLayerDrop", 1 }, { "UpperLayerDrop", 2 } };
    [SerializeField] PlaymodeInterfaceScript _failManager;
    [HideInInspector] public string _deathMessage;
    //List<LocalizedString> _listOfString = new List<LocalizedString>(3);//it dosent want to serialize :(
    [SerializeField] private LocalizedString _fallUpper;
    [SerializeField] private LocalizedString _fallMiddle;
    [SerializeField] private LocalizedString _fallLower;
    [SerializeField] private LocalizedString _bonkUpper;
    [SerializeField] private LocalizedString _bonkMiddle;
    [SerializeField] private LocalizedString _bonkLower;
    //^^^^^^^^^^ so yeah this is here for a reason
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerOfColidedObject;
        if (!_layers.TryGetValue(collision.gameObject.tag, out layerOfColidedObject))
        {
            return;
        }
        if(layerOfColidedObject != PlayerController.LayerID)
        {
            return;
        }
        if(collision.gameObject.tag.Contains("Drop"))
        {
            Debug.Log("fall on the layer " + layerOfColidedObject);
            switch (layerOfColidedObject)//im sorry
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
                    break;
            }
            _failManager.FailActions();
        }
        else
        {
            Debug.Log("bonk on the layer " + layerOfColidedObject);
            switch (layerOfColidedObject)//im really sorry
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
                    break;
            }
            _failManager.FailActions();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using System;
/// <summary>
/// I have not done justice to this script by calling it "Obstale Script" but
/// this script is basically for the logic behind hitting a wall with the player's face
/// It also contains all the death messages
/// </summary>
public class ObstacleScript : MonoBehaviour
{
    [Header("This script needs PlayerControoler attached to be working.")]
    [SerializeField] PlayerController _playerController;
    [Header("Parametters:")]
    [SerializeField] PlaymodeInterfaceScript _failManager;
    [SerializeField] float _invinsabilitySeconds = 1f;

    bool _isInvinsible = false;

    private Dictionary<string, int> _layers = new Dictionary<string, int>() {
        { "LowerLayer", 0 }, { "MiddleLayer", 1 }, { "UpperLayer", 2 },
        { "LowerLayerDrop", 0 }, { "MiddleLayerDrop", 1 }, { "UpperLayerDrop", 2 } };
    private Tuple<bool, int> _deathData = new Tuple<bool, int>(false,0);//static cause there will be only one instance of this script on scene 
    /// <summary>
    /// Return conditions of the player's death
    /// </summary>
    /// <returns> 
    /// First param: true = Fall, false = hit;  
    /// Second param: LayerID (0 = lower; 1 = middle; 2 = upper)
    /// </returns>
    public Tuple<bool, int> GetDeathData => _deathData;
    //^^^^^^^^^^ so yeah this is here for a reason
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerOfColidedObject;
        if(_isInvinsible)
        {
            return;
        }
        if (!_layers.TryGetValue(collision.gameObject.tag, out layerOfColidedObject))
        {
            return;
        }
        if(layerOfColidedObject != _playerController.LayerID)
        {
            return;
        }
        if(collision.gameObject.tag.Contains("Drop"))
        {
            _deathData = new Tuple<bool, int>(true, layerOfColidedObject);
            _failManager.FailActions();
        }
        else
        {
            _deathData = new Tuple<bool, int>(false, layerOfColidedObject);
            _failManager.FailActions();
        }
    }
    public void InvisabilityFrames()
    {
        StartCoroutine(InvFramesInSecondsCoroutine());
    }
    IEnumerator InvFramesInSecondsCoroutine()
    {
        _isInvinsible = true;
        yield return new WaitForSeconds(_invinsabilitySeconds);
        _isInvinsible = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// I have not done justice to this script by calling it "Obstale Script" but
/// this script is basically for the logic behind hitting a wall with the player's face
/// </summary>
public class ObstacleScript : MonoBehaviour
{
    private Dictionary<string, int> _layers = new Dictionary<string, int>() { 
        { "LowerLayer", 0 }, { "MiddleLayer", 1 }, { "UpperLayer", 2 }, 
        { "LowerLayerDrop", 0 }, { "MiddleLayerDrop", 1 }, { "UpperLayerDrop", 2 } };
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
        }
        else
        {
            Debug.Log("bonk on the layer " + layerOfColidedObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTutorialScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> _stuffToMove;//we cant move parent cause chunkmanager has te be at zero
    [SerializeField] private float _tutorialSpaceDeltaOnXAxis = 0f;
    [SerializeField] private GameObject _tutorialPrefab;
    [SerializeField] private GameObject _mainPrefab;
    [SerializeField] private GameObject _startLocationParent;
    void Start()
    {
        if (!SettingsSaver.IsTutorialCompleete)
        {
            GameObject.Instantiate(_tutorialPrefab, _startLocationParent.transform);
            _stuffToMove.ForEach(var => var.transform.position = new Vector3(var.transform.position.x + _tutorialSpaceDeltaOnXAxis, var.transform.position.y, var.transform.position.z));
        }
        else
        {
            GameObject.Instantiate(_mainPrefab, _startLocationParent.transform);
        }
    }
}

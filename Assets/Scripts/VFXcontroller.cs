using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXcontroller : MonoBehaviour
{
    [SerializeField] private GameObject _dustJumpPrefab;
    [SerializeField] private GameObject _dustRunPrefab;
    [SerializeField] private GameObject _collectDoughnutPrefab;
    [SerializeField] private GameObject _pointsPrefab;
    [SerializeField] private GameObject _parent;
    private ParticleSystem _dustRun;

    public void PlayDustJump()
    {
        Instantiate(_dustJumpPrefab, _parent.transform);
    }
    public void InitializeDustRun()
    {
        GameObject dustRun = Instantiate(_dustRunPrefab, _parent.transform);
        _dustRun = dustRun.GetComponent<ParticleSystem>();
    }
    public void SetActiveDustRun(bool value)
    {
        if (value)
        {
            _dustRun.Play();
        }
        else
        {
            _dustRun.Stop();
        }
    }
    public void PlayCollect()
    {
        Instantiate(_collectDoughnutPrefab, _parent.transform);
        Instantiate(_pointsPrefab, _parent.transform);
    }
}

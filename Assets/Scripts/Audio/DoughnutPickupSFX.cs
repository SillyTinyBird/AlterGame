using UnityEngine;

public class DoughnutPickupSFX : MonoBehaviour
{
    [SerializeField] AudioClip _doughnutSFX;
    [SerializeField, Range(0f, 1f)] float _volume = 0.5f;
    public void PickupSFX()
    {
        AudioManager.PlaySFX(_doughnutSFX, _volume);
    }
}

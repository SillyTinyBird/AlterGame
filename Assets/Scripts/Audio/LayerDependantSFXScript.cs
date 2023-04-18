using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerDependantSFXScript : MonoBehaviour
{
    [SerializeField] private AudioClip _metalJump;
    [SerializeField] private AudioClip _gravelJump;
    [SerializeField] private AudioClip _woodJump;
    [SerializeField] private AudioClip _trampoline;
    [SerializeField] private AudioClip _slide;
    [SerializeField, Range(0f, 1f)] float _volume = 0.5f;

    // 0 = lower; 1 = middle; 2 = upper.
    public void PlayAscendStartSFX(int layerID)//layerID should represent starting layer (change layer after playing sound)
    {
        if(layerID == 0)
        {
            AudioManager.PlaySFX(_woodJump, _volume);
        }
        else//you cant jump up from 2 level actually so we have only two options
        {
            AudioManager.PlaySFX(_gravelJump, _volume);
        }
    }
    public void PlayAscendEndSFX(int layerID)
    {
        if (layerID == 0)
        {
            AudioManager.PlaySFX(_gravelJump, _volume);
        }
        else
        {
            AudioManager.PlaySFX(_metalJump, _volume);
        }
    }
    public void PlayDescendStartSFX(int layerID)
    {
        if (layerID == 2)
        {
            AudioManager.PlaySFX(_metalJump, _volume);
        }
        else
        {
            AudioManager.PlaySFX(_gravelJump, _volume);
        }
    }
    public void PlayDescendEndSFX(int layerID)
    {
        if (layerID == 2)
        {
            AudioManager.PlaySFX(_gravelJump, _volume);
        }
        else
        {
            AudioManager.PlaySFX(_woodJump, _volume);
        }
    }
    public void PlayTrampolineSFX()
    {
        AudioManager.PlaySFX(_trampoline, _volume);
    }
    public void PlaySlideSFX()
    {
        AudioManager.PlaySFX(_slide, _volume);
    }
    public void PlayJumpSFX(int layerID)
    {
        switch (layerID)
        {
            case 0:
                AudioManager.PlaySFX(_woodJump, _volume);
                return;
            case 1:
                AudioManager.PlaySFX(_gravelJump, _volume);
                return;
            case 2:
                AudioManager.PlaySFX(_metalJump, _volume);
                return;
            default:
                return;
        }
    }
}

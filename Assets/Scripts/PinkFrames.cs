using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkFrames : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Color _color;
    public IEnumerator RenderPink(float duration = 1)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            _spriteRenderer.color = Color.Lerp(_color, Color.white, time / duration);
            yield return null;
        }
        _spriteRenderer.color = Color.white;
    }
}

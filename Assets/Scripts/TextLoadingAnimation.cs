using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TextLoadingAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textToAnimate;
    [SerializeField] private float _frameDelta = 0.5f;
    private List<string> _frames = new List<string>() { ".", "..", "..." };
    private int _currentFrame = 0;

    void Start()
    {
        StartCoroutine(AnimateText(_textToAnimate));
    }
    private IEnumerator AnimateText(TextMeshProUGUI textToAnimate)
    {
        while(true)
        {
            textToAnimate.text = _frames[_currentFrame];
            _currentFrame = _currentFrame == 2 ? 0 : _currentFrame + 1;
            yield return StartCoroutine(WaitForRealSeconds(_frameDelta));
        }
    }
    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}

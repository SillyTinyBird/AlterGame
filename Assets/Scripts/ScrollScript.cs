using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _objetToScroll;
    private static float _scrollAmount = 1f;
    void FixedUpdate()
    {
        _objetToScroll.MovePosition(new Vector3(transform.position.x - _scrollAmount, transform.position.y));
    }
    public static void SetSpeed(float speed) => _scrollAmount = speed;
}

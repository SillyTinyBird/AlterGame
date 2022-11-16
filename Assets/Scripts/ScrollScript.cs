using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _objetToScroll;
    [SerializeField] [Range(0f, 5f)] private float _scrollAmount;
    void FixedUpdate()
    {
        _objetToScroll.MovePosition(new Vector3(transform.position.x - _scrollAmount, transform.position.y));
    }
}

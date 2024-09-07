using System;
using TestGame;
using UnityEngine;

public class BubbleDestoyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.TryGetComponent<BubbleView>(out var component))
        {
            component.ShowBoom();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("OnCollisionEnter");
        if (other.gameObject.TryGetComponent<BubbleView>(out var component))
        {
            component.ShowBoom();
        }
    }

}

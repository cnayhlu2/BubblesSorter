using TestGame;
using UnityEngine;

public class BubbleDestoyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<BubbleView>(out var component))
        {
            component.ShowBoom();
        }
    }
}

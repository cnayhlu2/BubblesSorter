using UnityEngine;

namespace TestGame
{
    public class JoinLineDrawer : MonoBehaviour
    {
        [SerializeField] private Joint2D joint2D;
        [SerializeField] private LineRenderer lineRenderer;

        private void Update()
        {
            if (!joint2D.enabled)
            {
                if (lineRenderer.enabled)
                    lineRenderer.enabled = false;
                return;
            }

            lineRenderer.SetPosition(0, joint2D.transform.position);
            lineRenderer.SetPosition(1, joint2D.connectedBody.transform.position);

            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;
        }
    }
}
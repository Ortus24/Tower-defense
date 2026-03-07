using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform playerTarget;

    void LateUpdate()
    {
        if (playerTarget != null)
        {
            // Keep the Minimap camera at the same Z depth, but follow X and Y
            transform.position = new Vector3(playerTarget.position.x, playerTarget.position.y, transform.position.z);
        }
    }
}

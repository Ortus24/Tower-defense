using UnityEngine;

public class DestroyWhenCall : MonoBehaviour
{
    public void DestroyNow()
    {
        Debug.Log(gameObject.scene.name);
        Destroy(gameObject);
    }

}

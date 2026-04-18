using UnityEngine;

public class IceBlock : MonoBehaviour
{
    public void Break()
    {
        Debug.Log("Ice broken!");
        Destroy(gameObject);
    }
}
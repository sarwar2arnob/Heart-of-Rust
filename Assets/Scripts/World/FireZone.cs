using UnityEngine;

public class FireZone : MonoBehaviour
{
    public void Extinguish()
    {
        Debug.Log("Fire extinguished!");
        gameObject.SetActive(false);
    }
}
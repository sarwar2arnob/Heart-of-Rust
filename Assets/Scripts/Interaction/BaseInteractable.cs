using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour
{
    public abstract void Interact(PlayerState player);
}
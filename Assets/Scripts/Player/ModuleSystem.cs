using UnityEngine;

public class ModuleSystem : MonoBehaviour
{
    private PlayerState state;

    void Awake()
    {
        state = GetComponent<PlayerState>();
    }

    public bool HasModule(ModuleType type)
    {
        return state.equippedModule != null &&
               state.equippedModule.type == type;
    }
}

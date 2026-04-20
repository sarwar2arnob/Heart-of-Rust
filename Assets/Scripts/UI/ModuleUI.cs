using TMPro;
using UnityEngine;

public class ModuleUI : MonoBehaviour
{
    public TextMeshProUGUI moduleText;
    private PlayerEquipment equipment;

    void Start()
    {
        equipment = FindFirstObjectByType<PlayerEquipment>();
    }

    void Update()
    {
        if (equipment.equippedModule != null)
        {
            moduleText.text = equipment.equippedModule.type.ToString();
        }
    }
}
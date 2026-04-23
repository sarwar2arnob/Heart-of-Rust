using TMPro;
using UnityEngine;
using UnityEngine.UI; // 👈 Required for the Image component

public class ModuleUI : MonoBehaviour
{
    public TextMeshProUGUI moduleText;
    public Image moduleIcon; // 👈 Add this reference for your UI Image

    private PlayerEquipment equipment;

    void Start()
    {
        equipment = FindFirstObjectByType<PlayerEquipment>();
    }

    void Update()
    {
        if (equipment.equippedModule != null)
        {
            // Update the text
            moduleText.text = equipment.equippedModule.type.ToString();

            // Update the image and ensure it is visible
            if (equipment.equippedModule.icon != null)
            {
                moduleIcon.sprite = equipment.equippedModule.icon;
                moduleIcon.enabled = true;
            }
        }
        else
        {
            // Failsafe: Hide the icon and show "None" if nothing is equipped
            if (moduleIcon != null) moduleIcon.enabled = false;
            if (moduleText != null) moduleText.text = "None";
        }
    }
}
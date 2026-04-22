// UI/CraftResultPopup.cs

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftResultPopup : MonoBehaviour
{
    public static CraftResultPopup Instance;

    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Content")]
    [SerializeField] private Image resultIcon;
    [SerializeField] private TMP_Text resultNameText;
    [SerializeField] private TMP_Text resultTypeText; // "MODULE UNLOCKED" or "PART UNLOCKED"

    [Header("Timing")]
    [SerializeField] private float displayDuration = 2.5f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(RecipeData recipe)
    {
        // Stop previous coroutine if popup is already showing
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        // Fill content
        resultIcon.sprite = recipe.resultIcon;
        resultIcon.enabled = recipe.resultIcon != null;

        if (recipe.result.type == ResultType.Module)
        {
            resultTypeText.text = "MODULE UNLOCKED";
            resultNameText.text = recipe.result.module != null
                ? recipe.result.module.type.ToString()
                : "Unknown Module";
        }
        else
        {
            resultTypeText.text = "PART UNLOCKED";
            resultNameText.text = recipe.result.part.ToString();
        }

        panel.SetActive(true);
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        panel.SetActive(false);
    }
}
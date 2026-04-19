using UnityEngine;
using TMPro;
using DG.Tweening;

public class GeppettoDialogueUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI dialogueText;

    [TextArea]
    public string dialogue;

    public float startDelay = 2f;      // ⏱ wait before showing
    public float fadeDuration = 0.5f;  // fade speed
    public float visibleDuration = 5f; // ⏱ stay time

    void Start()
    {
        canvasGroup.alpha = 0;
        dialogueText.text = dialogue;

        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(startDelay);                      // wait 2 sec
        seq.Append(canvasGroup.DOFade(1f, fadeDuration));    // fade in
        seq.AppendInterval(visibleDuration);                 // stay 5 sec
        seq.Append(canvasGroup.DOFade(0f, fadeDuration));    // fade out
    }
}
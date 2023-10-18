using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScalingAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    [SerializeField] private float duration;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(maxScale, duration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(minScale, duration);
    }
}
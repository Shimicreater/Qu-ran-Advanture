using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverScale : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    public float hoverScale = 1.06f;
    public float pressedScale = 0.95f;
    public float lerpSpeed = 12f;

    private Vector3 defaultScale;
    private Vector3 targetScale;

    void Awake()
    {
        defaultScale = transform.localScale;
        targetScale = defaultScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * lerpSpeed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = defaultScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = defaultScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = defaultScale * pressedScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = defaultScale * hoverScale;
    }
}

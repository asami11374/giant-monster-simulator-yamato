using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 左下配置用のシンプルな仮想ジョイスティック。
/// </summary>
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("UI References")]
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    [Header("Settings")]
    [SerializeField] private float handleRange = 70f;

    /// <summary>
    /// -1〜1 の入力値（x:左右, y:前後）
    /// </summary>
    public Vector2 Input { get; private set; }

    private Canvas canvas;
    private Camera uiCamera;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = canvas.worldCamera;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handle == null)
        {
            return;
        }

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            uiCamera,
            out localPoint
        );

        Vector2 normalized = localPoint / handleRange;
        normalized = Vector2.ClampMagnitude(normalized, 1f);

        Input = normalized;
        handle.anchoredPosition = Input * handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Input = Vector2.zero;
        if (handle != null)
        {
            handle.anchoredPosition = Vector2.zero;
        }
    }
}

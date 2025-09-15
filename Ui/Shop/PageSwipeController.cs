using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;

public class PageSwipeController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [Header("UI References")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private UnityEvent<int> onSwipe;

    [Header("Settings")]
    [SerializeField] private float snapDuration = 0.3f;
    [SerializeField] private float swipeThresholdRatio = 0.5f;

    private float pageWidth;
    private int startPageIndex;
    private float dragStartPosX;

    private int TotalPages => contentPanel.childCount;
    public int CurrentPage => Mathf.Clamp(GetNearestPageIndex(), 0, Mathf.Max(TotalPages - 1, 0));

    private void Start()
    {
        if (scrollRect != null)
        {
            scrollRect.horizontal = true;
            scrollRect.vertical = false;
        }
        UpdateContentSize();
    }

    public void UpdateContentSize()
    {
        if (TotalPages == 0) return;

        var first = (RectTransform)contentPanel.GetChild(0);
        pageWidth = first.rect.width > 0 ? first.rect.width : contentPanel.rect.width;
        if (pageWidth <= 0) return;

        contentPanel.sizeDelta = new Vector2(TotalPages * pageWidth, contentPanel.sizeDelta.y);
        MoveToPage(CurrentPage, false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPosX = contentPanel.anchoredPosition.x;
        startPageIndex = GetNearestPageIndex();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (TotalPages == 0 || pageWidth <= 0) return;

        float delta = contentPanel.anchoredPosition.x - dragStartPosX;
        int targetPage = startPageIndex - GetSwipeDirection(delta);
        MoveToPage(targetPage);
    }

    private int GetSwipeDirection(float delta)
    {
        float swipeThreshold = pageWidth * swipeThresholdRatio;
        if (delta > swipeThreshold) return 1;   // vuốt phải
        if (delta < -swipeThreshold) return -1; // vuốt trái
        return 0;
    }

    public void MoveToPage(int pageIndex) => MoveToPage(pageIndex, true);

    public void MoveToPage(int pageIndex, bool invokeEvent)
    {
        pageIndex = Mathf.Clamp(pageIndex, 0, Mathf.Max(TotalPages - 1, 0));
        Vector2 targetPos = new Vector2(-pageIndex * pageWidth, contentPanel.anchoredPosition.y);
        contentPanel.DOAnchorPos(targetPos, snapDuration).SetEase(Ease.OutQuad);

        if (invokeEvent) onSwipe?.Invoke(pageIndex);
    }

    private int GetNearestPageIndex()
    {
        if (TotalPages == 0 || pageWidth <= 0) return 0;
        return Mathf.Clamp(Mathf.RoundToInt(-contentPanel.anchoredPosition.x / pageWidth), 0, TotalPages - 1);
    }

    public void NextPage() => MoveToPage(CurrentPage + 1);
    public void PreviousPage() => MoveToPage(CurrentPage - 1);
}

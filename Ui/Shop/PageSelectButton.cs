using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PageSelectButton : MonoBehaviour {
    [SerializeField] int pageIdx = 0;   
    [SerializeField] Image icon;
    [SerializeField] UnityEvent<int> onClick;

    [Header("For Debug")]
    [SerializeField] Button pageButton;

    public int PageIdx => pageIdx; 

    private void Start() {
        pageButton = GetComponent<Button>();
        pageButton?.onClick.AddListener(OnPageClick);
    }

    private void OnDestroy() {
        pageButton?.onClick.RemoveListener(OnPageClick);

    }

    private void OnPageClick() {
        onClick?.Invoke(pageIdx);
    }

    public void SetIcon(Sprite icon) {
        this.icon.sprite = icon;
    }
}

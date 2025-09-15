using System.Collections.Generic;
using UnityEngine;

public class PageButtonControl : MonoBehaviour {
    [SerializeField] List<PageSelectButton> buttons = new();
    [SerializeField] Sprite selectedIcon;
    [SerializeField] Sprite unselectedIcon;

    public void UpdateButtonIcon(int chooseIdx) {
        if (buttons.Count < 1) return;
        buttons.ForEach(b => b.SetIcon(b.PageIdx == chooseIdx ? selectedIcon : unselectedIcon));
    }
}

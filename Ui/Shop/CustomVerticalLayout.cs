using UnityEngine;
using UnityEngine.UI;

public class CustomVerticalLayoutGroup : VerticalLayoutGroup {
    public bool ignoreFirstElement = true; 

    public override void SetLayoutVertical() {
        base.SetLayoutVertical(); 

       
        RectTransform rect = GetComponent<RectTransform>();

        if (transform.childCount <= 1 || !ignoreFirstElement) return;

       
        float totalHeight = 0f;
        for (int i = 1; i < transform.childCount; i++) // Bắt đầu từ index 1 để bỏ qua phần tử đầu
        {
            RectTransform child = transform.GetChild(i) as RectTransform;
            if (child != null) {
                totalHeight += child.sizeDelta.y; // Cộng dồn chiều cao của từng phần tử con
            }
        }

       
        totalHeight += spacing * (transform.childCount - 2);

      
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, totalHeight);
    }
}

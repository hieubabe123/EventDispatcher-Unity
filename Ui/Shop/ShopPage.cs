using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPage : MonoBehaviour
{
    [SerializeField] ShopCell shopCellPrefab;
   // [SerializeField] int maxElement = 6;
   List<ShopCell> shopCells = new();
    public void SpawnCell(int startIdx, int endIdx) {
        int totalElement = (endIdx - startIdx + 1); // Because Idx can start at 0
        int initIdx = startIdx;
        for (int idx = 0; idx < totalElement; idx++) { 
            var cell = Instantiate(shopCellPrefab, this.transform);
            cell.Initialize(initIdx);
            shopCells.Add(cell);
            initIdx++;
        }
    }

    public void CheckCellCurrentPage(int selectedIdx) {
        shopCells.ForEach(c => c.UpdateCellState(selectedIdx));
    }

    public void GetPageDefauttState() {
        shopCells.ForEach(c => c.UpdateBasicCellState());
    }
}

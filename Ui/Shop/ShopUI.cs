using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [Header("Shop Reference")]
    [SerializeField] Transform pageParentTrs;
    [SerializeField] ShopPage pagePrefab;
    [SerializeField] PageSwipeController pageSwipeCtrl;
    [Header("Shop Config")]
    [SerializeField] int maxElementInPage = 6;

    int maxPage;
    List<ShopPage> pageList = new List<ShopPage>();
    ShopPage currentPage => pageList[pageSwipeCtrl.CurrentPage];
    int currentPageIdx => pageSwipeCtrl.CurrentPage;
    int currentCharacterIdx => Mathf.Clamp(UserData.CurrentCharacterIdx / maxElementInPage, 0, Mathf.Max(maxPage - 1, 0));
    bool spawned = false;

    private void Start()
    {
        SpawnPage();
        spawned = true;

        EventDispatcher.AddListener<EventDefine.OnSelectCharacter>(OnSelectCharacter);
        EventDispatcher.AddListener<EventDefine.OnChangeCharacter>(OnChangeCharacter);
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveListener<EventDefine.OnSelectCharacter>(OnSelectCharacter);
        EventDispatcher.RemoveListener<EventDefine.OnChangeCharacter>(OnChangeCharacter);

    }

    private void OnEnable()
    {
        if (!spawned) return;
        StartCoroutine(UpdateLayoutAndSnapToPage());

        EventDispatcher.Dispatch(new EventDefine.OnSelectCharacter { CharacterIdx = UserData.CurrentCharacterIdx, changeType = ChangeType.Character });
    }

    private void OnDisable()
    {

    }

    private void OnSelectCharacter(EventDefine.OnSelectCharacter character)
    {
        CheckShopCell(character.CharacterIdx);
    }

    private void OnChangeCharacter(EventDefine.OnChangeCharacter character)
    {
        CheckShopCell(UserData.CurrentCharacterIdx);
    }
    private void SpawnPage()
    {
        int currentElements = 0;
        maxPage = Mathf.CeilToInt((float)GameData.Instance.CharacterLength / maxElementInPage);

        for (int i = 0; i < maxPage; i++)
        {
            var page = Instantiate(pagePrefab, pageParentTrs);
            int endIdx = Mathf.Min(currentElements + maxElementInPage - 1, GameData.Instance.CharacterLength - 1);
            page.SpawnCell(currentElements, endIdx);
            currentElements += maxElementInPage;
            pageList.Add(page);
        }
    }

    private IEnumerator UpdateLayoutAndSnapToPage()
    {
        yield return new WaitForEndOfFrame();
        pageSwipeCtrl.UpdateContentSize();
        pageSwipeCtrl.MoveToPage(currentCharacterIdx, false);
        CheckShopCell(UserData.CurrentCharacterIdx);
        EventDispatcher.Dispatch(new EventDefine.OnSelectCharacter { CharacterIdx = UserData.CurrentCharacterIdx, changeType = ChangeType.Character });
    }


    void CheckShopCell(int selectIdx)
    {
        int pageIdx = Mathf.Clamp(selectIdx / maxElementInPage, 0, Mathf.Max(maxPage - 1, 0));
        for (int i = 0; i < pageList.Count; i++)
        {
            if (i == pageIdx) pageList[i].CheckCellCurrentPage(selectIdx);
            else pageList[i].GetPageDefauttState();
        }
    }
}

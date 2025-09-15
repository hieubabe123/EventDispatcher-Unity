using UnityEngine;
using UnityEngine.UI;

public class ShopCell : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image thumbnail;
    [SerializeField] private Sprite unlockBackground;
    [SerializeField] private Sprite lockBackground;
    [SerializeField] private Sprite normalBackground;
    [SerializeField] private GameObject ownedCheck;

    private Button button;
    private int itemIdx;
    private Character characterInfo;

    private void Start()
    {
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnSelectClick);
    }

    private void OnDestroy()
    {
        button?.onClick.RemoveListener(OnSelectClick);
    }

    private void OnSelectClick()
    {
        EventDispatcher.Dispatch(new EventDefine.OnSelectCharacter { CharacterIdx = itemIdx, changeType = ChangeType.Character });
        if (IsUnlocked())
        {
            UserData.CurrentCharacterIdx = itemIdx;
            EventDispatcher.Dispatch(new EventDefine.OnChangeCharacter { CharacterIdx = itemIdx, changeType = ChangeType.Character });
            FirebaseManager.Instance.LogEventEquipSkin(itemIdx);
            Debug.Log($"FIREBASE ------- Equip character {itemIdx} successfully -------");
        }
    }

    public void Initialize(int characterIdx)
    {
        itemIdx = characterIdx;
        characterInfo = GameData.Instance.GetCharacter(characterIdx);
        thumbnail.sprite = characterInfo.ShopIcon2D;
        UpdateCellState(UserData.CurrentCharacterIdx);
    }

    private bool IsUnlocked() => UserData.OwnedCharacterListIdx.Contains(itemIdx);

    public void UpdateCellState(int selectedIdx)
    {
        bool isSelected = itemIdx == selectedIdx && IsUnlocked();
        thumbnail.gameObject.SetActive(IsUnlocked());
        background.sprite = isSelected ? normalBackground : (IsUnlocked() ? unlockBackground : lockBackground);
        ownedCheck.SetActive(isSelected);
    }

    public void UpdateBasicCellState()
    {
        background.sprite = IsUnlocked() ? unlockBackground : lockBackground;
        ownedCheck.SetActive(false);
    }
}
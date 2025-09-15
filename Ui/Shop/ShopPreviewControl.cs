using Castle.Core.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPreviewControl : MonoBehaviour
{
    [SerializeField] private Button adBtn;
    [SerializeField] private Button coinBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private Image characterPreview;

    private Character currentCharacter;

    [Header("--------------------- Event Name ---------------------")]
    private const string BTN_BUY_CHARACTER = "btn_buy_character";
    private const string BTN_WATCH_AD_FOR_COIN = "btn_watch_ad_for_coin";
    private const string USER_COIN_CHANGE = "user_coin_change";

    private void Start()
    {
        SubscribeEvents();
        UpdateCharacterPreview(UserData.CurrentCharacterIdx);
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        EventDispatcher.AddListener<EventDefine.OnSelectCharacter>(OnSelectCharacter);
        EventDispatcher.AddListener<EventDefine.OnChangeCharacter>(OnChangeCharacter);
        adBtn.onClick.AddListener(OnAdsClick);
        coinBtn.onClick.AddListener(OnCoinClick);
        backBtn.onClick.AddListener(OnBackClick);
    }

    private void UnsubscribeEvents()
    {
        EventDispatcher.RemoveListener<EventDefine.OnSelectCharacter>(OnSelectCharacter);
        EventDispatcher.RemoveListener<EventDefine.OnChangeCharacter>(OnChangeCharacter);
        adBtn.onClick.RemoveListener(OnAdsClick);
        coinBtn.onClick.RemoveListener(OnCoinClick);
        backBtn.onClick.RemoveListener(OnBackClick);
    }

    private void OnSelectCharacter(EventDefine.OnSelectCharacter characterEvent)
    {
        UpdateCharacterPreview(characterEvent.CharacterIdx);
    }

    private void OnChangeCharacter(EventDefine.OnChangeCharacter _)
    {
        TogglePurchaseButtons(false);
    }

    private void OnAdsClick()
    {
        RewardAds.GetReward(ShowRewardEnum.get_coin.ToString(), () =>
        {
            FirebaseManager.Instance.LogEventCoin("+" + GameData.Instance.coinRewardedByAd, USER_COIN_CHANGE);
            FirebaseManager.Instance.LogEventButton(BTN_WATCH_AD_FOR_COIN);
            Debug.Log($"FIREBASE ------- Watch ad for coin successfully -------");
            RewardCoin();
        });
    }

    private void OnCoinClick()
    {
        FirebaseManager.Instance.LogEventButton(BTN_BUY_CHARACTER);
        if (UserData.CurrentCoin < currentCharacter.ShopPrice)
        {
            Debug.Log("Not enough coin");
            UINotification.Instance.ShowNotification(1, "Not enough coin");
            return;
        }
        UserData.CurrentCoin -= currentCharacter.ShopPrice;
        FirebaseManager.Instance.LogEventCoin("-" + currentCharacter.ShopPrice, USER_COIN_CHANGE);
        FirebaseManager.Instance.LogEventButton(BTN_BUY_CHARACTER);
        Debug.Log($"FIREBASE ------- Purchase character {currentCharacter.Idx} successfully -------");
        EventDispatcher.Dispatch<EventDefine.OnUpdateCurrency>();
        PurchaseCharacter();
        UINotification.Instance.ShowNotification(1, "New Character Unlocked!");
    }

    private void OnBackClick()
    {
        FirebaseManager.Instance.LogEventPopup(FbEventEnum.popup_close, "back_shop");
        Debug.Log("Shop Back Clicked");
    }

    private void RewardCoin()
    {
        UserData.CurrentCoin += GameData.Instance.coinRewardedByAd;
        UINotification.Instance.ShowNotification(0, $"You got {GameData.Instance.coinRewardedByAd} coins!");
        EventDispatcher.Dispatch<EventDefine.OnUpdateCurrency>();
    }

    private void PurchaseCharacter()
    {
        if (!UserData.OwnedCharacterListIdx.Contains(currentCharacter.Idx))
        {
            var list = UserData.OwnedCharacterListIdx;
            list.Add(currentCharacter.Idx);
            UserData.OwnedCharacterListIdx = list;
        }
        UserData.CurrentCharacterIdx = currentCharacter.Idx;
        EventDispatcher.Dispatch(new EventDefine.OnChangeCharacter { CharacterIdx = currentCharacter.Idx });
        FirebaseManager.Instance.LogEventBuySkin(currentCharacter.Idx);
        Debug.Log($"FIREBASE ------- Purchase character {currentCharacter.Idx} successfully -------");
    }

    private void UpdateCharacterPreview(int characterIdx)
    {
        currentCharacter = GameData.Instance.GetCharacter(characterIdx);
        characterPreview.sprite = currentCharacter.ShopIcon2D;
        priceTxt.text = currentCharacter.ShopPrice.ToString();
        TogglePurchaseButtons(!UserData.OwnedCharacterListIdx.Contains(currentCharacter.Idx));
    }

    private void TogglePurchaseButtons(bool isActive)
    {
        adBtn.gameObject.SetActive(isActive);
        coinBtn.gameObject.SetActive(isActive);
    }
}

using UnityEngine;

public class MainUICtrl : MonoBehaviour
{
    private MainUIView view;

    private void Awake()
    {
        view = GetComponent<MainUIView>();
        if (view != null)
            SubscribeToEvents();
    }
    private void OnDestroy()
    {
        if (view != null)
            UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        view.OnSettingClicked += OnSettingClick;
        view.OnShopClicked += OnShopClick;
        //view.OnNoAdClicked += OnNoAdClick;
        view.OnChallengeClicked += OnChallengeClick;
        view.OnMiniGameClicked += OnMiniGameClick;
        view.OnSeasonSectionBackClicked += OnSeasonSectionBackClick;
        view.OnMiniGameBackClicked += OnMiniGameBackClick;
    }

    private void UnsubscribeFromEvents()
    {
        view.OnSettingClicked -= OnSettingClick;
        view.OnShopClicked -= OnShopClick;
        //view.OnNoAdClicked -= OnNoAdClick;
        view.OnChallengeClicked -= OnChallengeClick;
        view.OnMiniGameClicked -= OnMiniGameClick;
        view.OnSeasonSectionBackClicked -= OnSeasonSectionBackClick;
        view.OnMiniGameBackClicked -= OnMiniGameBackClick;
    }

    private void OnSettingClick()
    {
        PopupManager.Instance.Show<SettingPopup>();
        FirebaseManager.Instance.LogEventPopup(FbEventEnum.btn_settings, "settings");
    }
    private void OnShopClick()
    {
        FirebaseManager.Instance.LogEventPopup(FbEventEnum.btn_shop, "shop");
        Debug.Log("Shop Clicked");
    }
    // private void OnNoAdClick() => Debug.Log("NoAd Clicked");
    private void OnChallengeClick()
    {
        FirebaseManager.Instance.LogEventPopup(FbEventEnum.btn_challenge, "challenge");
        Debug.Log("Challenge Called");
    }
    private void OnMiniGameClick()
    {
        FirebaseManager.Instance.LogEventPopup(FbEventEnum.btn_minigame, "minigame");
        Debug.Log("MiniGame Called");
    }

    private void OnSeasonSectionBackClick()
    {
        FirebaseManager.Instance.LogEventPopup(FbEventEnum.popup_close, "back_season_section");
        Debug.Log("Season Section Back Clicked");
    }

    private void OnMiniGameBackClick()
    {
        FirebaseManager.Instance.LogEventPopup(FbEventEnum.popup_close, "back_mini_game");
        Debug.Log("Mini Game Back Clicked");
    }

}

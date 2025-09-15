using UnityEngine;
using UnityEngine.UI;

public class SeasonElement : MonoBehaviour
{
    [SerializeField] int seasonIdx;
    [SerializeField] Button seasonButton;

    [SerializeField] GameObject seasonBlock;
    private void Start()
    {
        if (seasonButton == null) seasonButton = GetComponent<Button>();
        seasonBlock.SetActive(!UserData.IsUnlockSeason(seasonIdx));
        seasonButton?.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        seasonButton?.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        GameManager.Instance.SetStoryMode(true);
        GameManager.Instance.SetCurrentSeason(seasonIdx);
        EventDispatcher.Dispatch(new EventDefine.OnChangeScene() { SceneName = GameScene.Lobby });
        FirebaseManager.Instance.LogEventButton("btn_select_season_" + seasonIdx);
    }

}

using UnityEngine;
using UnityEngine.UI;

public class MiniGameSectionUI : MonoBehaviour {
    [SerializeField][Scene] string gameScene;
    [SerializeField] Button seasonButton;
    private void Start() {
        if (seasonButton == null) seasonButton = GetComponent<Button>();
        seasonButton?.onClick.AddListener(OnClick);
    }

    private void OnDestroy() {
        seasonButton?.onClick.RemoveListener(OnClick);
    }

    private void OnClick() {
        GameManager.Instance.SetStoryMode(false);
        EventDispatcher.Dispatch(new EventDefine.OnChangeScene() { SceneName = gameScene });
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIView : MonoBehaviour
{
    [Header("UIView")]
    [SerializeField] GameObject view;
    [Space]
    [Header("Button")]
    [Header("Common Buttons")]
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button shopBtn;
    [SerializeField] private Button noAdBtn;

    [Header("Mode Buttons")]
    [SerializeField] private Button challengeBtn;
    [SerializeField] private Button miniGameBtn;

    [Header("SeasonSectionView")]
    [SerializeField] private Button seasonSectionBackBtn;
    [SerializeField] private Button miniGameBackBtn;


    public event Action OnSettingClicked;
    public event Action OnShopClicked;
    public event Action OnNoAdClicked;
    public event Action OnChallengeClicked;
    public event Action OnMiniGameClicked;
    public event Action OnSeasonSectionBackClicked;
    public event Action OnMiniGameBackClicked;

    private void Start()
    {
        AddButtonListeners();
    }

    private void OnDestroy()
    {
        RemoveButtonListeners();
    }

    private void AddButtonListeners()
    {
        settingBtn?.onClick.AddListener(() => OnSettingClicked?.Invoke());
        shopBtn?.onClick.AddListener(() => OnShopClicked?.Invoke());
        noAdBtn?.onClick.AddListener(() => OnNoAdClicked?.Invoke());
        challengeBtn?.onClick.AddListener(() => OnChallengeClicked?.Invoke());
        miniGameBtn?.onClick.AddListener(() => OnMiniGameClicked?.Invoke());
        seasonSectionBackBtn?.onClick.AddListener(() => OnSeasonSectionBackClicked?.Invoke());
        miniGameBackBtn?.onClick.AddListener(() => OnMiniGameBackClicked?.Invoke());
    }

    private void RemoveButtonListeners()
    {
        settingBtn?.onClick.RemoveAllListeners();
        shopBtn?.onClick.RemoveAllListeners();
        noAdBtn?.onClick.RemoveAllListeners();
        challengeBtn?.onClick.RemoveAllListeners();
        miniGameBtn?.onClick.RemoveAllListeners();
        seasonSectionBackBtn?.onClick.RemoveAllListeners();
        miniGameBackBtn?.onClick.RemoveAllListeners();
    }


}

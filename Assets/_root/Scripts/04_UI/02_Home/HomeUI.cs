using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUI
{
    [TitleGroup("Basic info:")]
    [SerializeField] private Image imgAvatar;
    [SerializeField] private Image imgAvatarFrame;
    [SerializeField] private TMP_Text txtUsername;
    [SerializeField] private TMP_Text txtLevel;

    [TitleGroup("Currency:")]
    [SerializeField] private TMP_Text txtGold;
    [SerializeField] private TMP_Text txtDiamond;
    [SerializeField] private TMP_Text txtEnergy;

    [TitleGroup("Bot bar:")]
    [SerializeField] private Button btnTavern;
    [SerializeField] private Button btnLineUp;
    [SerializeField] private Button btnQuest;
    [SerializeField] private Button btnInventory;
    [SerializeField] private Button btnLeaderboard;
    [SerializeField] private Button btnCampaign;

    [TitleGroup("Others:")]
    [SerializeField] private TMP_Text fpsTxt;
    [SerializeField] private float logInterval = 0.5f;

    private float _curTime;
    private int _frameCount;

    private void Update()
    {
        CalculateFPS();
    }

    protected override void RegisterEvents()
    {
        this.AddListener(EventID.ON_UPDATE_CURRENCIES, UpdateCurrencies);
    }

    protected override void UnregisterEvents()
    {
        this.RemoveListener(EventID.ON_UPDATE_CURRENCIES, UpdateCurrencies);
    }

    protected override void AssignUICallback()
    {
        btnTavern.onClick.AddListener(OpenTavern);
        btnLineUp.onClick.AddListener(OpenLineUp);
        btnQuest.onClick.AddListener(OpenQuest);
        btnInventory.onClick.AddListener(OpenInventory);
        btnLeaderboard.onClick.AddListener(OpenLeaderboard);
        btnCampaign.onClick.AddListener(OpenCampaign);
    }

    private void CalculateFPS()
    {
        _curTime += Time.deltaTime;
        _frameCount++;
        if (_curTime >= logInterval)
        {
            var fps = Mathf.RoundToInt(_frameCount / _curTime);
            fpsTxt.text = "FPS: " + fps;
            _curTime -= logInterval;
            _frameCount = 0;
        }
    }

    private void UpdateCurrencies(object data = null)
    {
        PlayFabManager.Instance.FetchCurrencies((currenciesDict, rechargeDict) =>
        {
            txtGold.text = $"{currenciesDict[PlayFabKey.CURRENCY_GOLD]}";
            txtDiamond.text = $"{currenciesDict[PlayFabKey.CURRENCY_DIAMOND]}";
            txtEnergy.text = $"{currenciesDict[PlayFabKey.CURRENCY_ENERGY]}" +
                             $"/{rechargeDict[PlayFabKey.CURRENCY_ENERGY].RechargeMax}";
        });
    }

    public void SetUsername(string username)
    {
        txtUsername.text = username;
    }

    public void SetPlayerInfo(int level, string avatarID, string avatarFrameID)
    {
        txtLevel.text = $"{level}";
        imgAvatar.sprite = AssetLibrary.Instance.GetAvatar(avatarID);
        imgAvatarFrame.sprite = AssetLibrary.Instance.GetAvatarFrame(avatarID);
    }

    public void OpenTavern()
    {
        UIManager.Open<TavernUI>();
    }

    public void OpenLineUp()
    {
        UIManager.Open<LineUpUI>();
    }

    public void OpenQuest()
    {
        UIManager.Open<QuestUI>();
    }

    public void OpenInventory()
    {
        UIManager.Open<InventoryUI>();
    }

    public void OpenLeaderboard()
    {
        //todo
    }

    public void OpenCampaign()
    {
        //todo
    }
}
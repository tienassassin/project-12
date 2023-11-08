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

    [TitleGroup("Time")]
    [SerializeField] private Image imgTime;
    [SerializeField] private Sprite[] sprTimes;

    [TitleGroup("Others:")]
    [SerializeField] private TMP_Text fpsTxt;
    [SerializeField] private float logInterval = 0.5f;

    private float _curTime;
    private int _frameCount;

    public static void Open()
    {
        UIManager.Instance.ShowUI(nameof(HomeUI));
    }

    public static void Close()
    {
        UIManager.Instance.HideUI(nameof(HomeUI));
    }

    protected override void Awake()
    {
        base.Awake();

        SetTime();

        this.AddListener(EventID.ON_UPDATE_CURRENCIES, UpdateCurrencies);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.ON_UPDATE_CURRENCIES, UpdateCurrencies);
    }

    private void Update()
    {
        CalculateFPS();
    }

    private void SetTime()
    {
        var now = DateTime.Now;
        imgTime.sprite = sprTimes[now.Hour < 12 ? 0 : 1];
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

    public void UpdateCurrencies(object data = null)
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
        imgAvatar.sprite = AssetLibrary.Instance.GetAvatarAsset(avatarID).avatar;
        imgAvatarFrame.sprite = AssetLibrary.Instance.GetAvatarFrameAsset(avatarID).avatarFrame;
    }

    public void OpenValhalla()
    {
        TavernUI.Show();
    }

    public void OpenLineUp()
    {
        LineUpUI.Show();
    }

    public void OpenQuest()
    {
        // QuestUI.Show();
    }

    public void OpenInventory()
    {
        // InventoryUI.Show();
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
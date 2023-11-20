using System;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayFabManager : Singleton<PlayFabManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void LoginWithDeviceID()
    {
        GlobalUI.Instance.ShowNotification("Please wait...");

        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            result => { GlobalUI.Instance.ShowNotification("Login successfully."); }, error =>
            {
                DebugLog.Message("PlayFab: Login failed, error: " + error.ErrorMessage);
                GlobalUI.Instance.ShowNotification("Login failed. Please check your information.");
            });
    }

    public void LoginWithCredential(string email, string password,
        Action<LoginResult> success = null,
        Action<PlayFabError> fail = null)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, result => { success?.Invoke(result); }, error =>
        {
            DebugLog.Message("PlayFab: Login failed, error: " + error.GenerateErrorReport());
            fail?.Invoke(error);
        });
    }

    public void RegisterNewCredential(string email, string password,
        Action<RegisterPlayFabUserResult> success = null,
        Action<PlayFabError> fail = null)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, result => { success?.Invoke(result); }, error =>
        {
            DebugLog.Message("PlayFab: Login failed, error: " + error.GenerateErrorReport());
            fail?.Invoke(error);
        });
    }

    public void RecoveryCredential(string email,
        Action<SendAccountRecoveryEmailResult> success = null,
        Action<PlayFabError> fail = null)
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = PlayFabKey.TITLE_ID
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, result => { success?.Invoke(result); }, error =>
        {
            DebugLog.Message("PlayFab: Login failed, error: " + error.GenerateErrorReport());
            fail?.Invoke(error);
        });
    }

    public void ChangeUsername(string username,
        Action<UpdateUserTitleDisplayNameResult> success = null,
        Action<PlayFabError> fail = null)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result => { success?.Invoke(result); }, error =>
        {
            DebugLog.Message("PlayFab: Update username failed, error: " + error.GenerateErrorReport());
            fail?.Invoke(error);
        });
    }

    public void SavePlayerData(string key, string data,
        Action<UpdateUserDataResult> success = null,
        Action<PlayFabError> fail = null)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> { { key, data } }
        };

        PlayFabClientAPI.UpdateUserData(request,
            result =>
            {
                DebugLog.Message($"PlayFab: Save player data, key={key} data={data}");
                success?.Invoke(result);
            },
            error =>
            {
                DebugLog.Error($"PlayFab: Save player data failed, key={key}, error: {error.ErrorMessage}");
                fail?.Invoke(error);
            });
    }

    [Button]
    public void LoadPlayerData(string key, Action<string> onLoaded,
        Action<GetUserDataResult> success = null,
        Action<PlayFabError> fail = null)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result != null && result.Data.TryGetValue(key, out var record))
            {
                DebugLog.Message($"PlayFab: Load player data, key={key} data={record.Value}");
                onLoaded?.Invoke(record.Value);
            }
            else
            {
                DebugLog.Message($"PlayFab: Load player data, key={key} not exist");
            }

            success?.Invoke(result);
        }, error =>
        {
            DebugLog.Error($"PlayFab: Load player data failed, key={key}, error: {error.ErrorMessage}");
            fail?.Invoke(error);
        });
    }

    public void SaveAllPlayerData(Dictionary<string, string> data,
        Action<UpdateUserDataResult> success = null,
        Action<PlayFabError> fail = null)
    {
        var request = new UpdateUserDataRequest
        {
            Data = data
        };

        PlayFabClientAPI.UpdateUserData(request, result =>
        {
            DebugLog.Message($"PlayFab: Save user data");
            success?.Invoke(result);
        }, error =>
        {
            DebugLog.Error($"PlayFab: Save player data failed, error: {error.ErrorMessage}");
            fail?.Invoke(error);
        });
    }

    public void LoadAllPlayerData(Action<Dictionary<string, string>> onLoaded,
        Action<GetUserDataResult> success = null,
        Action<PlayFabError> fail = null)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result != null) onLoaded?.Invoke(result.Data.ToDictionary(pair => pair.Key, pair => pair.Value.Value));
            success?.Invoke(result);
        }, error =>
        {
            DebugLog.Error($"PlayFab: Load player data failed, error: {error.ErrorMessage}");
            fail?.Invoke(error);
        });
    }

    public void LoadGameData(Action<Dictionary<string, string>> onLoaded,
        Action<GetTitleDataResult> success = null,
        Action<PlayFabError> fail = null)
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), result =>
        {
            if (result != null) onLoaded?.Invoke(result.Data);
            success?.Invoke(result);
        }, error =>
        {
            DebugLog.Error($"PlayFab: Load game data failed, error: {error.ErrorMessage}");
            fail?.Invoke(error);
        });
    }

    public void FetchCurrencies(
        Action<Dictionary<string, int>, Dictionary<string, VirtualCurrencyRechargeTime>> onLoaded,
        Action<GetUserInventoryResult> success = null,
        Action<PlayFabError> fail = null)
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            if (result != null) onLoaded?.Invoke(result.VirtualCurrency, result.VirtualCurrencyRechargeTimes);
            success?.Invoke(result);
        }, error =>
        {
            DebugLog.Error($"PlayFab: Fetch currencies failed, error: {error.ErrorMessage}");
            fail?.Invoke(error);
        });
    }
}

public static class PlayFabKey
{
    public const string TITLE_ID = "F8171";

    public const string CURRENCY_GOLD = "GD";
    public const string CURRENCY_COIN = "CN";
    public const string CURRENCY_ENERGY = "EN";

    public const string PLAYER_DATA_LEVEL = "level";
    public const string PLAYER_DATA_EXP = "exp";
    public const string PLAYER_DATA_AVATAR_ID = "avatar_id";
    public const string PLAYER_DATA_AVATAR_FRAME_ID = "avatar_frame_id";
    public const string PLAYER_DATA_UNLOCKED_ENTITIES = "unlockedEntities";
    public const string PLAYER_DATA_READY_ENTITIES = "readyEntities";
}

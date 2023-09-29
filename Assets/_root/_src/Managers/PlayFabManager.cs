using System;
using System.Collections.Generic;
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
                EditorLog.Message("PlayFab: Login failed, error: " + error.ErrorMessage);
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
            EditorLog.Message("PlayFab: Login failed, error: " + error.GenerateErrorReport());
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
            EditorLog.Message("PlayFab: Login failed, error: " + error.GenerateErrorReport());
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
            TitleId = Constants.TITLE_ID
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, result => { success?.Invoke(result); }, error =>
        {
            EditorLog.Message("PlayFab: Login failed, error: " + error.GenerateErrorReport());
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
            EditorLog.Message("PlayFab: Update username failed, error: " + error.GenerateErrorReport());
            fail?.Invoke(error);
        });
    }

    public void Save(string key, string data,
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
                EditorLog.Message($"PlayFab: Save user data, key={key} data={data}");
                success?.Invoke(result);
            },
            error =>
            {
                EditorLog.Error($"PlayFab: Save user data failed, key={key}, error: {error.ErrorMessage}");
                fail?.Invoke(error);
            });
    }

    [Button]
    public void Load(string key, Action<string> onLoaded,
        Action<GetUserDataResult> success = null,
        Action<PlayFabError> fail = null)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result != null && result.Data.TryGetValue(key, out var record))
            {
                EditorLog.Message($"PlayFab: Load user data, key={key} data={record.Value}");
                onLoaded?.Invoke(record.Value);
            }
            else
            {
                EditorLog.Message($"PlayFab: Load user data, key={key} not exist");
            }

            success?.Invoke(result);
        }, error =>
        {
            EditorLog.Error($"PlayFab: Load user data failed, key={key}, error: {error.ErrorMessage}");
            fail?.Invoke(error);
        });
    }
}

public static class PlayerKey
{
    public const string PLAYER_ID = "PlayerID";
    public const string LEVEL = "Level";
}

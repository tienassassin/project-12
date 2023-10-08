using System.Text.RegularExpressions;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : DuztineBehaviour
{
    [SerializeField] private TMP_Text txtVersion;
    [SerializeField] private GameObject[] views;

    [SerializeField] private TMP_InputField[] inpEmails;
    [SerializeField] private TMP_InputField[] inpPasswords;
    [SerializeField] private Toggle tglRemember;

    [SerializeField] private Button btnGo;


    private int _curView = -1;

    private void Start()
    {
        txtVersion.text = "ver " + Application.version;
        SwitchView(0);

        var rememberedEmail = PlayerPrefs.GetString(PPKeys.EMAIL);
        var rememberedPassword = PlayerPrefs.GetString(PPKeys.PASSWORD);
        if (rememberedEmail != null && rememberedPassword != null)
        {
            inpEmails[0].text = rememberedEmail;
            inpPasswords[0].text = rememberedPassword;
        }

        tglRemember.isOn = PlayerPrefs.GetInt(PPKeys.STAY_SIGNED_IN, 1) != 0;

        // auto login
        SignIn();
    }

    public void SwitchView(int index)
    {
        if (_curView == index) return;
        index = Mathf.Clamp(index, 0, views.Length - 1);
        _curView = index;

        for (var i = 0; i < views.Length; i++)
        {
            views[i].SetActive(false);
        }

        views[_curView].SetActive(true);

        switch (_curView)
        {
            case 0:
                tglRemember.gameObject.SetActive(true);
                break;
            case 1:
                tglRemember.gameObject.SetActive(true);
                inpEmails[1].text = inpEmails[0].text;
                break;
            case 2:
                tglRemember.gameObject.SetActive(false);
                inpEmails[2].text = inpEmails[0].text;
                break;
        }
    }

    private void SignIn()
    {
        if (!IsValidEmail())
        {
            GlobalUI.Instance.ShowNotification("Please enter a valid email");
            return;
        }

        if (!IsValidPassword())
        {
            GlobalUI.Instance.ShowNotification("Password must have a minimum of 6 characters");
            return;
        }

        btnGo.interactable = false;
        PlayFabManager.Instance.LoginWithCredential(inpEmails[0].text, inpPasswords[0].text, result =>
        {
            GlobalUI.Instance.ShowNotification("Login successfully.");
            btnGo.interactable = true;
            var remember = tglRemember.isOn;
            PlayerPrefs.SetString(PPKeys.EMAIL, remember ? inpEmails[0].text : "");
            PlayerPrefs.SetString(PPKeys.PASSWORD, remember ? inpPasswords[0].text : "");
            PlayerPrefs.SetInt(PPKeys.STAY_SIGNED_IN, remember ? 1 : 0);
            SceneLoader.Instance.LoadScene(SceneName.HOME, 3f, PrepareData, () =>
            {
                var profile = result.InfoResultPayload.PlayerProfile;
                if (profile != null && !profile.DisplayName.IsNullOrWhitespace())
                {
                }
                else
                {
                    UIController.Open<UsernameUI>();
                }
            });
        }, error =>
        {
            GlobalUI.Instance.ShowNotification("Login failed. Please check your information.");
            btnGo.interactable = true;
        });
    }

    private void SignUp()
    {
        if (!IsValidEmail())
        {
            GlobalUI.Instance.ShowNotification("Please enter a valid email");
            return;
        }

        if (!IsValidPassword())
        {
            GlobalUI.Instance.ShowNotification("Password must have a minimum of 6 characters");
            return;
        }

        btnGo.interactable = false;
        PlayFabManager.Instance.RegisterNewCredential(inpEmails[1].text, inpPasswords[1].text, result =>
        {
            GlobalUI.Instance.ShowNotification("Login successfully.");
            btnGo.interactable = true;
            var remember = tglRemember.isOn;
            PlayerPrefs.SetString(PPKeys.EMAIL, remember ? inpEmails[1].text : null);
            PlayerPrefs.SetString(PPKeys.PASSWORD, remember ? inpPasswords[1].text : null);
            PlayerPrefs.SetInt(PPKeys.STAY_SIGNED_IN, remember ? 1 : 0);
            SceneLoader.Instance.LoadScene(SceneName.HOME, 3f, PrepareData,
                () => { UIController.Open<UsernameUI>(); });
        }, error =>
        {
            GlobalUI.Instance.ShowNotification("Login failed. Please check your information.");
            btnGo.interactable = true;
        });
    }

    private void ResetPassword()
    {
        if (!IsValidEmail())
        {
            GlobalUI.Instance.ShowNotification("Please enter a valid email");
            return;
        }

        btnGo.interactable = false;
        PlayFabManager.Instance.RecoveryCredential(inpEmails[2].text, result =>
        {
            GlobalUI.Instance.ShowNotification("Please check your email to reset your password");
            btnGo.interactable = true;
            SwitchView(0);
        }, error => { btnGo.interactable = true; });
    }

    private bool IsValidEmail()
    {
        var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(inpEmails[_curView].text, pattern);
    }

    private bool IsValidPassword()
    {
        return inpPasswords[_curView].text.Length >= 6;
    }

    private void PrepareData()
    {
        GameManager.Instance.LoadGameDataFromCloud();
        PlayerManager.Instance.LoadPlayerDataFromCloud();
    }

    public void OnClickGo()
    {
        switch (_curView)
        {
            case 0:
                SignIn();
                break;
            case 1:
                SignUp();
                break;
            case 2:
                ResetPassword();
                break;
        }
    }
}
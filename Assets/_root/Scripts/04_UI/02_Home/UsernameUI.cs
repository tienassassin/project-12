using TMPro;
using UnityEngine;

public class UsernameUI : BaseUI
{
    [SerializeField] private TMP_InputField inpUsername;
    [SerializeField] private TMP_Text txtWaifu;
    [SerializeField] private GameObject bubble;

    private ChangeUsernameState _state = ChangeUsernameState.Unchanged;
    private string _username;

    private string[] _randomNames = { "KháPảnh", "Jack5tr", "ĐầuMoi", "MaGêming", "HL14tỏi" };

    public void Confirm()
    {
        if (_state == ChangeUsernameState.Processing) return;

        if (_state == ChangeUsernameState.Changed)
        {
            UIManager.Close<UsernameUI>();
            return;
        }

        _state = ChangeUsernameState.Processing;
        var valid = inpUsername.text.Length > 0;
        _username = valid ? inpUsername.text : GetRandomUsername();
        PlayFabManager.Instance.ChangeUsername(_username, result =>
        {
            _state = ChangeUsernameState.Changed;
            bubble.SetActive(false);
            if (valid)
            {
                txtWaifu.text = $"Rất vui được gặp bạn, <b>{_username}</b>. " +
                                $"Tôi sống ở gần đây, ngay dưới gốc cây có tán lá vàng đằng kia kìa. " +
                                $"Bạn có thể ghé qua bất kỳ lúc nào. " +
                                $"Giờ tôi phải đi rồi. Tạm biệt.";
            }
            else
            {
                txtWaifu.text = $"Hả? Bạn không nhớ tên của mình sao? " +
                                $"Hmm, vậy tôi sẽ gọi bạn là <b>{_username}</b> nhé! " +
                                $"Nếu rảnh bạn có thể tìm tôi ở căn nhà nhỏ dưới gốc cây có tán lá vàng đằng kia. " +
                                $"Giờ tôi phải đi rồi. Tạm biệt.";
            }

            UIManager.Get<HomeUI>().SetUsername(_username);
        });
    }

    private string GetRandomUsername()
    {
        return _randomNames[Random.Range(0, _randomNames.Length)] + $"#{Random.Range(0, 1000):000}";
    }
}

public enum ChangeUsernameState
{
    Unchanged,
    Processing,
    Changed
}
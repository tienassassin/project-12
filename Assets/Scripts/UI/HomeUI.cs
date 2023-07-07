public class HomeUI : BaseUI
{
    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(HomeUI));
    }

    public static void Hide()
    {
        UIManager.Instance.HideUI(nameof(HomeUI));
    }

    private void OpenValhalla()
    {
        ValhallaUI.Show();
    }

    private void OpenLineUp()
    {
        LineUpUI.Show();
    }

    private void OpenQuest()
    {
        QuestUI.Show();
    }

    private void OpenInventory()
    {
        InventoryUI.Show();
    }

    #region Buttons

    public void OnClickValhalla()
    {
        OpenValhalla();
    }

    public void OnClickLineUp()
    {
        OpenLineUp();
    }

    public void OnClickQuest()
    {
        OpenQuest();
    }

    public void OnClickInventory()
    {
        OpenInventory();
    }

    #endregion
}
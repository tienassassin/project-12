public class HomeUI : BaseUI
{
    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(HomeUI));
    }

    public static void Hide()
    {
        UIManager.Instance.ShowUI(nameof(HomeUI));
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

    private void OnClickValhallaBtn()
    {
        OpenValhalla();
    }

    private void OnClickLineUpBtn()
    {
        OpenLineUp();
    }

    private void OnClickQuestBtn()
    {
        OpenQuest();
    }

    private void OnClickInventory()
    {
        OpenInventory();
    }

    #endregion
}
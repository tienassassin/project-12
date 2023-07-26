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

    public void OpenValhalla()
    {
        ValhallaUI.Show();
    }

    public void OpenLineUp()
    {
        LineUpUI.Show();
    }

    public void OpenQuest()
    {
        QuestUI.Show();
    }

    public void OpenInventory()
    {
        InventoryUI.Show();
    }
}
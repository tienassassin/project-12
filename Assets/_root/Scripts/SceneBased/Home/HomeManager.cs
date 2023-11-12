public class HomeManager : Singleton<HomeManager>
{
    private void Start()
    {
        UIManager.Open<HomeUI>();
    }
}
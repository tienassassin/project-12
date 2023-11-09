using UnityEditor;

public partial class SceneLoader
{
#if UNITY_EDITOR
    [MenuItem("Scenes/0_Login")]
    public static void Load0_Login()
    {
        OpenScene("Assets/_root/Scenes/0_Login.unity");
    }

    [MenuItem("Scenes/1_Home")]
    public static void Load1_Home()
    {
        OpenScene("Assets/_root/Scenes/1_Home.unity");
    }

    [MenuItem("Scenes/2_Battle")]
    public static void Load2_Battle()
    {
        OpenScene("Assets/_root/Scenes/2_Battle.unity");
    }
#endif
}
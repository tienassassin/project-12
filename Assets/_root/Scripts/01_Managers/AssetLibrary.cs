using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetLibrary : Singleton<AssetLibrary>
{
    [SerializeField] private EntityLibrary entityLibrary;

    private const string AvatarPath = "Sprites/Avatars/";
    private const string AvatarFramePath = "Sprites/AvatarFrames/";
    private const string ItemPath = "Sprites/Items/";

    protected override void Awake()
    {
        base.Awake();
        SceneManager.activeSceneChanged += (scene0, scene1) => { Resources.UnloadUnusedAssets(); };
    }

    public EntityAsset GetEntity(string id)
    {
        return entityLibrary.entityAssets.Find(x => x.id.Equals(id));
    }

    public Sprite GetAvatar(string id)
    {
        return Resources.Load<Sprite>(AvatarPath + id);
    }

    public Sprite GetAvatarFrame(string id)
    {
        return Resources.Load<Sprite>(AvatarFramePath + id);
    }

    public Sprite GetItem(string id)
    {
        return Resources.Load<Sprite>(ItemPath + id);
    }
}
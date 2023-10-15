using UnityEngine;

public class AssetLibrary : Singleton<AssetLibrary>
{
    [SerializeField] private AvatarLibrary avatarLibrary;
    [SerializeField] private ItemLibrary itemLibrary;
    [SerializeField] private EntityLibrary entityLibrary;

    public EntityAsset GetEntityAsset(string id)
    {
        return entityLibrary.entityAssets.Find(x => x.id.Equals(id));
    }

    public ItemAsset GetItemAsset(string id)
    {
        return itemLibrary.itemAssets.Find(x => x.id.Equals(id));
    }

    public AvatarAsset GetAvatarAsset(string id)
    {
        return avatarLibrary.avatarAssets.Find(x => x.id.Equals(id));
    }

    public AvatarFrameAsset GetAvatarFrameAsset(string id)
    {
        return avatarLibrary.avatarFrameAssets.Find(x => x.id.Equals(id));
    }
}
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetLibrary : Singleton<AssetLibrary>
{
    [SerializeField] private EntityLibrary entityLibrary;

    private const string AvatarPath = "Sprites/Avatars/";
    private const string AvatarFramePath = "Sprites/AvatarFrames/";
    private const string ItemPath = "Sprites/Items/";

    private const string EntityRolePath = "Sprites/Entity/Roles/";
    private const string EntityRealmPath = "Sprites/Entity/Realms/";

    protected override void Awake()
    {
        base.Awake();
        SceneManager.activeSceneChanged += (scene0, scene1) => { Resources.UnloadUnusedAssets(); };
    }

    #region Player
    
    public Sprite GetAvatar(string id)
    {
        return Resources.Load<Sprite>(AvatarPath + id);
    }

    public Sprite GetAvatarFrame(string id)
    {
        return Resources.Load<Sprite>(AvatarFramePath + id);
    }

    #endregion

    #region Items

    public Sprite GetItem(string id)
    {
        return Resources.Load<Sprite>(ItemPath + id);
    }

    #endregion

    #region Entities

    public EntityAsset GetEntity(string id)
    {
        return entityLibrary.entityAssets.Find(x => x.id.Equals(id));
    }

    public Sprite GetRole(Role role, string suffix = null)
    {
        return Resources.Load<Sprite>(EntityRolePath + role + suffix);
    }

    public Sprite GetRealm(Realm realm)
    {
        return Resources.Load<Sprite>(EntityRealmPath + realm);
    }

    #endregion

    //debug

    [Button]
    public void DebugGetAvatar(string id)
    {
        DebugLog.Message(GetAvatar(id).name);
    }
}
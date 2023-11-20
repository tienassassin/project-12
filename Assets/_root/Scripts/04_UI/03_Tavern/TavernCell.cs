using UnityEngine;
using UnityEngine.UI;

public class TavernCell : AssassinBehaviour
{
    [SerializeField] private Image imgEntityAvatar;
    [SerializeField] private Image imgRole;

    private EntityRecord _entityRecord;

    public void Init(EntityRecord entityRecord)
    {
        _entityRecord = entityRecord;
        imgEntityAvatar.sprite = AssetLibrary.Instance.GetEntity(_entityRecord.id).avatar;
        string roleSuffix = _entityRecord.IsNot(Role.Slayer) ? null :
            _entityRecord.Is(DamageType.Magical) ? "_MDmg" : "_PDmg";
        imgRole.sprite = AssetLibrary.Instance.GetRole(_entityRecord.type.role, roleSuffix);
    }
}
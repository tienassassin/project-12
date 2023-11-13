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
        string roleSuffix = _entityRecord.role != Role.Slayer ? null :
            _entityRecord.damageType == DamageType.Magical ? "_MDmg" : "_PDmg";
        imgRole.sprite = AssetLibrary.Instance.GetRole(_entityRecord.role, roleSuffix);
    }
}
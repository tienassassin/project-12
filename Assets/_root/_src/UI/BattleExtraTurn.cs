using TMPro;
using UnityEngine;

public class BattleExtraTurn : DuztineBehaviour
{
    [SerializeField] private TMP_Text txtName;

    public void Init(string entityName)
    {
        txtName.text = entityName;
        name = entityName;
    }
}
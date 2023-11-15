using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TavernUI : BaseUI
{
    // todo:
    // [TitleGroup("Filter & Sort:")]
    // [SerializeField] private Toggle togShowLocked;
    // [SerializeField] private TMP_Dropdown drpRealm;
    // [SerializeField] private TMP_Dropdown drpDamageType;
    // [SerializeField] private TMP_Dropdown drpAttackRange;
    // [SerializeField] private TMP_Dropdown drpSort;
    // [SerializeField] private Button btnApply;
    [TitleGroup("Entity:")]
    [SerializeField] private TavernCell cellPrefab;

    [TitleGroup("Categories:")]
    [SerializeField] private TMP_Text txtMortalNumber;
    [SerializeField] private TMP_Text txtDivineNumber;
    [SerializeField] private TMP_Text txtInfernalNumber;
    [SerializeField] private TMP_Text txtChaosNumber;
    [SerializeField] private Transform containerMortal;
    [SerializeField] private Transform containerDivine;
    [SerializeField] private Transform containerInfernal;
    [SerializeField] private Transform containerChaos;

    [TitleGroup("Others:")]
    [SerializeField] private Button btnClose;

    private List<TavernCell> _mortalCells = new();
    private List<TavernCell> _divineCells = new();
    private List<TavernCell> _infernalCells = new();
    private List<TavernCell> _chaosCells = new();
    
    
    

    

   


    
    private List<ValhallaHeroCard> _activeCards = new();
    private ValhallaHeroCard _selectedCard;

    private SortType _lvSort;
    private SortType _tierSort;

    protected override void Awake()
    {
        base.Awake();
        CleanUp();
    }

    private void OnEnable()
    {
        UpdateEntityCells();
    }

    protected override void AssignUICallback()
    {
        // btnApply.onClick.AddListener(ApplyFilterAndSort);
        btnClose.onClick.AddListener(Close);
    }

    private void CleanUp()
    {
        foreach (Transform child in containerMortal) { Destroy(child.gameObject); }

        foreach (Transform child in containerDivine) { Destroy(child.gameObject); }

        foreach (Transform child in containerInfernal) { Destroy(child.gameObject); }

        foreach (Transform child in containerChaos) { Destroy(child.gameObject); }
    }

    private void ApplyFilterAndSort()
    {
        // var showLocked = togShowLocked.isOn;
        // var realmOption = drpRealm.value;
        // var dmgTypeOption = drpDamageType.value;
        // var atkRangeOption = drpAttackRange.value;
        // var sortOption = drpSort.value;
    }

    public void UpdateEntityCell(string id)
    {
    }

    public void UpdateEntityCells()
    {
        var allEntities = GameManager.Instance.GetEntities();
        var mortalEntities = allEntities.Where(x => x.canUnlock && x.realm == Realm.Mortal).ToList();
        var divineEntities = allEntities.Where(x => x.canUnlock && x.realm == Realm.Divine).ToList();
        var infernalEntities = allEntities.Where(x => x.canUnlock && x.realm == Realm.Infernal).ToList();
        var chaosEntities = allEntities.Where(x => x.canUnlock && x.realm == Realm.Chaos).ToList();

        UpdateEntityCells(mortalEntities, _mortalCells, containerMortal, txtMortalNumber);
        UpdateEntityCells(divineEntities, _divineCells, containerDivine, txtDivineNumber);
        UpdateEntityCells(infernalEntities, _infernalCells, containerInfernal, txtInfernalNumber);
        UpdateEntityCells(chaosEntities, _chaosCells, containerChaos, txtChaosNumber);
    }

    private void UpdateEntityCells(List<EntityRecord> records, List<TavernCell> cells, Transform container,
        TMP_Text number)
    {
        if (records == null || records.Count == 0)
        {
            container.gameObject.SetActive(false);
            number.transform.parent.gameObject.SetActive(false);
            return;
        }

        container.gameObject.SetActive(true);
        number.transform.parent.gameObject.SetActive(true);
        number.text = $"[{records.Count}/{records.Count}]";
        while (cells.Count < records.Count)
        {
            var clone = Instantiate(cellPrefab, container);
            cells.Add(clone);
        }

        for (int i = 0; i < cells.Count; i++)
        {
            if (i >= records.Count)
            {
                cells[i].gameObject.SetActive(false);
                continue;
            }

            cells[i].Init(records[i]);
        }
    }

    public void SelectNextCard()
    {
        if (!_selectedCard || _activeCards.Count < 2) return;

        var nextIndex = _activeCards.IndexOf(_selectedCard) + 1;
        if (nextIndex >= _activeCards.Count) nextIndex = 0;
        _activeCards[nextIndex].SelectCard();
    }

    public void SelectPreviousCard()
    {
        if (!_selectedCard || _activeCards.Count < 2) return;

        var nextIndex = _activeCards.IndexOf(_selectedCard) - 1;
        if (nextIndex < 0) nextIndex = _activeCards.Count - 1;
        _activeCards[nextIndex].SelectCard();
    }

    private void Close()
    {
        UIManager.Close<TavernUI>();
    }
}
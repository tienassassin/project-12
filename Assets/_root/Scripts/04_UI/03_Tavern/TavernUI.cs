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
    [SerializeField] private TavernCell _cellPrefab;

    [TitleGroup("Categories:")]
    [SerializeField] private TMP_Text _txtMortalNumber;
    [SerializeField] private TMP_Text _txtDivineNumber;
    [SerializeField] private TMP_Text _txtInfernalNumber;
    [SerializeField] private TMP_Text _txtChaosNumber;
    [SerializeField] private Transform _containerMortal;
    [SerializeField] private Transform _containerDivine;
    [SerializeField] private Transform _containerInfernal;
    [SerializeField] private Transform _containerChaos;

    [TitleGroup("Others:")]
    [SerializeField] private Button _btnClose;

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
        _btnClose.onClick.AddListener(Close);
    }

    private void CleanUp()
    {
        foreach (Transform child in _containerMortal) { Destroy(child.gameObject); }

        foreach (Transform child in _containerDivine) { Destroy(child.gameObject); }

        foreach (Transform child in _containerInfernal) { Destroy(child.gameObject); }

        foreach (Transform child in _containerChaos) { Destroy(child.gameObject); }
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
        var mortalEntities = allEntities.Where(x => x.canUnlock && x.Is(Realm.Mortal)).ToList();
        var divineEntities = allEntities.Where(x => x.canUnlock && x.Is(Realm.Divine)).ToList();
        var infernalEntities = allEntities.Where(x => x.canUnlock && x.Is(Realm.Infernal)).ToList();
        var chaosEntities = allEntities.Where(x => x.canUnlock && x.Is(Realm.Chaos)).ToList();

        UpdateEntityCells(mortalEntities, _mortalCells, _containerMortal, _txtMortalNumber);
        UpdateEntityCells(divineEntities, _divineCells, _containerDivine, _txtDivineNumber);
        UpdateEntityCells(infernalEntities, _infernalCells, _containerInfernal, _txtInfernalNumber);
        UpdateEntityCells(chaosEntities, _chaosCells, _containerChaos, _txtChaosNumber);
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
            var clone = Instantiate(_cellPrefab, container);
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
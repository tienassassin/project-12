using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineUpDetail : AssassinBehaviour
{
    [SerializeField] private GameObject info;
    [SerializeField] private GameObject equipmentGroup;

    [SerializeField] private TMP_Text txtName;
    [SerializeField] private Image imgRace;
    [SerializeField] private Image imgElement;

    [SerializeField] private Transform heroCardContainer;
    [SerializeField] private LineUpHeroCard heroCardPref;
    [SerializeField] private Transform eqmCardContainer;
    [SerializeField] private EquipmentCard eqmCardPref;

    [SerializeField] private FilterOption[] raceFilterOptions;

    private readonly List<Realm> _raceOpts = new();
    private EntityData _entityData;

    private int _curSlotId;
    private List<EquipmentCard> _equipmentCards = new();

    private List<LineUpHeroCard> _heroCards = new();
    private List<MyEntity> _heroSaveDataList = new();
    private MyEntity _saveData;

    private void Awake()
    {
        foreach (var opt in raceFilterOptions)
        {
            opt.SetEvent(AddOptionToFilter);
        }

        _heroCards = new List<LineUpHeroCard>();
        foreach (Transform child in heroCardContainer)
        {
            _heroCards.Add(child.gameObject.GetComponent<LineUpHeroCard>());
        }
    }

    private void OnEnable()
    {
        _raceOpts.Clear();

        _heroSaveDataList = PlayerManager.Instance.GetAllHeroes();

        LoadHeroCards();
    }

    public void Init(int slotId, MyEntity data)
    {
        _curSlotId = slotId;
        _saveData = data;
        _entityData = _saveData?.GetEntity();
        info.SetActive(_saveData != null);
        equipmentGroup.SetActive(_saveData != null);

        Refresh();
    }

    private void Refresh()
    {
        if (_saveData == null) return;

        txtName.text = _entityData.name;
    }

    private void AddHeroToLineUp(string heroId)
    {
        PlayerManager.Instance.AddHeroToLineUp(_curSlotId, heroId);
    }

    private void RemoveHeroFromLineUp()
    {
        PlayerManager.Instance.RemoveHeroFromLineUp(_curSlotId);
    }

    private void AddOptionToFilter(object o)
    {
        switch (o)
        {
            case Realm r when _raceOpts.Contains(r):
                _raceOpts.Remove(r);
                break;
            case Realm r:
                _raceOpts.Add(r);
                break;
            default:
                DebugLog.Error($"Object {o} is not a valid filter option");
                return;
        }

        ApplyHeroCardFilter();
    }

    #region Hero Cards

    private void LoadHeroCards()
    {
        while (heroCardContainer.childCount < _heroSaveDataList.Count)
        {
            var o = Instantiate(heroCardPref, heroCardContainer);
            _heroCards.Add(o);
        }

        for (int i = 0; i < _heroCards.Count; i++)
        {
            var card = _heroCards[i];
            if (i >= _heroSaveDataList.Count)
            {
                card.gameObject.SetActive(false);
                card.name = Constants.EMPTY_MARK;
                continue;
            }

            card.gameObject.SetActive(true);
            card.Init(_heroSaveDataList[i], data =>
            {
                if (data != _saveData)
                {
                    AddHeroToLineUp(data.entityId);
                    Init(_curSlotId, data);
                }
                else
                {
                    RemoveHeroFromLineUp();
                    Init(_curSlotId, null);
                }

                UpdateHeroCards();
            });
        }

        UpdateHeroCards();
        SortHeroCards();
    }

    private void UpdateHeroCards()
    {
        _heroCards.ForEach(x => { x.UpdateReadyState(); });
    }

    private void ApplyHeroCardFilter()
    {
        bool acpAllRace = _raceOpts.Count < 1;

        _heroCards.ForEach(c =>
        {
            if (c.name == Constants.EMPTY_MARK) return;

            // var match = _raceOpts.Contains(c.Realm) || acpAllRace;
            // c.gameObject.SetActive(match);
        });
    }

    private void SortHeroCards()
    {
        _heroCards.Sort((c1, c2) => CompareLevel(c1, c2, false));

        _heroCards.ForEach(c => { c.transform.SetAsLastSibling(); });

        int CompareLevel(LineUpHeroCard c1, LineUpHeroCard c2, bool ascending)
        {
            if (c1.name == Constants.EMPTY_MARK) return 1;
            if (c2.name == Constants.EMPTY_MARK) return -1;

            // int levelComparision = c1.Level.CompareTo(c2.Level);
            // if (levelComparision != 0) return ascending ? levelComparision : -levelComparision;
            // int tierComparision = c1.Tier.CompareTo(c2.Tier);
            // return ascending ? tierComparision : -tierComparision;

            return 0;
        }
    }

    #endregion


    #region Equipment Cards

    // todo    

    #endregion
}
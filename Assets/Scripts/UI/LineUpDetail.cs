using System.Collections.Generic;
using System.DB;
using Player.DB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineUpDetail : DuztineBehaviour
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

    private int _curSlotId;
    private Player.DB.Hero _saveData;
    private System.DB.Hero _baseData;
    private List<Player.DB.Hero> _heroSaveDataList = new();
    
    private List<LineUpHeroCard> _heroCards = new();
    private List<EquipmentCard> _equipmentCards = new();

    private readonly List<Race> _raceOpts = new();

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

    public void Init(int slotId, Player.DB.Hero data)
    {
        _curSlotId = slotId;
        _saveData = data;
        _baseData = _saveData?.GetHeroWithID();
        info.SetActive(_saveData != null);
        equipmentGroup.SetActive(_saveData != null);
        
        Refresh();
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
            card.Init(_heroSaveDataList[i], (data)=>
                {
                    if (data != _saveData)
                    {
                        AddHeroToLineUp(data.heroId);
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
        _heroCards.ForEach(x =>
        {
            x.UpdateReadyState();
        });
    }

    private void ApplyHeroCardFilter()
    {
        bool acpAllRace = _raceOpts.Count < 1;
        
        _heroCards.ForEach(c =>
        {
            if (c.name == Constants.EMPTY_MARK) return;

            bool match = (_raceOpts.Contains(c.Race) || acpAllRace);
            c.gameObject.SetActive(match);
        });
    }
    
    private void SortHeroCards()
    {
        _heroCards.Sort((c1, c2) => CompareLevel(c1, c2, false));
        
        _heroCards.ForEach(c =>
        {
            c.transform.SetAsLastSibling();
        });
        
        int CompareLevel(LineUpHeroCard c1, LineUpHeroCard c2, bool ascending)
        {
            if (c1.name == Constants.EMPTY_MARK) return 1;
            if (c2.name == Constants.EMPTY_MARK) return -1;

            int levelComparision = c1.Level.CompareTo(c2.Level);
            if (levelComparision != 0) return ascending ? levelComparision : -levelComparision;
            int tierComparision = c1.Tier.CompareTo(c2.Tier);
            return ascending ? tierComparision : -tierComparision;
        }
    }
    
    #endregion

    
    #region Equipment Cards

    // todo    

    #endregion

    private void Refresh()
    {
        if (_saveData == null) return;

        txtName.text = _baseData.Name;
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
            case Race r when _raceOpts.Contains(r):
                _raceOpts.Remove(r);
                break;
            case Race r:
                _raceOpts.Add(r);
                break;
            default:
                EditorLog.Error($"Object {o} is not a valid filter option");
                return;
        }
        
       ApplyHeroCardFilter();
    }
}
using System.Collections.Generic;
using UnityEngine;

public class ValhallaUI : BaseUI
{
    [SerializeField] private FilterOption[] tierFilterOptions;
    [SerializeField] private FilterOption[] elementFilterOptions;
    [SerializeField] private FilterOption[] raceFilterOptions;

    [SerializeField] private ValhallaHeroCard heroCardPref;
    [SerializeField] private Transform heroCardContainer;

    [SerializeField] private ValhallaHeroDetail heroDetail;

    private List<EntityData> _entities = new();

    private List<ValhallaHeroCard> _cards = new();
    private List<ValhallaHeroCard> _activeCards = new();
    private ValhallaHeroCard _selectedCard;

    private List<Role> _elementOpts = new();
    private List<Realm> _raceOpts = new();
    private List<Tier> _tierOpts = new();

    private SortType _lvSort;
    private SortType _tierSort;

    protected override void Awake()
    {
        base.Awake();

        foreach (var opt in tierFilterOptions)
        {
            opt.SetEvent(AddOptionToFilter);
        }

        foreach (var opt in elementFilterOptions)
        {
            opt.SetEvent(AddOptionToFilter);
        }

        foreach (var opt in raceFilterOptions)
        {
            opt.SetEvent(AddOptionToFilter);
        }

        _cards = new List<ValhallaHeroCard>();
        foreach (Transform child in heroCardContainer)
        {
            _cards.Add(child.gameObject.GetComponent<ValhallaHeroCard>());
        }

        heroDetail.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _tierOpts.Clear();
        _elementOpts.Clear();
        _raceOpts.Clear();

        _lvSort = SortType.Descending;
        _tierSort = SortType.None;

        _entities = GameDatabase.Instance.GetAllEntities();

        LoadHeroCards();
        Refresh();
    }

    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(ValhallaUI));
    }

    public static void Hide()
    {
        UIManager.Instance.HideUI(nameof(ValhallaUI));
    }

    private void LoadHeroCards()
    {
        while (heroCardContainer.childCount < _entities.Count)
        {
            var o = Instantiate(heroCardPref, heroCardContainer);
            _cards.Add(o);
        }

        for (int i = 0; i < _cards.Count; i++)
        {
            var card = _cards[i];
            if (i >= _entities.Count)
            {
                card.gameObject.SetActive(false);
                card.name = Constants.EMPTY_MARK;
                continue;
            }

            card.gameObject.SetActive(true);
            if (PlayerManager.Instance.IsHeroUnlocked(_entities[i].info.id, out var hsd))
            {
                // unlocked hero
                card.Init(hsd, saveData =>
                {
                    ShowCardDetail(saveData);
                    _selectedCard = card;
                });
            }
            else
            {
                // locked hero
                card.Init(_entities[i]);
            }
        }
    }

    private void Refresh()
    {
        bool acpAllTier = _tierOpts.Count < 1;
        bool acpAllElement = _elementOpts.Count < 1;
        bool acpAllRace = _raceOpts.Count < 1;

        if (_lvSort != SortType.None)
        {
            _cards.Sort((c1, c2) =>
                CompareLevel(c1, c2, _lvSort != SortType.Descending)
            );
        }
        else if (_tierSort != SortType.None)
        {
            _cards.Sort((c1, c2) =>
                CompareTier(c1, c2, _tierSort != SortType.Descending)
            );
        }

        _activeCards.Clear();

        _cards.ForEach(c =>
        {
            c.transform.SetAsLastSibling();
            if (c.name == Constants.EMPTY_MARK) return;

            bool match = (_tierOpts.Contains(c.Tier) || acpAllTier)
                         && (_elementOpts.Contains(c.Role) || acpAllElement)
                         && (_raceOpts.Contains(c.Realm) || acpAllRace);

            c.gameObject.SetActive(match);

            if (match) _activeCards.Add(c);
        });

        int CompareLevel(ValhallaHeroCard c1, ValhallaHeroCard c2, bool ascending)
        {
            if (c1.name == Constants.EMPTY_MARK) return 1;
            if (c2.name == Constants.EMPTY_MARK) return -1;

            int lockComparision = c1.IsLocked.CompareTo(c2.IsLocked);
            if (lockComparision != 0) return lockComparision;
            int levelComparision = c1.Level.CompareTo(c2.Level);
            if (levelComparision != 0) return ascending ? levelComparision : -levelComparision;
            int tierComparision = c1.Tier.CompareTo(c2.Tier);
            return ascending ? tierComparision : -tierComparision;
        }

        int CompareTier(ValhallaHeroCard c1, ValhallaHeroCard c2, bool ascending)
        {
            if (c1.name == Constants.EMPTY_MARK) return 1;
            if (c2.name == Constants.EMPTY_MARK) return -1;

            int lockComparision = c1.IsLocked.CompareTo(c2.IsLocked);
            if (lockComparision != 0) return lockComparision;
            int tierComparision = c1.Tier.CompareTo(c2.Tier);
            if (tierComparision != 0) return ascending ? tierComparision : -tierComparision;
            int levelComparision = c1.Level.CompareTo(c2.Level);
            return ascending ? levelComparision : -levelComparision;
        }
    }

    private void ShowCardDetail(MyEntity saveData)
    {
        heroDetail.gameObject.SetActive(true);
        heroDetail.Init(saveData);
    }

    public void HideCardDetail()
    {
        heroDetail.gameObject.SetActive(false);
        _selectedCard = null;
    }

    public void SelectNextCard()
    {
        if (!_selectedCard || _activeCards.Count < 2) return;

        int nextIndex = _activeCards.IndexOf(_selectedCard) + 1;
        if (nextIndex >= _activeCards.Count) nextIndex = 0;
        _activeCards[nextIndex].SelectCard();
    }

    public void SelectPreviousCard()
    {
        if (!_selectedCard || _activeCards.Count < 2) return;

        int nextIndex = _activeCards.IndexOf(_selectedCard) - 1;
        if (nextIndex < 0) nextIndex = _activeCards.Count - 1;
        _activeCards[nextIndex].SelectCard();
    }

    private void AddOptionToFilter(object o)
    {
        switch (o)
        {
            case Tier t when _tierOpts.Contains(t):
                _tierOpts.Remove(t);
                break;
            case Tier t:
                _tierOpts.Add(t);
                break;
            case Role e when _elementOpts.Contains(e):
                _elementOpts.Remove(e);
                break;
            case Role e:
                _elementOpts.Add(e);
                break;
            case Realm r when _raceOpts.Contains(r):
                _raceOpts.Remove(r);
                break;
            case Realm r:
                _raceOpts.Add(r);
                break;
            default:
                EditorLog.Error($"Object {o} is not a valid filter option");
                return;
        }

        Refresh();
    }

    public void SortByLevel(bool asc)
    {
        _lvSort = (asc ? SortType.Ascending : SortType.Descending);
        _tierSort = SortType.None;
        Refresh();
    }

    public void SortByTier(bool asc)
    {
        _lvSort = SortType.None;
        _tierSort = (asc ? SortType.Ascending : SortType.Descending);
        Refresh();
    }

    #region Buttons

    public void OnClickBack()
    {
        Hide();
    }

    #endregion
}
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineUpAura : DuztineBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int minRank;
    [SerializeField] private int maxRank;
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private float delayShowPanel = 0.3f;
    [SerializeField] private TMP_Text[] txtRanks;
    [SerializeField] private Color color0;
    [SerializeField] private Color[] color1;
    [SerializeField] private float fontSize0;
    [SerializeField] private float fontSize1;
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtContent;

    private object _auraType;

    private string _hexColor0;
    private string _hexColor1;

    private Coroutine _coroutine;

    private void Awake()
    {
        detailPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _coroutine = StartCoroutine(ShowDetailPanel());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        detailPanel.SetActive(false);
        this.PostEvent(EventID.ON_HIGHLIGHT_AURA);
    }

    [Button]
    public void Init(Race race, int rank)
    {
        _auraType = race;
        txtTitle.text = $"<color={ColorPalette.Instance.GetRaceColorHex(race)}>" +
                        "<ico>" +
                        $"{race} Aura</color>";
        var auraList = DataManager.Instance.GetAuras(race);
        RefreshRank(rank);
        RefreshContent(auraList, rank);
    }

    public void Init(Element element, int rank)
    {
        _auraType = element;
        txtTitle.text = $"<color={ColorPalette.Instance.GetElementColorHex(element)}>" +
                        $"<size=50><sprite name=element-{element}></size> " +
                        $"{element} Aura</color>";
        var auraList = DataManager.Instance.GetAuras(element);
        var colorList = new List<string> { ColorPalette.Instance.GetElementColorHex(element) };
        RefreshRank(rank);
        RefreshContent(auraList, rank, colorList);
    }

    private void RefreshRank(int rank)
    {
        rank = Mathf.Clamp(rank, minRank, maxRank);
        int rankIndex = rank - minRank;

        for (int i = 0; i < txtRanks.Length; i++)
        {
            txtRanks[i].fontSize = (i != rankIndex) ? fontSize0 : fontSize1;
            txtRanks[i].color = (i != rankIndex) ? color0 : color1[maxRank - rank];
        }
    }

    private void RefreshContent(List<Aura> auraList, int rank, List<string> colorList = null)
    {
        _hexColor0 = "#" + ColorUtility.ToHtmlStringRGBA(color0);
        _hexColor1 = "#" + ColorUtility.ToHtmlStringRGBA(color1[1]);

        string content = "";
        for (int i = 0; i < auraList.Count; i++)
        {
            var aura = auraList[i];
            bool isCurRank = (rank == aura.rank);
            bool emptyName = aura.name.IsNullOrWhitespace();
            bool isLastAura = (i >= auraList.Count - 1);
            content += $"<color={(isCurRank ? _hexColor1 : _hexColor0)}>" +
                       $"{aura.name}{(emptyName ? "" : " ")}" +
                       $"({aura.rank}): {aura.description}" +
                       "</color>" +
                       $"{(isLastAura ? "" : "\n\n")}";
        }

        int index = 0;
        colorList?.ForEach(x =>
        {
            content = content.Replace($"#color{index}", x);
            index++;
        });

        txtContent.text = content;
    }

    private IEnumerator ShowDetailPanel()
    {
        yield return new WaitForSeconds(delayShowPanel);
        detailPanel.SetActive(true);
        this.PostEvent(EventID.ON_HIGHLIGHT_AURA, _auraType);
    }
}
using System.Collections;
using System.Collections.Generic;
using System.DB;
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
    [SerializeField] private TMP_Text[] rankTexts;
    [SerializeField] private Color color0;
    [SerializeField] private Color[] color1;
    [SerializeField] private float fontSize0;
    [SerializeField] private float fontSize1;
    [SerializeField] private TMP_Text titleTxt;
    [SerializeField] private TMP_Text contentTxt;
    
    private Coroutine _coroutine;
    private string _hexColor0;
    private string _hexColor1;
    private object _auraType;

    private void Awake()
    {
        detailPanel.SetActive(false);
    }

    [Button]
    public void Init(Race race, int rank)
    {
        _auraType = race;
        titleTxt.text = $"<color={ColorPalette.Instance.GetRaceColorHex(race)}>" +
                        $"<ico>" +
                        $"{race} Aura</color>";
        var auraList = Database.Instance.GetAuras(race);
        RefreshRank(rank);
        RefreshContent(auraList, rank);
    }

    public void Init(Element element, int rank)
    {
        _auraType = element;
        titleTxt.text = $"<color={ColorPalette.Instance.GetElementColorHex(element)}>" +
                        $"<size=50><sprite name=element-{element}></size> " +
                        $"{element} Aura</color>";
        var auraList = Database.Instance.GetAuras(element);
        var colorList = new List<string> { ColorPalette.Instance.GetElementColorHex(element) };
        RefreshRank(rank);
        RefreshContent(auraList, rank, colorList);
    }

    private void RefreshRank(int rank)
    {
        rank = Mathf.Clamp(rank, minRank, maxRank);
        int rankIndex = rank - minRank;
        
        for (int i = 0; i < rankTexts.Length; i++)
        {
            rankTexts[i].fontSize = (i != rankIndex) ? fontSize0 : fontSize1;
            rankTexts[i].color = (i != rankIndex) ? color0 : color1[maxRank - rank];
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
            bool isCurRank = (rank == aura.Rank);
            bool emptyName = aura.Name.IsNullOrWhitespace();
            bool isLastAura = (i >= auraList.Count - 1);
            content += $"<color={(isCurRank ? _hexColor1 : _hexColor0)}>" +
                        $"{aura.Name}{(emptyName ? "" : " ")}" +
                        $"({aura.Rank}): {aura.Description}" +
                        "</color>" +
                        $"{(isLastAura ? "" : "\n\n")}";
        }

        int index = 0;
        colorList?.ForEach(x =>
        {
            content = content.Replace($"#color{index}", x);
            index++;
        });

        contentTxt.text = content;
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

    IEnumerator ShowDetailPanel()
    {
        yield return new WaitForSeconds(delayShowPanel);
        detailPanel.SetActive(true);
        this.PostEvent(EventID.ON_HIGHLIGHT_AURA, _auraType);
    }
}
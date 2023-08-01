using System;
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
    [SerializeField] private TMP_Text[] rankTexts;
    [SerializeField] private Color color0;
    [SerializeField] private Color[] color1;
    [SerializeField] private float fontSize0;
    [SerializeField] private float fontSize1;
    [SerializeField] private TMP_Text titleTxt;
    [SerializeField] private TMP_Text contentTxt;
    private Coroutine c;
    private string color0Hex;
    private string color1Hex;
    private object auraType;

    private void Awake()
    {
        detailPanel.SetActive(false);
    }

    [Button]
    public void Init(Race race, int rank)
    {
        auraType = race;
        titleTxt.text = $"<color={ColorPalette.Instance.GetRaceColorHex(race)}>" +
                        $"<ico>" +
                        $"{race} Aura</color>";
        var auraList = Database.Instance.GetRaceAura(race);
        RefreshRank(rank);
        RefreshContent(auraList, rank);
    }

    public void Init(Element element, int rank)
    {
        auraType = element;
        titleTxt.text = $"<color={ColorPalette.Instance.GetElementColorHex(element)}>" +
                        $"<size=50><sprite name=element-{element}></size> " +
                        $"{element} Aura</color>";
        var auraList = Database.Instance.GetElementAura(element);
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
        color0Hex = "#" + ColorUtility.ToHtmlStringRGBA(color0);
        color1Hex = "#" + ColorUtility.ToHtmlStringRGBA(color1[1]);
        
        string content = "";
        for (int i = 0; i < auraList.Count; i++)
        {
            var aura = auraList[i];
            bool isCurRank = (rank == aura.rank);
            bool emptyName = aura.name.IsNullOrWhitespace();
            bool isLastAura = (i >= auraList.Count - 1);
            content += $"<color={(isCurRank ? color1Hex : color0Hex)}>" +
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

        contentTxt.text = content;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        c = StartCoroutine(ShowDetailPanel());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (c != null) StopCoroutine(c);
        detailPanel.SetActive(false);
        this.PostEvent(EventID.ON_HIGHLIGHT_AURA, null);
    }

    IEnumerator ShowDetailPanel()
    {
        yield return new WaitForSeconds(delayShowPanel);
        detailPanel.SetActive(true);
        this.PostEvent(EventID.ON_HIGHLIGHT_AURA, auraType);
    }
}
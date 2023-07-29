using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineUpAura : DuztineBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int maxRank;
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private float delayShowPanel = 0.3f;
    [SerializeField] private TMP_Text[] rankTexts;
    [SerializeField] private Color color0;
    [SerializeField] private Color color1;
    [SerializeField] private float fontSize0;
    [SerializeField] private float fontSize1;
    [SerializeField] private TMP_Text titleTxt;
    [SerializeField] private TMP_Text contentTxt;
    private Coroutine c;
    private string color0Hex;
    private string color1Hex;

    private void Awake()
    {
        detailPanel.SetActive(false);
        color0Hex = "#" + ColorUtility.ToHtmlStringRGBA(color0);
        color1Hex = "#" + ColorUtility.ToHtmlStringRGBA(color1);
    }

    [Button]
    public void Init(Race race, int rank, string[] auraDesc)
    {
        rank = Mathf.Clamp(rank, 0, maxRank - 1);
        
        for (int i = 0; i < rankTexts.Length; i++)
        {
            rankTexts[i].fontSize = (i != rank) ? fontSize0 : fontSize1;
            rankTexts[i].color = (i != rank) ? color0 : color1;
        }
    }

    public void Init(Element element, int rank, string[] auraDesc)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        c = StartCoroutine(ShowDetailPanel());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (c != null) StopCoroutine(c);
        detailPanel.SetActive(false);
    }

    IEnumerator ShowDetailPanel()
    {
        yield return new WaitForSeconds(delayShowPanel);
        detailPanel.SetActive(true);
    }
}
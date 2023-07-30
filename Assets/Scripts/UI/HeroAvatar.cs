using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroAvatar : DuztineBehaviour
{
   [SerializeField] private Image avatarImg;
   [SerializeField] private Image hpImg;
   [SerializeField] private Image energyImg;

   private HeroSaveData saveData;
   private BaseHero baseHero;
   
   public void Init(HeroSaveData data)
   {
      saveData = data;
      baseHero = saveData?.GetHeroWithID();
      name = (baseHero != null ? baseHero.name : Constants.EMPTY_MARK);

      Refresh();
   }

   private void Refresh()
   {
      if (name == Constants.EMPTY_MARK)
      {
         hpImg.fillAmount = 0;
         energyImg.fillAmount = 0;
         return;
      }

      hpImg.fillAmount = (saveData.curHp / baseHero.stats.health) / 2;
      energyImg.fillAmount = (saveData.energy / 100) / 2;
   }

   public void OnClickAvatar()
   {
      if (name == Constants.EMPTY_MARK) return;
      
      LineUpUI.Show();
   }
}

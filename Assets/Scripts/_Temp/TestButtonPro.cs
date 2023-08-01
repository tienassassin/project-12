using Sirenix.OdinInspector;
using UnityEngine;

public class TestButtonPro : DuztineBehaviour
{
    public SpriteRenderer sr;
    
    [Button]
    public void Test(string s, string subSprite)
    {
        var arr = Resources.LoadAll<Sprite>("Icons/" + s);
        foreach (var i in arr)
        {
            if (i.name == subSprite)
            {
                sr.sprite = i;
            }
        }
    }
}
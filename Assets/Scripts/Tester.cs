using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public List<Character> leftTeam;
    public List<Character> rightTeam;

    [ShowInInspector]
    public Queue<Character> turn = new();

    [Button]
    public void SetUpTurnQueue()
    {
        var mergedList = new List<Character>(leftTeam);
        mergedList.AddRange(rightTeam);
        mergedList.Sort((c1, c2) => c1.Speed.CompareTo(c2.Speed));
        mergedList.ForEach(c =>
        {
            turn.Enqueue(c);
        });
    }

    [Button]
    public void EndTurn()
    {
        var lastChar = turn.Dequeue();
        turn.Enqueue(lastChar);
    }

    [Button]
    public string ExportJson(Character c)
    {
        return JsonConvert.SerializeObject(c);
    }

    [Button]
    public void ImportJson(string s)
    {
        var c = JsonConvert.DeserializeObject<Character>(s);
        Instantiate(c);
    }
}

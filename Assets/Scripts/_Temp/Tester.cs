using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tester : MonoBehaviour
{
    public List<Character> leftTeam;
    public List<Character> rightTeam;

    [ShowInInspector]
    public Queue<Character> turn = new();

    public bool isEndBattle = false;

    private void Start()
    {
        SetUpTurnQueue();
    }

    [Button]
    public void SetUpTurnQueue()
    {
        var mergedList = new List<Character>(leftTeam);
        mergedList.AddRange(rightTeam);
        mergedList.Sort((c1, c2) => c1.Stats.speed.CompareTo(c2.Stats.speed));
        mergedList.ForEach(c =>
        {
            turn.Enqueue(c);
        });
    }
    
    public void EndTurn()
    {
        var lastChar = turn.Dequeue();
        turn.Enqueue(lastChar);
    }

    [Button]
    public void StartBattle()
    {
        StartCoroutine(FightTurnByTurn());
    }

    IEnumerator FightTurnByTurn()
    {
        while (!isEndBattle)
        {
            var curChar = turn.Peek();

            if (!curChar.IsAlive)
            {
                EndTurn();
                continue;
            }

            var target = GetRandomTarget(GetEnemyTeam(curChar));

            var isEndTurn = false;
            var originalPos = curChar.transform.position;
            var seq = DOTween.Sequence();
            seq.Append(curChar.transform.DOMove(target.transform.position, 0.5f))
                .AppendCallback(() => curChar.Attack(target))
                .Append(curChar.transform.DOMove(originalPos, 0.5f))
                .AppendInterval(0.5f)
                .AppendCallback(() =>
                {
                    isEndTurn = true;
                    if (GetAliveCharacterList(GetEnemyTeam(curChar)).Count < 1)
                    {
                        isEndBattle = true;
                    }
                    else
                    {
                        EndTurn();
                    }
                });

            yield return new WaitUntil(() => isEndTurn || isEndBattle);
        }
    }

    private Character GetRandomTarget(List<Character> enemyTeam)
    {
        var aliveEnemyList = GetAliveCharacterList(enemyTeam);
        if (aliveEnemyList.Count < 1) return null;
        return aliveEnemyList[Random.Range(0, aliveEnemyList.Count)];
    }

    private List<Character> GetAllyTeam(Character c)
    {
        return leftTeam.Contains(c) ? leftTeam : rightTeam;
    }

    private List<Character> GetEnemyTeam(Character c)
    {
        return leftTeam.Contains(c) ? rightTeam : leftTeam;
    }

    private List<Character> GetAliveCharacterList(List<Character> team)
    {
        return team.FindAll(c => c.IsAlive);
    }
}

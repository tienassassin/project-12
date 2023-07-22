using System.Collections.Generic;
using UnityEngine;

public class UserManager : Singleton<UserManager>
{
    public List<CharacterSaveData> chrSaveDataList = new();
}
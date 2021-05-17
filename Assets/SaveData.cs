using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SaveData
{
    public List<CollectedLocation> collectedLocationsList = new List<CollectedLocation>();
}

[Serializable]
public class CollectedLocation
{
    public int location;
    public String expiredTime;
}
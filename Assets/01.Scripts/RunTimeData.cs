using System.Collections.Generic;
using UnityEngine;

public class RunTimeData : MonoSingleton<RunTimeData>
{
    public List<int> DeckList = new List<int>();
    public int HP = 100;
}

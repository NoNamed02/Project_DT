using UnityEngine;

[CreateAssetMenu(menuName = "Cards/CardDefinition")]
public class CardSpec : ScriptableObject
{
    // 나중에 enum으로 정리하는 것을 고려해봐야 할 것 같다
    public int id;
    public string cardName;
    public int cost;
    public string type;
    public string instruction;
    public string[] targeting;
    public string rarity;
    public string discardPolicy;
    public string[] effect;
    public int[] effectAmount;
    public int[] effectHoldingTime;
    public Sprite cardImage;
}

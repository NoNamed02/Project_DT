using UnityEngine;

[CreateAssetMenu(menuName = "Cards/CardDefinition")]
public class CardSpec : ScriptableObject
{
    public int id;
    public string cardName;
    public int cost;
    // public TargetingMode targeting;
    public Sprite artwork;
    public CardEffect[] effects;
}

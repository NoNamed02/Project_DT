using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Execute(Player source, Character target, Card card);
}

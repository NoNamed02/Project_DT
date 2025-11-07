using UnityEngine;

public interface CardEffect
{
    // public abstract void Execute(Player source, Character target, Card card);

    /// <summary>
    /// 카드 효과 처리 함수
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="card"></param>
    public abstract void Execute(Character target, int effectAmount, int effectHoldingTime);
}

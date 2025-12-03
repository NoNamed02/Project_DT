using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using DarkTonic.MasterAudio;
using DG.Tweening;

public class BattleManager : MonoSingleton<BattleManager>
{
    [SerializeField]
    private Player _player;

    public Player Player => _player;
    [SerializeField]
    private List<Enemy> _enemys = new List<Enemy>();

    private event ActionTest OnApplyDamage;
    public delegate void ActionTest();

    [SerializeField]
    private GameObject _resultUI;
    [SerializeField]
    private bool _isBattleEnd = false;
    void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _enemys = FindObjectsByType<Enemy>(FindObjectsSortMode.None).ToList();
        AddCameraEvent(CameraController.CameraMove);
    }

    void Update()
    {
        if (_enemys.Count == 0 && !_isBattleEnd)
        {
            _isBattleEnd = true;
            RunTimeData.Instance.HP = _player.Stats.CurrentHP;
            ShowResultUI();
        }
    }
    private void ShowResultUI()
    {
        _resultUI.SetActive(true);

        CanvasGroup cg = _resultUI.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = _resultUI.AddComponent<CanvasGroup>();
        }

        cg.alpha = 0f;
        cg.DOFade(1f, 0.5f)
        .SetEase(Ease.OutQuad);
    }
    public void AddCameraEvent(ActionTest test)
    {
        OnApplyDamage += test;
    }

    /// <summary>
    /// target에게 damage값 만큼 피해
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    public void ApplyDamage(Character target, int damage)
    {
        // 필요하면 여기서 방어력, 크리티컬 계산
        int finalDamage = damage;
        target.TakeDamage(finalDamage);
        Debug.Log($"------{target}에게 {damage}만큼의 데미지를 줌");
        OnApplyDamage?.Invoke();
        MasterAudio.PlaySound("blade_whoosh");
        VFXManager.Instance.SpawnVFX(target.gameObject, VFXManager.VFXList.Hit);
    }

    /// <summary>
    /// target 방어도 += shieldPoint연산
    /// </summary>
    /// <param name="target"></param>
    /// <param name="shieldPoint"></param>
    public void ApplyShield(Character target, int shieldPoint)
    {
        target.ApplyShield(shieldPoint);
        MasterAudio.PlaySound("shield_metal_deflect");
        
    }

    /// <summary>
    /// target 방어도 0으로 처리
    /// </summary>
    /// <param name="target"></param>
    public void ResetShield(Character target)
    {
        target.Stats.Shield = 0;
    }

    /// <summary>
    /// target 체력 회복
    /// </summary>
    /// <param name="target"></param>
    /// <param name="amount"></param>
    public void HealTarget(Character target, int amount)
    {
        target.Heal(amount);
    }

    /// <summary>
    /// target에게 출혈 효과 부여
    /// </summary>
    /// <returns></returns>
    public void ApplyEffect(Character target, StatusAbnormalityNumber effectID, int amount, int holdingTime)
    {
        target.ApplyEffect(effectID, amount, holdingTime);
    }

    /// <summary>
    /// target에게 출혈 처리
    /// </summary>
    /// <returns></returns>
    public void EffectBleeding(Character target)
    {
        target.EffectBleeding();
    }

    public List<Enemy> GetEnemys()
    {
        return _enemys;
    }
}

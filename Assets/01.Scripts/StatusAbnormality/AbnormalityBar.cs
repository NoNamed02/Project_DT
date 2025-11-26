using UnityEngine;
using System.Collections.Generic;

public class AbnormalityBar : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private AbnormalityIconUI[] _icons; // enum 순서

    private Dictionary<StatusAbnormalityNumber, AbnormalityIconUI> _spawned =
        new Dictionary<StatusAbnormalityNumber, AbnormalityIconUI>();

    void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        var list = _character.StatusAbnormalitys;

        // 종류별로 묶기
        Dictionary<StatusAbnormalityNumber, (int amount, int time)> merged =
            new Dictionary<StatusAbnormalityNumber, (int, int)>();

        foreach (var s in list)
        {
            if (!merged.ContainsKey(s.EffectID))
                merged[s.EffectID] = (0, 0);

            var m = merged[s.EffectID];
            m.amount += s.Amount;
            m.time = Mathf.Max(m.time, s.HoldingTime);
            merged[s.EffectID] = m;
        }

        // UI 생성/갱신
        foreach (var kv in merged)
        {
            var type = kv.Key;

            if (!_spawned.ContainsKey(type))
            {
                var ui = Instantiate(_icons[(int)type], transform);
                _spawned.Add(type, ui);
            }

            _spawned[type].UpdateMergedData(kv.Value.amount, kv.Value.time);
        }

        // UI 제거 (해당 타입이 더 이상 존재하지 않는 경우)
        List<StatusAbnormalityNumber> removeList = new List<StatusAbnormalityNumber>();
        foreach (var pair in _spawned)
        {
            if (!merged.ContainsKey(pair.Key))
            {
                Destroy(pair.Value.gameObject);
                removeList.Add(pair.Key);
            }
        }

        foreach (var key in removeList)
            _spawned.Remove(key);
    }
}
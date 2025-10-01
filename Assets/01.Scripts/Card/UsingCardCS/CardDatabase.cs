using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    [SerializeField] private CardSpec[] cardSpecs;

    private Dictionary<int, CardSpec> _map;

    private void Awake()
    {
        _map = new Dictionary<int, CardSpec>();
        foreach (var spec in cardSpecs)
        {
            _map[spec.id] = spec;
        }
    }

    public CardSpec Get(int id) => _map.TryGetValue(id, out var spec) ? spec : null;
}

using TMPro;
using UnityEngine;

public class AbnormalityIconUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ahTime;

    public void UpdateMergedData(int amount, int holdingTime)
    {
        _ahTime.text = $"{amount} / {holdingTime}";
    }
}
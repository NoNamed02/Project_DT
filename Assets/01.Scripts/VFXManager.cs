using UnityEngine;

public class VFXManager : MonoSingleton<VFXManager>
{
    public enum VFXList
    {
        Hit
    }

    [SerializeField]
    private GameObject[] vfxPrefabs;

    public void SpawnVFX(GameObject target, int index)
    {
        if (target == null) return;
        if (index < 0 || index >= vfxPrefabs.Length) return;
        if (vfxPrefabs[index] == null) return;

        Instantiate(
            vfxPrefabs[index],
            target.transform.position,
            Quaternion.identity
        );
    }

    public void SpawnVFX(GameObject target, VFXList type)
    {
        SpawnVFX(target, (int)type);
    }
}

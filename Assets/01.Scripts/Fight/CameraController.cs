using DG.Tweening;
using UnityEngine;

public static class CameraController
{
    public static void CameraMove()
    {
        var cam = Camera.main.transform;

        cam.DOKill();

        Vector3 originPos = cam.localPosition;
        Vector3 originScale = cam.localScale;

        cam.DOPunchPosition(
            punch: new Vector3(0.12f, 0.08f, 0f), // 좌우 + 상하 미세 흔들림
            duration: 0.08f,
            vibrato: 10,
            elasticity: 0.3f
        );

        cam.DOPunchScale(
            punch: new Vector3(0.06f, -0.04f, 0f), // X 축 약간 커지고 Y 축 살짝 눌림
            duration: 0.10f,
            vibrato: 8,
            elasticity: 0.4f
        )
        .OnComplete(() =>
        {
            cam.localPosition = originPos;
            cam.localScale = originScale;
        });
    }
}

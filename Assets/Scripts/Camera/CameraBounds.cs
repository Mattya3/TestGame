using System;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[Serializable]
public class CameraBounds
{
    [SerializeField]
    private bool _freezeX = false; // X���̈ړ������b�N���邩�ǂ���

    [SerializeField]
    private bool _freezeY = false; // Y���̈ړ������b�N���邩�ǂ���

    [SerializeField]
    private float _leftBound = float.NegativeInfinity; // �J������X���W�̍ŏ��l

    [SerializeField]
    private float _rightBound = float.PositiveInfinity; // �J������X���W�̍ő�l

    [SerializeField]
    private float _bottomBound = float.NegativeInfinity; // �J������Y���W�̍ŏ��l

    [SerializeField]
    private float _topBound = float.PositiveInfinity; // �J������Y���W�̍ő�l

    public bool HasReversedBounds()
    {
        return _leftBound > _rightBound || _bottomBound > _topBound;
    }

    public bool HasNaN()
    {
        return float.IsNaN(_leftBound)
            || float.IsNaN(_rightBound)
            || float.IsNaN(_bottomBound)
            || float.IsNaN(_topBound);
    }

    public bool HasInfinity()
    {
        return float.IsInfinity(_leftBound)
            || float.IsInfinity(_rightBound)
            || float.IsInfinity(_bottomBound)
            || float.IsInfinity(_topBound);
    }

    public Vector3 Bound(Vector3 pos, Vector3 originalPos)
    {
        return new Vector3(
            _freezeX ? originalPos.x : Mathf.Clamp(pos.x, _leftBound, _rightBound),
            _freezeY ? originalPos.y : Mathf.Clamp(pos.y, _bottomBound, _topBound),
            pos.z
        );
    }

    public void DrawGizmos(Camera camera, Vector3 originalPos)
    {
        if (!_CanDrawGizmos())
            return;

        var halfHeight = camera.orthographicSize;
        var halfWidth = halfHeight * camera.aspect;

        var visualLeft = (_freezeX ? originalPos.x : _leftBound) - halfWidth;
        var visualRight = (_freezeX ? originalPos.x : _rightBound) + halfWidth;
        var visualBottom = (_freezeY ? originalPos.y : _bottomBound) - halfHeight;
        var visualTop = (_freezeY ? originalPos.y : _topBound) + halfHeight;

        var cameraSize = new Vector3((visualRight - visualLeft), (visualTop - visualBottom), 1);
        var cameraCenter = new Vector3(
            (visualLeft + visualRight) / 2,
            (visualBottom + visualTop) / 2,
            0
        );
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(cameraCenter, cameraSize);
    }

    private bool _CanDrawGizmos()
    {
        return !HasReversedBounds() && !HasNaN() && !HasInfinity();
    }
}

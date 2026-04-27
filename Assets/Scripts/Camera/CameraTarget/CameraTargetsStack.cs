using UnityEngine;
using System.Collections.Generic;

// カメラターゲットのスタック
// 最上位のターゲットが現在のカメラターゲットとなる
public class CameraTargetsStack
{
    private Stack<ICameraTarget> _stack = new Stack<ICameraTarget>();

    public CameraTargetsStack(ICameraTarget[] targetsArray)
    {
        // 配列の順序を逆にしてスタックに積む
        for (int i = targetsArray.Length - 1; i >= 0; i--)
            _stack.Push(targetsArray[i]);
    }

    public bool IsEmpty => _stack.Count == 0;

    public void Push(ICameraTarget target)
    {
        _stack.Push(target);
        target.OnStart();
    }

    public void Pop()
    {
        // スタックが空になる場合はポップしない
        if (_stack.Count <= 1)
            return;

        _stack.Pop();
        Start();
    }

    public void Start()
    {
        if (_stack.Count > 0)
            _stack.Peek().OnStart();

        // スタックの最上位のターゲットがアクティブでない場合は、スタックから削除して次のターゲットを確認する
        while (_stack.Count > 1 && !_stack.Peek().IsActive)
        {
            _stack.Pop();
            if (_stack.Count > 0)
                _stack.Peek().OnStart();
        }
    }

    public Vector3 Position {
        get
        {
            if (_stack.Count > 0)
                return _stack.Peek().Position;
            else
                return Vector3.zero; // スタックが空の場合は原点を返す
        }
    }

    public bool EnableCollider {
        get
        {
            if (_stack.Count > 0)
                return _stack.Peek().EnableCollider;
            else
                return false; // スタックが空の場合はコライダーを無効にする
        }
    }
}

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ShowIfAttribute : PropertyAttribute
{
    /// <summary>
    /// 条件となるフィールド名、プロパティ名、メソッド名。
    /// </summary>
    public string MemberName { get; }

    /// <summary>
    /// メソッドに渡す引数。
    /// </summary>
    public object[] Arguments { get; }

    public ShowIfAttribute(string memberName)
    {
        MemberName = memberName;
        Arguments = Array.Empty<object>();
    }

    public ShowIfAttribute(string memberName, params object[] arguments)
    {
        MemberName = memberName;
        Arguments = arguments ?? Array.Empty<object>();
    }
}
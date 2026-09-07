using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override void OnGUI(
        Rect position,
        SerializedProperty property,
        GUIContent label)
    {
        if (!_ShouldShow(property))
            return;

        EditorGUI.PropertyField(
            position,
            property,
            label,
            true
        );
    }

    public override float GetPropertyHeight(
        SerializedProperty property,
        GUIContent label)
    {
        if (!_ShouldShow(property))
            return 0f;

        return EditorGUI.GetPropertyHeight(
            property,
            label,
            true
        );
    }

    private bool _ShouldShow(SerializedProperty property)
    {
        var attribute = (ShowIfAttribute)this.attribute;

        object target = _GetTargetObject(property);

        if (target == null)
            return false;

        return _EvaluateMember(
            target,
            attribute.MemberName,
            attribute.Arguments
        );
    }

    private bool _EvaluateMember(
        object target,
        string memberName,
        object[] arguments)
    {
        Type type = target.GetType();

        // --------------------------------
        // 1. フィールド
        // --------------------------------

        FieldInfo field = _FindField(type, memberName);

        if (field != null)
        {
            object value = field.GetValue(target);

            return _ConvertToBool(value);
        }

        // --------------------------------
        // 2. プロパティ
        // --------------------------------

        PropertyInfo property = _FindProperty(type, memberName);

        if (property != null &&
            property.CanRead)
        {
            object value = property.GetValue(target);

            return _ConvertToBool(value);
        }

        // --------------------------------
        // 3. メソッド
        // --------------------------------

        MethodInfo method = _FindMethod(
            type,
            memberName,
            arguments
        );

        if (method != null)
        {
            object[] convertedArguments =
                _ConvertArguments(
                    method.GetParameters(),
                    arguments
                );

            if (convertedArguments == null)
            {
                Debug.LogError(
                    $"ShowIf: メソッド '{memberName}' の引数を変換できません。"
                );

                return false;
            }

            object result = method.Invoke(
                target,
                convertedArguments
            );

            return _ConvertToBool(result);
        }

        Debug.LogError(
            $"ShowIf: '{memberName}' が " +
            $"{type.Name} に見つかりません。"
        );

        return false;
    }

    // ============================================================
    // Field
    // ============================================================

    private FieldInfo _FindField(
        Type type,
        string name)
    {
        while (type != null)
        {
            FieldInfo field = type.GetField(
                name,
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.DeclaredOnly
            );

            if (field != null)
                return field;

            type = type.BaseType;
        }

        return null;
    }

    // ============================================================
    // Property
    // ============================================================

    private PropertyInfo _FindProperty(
        Type type,
        string name)
    {
        while (type != null)
        {
            PropertyInfo property = type.GetProperty(
                name,
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.DeclaredOnly
            );

            if (property != null)
                return property;

            type = type.BaseType;
        }

        return null;
    }

    // ============================================================
    // Method
    // ============================================================

    private MethodInfo _FindMethod(
        Type type,
        string name,
        object[] arguments)
    {
        while (type != null)
        {
            MethodInfo[] methods = type.GetMethods(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.DeclaredOnly
            );

            foreach (MethodInfo method in methods)
            {
                if (method.Name != name)
                    continue;

                ParameterInfo[] parameters =
                    method.GetParameters();

                if (parameters.Length != arguments.Length)
                    continue;

                if (_CanConvertArguments(
                    parameters,
                    arguments))
                {
                    return method;
                }
            }

            type = type.BaseType;
        }

        return null;
    }

    // ============================================================
    // Argument conversion
    // ============================================================

    private bool _CanConvertArguments(
        ParameterInfo[] parameters,
        object[] arguments)
    {
        for (int i = 0; i < parameters.Length; i++)
        {
            if (!_CanConvertArgument(
                arguments[i],
                parameters[i].ParameterType))
            {
                return false;
            }
        }

        return true;
    }

    private bool _CanConvertArgument(
        object value,
        Type targetType)
    {
        if (value == null)
            return !targetType.IsValueType ||
                   Nullable.GetUnderlyingType(targetType) != null;

        Type valueType = value.GetType();

        if (targetType.IsAssignableFrom(valueType))
            return true;

        if (targetType.IsEnum)
        {
            return value is string ||
                   value is Enum ||
                   value is int;
        }

        try
        {
            Convert.ChangeType(
                value,
                targetType
            );

            return true;
        }
        catch
        {
            return false;
        }
    }

    private object[] _ConvertArguments(
        ParameterInfo[] parameters,
        object[] arguments)
    {
        object[] result =
            new object[arguments.Length];

        for (int i = 0; i < arguments.Length; i++)
        {
            try
            {
                result[i] = _ConvertArgument(
                    arguments[i],
                    parameters[i].ParameterType
                );
            }
            catch
            {
                return null;
            }
        }

        return result;
    }

    private object _ConvertArgument(
        object value,
        Type targetType)
    {
        if (value == null)
            return null;

        if (targetType.IsAssignableFrom(
            value.GetType()))
        {
            return value;
        }

        if (targetType.IsEnum)
        {
            if (value is string stringValue)
            {
                return Enum.Parse(
                    targetType,
                    stringValue
                );
            }

            return Enum.ToObject(
                targetType,
                value
            );
        }

        return Convert.ChangeType(
            value,
            targetType
        );
    }

    // ============================================================
    // bool conversion
    // ============================================================

    private bool _ConvertToBool(object value)
    {
        if (value == null)
            return false;

        if (value is bool boolValue)
            return boolValue;

        // UnityEngine.Objectにも対応
        if (value is UnityEngine.Object unityObject)
            return unityObject != null;

        return false;
    }

    // ============================================================
    // SerializedProperty → 実際のオブジェクト
    // ============================================================

    private object _GetTargetObject(SerializedProperty property)
    {
        object obj = property.serializedObject.targetObject;

        string path = property.propertyPath;

        string[] elements = path.Split('.');

        // 最後の要素はShowIfが付いている
        // プロパティ自身なので、そこまでは辿らない
        for (int i = 0; i < elements.Length - 1; i++)
        {
            string element = elements[i];

            if (element == "Array")
                continue;

            if (element.StartsWith("data["))
            {
                int index = int.Parse(
                    element.Substring(
                        5,
                        element.Length - 6
                    )
                );

                if (obj is System.Collections.IList list)
                {
                    obj = list[index];
                }

                continue;
            }

            obj = _GetMemberValue(obj, element);

            if (obj == null)
                return null;
        }

        return obj;
    }

    private object _GetMemberValue(
        object obj,
        string name)
    {
        if (obj == null)
            return null;

        Type type = obj.GetType();

        FieldInfo field =
            _FindField(type, name);

        if (field != null)
            return field.GetValue(obj);

        PropertyInfo property =
            _FindProperty(type, name);

        if (property != null &&
            property.CanRead)
        {
            return property.GetValue(obj);
        }

        return null;
    }
}
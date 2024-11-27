using UnityEditor;
using UnityEngine;

namespace PunkPlatformerGame
{
    [System.Serializable]
    public class FloatReference
    {
        public FloatVariable variable;
        public float constantValue;
        public bool useConstant;

        public float Value
        {
            get
            {
                return useConstant ? constantValue : variable.value;
            }
            set
            {
                if (useConstant)
                {
                    constantValue = value;
                }
                else if (variable != null)
                {
                    variable.value = value;
                }
                else
                {
                    Debug.LogWarning("Attempting to set a FloatReference value, but no FloatVariable is assigned.");
                }
            }
        }
    }


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(FloatReference))]
    public class FloatReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Calculate rects
            Rect toggleRect = new Rect(position.x, position.y, 20, position.height);
            Rect fieldRect = new Rect(position.x + 25, position.y, position.width - 25, position.height);

            // Get properties
            SerializedProperty useConstantProp = property.FindPropertyRelative("useConstant");
            SerializedProperty constantValueProp = property.FindPropertyRelative("constantValue");
            SerializedProperty variableProp = property.FindPropertyRelative("variable");

            // Draw toggle
            useConstantProp.boolValue = EditorGUI.Toggle(toggleRect, useConstantProp.boolValue);

            // Draw field based on toggle
            if (useConstantProp.boolValue)
            {
                EditorGUI.PropertyField(fieldRect, constantValueProp, GUIContent.none);
            }
            else
            {
                EditorGUI.PropertyField(fieldRect, variableProp, GUIContent.none);
            }

            EditorGUI.EndProperty();
        }
    }
#endif
}

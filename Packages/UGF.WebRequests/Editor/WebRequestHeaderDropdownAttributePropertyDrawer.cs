using System;
using System.Collections.Generic;
using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Dropdown;
using UGF.EditorTools.Editor.IMGUI.PropertyDrawers;
using UGF.WebRequests.Runtime;
using UnityEditor;
using UnityEngine;

namespace UGF.WebRequests.Editor
{
    [CustomPropertyDrawer(typeof(WebRequestHeaderDropdownAttribute), true)]
    internal class WebRequestHeaderDropdownAttributePropertyDrawer : PropertyDrawerTyped<WebRequestHeaderDropdownAttribute>
    {
        private readonly Func<IEnumerable<DropdownItem<string>>> m_handler;

        public WebRequestHeaderDropdownAttributePropertyDrawer() : base(SerializedPropertyType.String)
        {
            m_handler = WebRequestHeaderEditorGUIUtility.GetHeaderDropdownItems;
        }

        protected override void OnDrawProperty(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            EditorElementsUtility.TextFieldWithDropdown(position, label, serializedProperty, m_handler);
        }
    }
}

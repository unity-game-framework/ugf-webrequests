using System.Collections.Generic;
using UGF.EditorTools.Editor.IMGUI.Dropdown;
using UGF.WebRequests.Runtime;

namespace UGF.WebRequests.Editor
{
    public static class WebRequestHeaderEditorGUIUtility
    {
        public static IEnumerable<DropdownItem<string>> GetHeaderDropdownItems()
        {
            var items = new List<DropdownItem<string>>();

            for (int i = 0; i < WebRequestHeaders.All.Count; i++)
            {
                string value = WebRequestHeaders.All[i];

                items.Add(new DropdownItem<string>(value, value));
            }

            return items;
        }
    }
}

using UI.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DynamicMenus
{
    public class EntryView : MonoBehaviour
    {
        public EntryStruct entry;
        public Image image;
        public ToolType type = ToolType.BUILD;

        public void Click()
        {
            FindObjectOfType<ToolsController>().SetTool(type, entry.item);
        }
    }
}
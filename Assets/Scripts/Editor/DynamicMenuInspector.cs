using Pawns;
using UI.DynamicMenus;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(DynamicMenu))]
    public class DynamicMenuInspector : UnityEditor.Editor
    {
        private ReorderableList _reorderableList;
        private DynamicMenu ListExample => target as DynamicMenu;

        private void OnEnable()
        {
            _reorderableList =
                new ReorderableList(ListExample.entries, typeof(EntryStruct), true, true, true, true)
                {
                    elementHeight = 50
                };

            // This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
            // Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
            // which is a UnityEngine.Object
            // reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("list"), true, true, true, true);

            // Add listeners to draw events
            _reorderableList.drawHeaderCallback += DrawHeader;
            _reorderableList.drawElementCallback += DrawElement;

            _reorderableList.onAddCallback += AddItem;
            _reorderableList.onRemoveCallback += RemoveItem;
        }

        private void OnDisable()
        {
            // Make sure we don't get memory leaks etc.
            _reorderableList.drawHeaderCallback -= DrawHeader;
            _reorderableList.drawElementCallback -= DrawElement;

            _reorderableList.onAddCallback -= AddItem;
            _reorderableList.onRemoveCallback -= RemoveItem;
        }

        /// <summary>
        /// Draws the header of the list
        /// </summary>
        /// <param name="rect"></param>
        private void DrawHeader(Rect rect)
        {
            GUI.Label(rect, "Dynamic Menu");
        }

        /// <summary>
        /// Draws one element of the list (ListItemExample)
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="index"></param>
        /// <param name="active"></param>
        /// <param name="focused"></param>
        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            var item = ListExample.entries[index];

            EditorGUI.BeginChangeCheck();

            var width = rect.height;

            item.icon = (Sprite) EditorGUI.ObjectField(
                new Rect(rect.x, rect.y, width, rect.height),
                item.icon,
                typeof(Sprite),
                false
            );

            item.item = (Pawn) EditorGUI.ObjectField(
                new Rect(rect.x + width, rect.y + ((rect.height - 16) / 2 ), rect.width - width, 16),
                item.item,
                typeof(Pawn),
                true
            );

            ListExample.entries[index] = item;

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);   
            }

            // If you are using a custom PropertyDrawer, this is probably better
            // EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
            // Although it is probably smart to cach the list as a private variable ;)
        }

        private void AddItem(ReorderableList list)
        {
            ListExample.entries.Add(new EntryStruct());

            EditorUtility.SetDirty(target);
        }

        private void RemoveItem(ReorderableList list)
        {
            ListExample.entries.RemoveAt(list.index);

            EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Actually draw the list in the inspector
            _reorderableList.DoLayoutList();
        }
    }
}
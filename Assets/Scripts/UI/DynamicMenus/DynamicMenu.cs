using System;
using System.Collections.Generic;
using Pawns;
using UnityEngine;

namespace UI.DynamicMenus
{
    [Serializable]
    public struct EntryStruct
    {
        public Pawn item;
        public Sprite icon;
    }

    [CreateAssetMenu(menuName = "Items/Menu")]
    public class DynamicMenu : ScriptableObject
    {
        public EntryView entryPrefab;
        public List<EntryStruct> entries;

        public void Generate(GameObject parent)
        {
            foreach (var entry in entries)
            {
                entryPrefab.entry = entry;
                entryPrefab.image.sprite = entry.icon;
                entryPrefab.image.SetNativeSize();

                var newEntry = Instantiate(entryPrefab, parent.transform);
                newEntry.name = entry.item.name; 
                newEntry.transform.SetAsFirstSibling();
            }
        }
    }
}
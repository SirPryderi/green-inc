using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        public string name;
        public Sprite sprite;
    }
}
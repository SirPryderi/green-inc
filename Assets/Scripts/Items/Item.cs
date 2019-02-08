using UnityEngine;

namespace Items
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public string name;
        public Sprite sprite;
    }
}
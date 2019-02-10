using Organisations;
using UnityEngine;

namespace Pawns
{
    public class Pawn : MonoBehaviour
    {
        [Header("Pawn")]
        [Min(0)] public int price;
        [Min(0)] public int upkeep;
        public Organisation owner;
        
        protected HexCell ParentCell;
        protected GameObject Mesh;

        protected virtual void Awake()
        {
            ParentCell = transform.parent.GetComponent<HexCell>();

            Mesh = transform.Find("mesh").gameObject;
        }

        public virtual void Evaluate()
        {
        }

        public static Pawn Load(string pawn)
        {
            var obj = Resources.Load(pawn) as GameObject;

            return obj?.GetComponent<Pawn>();
        }
    }
}
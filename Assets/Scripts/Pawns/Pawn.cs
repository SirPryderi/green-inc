using Organisations;
using UnityEngine;

namespace Pawns
{
    public class Pawn : MonoBehaviour
    {
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
    }
}
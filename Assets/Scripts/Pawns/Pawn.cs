using UnityEngine;

namespace Pawns
{
    public abstract class Pawn : MonoBehaviour
    {
        protected HexCell ParentCell;
        protected GameObject Mesh;

        protected virtual void Awake()
        {
            ParentCell = transform.parent.GetComponent<HexCell>();

            Mesh = transform.Find("mesh").gameObject;
        }

        public abstract void Evaluate();
    }
}
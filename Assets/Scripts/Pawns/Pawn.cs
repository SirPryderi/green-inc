using System;
using cakeslice;
using Mechanics;
using Organisations;
using UnityEngine;

namespace Pawns
{
    public class Pawn : MonoBehaviour, IObservable
    {
        [Header("Pawn")] [Min(0)] public int price;
        [Min(0)] public int upkeep;
        public Organisation owner;

        public bool CanBePlacedOnLand = true;
        public bool CanBePlacedOnWater = false;

        protected HexCell ParentCell;
        protected GameObject Mesh;

        protected virtual void Awake()
        {
            ParentCell = transform.parent.GetComponent<HexCell>();
            Mesh = transform.Find("mesh").gameObject;
            G.O.Register(this);
        }

        public void Focus()
        {
            var meshFilters = GetComponentsInChildren<MeshFilter>();

            foreach (var filter in meshFilters)
            {
                filter.gameObject.AddComponent<Outline>();
            }
        }

        public void UnFocus()
        {
            var outlines = GetComponentsInChildren<Outline>();
            foreach (var outline in outlines) Destroy(outline);
        }

        private void OnDestroy()
        {
            try
            {
                #pragma warning disable 0168
                G.O.UnRegister(this);
                #pragma warning enable 0168
            }
            catch (NullReferenceException)
            {
            }
        }

        public virtual void Evaluate()
        {
        }

        public static Pawn Load(string pawn)
        {
            var obj = Resources.Load(pawn) as GameObject;

            return obj?.GetComponent<Pawn>();
        }

        public virtual void PreTick()
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void PostTick()
        {
            owner?.ConsumeMoney(upkeep * G.DeltaTime, true);
        }

        public virtual void LateTick()
        {
        }

        public virtual bool CanBePlacedOn(HexCell cell)
        {
            return cell.HasWater && CanBePlacedOnWater || !cell.HasWater && CanBePlacedOnLand;
        }
    }
}
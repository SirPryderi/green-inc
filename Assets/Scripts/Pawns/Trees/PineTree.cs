using UnityEngine;

namespace Pawns.Trees
{
    public class PineTree : Tree
    {
        private GameObject _snow;

        private void Start()
        {
            Evaluate();
        }

        protected override void Awake()
        {
            base.Awake();

            _snow = Mesh.transform.Find("Snow").gameObject;
        }

        public override void Evaluate()
        {
            _snow.SetActive(ParentCell.Temperature < -5f);
        }
    }
}
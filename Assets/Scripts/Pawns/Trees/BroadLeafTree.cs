using UnityEngine;

namespace Pawns.Trees
{
    public class BroadLeafTree : Tree
    {
        private GameObject _leaves;
        private GameObject _snow;

        protected override void Awake()
        {
            base.Awake();

            _leaves = Mesh.transform.Find("Leaves").gameObject;
            _snow = Mesh.transform.Find("Snow").gameObject;
        }

        private void Start()
        {
            Evaluate();
        }

        public override void Evaluate()
        {
            var temp = ParentCell.Temperature;

            if (temp > 35)
            {
                _snow.SetActive(false);
                _leaves.SetActive(false);
            }
            else if (temp > 10)
            {
                _snow.SetActive(false);
                _leaves.SetActive(true);
            }
            else if (temp > 5)
            {
                _snow.SetActive(false);
                _leaves.SetActive(false);
            }
            else
            {
                _snow.SetActive(true);
                _leaves.SetActive(false);
            }
        }
    }
}
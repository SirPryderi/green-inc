using System.Collections.Generic;
using UnityEngine;

namespace Logistics
{
    public class DistanceFrom : Comparer<MonoBehaviour>
    {
        private readonly GameObject _target;

        public DistanceFrom(GameObject target)
        {
            _target = target;
        }

        public override int Compare(MonoBehaviour x, MonoBehaviour y)
        {
            var transformPosition = _target.transform.position;

            var distance1 = (x.transform.position - transformPosition).sqrMagnitude;
            var distance2 = (y.transform.position - transformPosition).sqrMagnitude;

            return distance1.CompareTo(distance2);
        }
    }
}
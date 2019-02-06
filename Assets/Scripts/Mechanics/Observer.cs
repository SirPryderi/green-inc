using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Mechanics
{
    public class Observer
    {
        private readonly List<IObservable> _observables;
        public int Time { get; private set; }
        public int DeltaTime { get; private set; }

        public Observer()
        {
            _observables = new List<IObservable>();
        }

        public void Register(IObservable observable)
        {
            _observables.Add(observable);
        }

        public void UnRegister(IObservable observable)
        {
            _observables.Remove(observable);
        }

        public void AdvanceTime(int hours)
        {
            G.MP.Statistics.TakeSnapshot();

            Time += hours;
            DeltaTime = hours;

            foreach (var observable in _observables)
            {
                observable.StartFrame();
            }

            foreach (var observable in _observables)
            {
                observable.EndFrame();
            }

            Object.FindObjectOfType<HexGrid>().Refresh();
        }
    }
}
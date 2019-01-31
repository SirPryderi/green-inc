using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Mechanics
{
    public class Observer
    {
        private readonly List<IObservable> observables;
        public int Time { get; private set; }

        public Observer()
        {
            observables = new List<IObservable>();
        }

        public void Register(IObservable observable)
        {
            observables.Add(observable);
        }

        public void UnRegister(IObservable observable)
        {
            observables.Remove(observable);
        }

        public void AdvanceTime(int hours)
        {
            foreach (var observable in observables)
            {
                observable.StartFrame();
            }

            foreach (var observable in observables)
            {
                observable.EndFrame();
            }

            Time += hours;
        }
    }
}
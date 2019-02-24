using System.Collections.Generic;
using AI;
using UI;
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
            Time += hours;
            DeltaTime = hours;

            foreach (var observable in _observables)
            {
                observable.PreTick();
            }

            foreach (var observable in _observables)
            {
                observable.Tick();
            }

            foreach (var observable in _observables)
            {
                observable.PostTick();
            }
            
            G.MP.Statistics.TakeSnapshot();

            foreach (var observable in _observables)
            {
                observable.LateTick();
            }

            Object.FindObjectOfType<HexGrid>().Refresh();
            CompanyController.ControlAll();
            PawnWindow.RefreshAll();
        }
    }
}
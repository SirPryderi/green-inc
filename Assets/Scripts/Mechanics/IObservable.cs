namespace Mechanics
{
    public interface IObservable
    {
        void PreTick();

        void Tick();

        void PostTick();

        void LateTick();
    }
}
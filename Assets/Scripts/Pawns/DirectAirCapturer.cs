namespace Pawns
{
    public class DirectAirCapturer : Pawn
    {
        public string gas;
        public float hourlyRate;

        public override void Tick()
        {
            G.CM.Atmosphere.Absorb(gas, hourlyRate * G.DeltaTime);
        }
    }
}
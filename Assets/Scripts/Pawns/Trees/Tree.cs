using UnityEngine;

namespace Pawns.Trees
{
    public class Tree : Pawn
    {
        [Header("Tree")] 
        [Min(0)] public float absorbedCo2;
        [Min(0)] public float producedO2;

        public override void Tick()
        {
            G.CM.Atmosphere.Absorb("Carbon Dioxide", absorbedCo2 * G.DeltaTime);
        }
    }
}
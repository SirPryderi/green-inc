using Logistics;
using Pawns;
using UI.PawnWindowComponents;
using UnityEngine;

namespace UI
{
    public class PawnWindow : MonoBehaviour
    {
        public Window window;
        public Pawn pawn;

        public CityTileComponent cityTileComponent;
        public RequesterComponent requesterComponent;
        public GeneratorComponent generatorComponent;
        public TreeComponent treeComponent;

        private void Awake()
        {
            window = GetComponent<Window>();

            window.title.text = pawn.name.Replace("(Clone)", "");

            var cityTile = pawn.GetComponent<CityTile>();

            if (cityTile != null)
            {
                cityTileComponent.component = cityTile;
                cityTileComponent.parent = this;
                Instantiate(cityTileComponent, window.body);
            }

            foreach (var requester in pawn.GetComponents<Requester>())
            {
                requesterComponent.component = requester;
                requesterComponent.parent = this;
                Instantiate(requesterComponent, window.body);
            }
            
            foreach (var provider in pawn.GetComponents<Provider>())
            {
                generatorComponent.component = provider;
                generatorComponent.parent = this;
                Instantiate(generatorComponent, window.body);
            }
            
            foreach (var tree in pawn.GetComponents<Pawns.Trees.Tree>())
            {
                treeComponent.component = tree;
                treeComponent.parent = this;
                Instantiate(treeComponent, window.body);
            }
        }

        public void Refresh()
        {
            foreach (var component in GetComponentsInChildren<PawnWindowComponent>())
            {
                component.Refresh();
            }
        }

        public static void RefreshAll()
        {
            foreach (var window in FindObjectsOfType<PawnWindow>()) window.Refresh();
        }
    }
}
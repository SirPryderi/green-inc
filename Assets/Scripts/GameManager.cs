using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Atmosphere atmosphere { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();

        atmosphere = new Atmosphere();

        Debug.Log(Instance.atmosphere.text);
    }
}

public class Atmosphere
{
    public string text = "Atmosphere placeholder text";
}
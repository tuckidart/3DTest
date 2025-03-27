using UnityEngine;

public class DirtPile : MonoBehaviour
{
    [SerializeField]
    private GameObject _carrotPrefab = null;

    private void Start()
    {
        GrowCarrot();
    }

    //Spawns a new carrot in the dirt pile.
    public void GrowCarrot()
    {
        GameObject go = Instantiate(_carrotPrefab, transform);
        go.transform.localScale = Vector3.zero;

        //Register the GrowCarrot method as a callback so a new carrot grows when one is plucked.
        go.GetComponent<Carrot>().AddOnPluckedCallback(GrowCarrot);
    }
}

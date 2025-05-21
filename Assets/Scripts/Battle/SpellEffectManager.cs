using UnityEngine;

public class SpellEffectManager : MonoBehaviour
{
    public static SpellEffectManager Instance;

    public GameObject fireEffect;
    public GameObject waterEffect;
    public GameObject earthEffect;
    public GameObject airEffect;
    public GameObject metalEffect;
    public GameObject poisonEffect;
    public GameObject eletricEffect;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SpawnEffect(Element element, Vector3 position)
    {
        GameObject effectToSpawn = null;

        switch (element)
        {
            case Element.Fire:
                effectToSpawn = fireEffect;
                break;
            case Element.Water:
                effectToSpawn = waterEffect;
                break;
            case Element.Earth:
                effectToSpawn = earthEffect;
                break;
            case Element.Air:
                effectToSpawn = airEffect;
                break;
            case Element.Metal:
                effectToSpawn = metalEffect;
                break;
            case Element.Poison:
                effectToSpawn = poisonEffect;
                break;
            case Element.Eletric:
                effectToSpawn = eletricEffect;
                break;
        }

        if (effectToSpawn != null)
        {
            Instantiate(effectToSpawn, position, Quaternion.identity);
        }
    }
}


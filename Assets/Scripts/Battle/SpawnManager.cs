using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Escolha o tipo de inimigo")]
    public Elemento tipoDoInimigo;

    [Header("Prefabs dos inimigos")]
    public GameObject enemyFogoPrefab;
    public GameObject enemyAguaPrefab;
    public GameObject enemyTerraPrefab;
    public GameObject enemyArPrefab;

    private GameObject inimigoAtual;

    void Start()
    {
        SpawnarInimigo();
    }

    void SpawnarInimigo()
    {
        GameObject prefabSelecionado = GetPrefabDoElemento(tipoDoInimigo);

        if (prefabSelecionado != null)
        {
            inimigoAtual = Instantiate(prefabSelecionado, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Prefab do inimigo não está atribuído para o elemento: " + tipoDoInimigo);
        }
    }

    GameObject GetPrefabDoElemento(Elemento tipo)
    {
        switch (tipo)
        {
            case Elemento.Fogo:
                return enemyFogoPrefab;
            case Elemento.Agua:
                return enemyAguaPrefab;
            case Elemento.Terra:
                return enemyTerraPrefab;
            case Elemento.Ar:
                return enemyArPrefab;
            default:
                return null;
        }
    }

    // Método público opcional para trocar o inimigo durante o jogo
    public void TrocarInimigo(Elemento novoTipo)
    {
        if (inimigoAtual != null)
            Destroy(inimigoAtual);

        tipoDoInimigo = novoTipo;
        SpawnarInimigo();
    }
}

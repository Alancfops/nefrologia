using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VrButtonLoadScene : MonoBehaviour
{
    [Header("Nome exato da cena (Build Settings)")]
    [SerializeField] private string sceneName;

    private Button btn;

    private void Awake()
    {
        // Pega o componente Button automaticamente
        btn = GetComponent<Button>();

        // Adiciona o listener pro clique
        btn.onClick.AddListener(LoadScene);
    }

    public void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning($"⚠️ Nenhum nome de cena definido em {gameObject.name}");
            return;
        }

        // Verifica se a cena está nas Build Settings
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log($"🔄 Carregando cena: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"❌ Cena '{sceneName}' não está adicionada nas Build Settings!");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VRButtonExitGame : MonoBehaviour
{
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ExitGame);
    }

    public void ExitGame()
    {
        Debug.Log("🚪 Saindo do jogo...");

#if UNITY_EDITOR
        // Encerra o modo Play dentro do editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Fecha o aplicativo no build final (PC, Android, etc)
        Application.Quit();
#endif
    }
}

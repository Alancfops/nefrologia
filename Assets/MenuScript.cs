using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [Header("Scene Names (Build Settings)")]
    [SerializeField] private string menuScene = "MainMenu";
    [SerializeField] private string quizScene = "QuizScene";
    [SerializeField] private string consultaScene = "ConsultaScene";

    // Botões do Menu
    public void LoadQuiz() => SceneManager.LoadScene(quizScene, LoadSceneMode.Single);
    public void LoadConsulta() => SceneManager.LoadScene(consultaScene, LoadSceneMode.Single);

    // Em qualquer cena do jogo
    public void LoadMenu() => SceneManager.LoadScene(menuScene, LoadSceneMode.Single);
    public void ReloadCurrent() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

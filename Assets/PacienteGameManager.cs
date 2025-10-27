using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PacienteGameManager : MonoBehaviour
{
    [Header("Referências de UI")]
    public TextMeshProUGUI sintomaTexto;
    public Button botaoProsseguir;
    public GameObject painelSintomas;
    public GameObject painelPerguntaArriscar;
    public GameObject painelDoencaEscolha;
    public GameObject painelLaudo;
    public GameObject painelResultado;
    public TextMeshProUGUI resultadoTexto;

    [Header("Botões de escolha")]
    public Button botaoSimArriscar;
    public Button botaoNaoArriscar;
    public Button botaoDoencaRenalCronica;
    public Button botaoSindromeNefrotica;
    public Button botaoPielonefrite;
    public Button botaoLaudo1;
    public Button botaoLaudo2;
    public Button botaoLaudo3;

    [Header("Textos dos Laudos (TMP)")]
    public TextMeshProUGUI textoLaudo1;
    public TextMeshProUGUI textoLaudo2;
    public TextMeshProUGUI textoLaudo3;

    private List<string> sintomasRenal = new();
    private List<string> sintomasPielonefrite = new();
    private List<string> sintomasNefrotica = new();
    private List<string> sintomasAtuais = new();

    private int indiceSintoma = 0;
    private int doencaCorreta = -1;
    private int doencaEscolhida = -1;
    private bool arriscou = false;
    private int pontos = 0;
    private bool jogoFinalizado = false;

    void Start()
    {
        AdicionarSintomas();
        SortearDoenca();
        MostrarSintoma();
        ConfigurarBotoes();

        Debug.Log($"Iniciando jogo — sorteio de doença...");
        Debug.Log($"🧬 Doença sorteada: {GetNomeDoenca()} (ID {doencaCorreta})");
    }

    void ConfigurarBotoes()
    {
        botaoProsseguir.onClick.RemoveAllListeners();
        botaoProsseguir.onClick.AddListener(ProsseguirSintoma);

        botaoSimArriscar.onClick.RemoveAllListeners();
        botaoSimArriscar.onClick.AddListener(() => EscolherArriscar(true));
        botaoNaoArriscar.onClick.RemoveAllListeners();
        botaoNaoArriscar.onClick.AddListener(() => EscolherArriscar(false));

        botaoDoencaRenalCronica.onClick.AddListener(() => AdivinharDoenca(0));
        botaoPielonefrite.onClick.AddListener(() => AdivinharDoenca(1));
        botaoSindromeNefrotica.onClick.AddListener(() => AdivinharDoenca(2));

        botaoLaudo1.onClick.AddListener(() => EscolherLaudo(0));
        botaoLaudo2.onClick.AddListener(() => EscolherLaudo(1));
        botaoLaudo3.onClick.AddListener(() => EscolherLaudo(2));
    }

    void AdicionarSintomas()
    {
        sintomasRenal.AddRange(new string[]
        {
            "O paciente relata cansaço frequente e fraqueza.",
            "Apresenta inchaço visível nos tornozelos e pés.",
            "Relata urina com coloração escura e espuma.",
            "Refere pressão arterial elevada.",
            "Queixa-se de náusea matinal persistente.",
            "Sente dificuldade para se concentrar em tarefas simples.",
            "Percebeu redução no volume de urina diário.",
            "Refere dores lombares leves.",
            "Apresenta falta de apetite constante.",
            "Mostra sinais de anemia e palidez."
        });

        sintomasPielonefrite.AddRange(new string[]
        {
            "Relata febre alta e calafrios recorrentes.",
            "Sente dores intensas na região lombar.",
            "Refere ardência e dor ao urinar.",
            "Apresenta urina turva e com odor forte.",
            "Sente cansaço extremo durante o dia.",
            "Apresenta náusea e enjoo após as refeições.",
            "A febre persiste por mais de dois dias.",
            "Sente calafrios noturnos.",
            "Relata dor de cabeça constante.",
            "Refere mal-estar generalizado."
        });

        sintomasNefrotica.AddRange(new string[]
        {
            "O paciente relata inchaço ao redor dos olhos ao acordar.",
            "Sente aumento de peso repentino sem motivo aparente.",
            "A urina apresenta espuma em grande quantidade.",
            "Refere fadiga e fraqueza constantes.",
            "Apresenta dor abdominal leve.",
            "Relata perda de apetite e náusea.",
            "Os inchaços pioram ao final do dia.",
            "Sente tonturas ocasionais.",
            "Refere pressão arterial alterada.",
            "Relata sensação de aperto no peito em alguns momentos."
        });
    }

    void SortearDoenca()
    {
        doencaCorreta = Random.Range(0, 3);
        sintomasAtuais = doencaCorreta switch
        {
            0 => new List<string>(sintomasRenal),
            1 => new List<string>(sintomasPielonefrite),
            2 => new List<string>(sintomasNefrotica),
            _ => new List<string>()
        };

        Debug.Log($"🧬 Doença sorteada: {GetNomeDoenca()} (ID {doencaCorreta})");
    }

    void MostrarSintoma()
    {
        if (indiceSintoma < sintomasAtuais.Count)
        {
            sintomaTexto.text = sintomasAtuais[indiceSintoma];

            if (indiceSintoma == 3) // após o 4º sintoma
            {
                painelSintomas.SetActive(false);
                painelPerguntaArriscar.SetActive(true);
            }
        }
        else MostrarPerguntaFinal();
    }

    void ProsseguirSintoma()
    {
        indiceSintoma++;
        if (indiceSintoma < sintomasAtuais.Count)
        {
            MostrarSintoma();
        }
        else
        {
            MostrarPerguntaFinal();
        }
    }

    void EscolherArriscar(bool resposta)
    {
        painelPerguntaArriscar.SetActive(false);

        if (resposta)
        {
            painelDoencaEscolha.SetActive(true);
            arriscou = true;
        }
        else
        {
            painelSintomas.SetActive(true);
            indiceSintoma = 4; // continua do sintoma 5 em diante
            MostrarSintoma();
        }
    }

    void AdivinharDoenca(int idEscolhida)
    {
        doencaEscolhida = idEscolhida;

        if (doencaEscolhida == doencaCorreta)
        {
            if (arriscou)
            {
                pontos += 10;
                Debug.Log("💰 +10 pontos por acertar ao arriscar!");
            }
            else
            {
                pontos += 5;
                Debug.Log("💰 +5 pontos por acertar ao final!");
            }

            MostrarTelaLaudo();
        }
        else
        {
            if (arriscou)
                Debug.Log("❌ Errou o diagnóstico ao arriscar (0 pontos)");
            else
                Debug.Log("❌ Errou o diagnóstico final (0 pontos)");

            MostrarTelaLaudo();
        }
    }

    void MostrarTelaLaudo()
    {
        painelDoencaEscolha.SetActive(false);
        PreencherLaudos(); // 🔹 exibe os textos corretos nos botões
        painelLaudo.SetActive(true);
    }

    void PreencherLaudos()
    {
        string[] laudos;

        switch (doencaCorreta)
        {
            case 0: // Doença Renal Crônica
                laudos = new string[]
                {
                    "Recomenda controle rigoroso da pressão arterial e redução do sal.",
                    "Uso imediato de antibióticos de amplo espectro.",
                    "Tratamento com corticoides e dieta rica em proteínas."
                };
                break;

            case 1: // Pielonefrite
                laudos = new string[]
                {
                    "Controle dietético e uso de corticoides sob supervisão médica.",
                    "Uso de antibióticos e hidratação adequada para combater infecção.",
                    "Apenas acompanhamento com nefrologista sem medicação."
                };
                break;

            case 2: // Síndrome Nefrótica
                laudos = new string[]
                {
                    "Uso de corticoides, repouso e dieta hipossódica.",
                    "Tratamento com antibióticos intravenosos.",
                    "Apenas controle de líquidos e evitar esforço físico."
                };
                break;

            default:
                laudos = new string[] { "", "", "" };
                break;
        }

        textoLaudo1.text = laudos[0];
        textoLaudo2.text = laudos[1];
        textoLaudo3.text = laudos[2];
    }

    void EscolherLaudo(int opcao)
    {
        painelLaudo.SetActive(false);

        // Define qual é o laudo correto para cada doença
        int laudoCorreto = doencaCorreta switch
        {
            0 => 0, // Doença Renal Crônica → opção 1
            1 => 1, // Pielonefrite → opção 2
            2 => 0, // Síndrome Nefrótica → opção 1
            _ => 0
        };

        if (opcao == laudoCorreto)
        {
            if (arriscou) pontos += 5;
            else pontos += 3;
            Debug.Log("✅ Acertou o laudo!");
        }
        else
        {
            if (arriscou) pontos -= 2;
            else pontos -= 1;
            Debug.Log("❌ Errou o laudo!");
        }

        MostrarResultado();
    }

    void MostrarPerguntaFinal()
    {
        painelSintomas.SetActive(false);
        painelDoencaEscolha.SetActive(true);
    }

    void MostrarResultado()
    {
        jogoFinalizado = true;
        painelResultado.SetActive(true);
        resultadoTexto.text =
            $"Diagnóstico concluído!\n\nPontuação final: {pontos}\n" +
            $"Doença correta: {GetNomeDoenca()}";
    }

    string GetNomeDoenca() => doencaCorreta switch
    {
        0 => "Doença Renal Crônica",
        1 => "Pielonefrite",
        2 => "Síndrome Nefrótica",
        _ => "Desconhecida"
    };
}

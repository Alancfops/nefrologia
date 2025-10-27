using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Pergunta
    {
        public string pergunta;
        public int respostaCorreta; // 0 = Sim, 1 = Não
    }

    [Header("Referências de UI")]
    public TextMeshProUGUI perguntaTexto;
    public Button botaoSim;
    public Button botaoNao;
    public Button botaoSimArriscar;
    public Button botaoNaoArriscar;
    public Button botaoDoencaRenalCronica;
    public Button botaoSindromeNefrotica;
    public Button botaoPolienefrite;
    public GameObject painelResultado;
    public TextMeshProUGUI resultadoTexto;
    public GameObject painelQuiz;
    public GameObject painelDoencaEscolha;
    public GameObject painelPerguntaContinuar;
    public GameObject painelFinal;

    private List<Pergunta> perguntasRenal = new();
    private List<Pergunta> perguntasNefrotica = new();
    private List<Pergunta> perguntasPielonefrite = new();
    private List<Pergunta> perguntasAtuais = new();

    private int indiceAtual = 0;
    private int acertos = 0;
    private int pontos = 0;
    private int numeroPerguntasRespondidas = 0;
    private bool quizFinalizado = false;
    private int doencaEscolhida = -1;
    private int doencaCorreta = -1;
    private bool arriscou = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Mantém o QuizManager vivo
    }

    void Start()
    {
        AdicionarPerguntas();
        SortearDoenca();
        MostrarPergunta();
        ConfigurarBotoes();
    }

    void OnEnable()
    {
        // Garante que os botões sempre chamem os métodos corretos
        ConfigurarBotoes();
    }

    void ConfigurarBotoes()
    {
        botaoSim.onClick.RemoveAllListeners();
        botaoNao.onClick.RemoveAllListeners();
        botaoSimArriscar.onClick.RemoveAllListeners();
        botaoNaoArriscar.onClick.RemoveAllListeners();
        botaoDoencaRenalCronica.onClick.RemoveAllListeners();
        botaoSindromeNefrotica.onClick.RemoveAllListeners();
        botaoPolienefrite.onClick.RemoveAllListeners();

        botaoSim.onClick.AddListener(() => Responder(0));
        botaoNao.onClick.AddListener(() => Responder(1));
        botaoSimArriscar.onClick.AddListener(() => RespostaPerguntaContinuar(true));
        botaoNaoArriscar.onClick.AddListener(() => RespostaPerguntaContinuar(false));

        botaoDoencaRenalCronica.onClick.AddListener(() => AdivinharDoenca(0));
        botaoPolienefrite.onClick.AddListener(() => AdivinharDoenca(1));
        botaoSindromeNefrotica.onClick.AddListener(() => AdivinharDoenca(2));
    }

    void AdicionarPerguntas()
    {
        perguntasRenal.AddRange(new Pergunta[]
        {
            new() { pergunta = "Inchaço nos pés ou tornozelos pode indicar um problema relacionado à função renal?", respostaCorreta = 0 },
            new() { pergunta = "Alterações no nível de glicose no sangue podem afetar a função dos rins?", respostaCorreta = 0 },
            new() { pergunta = "A pressão arterial não tem relação com a saúde dos rins?", respostaCorreta = 1 },
            new() { pergunta = "Cansaço excessivo e falta de energia podem estar relacionados a uma condição renal?", respostaCorreta = 0 },
            new() { pergunta = "Problemas nos rins podem piorar ao longo do tempo se não tratados adequadamente?", respostaCorreta = 0 },
            new() { pergunta = "Mudanças na urina, como cor ou volume, podem ser sinais de um problema nos rins?", respostaCorreta = 0 },
            new() { pergunta = "Pressão alta é um fator comum que contribui para o declínio da função renal?", respostaCorreta = 0 },
            new() { pergunta = "Em estágios avançados, pode ser necessário recorrer a tratamentos como a diálise?", respostaCorreta = 1 },
            new() { pergunta = "O envelhecimento pode aumentar o risco de disfunção renal?", respostaCorreta = 0 }
        });

        perguntasPielonefrite.AddRange(new Pergunta[]
        {
            new() { pergunta = "Febre alta acompanhada de dor nas costas pode ser um sinal de infecção nos rins?", respostaCorreta = 0 },
            new() { pergunta = "Infecções no trato urinário podem se espalhar para os rins?", respostaCorreta = 0 },
            new() { pergunta = "Infecções renais podem ser tratadas com antibióticos?", respostaCorreta = 0 },
            new() { pergunta = "Dor ao urinar pode ser um sintoma de infecção urinária que atinge os rins?", respostaCorreta = 0 },
            new() { pergunta = "A febre alta é um sintoma comum em infecções graves nos rins?", respostaCorreta = 0 },
            new() { pergunta = "Febre persistente, associada a dores nos rins, pode indicar uma infecção séria?", respostaCorreta = 0 },
            new() { pergunta = "Infecções nos rins podem causar complicações se não tratadas de forma adequada?", respostaCorreta = 0 },
            new() { pergunta = "Homens têm menor risco de infecções urinárias graves nos rins em comparação com mulheres?", respostaCorreta = 1 },
            new() { pergunta = "O tratamento antibiótico é essencial para infecções renais?", respostaCorreta = 0 }
        });

        perguntasNefrotica.AddRange(new Pergunta[]
        {
            new() { pergunta = "Inchaço nas pernas ou ao redor dos olhos pode ser causado por problemas renais?", respostaCorreta = 0 },
            new() { pergunta = "A perda excessiva de proteína na urina pode ser indicativa de distúrbios nos rins?", respostaCorreta = 0 },
            new() { pergunta = "Infecções urinárias podem agravar condições renais que afetam a função do organismo?", respostaCorreta = 1 },
            new() { pergunta = "Acúmulo de líquidos no corpo pode ser um sinal de complicações renais?", respostaCorreta = 0 },
            new() { pergunta = "Ganho de peso rápido devido a inchaço é um sintoma de problemas nos rins?", respostaCorreta = 0 },
            new() { pergunta = "Alterações hormonais e glicose descontrolada podem afetar a saúde renal?", respostaCorreta = 0 },
            new() { pergunta = "Essa condição é mais prevalente em crianças do que em adultos?", respostaCorreta = 0 },
            new() { pergunta = "O uso de medicamentos como corticoides pode ajudar no tratamento de distúrbios renais?", respostaCorreta = 0 },
            new() { pergunta = "Antibióticos são usados no tratamento de doenças renais que causam alterações no metabolismo da proteína?", respostaCorreta = 1 }
        });

    }

    void SortearDoenca()
    {
        int doencaSorteada = Random.Range(0, 3);
        doencaCorreta = doencaSorteada;
        PlayerPrefs.SetInt("ultimaDoencaCorreta", doencaCorreta);

        perguntasAtuais = doencaCorreta switch
        {
            0 => new List<Pergunta>(perguntasRenal),
            1 => new List<Pergunta>(perguntasPielonefrite),
            2 => new List<Pergunta>(perguntasNefrotica),
            _ => new List<Pergunta>()
        };

        Debug.Log($"🧬 Doença sorteada: {GetNomeDoenca()} (ID {doencaCorreta})");
    }

    void MostrarPergunta()
    {
        if (indiceAtual < perguntasAtuais.Count)
        {
            Pergunta p = perguntasAtuais[indiceAtual];
            perguntaTexto.text = p.pergunta;

            // apos finalizar as perguntas, chamar pergunta final. chama quando responder 10 perguntas
            if (indiceAtual == perguntasAtuais.Count - 1)
                MostrarPerguntaFinal();
        }
        // chamar pergunta final quando iindice atual chegar em 10
        else MostrarResultado();
    }

    public void Responder(int respostaEscolhida)
    {
        if (quizFinalizado) return;

        if (respostaEscolhida == perguntasAtuais[indiceAtual].respostaCorreta)
        {
            acertos++;
            pontos += 1;
        }

        indiceAtual++;
        numeroPerguntasRespondidas++;

        if (numeroPerguntasRespondidas == 4)
            PerguntarSeQuerArriscar();
        else if (numeroPerguntasRespondidas < 10)
            MostrarPergunta();
        else
            MostrarPerguntaFinal();
    }

    void MostrarPerguntaFinal()
    {
        DesativarTelas();
        painelFinal.SetActive(true);
        
    }

    void PerguntarSeQuerArriscar()
    {
        DesativarTelas();
        painelPerguntaContinuar.SetActive(true);
    }

    public void RespostaPerguntaContinuar(bool resposta)
    {
        DesativarTelas();

        if (resposta)
        {
            painelDoencaEscolha.SetActive(true);
            arriscou = true;
            Debug.Log("⚠️ Jogador escolheu arriscar!");
        }
        else
        {
            painelQuiz.SetActive(true);
            indiceAtual = 4;
            numeroPerguntasRespondidas = 4;
            MostrarPergunta();
        }
    }

    public void AdivinharDoenca(int doencaEscolhida)
    {
        this.doencaEscolhida = doencaEscolhida;

        if (doencaCorreta == -1)
        {
            doencaCorreta = PlayerPrefs.GetInt("ultimaDoencaCorreta", 0);
        }

        Debug.Log($"🎯 Jogador escolheu {GetNomeDoencaEscolhida(doencaEscolhida)}");
        Debug.Log($"✅ Doença correta é {GetNomeDoenca()}");

        if (doencaEscolhida == doencaCorreta)
        {
            if (arriscou)
            {
                pontos += 20;
                Debug.Log("💰 +20 pontos por acertar ao arriscar!");
            }
            else
            {
                pontos += 5;
                Debug.Log("💰 +5 pontos por acertar no final!");
            }
        }
        else Debug.Log("❌ Errou a doença!");

        MostrarResultado();
    }

    void MostrarResultado()
    {
        quizFinalizado = true;
        DesativarTelas();
        painelResultado.SetActive(true);

        resultadoTexto.text = $"Você fez {pontos} pontos.\n\nLaudo: " +
            $"{(acertos >= perguntasAtuais.Count / 2 ? "Sintomas detectados." : "Sintomas leves detectados.")}\n" +
            $"A doença correta era: {GetNomeDoenca()}";
    }

    string GetNomeDoenca() => doencaCorreta switch
    {
        0 => "Doença Renal Crônica",
        1 => "Pielonefrite",
        2 => "Síndrome Nefrótica",
        _ => "Desconhecida"
    };

    string GetNomeDoencaEscolhida(int id) => id switch
    {
        0 => "Doença Renal Crônica",
        1 => "Pielonefrite",
        2 => "Síndrome Nefrótica",
        _ => "Desconhecida"
    };

    void DesativarTelas()
    {
        painelQuiz.SetActive(false);
        painelDoencaEscolha.SetActive(false);
        painelPerguntaContinuar.SetActive(false);
        painelFinal.SetActive(false);
    }
}

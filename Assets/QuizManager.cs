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
            new() { pergunta = "A Doença Renal Crônica pode causar inchaço nos pés e tornozelos?", respostaCorreta = 0 },
            new() { pergunta = "Está relacionada com diabetes?", respostaCorreta = 0 },
            new() { pergunta = "Não afeta a pressão arterial.", respostaCorreta = 1 },
            new() { pergunta = "Pode causar fadiga e cansaço excessivo?", respostaCorreta = 0 },
            new() { pergunta = "A função renal pode piorar ao longo do tempo?", respostaCorreta = 0 },
            new() { pergunta = "Pode causar alterações na urina?", respostaCorreta = 0 },
            new() { pergunta = "A pressão alta é uma das principais causas.", respostaCorreta = 0 },
            new() { pergunta = "O tratamento com diálise é sempre necessário.", respostaCorreta = 1 },
            new() { pergunta = "É mais comum em pessoas idosas.", respostaCorreta = 0 }
        });


        perguntasPielonefrite.AddRange(new Pergunta[]
        {
            new() { pergunta = "Pode causar febre alta e dor nas costas?", respostaCorreta = 0 },
            new() { pergunta = "É uma infecção nos rins causada por bactérias.", respostaCorreta = 0 },
            new() { pergunta = "Não é tratada com antibióticos.", respostaCorreta = 1 },
            new() { pergunta = "Pode ser uma complicação de uma infecção urinária.", respostaCorreta = 0 },
            new() { pergunta = "A dor ao urinar é um sintoma comum.", respostaCorreta = 0 },
            new() { pergunta = "É uma infecção crônica e não desaparece.", respostaCorreta = 1 },
            new() { pergunta = "A febre alta é um sintoma clássico.", respostaCorreta = 0 },
            new() { pergunta = "É mais comum em homens.", respostaCorreta = 1 },
            new() { pergunta = "O tratamento com antibióticos é fundamental.", respostaCorreta = 0 }
        });
        perguntasNefrotica.AddRange(new Pergunta[]
        {
            new() { pergunta = "Pode causar inchaço nas pernas e nos olhos?", respostaCorreta = 0 },
            new() { pergunta = "O corpo perde proteína pela urina.", respostaCorreta = 0 },
            new() { pergunta = "É uma infecção urinária.", respostaCorreta = 1 },
            new() { pergunta = "Leva ao acúmulo de líquidos no corpo.", respostaCorreta = 0 },
            new() { pergunta = "Pode causar ganho de peso rápido devido ao inchaço.", respostaCorreta = 0 },
            new() { pergunta = "Pode ser causada por diabetes.", respostaCorreta = 0 },
            new() { pergunta = "Mais comum em crianças do que em adultos.", respostaCorreta = 0 },
            new() { pergunta = "Pode ser tratada com antibióticos.", respostaCorreta = 1 },
            new() { pergunta = "Pode precisar de corticoides no tratamento.", respostaCorreta = 0 }
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
        }
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

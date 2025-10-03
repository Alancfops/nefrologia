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

    public TextMeshProUGUI perguntaTexto;
    public Button botaoSim;
    public Button botaoNao;
    public GameObject painelResultado; // Painel do laudo
    public TextMeshProUGUI resultadoTexto;

    public GameObject painelQuiz; // Painel do quiz com perguntas

    private List<Pergunta> perguntas = new List<Pergunta>();
    private int indiceAtual = 0;
    private int acertos = 0;
    private bool quizFinalizado = false; // Variável para evitar avanço automático

    void Start()
    {
        // Adicionando perguntas com base no conteúdo do documento
        perguntas.Add(new Pergunta { pergunta = "A Doença Renal Crônica pode causar inchaço nos pés e tornozelos?", respostaCorreta = 0 }); // Sim
        perguntas.Add(new Pergunta { pergunta = "Na Síndrome Nefrótica, o corpo perde proteína pela urina?", respostaCorreta = 0 }); // Sim
        perguntas.Add(new Pergunta { pergunta = "A Pielonefrite não causa febre alta.", respostaCorreta = 1 }); // Não
        perguntas.Add(new Pergunta { pergunta = "A Síndrome Nefrótica pode causar aumento rápido de peso devido ao acúmulo de líquidos.", respostaCorreta = 0 }); // Sim
        perguntas.Add(new Pergunta { pergunta = "A Doença Renal Crônica é muitas vezes causada por diabetes ou pressão alta.", respostaCorreta = 0 }); // Sim

        // Configurar interações dos botões
        botaoSim.onClick.AddListener(() => Responder(0)); // Resposta Sim
        botaoNao.onClick.AddListener(() => Responder(1)); // Resposta Não

        // Mostrar a primeira pergunta
        MostrarPergunta();
    }

    void MostrarPergunta()
    {
        if (indiceAtual < perguntas.Count)
        {
            Pergunta p = perguntas[indiceAtual];
            perguntaTexto.text = p.pergunta;
        }
        else
        {
            MostrarResultado();
        }
    }

    public void Responder(int respostaEscolhida)
    {
        if (quizFinalizado) return;  // Impede que o quiz avance depois de finalizar

        // Verifique se a resposta está correta
        if (respostaEscolhida == perguntas[indiceAtual].respostaCorreta)
        {
            acertos++;  // Incrementa os acertos
        }

        indiceAtual++;  // Passa para a próxima pergunta

        // Exibe a próxima pergunta ou mostra o resultado final
        if (indiceAtual < perguntas.Count)
        {
            MostrarPergunta();
        }
        else
        {
            MostrarResultado();
        }
    }

    void MostrarResultado()
    {
        quizFinalizado = true; // Impede interação após finalizar

        // Desativa o painel de perguntas
        painelQuiz.SetActive(false);

        // Ativa o painel de laudo
        painelResultado.SetActive(true);

        // Mostra o laudo
        resultadoTexto.text = "Você acertou " + acertos + " de " + perguntas.Count + " perguntas.\n\nLaudo: " +
            (acertos >= perguntas.Count / 2 ? "Paciente apresenta sintomas que indicam atenção médica." : "Sintomas leves detectados.");
    }
}

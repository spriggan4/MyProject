//18년 6월 30일 황재석

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingTextMng : MonoBehaviour
{
    public Text endingTextBox = null;

    private TextTrigger textTrigger = null;
    private Queue<string> sentences = null;

    [Header("타이핑 속도")]
    [SerializeField]
    private float typingSpeed=0;

    private void Start()
    {
        textTrigger = this.gameObject.GetComponent<TextTrigger>();
        if (textTrigger == null)
        {
            Debug.LogError("엔딩텍스트매니져의 텍스트 트리거에 값이 없음");
        }
        sentences = new Queue<string>();
        if (sentences == null)
        {
            Debug.LogError("엔딩텍스트매니져의 센텐스 is missing.");
        }
        TypingEndingText(textTrigger.texts);
    }

    public void TypingEndingText(Texts _endingTexts)
    {
        sentences.Clear();

        foreach (string _sentence in _endingTexts.sentences)
        {
            sentences.Enqueue(_sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            //출력이 끝나면 종료
            Application.Quit();
            return;
        }
        string sentence = sentences.Dequeue();

        //타이핑이 끝나기 전에 continue 버튼 누르면 타이핑이 정지되도록
        //StopAllCoroutines 함수 추가
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string _sentence)
    {
        //텍스트 박스 비워두기
        endingTextBox.text = "";

        foreach (char letter in _sentence.ToCharArray())
        {
            endingTextBox.text += letter;
            
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}

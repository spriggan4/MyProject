//18년 7월 15일 황재석
//튜토리얼 텍스트 매니

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextMng : MonoBehaviour
{
    public Text tutorialTextBox = null;//저장된 문장들을 출력하는 텍스트 박스
    public Button tutorialTextButton = null;//출력된 후 다음으로 넘기기 위한 continue 버튼

    private TextTrigger textTrigger = null;//문장들을 저장 하는 변수
    private Queue<string> sentences = null;//텍스트들를 먼저 저장된 순서로 빼기 위해 Queue형 사용

    private void Start()
    {
        textTrigger = this.gameObject.GetComponent<TextTrigger>();
        if (textTrigger == null)
        {
            Debug.LogError("TutorialTextMng의 texitTrigger 값이 없음");
        }
        sentences = new Queue<string>();
        if (sentences == null)
        {
            Debug.LogError("TutorialTextMng의 sentences is missing.");
        }
        TypingDutorialText(textTrigger.texts);
    }

    public void TextBoxOn()
    {
        tutorialTextBox.gameObject.SetActive(true);
    }

    public void TextButtonOn()
    {
        tutorialTextButton.gameObject.SetActive(true);
    }

    public void TextBoxOff()
    {
        tutorialTextBox.gameObject.SetActive(false);
    }

    public void TextButtonOff()
    {
        tutorialTextButton.gameObject.SetActive(false);
    }

    //TutorialMng에서 읽고 상태를 변화할 수 있게 만드는 함수
    public int CountSentencesNum()
    {
        return sentences.Count;
    }

    public void DestroyDutorialText()
    {
        Destroy(tutorialTextBox.gameObject);
        Destroy(tutorialTextButton.gameObject);
    }

    public void TypingDutorialText(Texts _dutorialTexts)
    {
        sentences.Clear();

        foreach (string _sentence in _dutorialTexts.sentences)
        {
            sentences.Enqueue(_sentence);
        }
        DisplayNextSentence();
    }
    
    public void DisplayNextSentence()
    {
        string sentence = sentences.Dequeue();

        //텍스트 박스 비워두기
        tutorialTextBox.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            tutorialTextBox.text += letter;
        }
    }
}

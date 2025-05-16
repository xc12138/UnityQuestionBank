using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using System;

public class InputPanel : MonoBehaviour
{
    // 题目类别下拉框
    public Dropdown questionTypeDropdown;
    // 题目输入框
    public InputField questionInput;
    // 代码输入框
    public InputField codeInput;
    // 答案输入框
    public InputField answerInput;
    // 提交按钮
    public Button okBtn;
    // 返回按钮
    public Button quitBtn;


    public static void Show(List<Dropdown.OptionData> options, int initQuestionType)
    {
        var uiObj = UIMgr.instance.ShowUI("UIPrefabs/InputPanel");
        var panel = uiObj.GetComponent<InputPanel>();
        panel.OnShow(options, initQuestionType);
    }

    private void OnShow(List<Dropdown.OptionData> options, int initQuestionType)
    {
        questionTypeDropdown.ClearOptions();
        questionTypeDropdown.AddOptions(options);
        questionTypeDropdown.value = initQuestionType;

        // 提交题目到服务器
        okBtn.onClick.AddListener(() =>
        {
            var questionType = questionTypeDropdown.options[questionTypeDropdown.value].text;
            var question = questionInput.text;
            var code = codeInput.text;
            var answer = answerInput.text;
            if (string.IsNullOrEmpty(question))
            {
                FlyTips.Show("请输入题目");
                return;
            }
            if (string.IsNullOrEmpty(answer))
            {
                FlyTips.Show("请输入答案");
                return;
            }
            HttpHelper.instance.StartPostAddQuestion(questionType, question, code, answer, (result) =>
            {
                Debug.Log("试题类型  " + questionType);
                var jd = JsonMapper.ToObject(result);
                var errorCode = int.Parse(jd["error_code"].ToString());
                if (0 == errorCode)
                {
                    FlyTips.Show("试题录入成功！");
                    questionInput.text = "";
                    codeInput.text = "";
                    answerInput.text = "";
                }
                else
                {
                    FlyTips.Show("服务器出错，录入失败！");
                }
            });
        });

        quitBtn.onClick.AddListener(()=>
        {
            Destroy(gameObject);
        });
    }
}

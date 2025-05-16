using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class MainPanel : MonoBehaviour
{
    // 题目类别下拉框
    public Dropdown questionTypeDropdown;
    // 题目文本
    public Text questionText;
    // 题目录入按钮
    public Button inputQuestionBtn;
    // 看答案按钮
    public Button answerBtn;
    // 下一题按钮
    public Button nextBtn;

    private JsonData m_questionDate;

    public static void Show()
    {
        var uiObj = UIMgr.instance.ShowUI("UIPrefabs/MainPanel");
        var panel = uiObj.GetComponent<MainPanel>();
        panel.OnShow();
    }

    private void OnShow()
    {
        questionText.text = "正在请求服务器，请稍等···";
        questionTypeDropdown.ClearOptions();
        HttpHelper.instance.StartGetAllQuestionTypes((result) =>
        {
            Debug.Log(result);
            var jd = JsonMapper.ToObject(result);
            List<string> options = new List<string>();
            // C#基础排在最前面
            options.Add("C#基础");
            foreach (var item in jd)
            {
                var option = item.ToString();
                if ("C#基础" == option)
                    continue;
                options.Add(option);
            }

            questionTypeDropdown.AddOptions(options);

            ReqOneQuestion();
        });

        nextBtn.onClick.AddListener(() =>
        {
            ReqOneQuestion();
        });

        answerBtn.onClick.AddListener(() =>
        {
            UpdateQuestionText(true);
        });


        questionTypeDropdown.onValueChanged.AddListener((v) =>
        {
            ReqOneQuestion();
        });

        // 录入新题
        inputQuestionBtn.onClick.AddListener(() =>
        {
            InputPanel.Show(questionTypeDropdown.options, questionTypeDropdown.value);
        });
    }

    /// <summary>
    /// 请求下一题
    /// </summary>
    public void ReqOneQuestion()
    {
        var questionType = questionTypeDropdown.options[questionTypeDropdown.value].text;
        HttpHelper.instance.StartGetOneQuestion(questionType, (result) =>
        {
            var jd = JsonMapper.ToObject(result);
            var errorCode = int.Parse(jd["error_code"].ToString());
            if (0 == errorCode)
            {
                m_questionDate = jd["data"];
                UpdateQuestionText(false);
            }
            else
            {
                m_questionDate = null;
                questionText.text = "当前类型题库为空！";
                FlyTips.Show("当前类型题库为空！");
            }
        });
    }

    /// <summary>
    /// 更新题目文本
    /// </summary>
    /// <param name="withAnswer">是否包含答案</param>
    public void UpdateQuestionText(bool withAnswer)
    {
        if (m_questionDate == null)
        {
            FlyTips.Show("未选择任何题目！");
            return;
        }
        if (withAnswer)
            {
                questionText.text = string.Format("题目:\n{0}\n{1}\n\n解答:\n{2}", m_questionDate["question"].ToString(), m_questionDate["code"].ToString(), m_questionDate["answer"].ToString());
            }
            else
            {
                questionText.text = string.Format("题目:\n{0}", m_questionDate["question"].ToString());
            }
    }

    


}

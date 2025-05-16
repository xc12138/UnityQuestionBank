using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class HttpHelper : MonoBehaviour
{
    private static HttpHelper s_instance;
    public static HttpHelper instance
    {
        get
        {
            if (!s_instance)
            {
                var go = new GameObject("HttpHelper");
                s_instance = go.AddComponent<HttpHelper>();
            }
            return s_instance;
        }
    }


    // web服务器地址
    public const string WebUrl = "http://192.168.3.6:3209/";

    /// <summary>
    /// 请求题目所有类别
    /// </summary>
    /// <param name="cb"></param>
    public void StartGetAllQuestionTypes(Action<string> cb)
    {
        // StartCoroutine调用Get接口
        StartCoroutine(CoroutineHttpGet(WebUrl + "get_question_types", cb));
    }

    /// <summary>
    /// 随机获取一个题目
    /// </summary>
    /// <param name="cb"></param>
    public void StartGetOneQuestion(string questionType, Action<string> cb)
    {
        // StartCoroutine调用Get接口
        questionType = UnityWebRequest.EscapeURL(questionType);
        // 执行Get请求
        StartCoroutine(CoroutineHttpGet(WebUrl + "get_one_question?question_type=" + questionType, cb));
    }

    /// <summary>
    /// 试题录入
    /// </summary>
    /// <param name="questionType">题目类别</param>
    /// <param name="question">题目</param>
    /// <param name="code">代码</param>
    /// <param name="answer">答案</param>
    /// <param name="cb">回调</param>
    public void StartPostAddQuestion(string questionType, string question, string code, string answer, Action<string> cb)
    {
        // StartCoroutine调用Post接口
        WWWForm form = new WWWForm();
        form.AddField("question_type", questionType);
        form.AddField("question", question);
        form.AddField("code", code);
        form.AddField("answer", answer);

        StartCoroutine(CoroutineHttpPost(WebUrl + "add_one_question", form, cb));
    }

    // http Get接口
    IEnumerator CoroutineHttpGet(string url, Action<string> cb)
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        if (!string.IsNullOrEmpty(req.error))
        {
            Debug.Log(req.error);
            yield break;
        }
        cb?.Invoke(req.downloadHandler.text);
        req.Dispose();
    }

    // http Post接口
    IEnumerator CoroutineHttpPost(string url, WWWForm form, Action<string> cb)
    {
        UnityWebRequest req = UnityWebRequest.Post(url, form);
        yield return req.SendWebRequest();
        if (!string.IsNullOrEmpty(req.error))
        {
            Debug.Log(req.error);
            cb?.Invoke("{'error_code:-1}");
            yield break;
        }
        cb?.Invoke(req.downloadHandler.text);
        req.Dispose();
    }

}

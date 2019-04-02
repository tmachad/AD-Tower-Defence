using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ColorFadeText : MonoBehaviour {

    public Color m_StartColor;
    public Color m_EndColor;
    public float m_FadeDelay;
    public float m_FadeDuration;
    public bool m_ToggleActive;

    private Text m_Text;
    private IEnumerator m_Coroutine;

    private void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    public void StartFade()
    {
        if (m_ToggleActive)
        {
            gameObject.SetActive(true);
        }

        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
        }

        m_Coroutine = FadeColor();
        StartCoroutine(m_Coroutine);
    }

    private IEnumerator FadeColor()
    {

        m_Text.color = m_StartColor;

        if (m_FadeDelay > 0)
        {
            yield return new WaitForSeconds(m_FadeDelay);
        }

        float elapsedTime = 0;
        while(elapsedTime < m_FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            m_Text.color = Color.Lerp(m_StartColor, m_EndColor, Mathf.SmoothStep(0, 1, elapsedTime / m_FadeDuration));
            yield return null;
        }

        m_Text.color = m_EndColor;

        if (m_ToggleActive)
        {
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorFadeSprite : MonoBehaviour {

    public Color m_StartColor;
    public Color m_EndColor;
    public float m_FadeDelay;
    public float m_FadeDuration;
    public bool m_DestroyWhenDone;

    private SpriteRenderer m_SpriteRenderer;
    private IEnumerator m_Coroutine;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        m_Coroutine = FadeColor();
        StartCoroutine(m_Coroutine);
    }

    private IEnumerator FadeColor()
    {
        m_SpriteRenderer.color = m_StartColor;

        if (m_FadeDelay > 0)
        {
            yield return new WaitForSeconds(m_FadeDelay);
        }

        float elapsedTime = 0;
        while (elapsedTime < m_FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            m_SpriteRenderer.color = Color.Lerp(m_StartColor, m_EndColor, elapsedTime / m_FadeDuration);
            yield return null;
        }

        m_SpriteRenderer.color = m_EndColor;

        if (m_DestroyWhenDone)
        {
            Destroy(gameObject);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMover : MonoBehaviour {

    public Vector2[] m_Positions;
    public int m_PositionIndex;
    public float m_TransitionTime;


    private IEnumerator m_Coroutine;
    private RectTransform m_RectTransform;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        m_RectTransform.anchoredPosition = m_Positions[m_PositionIndex];
    }

    public void MoveRelative(int indexDelta)
    {
        m_PositionIndex = Mathf.Clamp(m_PositionIndex + indexDelta, 0, m_Positions.Length - 1);

        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
        }

        m_Coroutine = MoveTo(m_Positions[m_PositionIndex]);
        StartCoroutine(m_Coroutine);
    }

    public void MoveTo(int index)
    {
        m_PositionIndex = Mathf.Clamp(index, 0, m_Positions.Length - 1);

        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
        }

        m_Coroutine = MoveTo(m_Positions[m_PositionIndex]);
        StartCoroutine(m_Coroutine);
    }

    private IEnumerator MoveTo(Vector2 destination)
    {
        float elapsedTime = 0f;
        Vector2 startingPosition = m_RectTransform.anchoredPosition;

        while (Vector2.Distance(transform.localPosition, destination) > 0)
        {
            elapsedTime += Time.deltaTime;
            m_RectTransform.anchoredPosition = Vector2.Lerp(startingPosition, destination, Mathf.SmoothStep(0, 1, elapsedTime / m_TransitionTime));
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}

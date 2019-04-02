﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Image m_Image;
    public Text m_NameText;
    public Text m_DescriptionText;
    public Buff m_Buff;
    public bool m_Interactive;

    private GameObject m_DragIcon;
    private Canvas m_ParentCanvas;

    private void OnEnable()
    {
        m_ParentCanvas = GetComponentInParent<Canvas>();
    }

    public void Init(Buff buff, bool interactive = true)
    {
        m_Buff = buff;

        m_Image.sprite = m_Buff.m_Sprite;
        m_NameText.text = m_Buff.m_PrettyName;
        m_DescriptionText.text = m_Buff.m_Description;

        m_Interactive = interactive;
        GetComponent<Button>().interactable = interactive;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_Interactive)
        {
            m_DragIcon = new GameObject("Drag Icon");
            m_DragIcon.transform.SetParent(m_ParentCanvas.transform, false);
            m_DragIcon.transform.SetAsLastSibling();

            Image dragImage = m_DragIcon.AddComponent<Image>();
            dragImage.sprite = m_Buff.m_Sprite;
            dragImage.SetNativeSize();

            // Make drag icon transparent to raycasts so it doesn't block drop events
            CanvasGroup dragGroup = m_DragIcon.AddComponent<CanvasGroup>();
            dragGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_DragIcon != null)
        {
            SetDraggedPosition(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_DragIcon != null)
        {
            Destroy(m_DragIcon);
        }
    }

    private void SetDraggedPosition(PointerEventData eventData)
    {
        Vector3 mousePos;
        RectTransform iconTrans = m_DragIcon.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)m_ParentCanvas.transform, eventData.position, eventData.pressEventCamera, out mousePos))
        {
            iconTrans.position = mousePos;
        }
    }
}

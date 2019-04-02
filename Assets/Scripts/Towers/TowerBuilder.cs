using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerBuilder : MonoBehaviour {

    public static TowerBuilder Instance;

    public Sprite m_ErrorCursorSprite;
    public GameObject m_BuildCursor;

    public LayerMask m_TileMask;

    public Text m_ErrorText;

    public bool m_UsingMouse
    {
        get
        {
            return m_SelectedTowerPrefab != null;
        }
    }

    private GameObject m_SelectedTowerPrefab;

    private LineRenderer m_RangePreview;
    private SpriteRenderer m_CursorSprite;

    private bool m_MouseUp;
    private int m_FingerID = -1;

    private void Awake()
    {
        Instance = this;

        m_BuildCursor = Instantiate(m_BuildCursor); // Create a clone so we're not playing around with the prefab
        m_BuildCursor.SetActive(false);

        m_CursorSprite = m_BuildCursor.GetComponent<SpriteRenderer>();
        m_RangePreview = m_BuildCursor.GetComponent<LineRenderer>();
        m_RangePreview.positionCount = 50;

#if !UNITY_EDITOR
        m_FingerID = 0;
#endif
    }

    private void Update()
    {
        m_MouseUp = Input.GetMouseButtonUp(0);

        if (m_SelectedTowerPrefab != null && Input.GetKeyUp(KeyCode.Escape))
        {
            SetSelectedTowerPrefab(null);
        }
    }

    private void FixedUpdate()
    {
        if (m_SelectedTowerPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 20, m_TileMask);

            if (hit.collider != null && !EventSystem.current.IsPointerOverGameObject(m_FingerID))
            {
                ITile hitTile = hit.collider.GetComponent<ITile>();
                
                if (m_MouseUp)
                {
                    Tower tower = m_SelectedTowerPrefab.GetComponent<Tower>();

                    if (hitTile.IsBuildable() && GameManager.Instance.m_Money >= tower.m_Cost)
                    {
                        hitTile.Build(m_SelectedTowerPrefab);
                        GameManager.Instance.m_Money -= tower.m_Cost;
                        m_SelectedTowerPrefab = null;
                        m_BuildCursor.SetActive(false);
                    }
                    else
                    {
                        if (!hitTile.IsBuildable() && !hitTile.IsBlocked())
                        {
                            m_ErrorText.text = "You can't build on that tile!";
                        }
                        else if (!hitTile.IsBuildable() && hitTile.IsBlocked())
                        {
                            m_ErrorText.text = "That tile is blocked!";
                        }
                        else if (GameManager.Instance.m_Money < tower.m_Cost)
                        {
                            m_ErrorText.text = "You don't have enough money! (Need $" + (tower.m_Cost - GameManager.Instance.m_Money) + ")";
                        }

                        m_ErrorText.GetComponent<ColorFadeText>().StartFade();
                    }
                }
                else
                {
                    m_BuildCursor.SetActive(true);
                    Vector3 cursorPos = hit.collider.transform.position;
                    cursorPos.z -= 3;   // Move cursor closer to the camera than enemies and towers so it shows on top
                    m_BuildCursor.transform.position = cursorPos;

                    if (hitTile.IsBuildable())
                    {
                        // The tile is buildable, so show a preview of the tower and it's range
                        m_CursorSprite.sprite = m_SelectedTowerPrefab.GetComponent<SpriteRenderer>().sprite;

                        // Do stuff to display the tower's range with the line renderer
                        m_RangePreview.enabled = true;
                    }
                    else
                    {
                        // The tile is not buildable, so show the error sprite
                        m_RangePreview.enabled = false;
                        m_CursorSprite.sprite = m_ErrorCursorSprite;
                    }
                }
            }
            else
            {
                m_BuildCursor.SetActive(false);
            }
        }
    }

    public void SetSelectedTowerPrefab(GameObject towerPrefab)
    {
        if (towerPrefab != null)
        {
            m_RangePreview.SetPositions(towerPrefab.GetComponent<Tower>().GetRangePreviewPoints());
        }
        else
        {
            m_BuildCursor.SetActive(false);
        }
        m_SelectedTowerPrefab = towerPrefab;
    }
}

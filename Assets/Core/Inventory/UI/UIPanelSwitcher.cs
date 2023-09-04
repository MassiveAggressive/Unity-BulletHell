using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelSwitcher : MonoBehaviour
{
    [SerializeField] List<GameObject> panels = new List<GameObject>();

    [SerializeField] GameObject currentPanel;
    public GameObject CurrentPanel { get { return currentPanel; } set { currentPanel = value; SetCurrentPanel(currentPanel); } } 

    [SerializeField] int currentPanelIndex = 0;
    public int CurrentPanelIndex { get { return currentPanelIndex; } set { currentPanelIndex = value; SetCurrentPanel(currentPanelIndex); } }

    private void OnTransformChildrenChanged()
    {
        panels.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject currentChild = transform.GetChild(i).gameObject;
            panels.Add(currentChild);
            currentChild.SetActive(false);
        }

        if(currentPanel)
        {
            currentPanelIndex = panels.IndexOf(currentPanel);
            currentPanel.SetActive(true);
        }
        else
        {
            currentPanel = panels[currentPanelIndex];
            currentPanel.SetActive(true);
        }
    }

    private void Awake()
    {
        if(transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject currentChild = transform.GetChild(i).gameObject;
                panels.Add(currentChild);
                currentChild.SetActive(false);
            }
            panels[currentPanelIndex].SetActive(true);
        }
    }

    public void SetCurrentPanel(int panelIndex)
    {
        if(panelIndex < panels.Count) 
        {
            currentPanelIndex = panelIndex;

            foreach(GameObject panel in panels) 
            {
                panel.SetActive(false);
            }

            currentPanel = panels[currentPanelIndex];
            currentPanel.SetActive(true);
        }
    }

    public void SetCurrentPanel(GameObject currentPanel)
    {
        if(panels.Contains(currentPanel))
        {
            currentPanelIndex = panels.IndexOf(currentPanel);

            foreach (GameObject panel in panels)
            {
                panel.SetActive(false);
            }

            this.currentPanel = panels[currentPanelIndex];
            this.currentPanel.SetActive(true);
        }
    }
}

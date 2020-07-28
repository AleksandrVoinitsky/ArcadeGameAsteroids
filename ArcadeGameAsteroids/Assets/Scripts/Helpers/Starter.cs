using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEditor;

public class Starter : MonoBehaviour
{
    public List<ManagerBase> managers = new List<ManagerBase>();

    private GameObject m_setup;

    void Awake()
    {
        Started();
    }

    private void Started()
    {
        m_setup = GameObject.Find("[Sys_Core]") ?? new GameObject("[Sys_Core]");
        if (m_setup.GetComponent<CoreTools>() == null) m_setup.AddComponent<CoreTools>();

        foreach (var managerBase in managers)
        {
            CoreTools.Add(managerBase);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    private bool _isActivated = false;
    void SpawnObject(GameObject go)
    {
        if (_isActivated) return;
        _isActivated = true;
        Debug.Log(1);
        Instantiate(go, transform.position, Quaternion.identity);
    }
}

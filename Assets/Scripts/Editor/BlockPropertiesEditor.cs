using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlockProperties))]
public class BlockPropertiesEditor : Editor
{
    private BlockProperties _blockProperties;
    private void OnEnable()
    {
        _blockProperties = target as BlockProperties;
        _blockProperties.ChangeOutLineColor(ColorTagUtilities.ColorTag2Color(_blockProperties.BlockColorTag));
    }
    

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUI.changed)
        {
            _blockProperties.ChangeOutLineColor(ColorTagUtilities.ColorTag2Color(_blockProperties.BlockColorTag));
        }
    }
    
}

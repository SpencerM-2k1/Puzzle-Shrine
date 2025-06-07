using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Location", order = 1)]
public class LocationSO : ScriptableObject
{
    [SerializeField] string _id;
    public string id {
        get { return _id; }
        private set { _id = value; }
    }

    [SerializeField] string _displayName;
    public string displayName {
        get { return _displayName; }
        private set { _displayName = value; }
    }

    [SerializeField] Color _textColor = new Color(255, 255, 255);
    public Color textColor {
        get { return _textColor; }
        private set { _textColor = value; }
    }

    [TextArea]
    [SerializeField] string _instructions;
    public string instructions {
        get { return _instructions; }
        private set { _instructions = value; }
    }
}

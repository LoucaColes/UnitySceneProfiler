using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfigTypes {
    [System.Serializable]
	public struct IntConfig
    {
        public int warningLimit;
        public string warningMessage;
        public int errorLimit;
        public string errorMessage;
    }
}

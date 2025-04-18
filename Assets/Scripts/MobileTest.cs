using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobileTest : MonoBehaviour
{
    #if !UNITY_EDITOR && UNITY_WEBGL
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool IsMobile();
    #endif
    
    [SerializeField] private TextMeshProUGUI textMesh;
    
    // Update is called once per frame
    void Update()
    {
        CheckIfMobile();
    }

    private void CheckIfMobile()
    {
        var isMobile = true;

        #if !UNITY_EDITOR && UNITY_WEBGL
                isMobile = IsMobile();
        #endif

        textMesh.text = isMobile ? "Mobile" : "PC";
    }
}

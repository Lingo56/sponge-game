using UnityEngine;
using TMPro;

public class BrowserDetector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI browserInfoText;
    [SerializeField] private TextMeshProUGUI mouseStateText;
    
#if !UNITY_EDITOR && UNITY_WEBGL
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool IsSafari();
#endif
    
    void Update()
    {
        // Update browser info
        bool isSafari = false;
        
#if !UNITY_EDITOR && UNITY_WEBGL
            isSafari = IsSafari();
#endif
        
        if (browserInfoText != null)
        {
            browserInfoText.text = $"Browser: {(isSafari ? "Safari" : "Not Safari")}";
        }
        
        // Display mouse state
        if (mouseStateText != null && Input.GetMouseButton(0))
        {
            mouseStateText.text = "Mouse Down";
        }
        else if (mouseStateText != null)
        {
            mouseStateText.text = "Mouse Up";
        }
    }
}
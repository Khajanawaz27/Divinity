using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenAd : MonoBehaviour
{
    public Image bannerImage;
    public GameObject fullscreenPopup;
    
    
    public void ClosePopUp()
    {
        Destroy(fullscreenPopup);
    }
}

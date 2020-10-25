using UnityEngine;
using UnityEngine.UI;

public class RetainerBox : MonoBehaviour
{
  public RetainerBoxCapture targetCapture;
  public RawImage img;
  public bool autoEnable;
  public GameObject objToDisable;

  public void Awake()
  {
    if (autoEnable)
    {
      objToDisable.SetActive(false);
      EnableImage();
    }
  }

  void Update()
  {
    if (img != null && targetCapture != null)
    {
      img.texture = targetCapture.rt;
    }
  }

  public void EnableImage()
  {
    if (img != null && targetCapture != null)
    {
      img.texture = targetCapture.rt;
      img.gameObject.SetActive(true);
    }
  }

  public void DisableImage()
  {
    if (img != null && targetCapture != null)
    {
      img.gameObject.SetActive(false);
    }
  }
}

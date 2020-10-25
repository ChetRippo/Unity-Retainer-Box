using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class RetainerBoxCapture : MonoBehaviour
{
  public Canvas TargetCanvas;
  [HideInInspector]
  public RenderTexture rt;

  // assumes canvases all have a consistent reference resolution that is 1920 x 1080.
  // Additional aspect ratios beyond 16:9, 16:10, 3:2, and 21:9 will need to be added here
  public static Vector2Int TargetRes;
  public Vector2Int GetCanvasTargetRes()
  {
    var ReferenceRes = TargetCanvas.GetComponent<CanvasScaler>().referenceResolution;
    if (TargetRes == null || TargetRes == Vector2Int.zero)
    {
      TargetRes = new Vector2Int(Mathf.FloorToInt(ReferenceRes.x), Mathf.FloorToInt(ReferenceRes.y));
    }
    TargetRes.x = Mathf.FloorToInt(ReferenceRes.x);
    TargetRes.y = Mathf.FloorToInt(ReferenceRes.y);
    // 16:10
    if (Mathf.Approximately(Camera.main.aspect, 1.6f))
    {
      TargetRes.y += 120;
    }
    // 3:2
    else if (Mathf.Approximately(Camera.main.aspect, 1.5f))
    {
      TargetRes.y += 200;
    }
    // 21:9
    else if (Mathf.Approximately(Camera.main.aspect, 2.3f))
    {
      TargetRes.x += 640;
    }
    return TargetRes;
  }

  public void Start()
  {
    var Target = TargetCanvas.transform.GetChild(0).GetComponent<RectTransform>();
    var Size = RectTransformUtility.PixelAdjustRect(Target, GetComponent<Canvas>());
    var EffectiveSize = new Vector2(Size.width, Size.height) * (new Vector2(Screen.width, Screen.height) / GetCanvasTargetRes());
    var Tex = new RenderTexture(Mathf.FloorToInt(EffectiveSize.x), Mathf.FloorToInt(EffectiveSize.y), 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
    Tex.Create();
    Tex.filterMode = FilterMode.Point;
    GetComponent<Camera>().targetTexture = Tex;
    this.rt = Tex;
    StartCoroutine(TurnOff());
  }

  IEnumerator TurnOff()
  {
    // for some reason it needs to wait two frames for the texture to be rendered
    yield return 0;
    yield return 0;
    GetComponent<Camera>().enabled = false;
    TargetCanvas.gameObject.SetActive(false);
  }

  public IEnumerator Rerender()
  {
    yield return 0;
    if (TargetCanvas.transform.childCount == 0) yield return 0f;
    GetComponent<Camera>().enabled = true;
    TargetCanvas.gameObject.SetActive(true);
    var Target = TargetCanvas.transform.GetChild(0).GetComponent<RectTransform>();
    var Size = RectTransformUtility.PixelAdjustRect(Target, GetComponent<Canvas>());
    var EffectiveSize = new Vector2(Size.width, Size.height) * (new Vector2(Screen.width, Screen.height) / GetCanvasTargetRes());
    this.rt = new RenderTexture(Mathf.FloorToInt(EffectiveSize.x), Mathf.FloorToInt(EffectiveSize.y), 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
    rt.Create();
    rt.filterMode = FilterMode.Point;
    GetComponent<Camera>().targetTexture = this.rt;

    StartCoroutine(TurnOff());
  }
}

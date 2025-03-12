using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoading : MonoBehaviour
{
    public static SceneLoading Instance;
    [SerializeField] private GameObject _LoaderPanel;
    [SerializeField] private Image _LoadFillBar;
    [SerializeField] private int hubIndex;
    [SerializeField] TMP_Text progressUpdate;

    [Header("Loading Timer")]
    private string percentage;
    public float loadingTimer;
    private float fillAmountOverTime;

    private void Awake()
    {
        _LoadFillBar.fillAmount = 0f;
        StartCoroutine(LoadAsynchronously());
       
        
    }
    private void Start() {
         Fades.instantOfFades.OnFadeIn();
    }

    private void Update()
    {
         fillAmountOverTime = 1 / loadingTimer;

        if (_LoadFillBar.fillAmount < 1.0f) //cap loadingbar to 100%
        {
            loadingTimer -= Time.deltaTime; // timer to count down
            _LoadFillBar.fillAmount = fillAmountOverTime; // update loading bar
            progressUpdate.text = Mathf.Round(fillAmountOverTime * 100).ToString() + "%"; // update text
        }

    }

    IEnumerator LoadAsynchronously()
    {
        yield return new WaitForSeconds(.1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SwitchScene.Instance.sceneIndex);

        yield return null;
    }


}

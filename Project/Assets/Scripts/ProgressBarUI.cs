using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] Image progressBar;
    [SerializeField] GameObject hasProgressGameObject;


    void Start()
    {
        IHasProgress hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        if (hasProgress == null)
        {
            Debug.LogError("Object" + hasProgressGameObject + "should have IHasProgress component");
        }

        hasProgress.OnProgressChanged += CuttingCounter_OnProgressChanged;
        progressBar.fillAmount = 0;
        Hide();
    }

    private void CuttingCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        progressBar.fillAmount = e.progressNormalized;

        if (progressBar.fillAmount == 0 || progressBar.fillAmount == 1)
            Hide();
        else
            Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}

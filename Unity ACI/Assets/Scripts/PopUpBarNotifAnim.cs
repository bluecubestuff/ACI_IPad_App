using UnityEngine;
using System.Collections;

public class PopUpBarNotifAnim : MonoBehaviour
{
    public Animator popbarAnim;

    [SerializeField]
    private GameObject Background;

    private void OnEnable()
    {
        Background.SetActive(true);
        popbarAnim.SetTrigger("Show");
    }

    public IEnumerator RunPopCloseAnimProcess()
    {
        Background.SetActive(false);
        yield return new WaitForSeconds(popbarAnim.GetCurrentAnimatorStateInfo(0).length);

        //When Animation is ended in "Hide", set gameobject to false
        gameObject.SetActive(false);
    }

    public void DismissPopup()
    {
        if (!gameObject.activeSelf)
            return;

        popbarAnim.SetTrigger("Hide");
        StartCoroutine(RunPopCloseAnimProcess());
    }
}

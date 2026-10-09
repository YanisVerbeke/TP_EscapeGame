using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    private TextMeshProUGUI _hintTxt;
    private TextMeshProUGUI _hintTitleTxt;
    [SerializeField] private string _hintTitle;
    [SerializeField] private string _hint;

    private void Awake()
    {
        _hintTitleTxt = transform.Find("Canvas").Find("hintTitle").GetComponent<TextMeshProUGUI>();
        _hintTitleTxt.text = _hintTitle;
        _hintTxt = transform.Find("Canvas").Find("hintTxt").GetComponent<TextMeshProUGUI>();
        _hintTxt.text = _hint;
        _hintTxt.gameObject.SetActive(false);
    }

    public void DisplayHint()
    {
        _hintTxt.gameObject.SetActive(true);
    }



}

using TMPro;
using UnityEngine;

public class HeadTarget : MonoBehaviour
{
    public int LivesCount = 3;
    public string TargetTag = "Fish";
    public Canvas GameOverUI;

    [SerializeField]
    private TMP_Text _healthUi;

    private bool _gameOver = false;
    void Start()
    {
        _healthUi.text = LivesCount.ToString();
    }

    void Update()
    {
        if (_gameOver)
        {
            GameOverUI.gameObject.SetActive(true);
            _healthUi.gameObject.SetActive(false);
        }
        else
        {
            _healthUi.text = LivesCount.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TargetTag)
        {
            Destroy(other.gameObject);
            if (LivesCount > 0)
                LivesCount--;
            else
                _gameOver = true;
        }
    }
}
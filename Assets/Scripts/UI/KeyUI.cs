using TMPro;
using UnityEngine;

public class KeyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textOfKeys;
    
    [HideInInspector] public int keysAmount;

    private void Start()
    {
        keysAmount = 0;

        Player.TakeKey += TakeKey;
    }

    private void Update()
    {
        textOfKeys.text = keysAmount.ToString();
    }
    private void TakeKey()
    {
        keysAmount++;
    }

    private void OnDisable()
    {
        Player.TakeKey -= TakeKey;
    }
}

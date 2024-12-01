using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    int secrets;
    public int SecretQuant { get { return secrets; } }

    public List<string> collectedSecrets = new();

    private void Start()
    {
        secrets = GameController.instance.intelCount;
    }

    public void AddSecret(string secretMessage, GameObject secretObject)
    {
        collectedSecrets.Add(secretMessage);
        Debug.Log(secretMessage);
        UIHandler.instance.UpdateInventoryListLabel(secretMessage);
        StartCoroutine(UIHandler.instance.FlashNewSecretBanner());

        if (secretObject.CompareTag("Intel"))
        {
            secrets += 1;
            GameController.instance.intelCount += 1;
            UIHandler.instance.UpdateIntelCounter();
            Debug.Log("intel found");
        }
    }

}

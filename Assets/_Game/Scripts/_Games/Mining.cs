using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mining : MonoBehaviour
{
    public Rock rock;
    public PlayerController playerController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rock")
        {
            if (Root.GameManager.currentLevel == 0)
                Root.GameManager.EndTaksTutorial(8);

            if (Root.GameManager.CurrentStorage >= Root.GameManager.MaxSacStorage)
            {
                playerController.ShakeMaxUi();
                return;
            }

            rock = other.GetComponentInParent<Rock>();
            if (rock.levelNeeded <= Root.GameManager.PlayerLevel)
            {
                if (!rock.isDestroyed)
                {
                    playerController.currentRock = rock;
                    playerController.Attack(true);
                    print("hit");
                }
            }
            else
            {
                rock.ShowLevel();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Rock")
        {
            playerController.currentRock = null;
            playerController.Attack(false);
        }
    }
}

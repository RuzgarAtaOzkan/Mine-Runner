using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MonoBehaviour
{
    public IEnumerator FlashObstacle(Collision collision, Material flashMat, float flashTime)
    {
        Debug.Log(" flash corotuine");
        int counter = 0;
        int switcher = 1;
        bool isFlashing = true;
        while (isFlashing)
        {
            Material[] savedMaterials = collision.gameObject.GetComponent<MeshRenderer>().materials;
            Material[] flashMaterialsToPlace = new Material[savedMaterials.Length];
            for (int i = 0; i < flashMaterialsToPlace.Length; i++)
            {
                flashMaterialsToPlace[i] = flashMat;
            }

            switch (switcher)
            {
                case -1:
                    collision.gameObject.GetComponent<MeshRenderer>().materials = savedMaterials;
                    break;
                case 1:
                    collision.gameObject.GetComponent<MeshRenderer>().materials = flashMaterialsToPlace;
                    break;
            }

            counter++;
            switcher *= -1;

            if (counter < 40) { isFlashing = false; }
            yield return new WaitForSeconds(flashTime);
        }
    }
}

using UnityEngine;
using System.Collections;

public class DrinkBleach : MonoBehaviour
{

    private float bleachLevels;

	void Update ()
	{
	    bleachLevels += Time.deltaTime;
	    if (bleachLevels > 7)
	    {
	        Destroy(gameObject);
	    }
	}
}

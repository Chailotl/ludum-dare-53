using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Indicator : MonoBehaviour
{
	[SerializeField]
	private Transform target;
	[SerializeField]
	private float border = 50f;
    [SerializeField]
    private Sprite boxSprite;
    [SerializeField]
    private Sprite arrowSprite;

    private Image image;

    void Start()
	{
        image = GetComponent<Image>();
	}

	void Update()
	{
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
        bool isTargetVisible = screenPos.z > 0 && screenPos.x > 0 && screenPos.y > 0 && screenPos.x < Screen.width && screenPos.y < Screen.height;
        Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        Vector3 screenBounds = new Vector3(screenCenter.x - border, screenCenter.y - border, 0);

        if (isTargetVisible)
        {
            transform.position = screenPos;
            transform.rotation = Quaternion.identity;

            image.sprite = boxSprite;
        }
        else
        {
            screenPos -= screenCenter;

            if (screenPos.z < 0) { screenPos *= -1; }

            float angle = Mathf.Atan2(screenPos.y, screenPos.x);
            float slope = Mathf.Tan(angle);

            if (screenPos.x > 0)
            {
                screenPos = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
            }
            else
            {
                screenPos = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);
            }

            if (screenPos.y > screenBounds.y)
            {
                screenPos = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
            }
            else if (screenPos.y < -screenBounds.y)
            {
                screenPos = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
            }

            screenPos += screenCenter;

            transform.position = screenPos;
            transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);

            image.sprite = arrowSprite;
        }
    }
}
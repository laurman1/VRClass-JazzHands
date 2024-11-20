using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverRotate : MonoBehaviour
{
    public Renderer objectRenderer;
    public float fadeDutation;

    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(1f, 0f, deactivateAfter: true));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1f, 0f, deactivateAfter: true));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, bool deactivateAfter = false)
    {
        float elapsed = 0f;
        Color color = objectRenderer.material.color;

        while (elapsed < fadeDutation)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDutation);
            color.a = alpha;
            objectRenderer.material.color = color;
            yield return null;
        }

        color.a = endAlpha;
        objectRenderer.material.color = color;

        /*if (deactivateAfter)
            gameObject.SetActive(false);*/
        
            

    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}

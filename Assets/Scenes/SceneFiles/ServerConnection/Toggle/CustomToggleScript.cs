using UnityEngine;

public class CustomToggleScript : MonoBehaviour
{
    private Animator anim;
    private bool isOn = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Toggle()
    {
        isOn = !isOn;
        if (isOn)
        {
            anim.Play(0, 0, 0f);
            anim.speed = 1f;
        }
        else
        {
            anim.Play(0, 0, 1f);
            anim.speed = -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

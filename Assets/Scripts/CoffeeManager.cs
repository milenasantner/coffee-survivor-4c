using UnityEngine;
using UnityEngine.UI;

public class CoffeeManager : MonoBehaviour
{
    public static CoffeeManager Instance;
    
    [SerializeField] private GameObject coffeeSlider;
    [SerializeField] private float drainRate = 5f;
    private float coffeeLevel = 100f;
    private Vector3 originalScale;
    
    void Awake()
    {
        Instance = this;
        if (coffeeSlider != null)
        {
            originalScale = coffeeSlider.transform.localScale;
        }
        else
        {
            originalScale = Vector3.one;
        }
    }
    
    void Update()
    {
        coffeeLevel -= drainRate * Time.deltaTime;
        //Change total height of go slider according to coffee level
        if (coffeeSlider != null)
        {
            float pct = coffeeLevel / 100f;
            coffeeSlider.transform.localScale = new Vector3(originalScale.x * pct, originalScale.y, originalScale.z);
        }
        
        if (coffeeLevel <= 0)
        {
            Debug.Log("TODO: Hier macht PERSON 2 weiter - Game Over auslösen");
        }
    }
    
    public void RefillCoffee()
    {
        coffeeLevel = 100f;
        Debug.Log("Kaffee aufgefüllt!");
    }
}
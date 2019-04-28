using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ProtoTurtle.BitmapDrawing;

public class ScoutFlight : MonoBehaviour
{
    public float kniplSensitivity = 30;
    public float planeSpeed = 5;
    public float fuelMax = 100;
    public float fuelCoeficient = 1;

    public Slider fuelSlider;
    public RawImage fogOfWar;
    public Transform lefttop;
    public Transform rightbot;
    public Texture2D texture;
    Texture2D txt;

    private Vector2 lastPosition;
    private bool hasLastPosition = false;
    private float fuelRemaining;

    private int resX, resY;

    private void Start()
    {
        fuelRemaining = fuelMax;
        fuelSlider.maxValue = fuelMax;
        resX = fogOfWar.texture.width;
        resY = fogOfWar.texture.height;

        txt = (Texture2D)Instantiate(fogOfWar.texture);
    }

    private void Update()
    {
        Flight();

        UncoverMap();
    }

    void Flight()
    {
        if (!hasLastPosition)
        {
            lastPosition = this.transform.position;
            hasLastPosition = true;
        }
        else
        {
            float deltaDistance = Vector2.Distance(lastPosition, this.transform.position);
            fuelRemaining -= deltaDistance * fuelCoeficient;
            fuelSlider.value = fuelRemaining;

            if (fuelRemaining <= 0)
            {
                SceneManager.LoadScene("Game Over");
            }
        }

        float deltaAngle = Input.GetAxis("Horizontal");
        deltaAngle *= Time.deltaTime;
        deltaAngle *= kniplSensitivity;

        lastPosition = this.transform.position;

        this.transform.Rotate(new Vector3(0, 0, deltaAngle));
        this.transform.Translate(Vector2.up * Time.deltaTime * planeSpeed);
    }

    void Landing()
    {
        SceneManager.LoadScene("Officer");
    }

    void UncoverMap()
    {
        int circleX = 0;
        int circleY = 0;

        Vector2 plane = this.transform.position;
        Vector2 start = lefttop.position;
        Vector2 end = rightbot.position;

        float percentX = (plane.x - start.x) /(end.x - start.x);
        float percentY = (plane.y - start.y) /(end.y - start.y);

        circleX = (int)(resX * percentX);
        circleY = (int)(resY * percentY);

        //txt = (Texture2D)fogOfWar.texture;
        txt.DrawFilledCircle(circleX,circleY, 50, new Color(0,0,0,0));
        txt.Apply();
        fogOfWar.texture = txt;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Airport")
        {
            Landing();
        }
    }
}

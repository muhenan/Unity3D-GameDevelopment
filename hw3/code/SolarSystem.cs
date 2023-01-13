using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public Transform mercury;
    public Transform venus;
    public Transform earth;
    public Transform mars;
    public Transform jupiter;
    public Transform saturn;
    public Transform uranus;
    public Transform neptune;
    void Start()
    {
        mercury.position = new Vector3(3, 0.5f, 0);
        venus.position = new Vector3(-5, 1, 0);
        earth.position = new Vector3(7, -0.5f, 0);
        mars.position = new Vector3(-9, 0, 0);
        jupiter.position = new Vector3(-11, -1, 0);
        saturn.position = new Vector3(13, 0.5f, 0);
        uranus.position = new Vector3(15, 1.5f, 0);
        neptune.position = new Vector3(-17, 0, 0);
    }

    void Update()
    {
        earth.RotateAround(this.transform.position, new Vector3(0, 0.99f, 0), 30 * Time.deltaTime);
        mercury.RotateAround(this.transform.position, new Vector3(0, 2.11f, 0), 47 * Time.deltaTime);
        venus.RotateAround(this.transform.position, new Vector3(0, 3.23f, 0), 35 * Time.deltaTime);
        mars.RotateAround(this.transform.position, new Vector3(0, 4.34f, 0), 24 * Time.deltaTime);
        jupiter.RotateAround(this.transform.position, new Vector3(0, 1.02f, 0), 13 * Time.deltaTime);
        saturn.RotateAround(this.transform.position, new Vector3(0, 0.98f, 0), 9 * Time.deltaTime);
        uranus.RotateAround(this.transform.position, new Vector3(0, 0.97f, 0), 6 * Time.deltaTime);
        neptune.RotateAround(this.transform.position, new Vector3(0, 0.96f, 0), 5 * Time.deltaTime);
        earth.Rotate(Vector3.up * Time.deltaTime * 250);
        mercury.Rotate(Vector3.up * Time.deltaTime * 300);
        venus.Rotate(Vector3.up * Time.deltaTime * 280);
        mars.Rotate(Vector3.up * Time.deltaTime * 220);
        jupiter.Rotate(Vector3.up * Time.deltaTime * 180);
        saturn.Rotate(Vector3.up * Time.deltaTime * 160);
        uranus.Rotate(Vector3.up * Time.deltaTime * 150);
        neptune.Rotate(Vector3.up * Time.deltaTime * 140);
    }

}

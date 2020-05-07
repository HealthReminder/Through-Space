using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [Header("Cloud")]
    public AnimationCurve cloud_movement_curve;
    public Image[] moving_background;
    public Vector2[] initial_positions;

    [Header("Title")]
    public AnimationCurve title_movement_curve;
    public AnimationCurve title_size_curve;
    public RectTransform title_transform;

    [Header("Map")]
    public Transform map_transform;

   

    private void Start() {
        initial_positions = new Vector2[moving_background.Length];

        for (int i = 0; i < moving_background.Length; i++)
            initial_positions[i] = moving_background[i].rectTransform.localPosition;
        
        StartCoroutine(CloudRoutine());
        StartCoroutine(TitleRoutine());
    }

    float current_speed = 3f;
    float skip_delay = 0;
    private void Update()
    {
        //SKIP INTRO
        if (skip_delay >= 1)
        {
            if (Input.anyKeyDown || Input.GetMouseButton(0))
                current_speed = 10;
        }
        else
            skip_delay += Time.deltaTime;
    }

    float target_map_y = 0;
    IEnumerator MoveMapUp()
    {
        //This function is called by the TitleRoutine, after title is shown
        float difference = target_map_y - map_transform.localPosition.y;
        Debug.Log(difference);
        Debug.Log(map_transform.localPosition);
        while (difference > 0)
        {
            if (map_transform.localPosition.y + Time.deltaTime * current_speed * 150 >= target_map_y)
            {
                map_transform.localPosition = new Vector3(0, target_map_y, 0);
                yield break;
            }
            map_transform.localPosition += new Vector3(0, Time.deltaTime * current_speed * 150, 0);
            difference -= Time.deltaTime*current_speed * 100;
            yield return null;
        }
        yield break;
    }
    
    IEnumerator BackClouds ()
    {
        //This function is called by the TitleRoutine, after title is shown
        for (int o = 0; o < 50; o++)
        {
            float l_cache = moving_background.Length;
            for (int i = 0; i < l_cache; i++)
            {
                initial_positions[i] = initial_positions[i] - new Vector2(0, (l_cache - i));
            }
            yield return null;
        }
        yield break;
    }

    IEnumerator TitleRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        SoundtrackManager.instance.ChangeSet("Intro");

        float progress = 0;
        float y_amount = 1500;
        Vector3 initial_pos = title_transform.localPosition;
        //bool has_clouds = true;
        title_transform.localPosition = initial_pos + new Vector3(0, title_movement_curve.Evaluate(0) * y_amount, 0);
        float s = title_size_curve.Evaluate(0);
        title_transform.gameObject.SetActive(true);
        while (progress <= 1)
        {
           // if (has_clouds)
               // if (progress >= 0.7)
                //{
                //    has_clouds = false;
                //    StartCoroutine(BackClouds());
                //}

            title_transform.localPosition = initial_pos + new Vector3(0, title_movement_curve.Evaluate(progress)*y_amount,0);
            s = title_size_curve.Evaluate(progress);
            title_transform.localScale = new Vector3(s,s,s);
            progress += Time.deltaTime * current_speed / 8;
            yield return new WaitForSeconds(Time.deltaTime * 2);
        }
        AudioManager.instance.Play("Intro_Off");
        yield return new WaitForSeconds(0.5f);
        SoundtrackManager.instance.ChangeSet("Menu");
        StartCoroutine(MoveMapUp());
        StartCoroutine(BackClouds());
        yield break;
    }

    IEnumerator CloudRoutine () {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(Random.Range(0, 0.5f));

        float progress = 0;
        float movement_speed = 3f;
        float l_cache = moving_background.Length;

        //Parameters used by each iteration
        float movement_modifier = 1f;
        float offset = 0;
        float curve_progress = 0;
        while (true) {
            for (int i = 0; i < l_cache; i++)
            {
                offset = (float)(i/l_cache);
                curve_progress = progress + offset;
                while (curve_progress > 1)
                    curve_progress -= 1;
                //How strong is the movement. Should be less apparent on the objects in the back. Consider the hierarchy
                movement_modifier = 1 + i;
                moving_background[i].rectTransform.localPosition = initial_positions[i ] - new Vector2 (0,cloud_movement_curve.Evaluate(curve_progress) * movement_modifier);
            }
            progress += Time.deltaTime * movement_speed;
            if(progress > 1) 
                progress = 0;
            yield return new WaitForSeconds(Time.deltaTime*3);
        }

        yield break;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackPlayer : MonoBehaviour
{
    List<Feedback> feedbacks = new List<Feedback>();

    private void Start()
    {
        feedbacks.AddRange(GetComponentsInChildren<Feedback>());
        foreach (var feedback in feedbacks)
        {
            feedback.Initialize();
        }
    }

    public void Play(Vector3? position)
    {
        if (position == null)
        {
            position = transform.position;
        }

        foreach (var feedback in feedbacks)
        {
            feedback.Play(position.Value);
        }
    }

}
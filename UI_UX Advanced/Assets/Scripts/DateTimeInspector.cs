using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Reservation))]
public class DateTimeInspector : Editor
{
    bool runOnce = false;
    DateTime now;
    DateTime end;

    int year0, month0, day0, hour0, minute0;
    int year1, month1, day1, hour1, minute1;

    void OnEnable()
    {
        runOnce = false;
        Debug.Log("REAL ENABLE");
    }

    public override void OnInspectorGUI()
    {
        if(!runOnce)
        {
            Debug.Log("Enabled");
            now = DateTime.Now;
            end = Reservation.GetDateTimeAfterTime(now, new DateTime(2020, 1, 1, 0, 30, 0));

            year0 = now.Year;
            month0 = now.Month;
            day0 = now.Day;
            hour0 = now.Hour;
            day0 = now.Day;

            year1 = end.Year;
            month1 = end.Month;
            day1 = end.Day;
            hour1 = end.Hour;
            day1 = end.Day;
        }

        Reservation reservation = (Reservation)target;
        reservation.madeBy = EditorGUILayout.TextField(reservation.madeBy);

        GUILayout.Label("Start Time and Date");
        year0 = EditorGUILayout.IntField("Year", year0);
        month0 = EditorGUILayout.IntSlider("Month", month0, 1, 12);
        day0 = EditorGUILayout.IntSlider("Day", day0, 1, 31);
        hour0 = EditorGUILayout.IntSlider("Hour", hour0, 0, 24);
        minute0 = EditorGUILayout.IntSlider("Minute", minute0, 0, 60);


        GUILayout.Label("End Time and Date");
        year1 = EditorGUILayout.IntField("Year", year1);
        month1 = EditorGUILayout.IntSlider("Month", month1, 1, 12);
        day1 = EditorGUILayout.IntSlider("Day", day1, 1, 31);
        hour1 = EditorGUILayout.IntSlider("Hour", hour1, 0, 24);
        minute1 = EditorGUILayout.IntSlider("Minute", minute1, 0, 60);

        reservation.startTime = new DateTime(year0, month0, day0, hour0, minute0, 0);
        reservation.endTime = new DateTime(year1, month1, day1, hour1, minute1, 1);
        runOnce = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalPointsMove))]
public class LocalPointsMoveEditor : Editor
{
    LocalPointsMove creator;

    const float segmentSelectDistanceThreshold = .1f;
    int selectedSegmentIndex = -1;
    float anchorDiameter = 0.5f;

    public override void OnInspectorGUI()
    {
        GUILayout.Box("MouseDown + shift to create point \n MouseDown + control to destroy point");
        base.OnInspectorGUI();
    }

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;

        Ray ray = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        Vector3 mousePos = ray.origin;


        // create a plane at 0,0,0 whose normal points to +Y:
        Plane hPlane = new Plane(Vector3.up, new Vector3(0, creator.transform.position.y, 0));
        // if the ray hits the plane...
        if (hPlane.Raycast(ray, out float distance))
        {
            // get the hit point:
            Vector3 location = ray.GetPoint(distance);
            mousePos = location;
        }


        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            if (selectedSegmentIndex != -1)
            {
                Undo.RecordObject(creator, "Split segment");
                creator.SplitSegment(mousePos, selectedSegmentIndex);
            }
            else
            {
                Undo.RecordObject(creator, "Add segment");
                creator.AddSegment(mousePos);
                HandleUtility.Repaint();
            }
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.control)
        {
            float minDstToAnchor = anchorDiameter;
            int closestAnchorIndex = -1;

            for (int i = 0; i < creator.numPoints; i++)
            {
                float dst = Vector2.Distance(mousePos, creator[i]);
                if (dst < minDstToAnchor)
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }
            }

            if (closestAnchorIndex != -1)
            {
                Undo.RecordObject(creator, "Delete segment");
                creator.DeleteSegment(closestAnchorIndex);
                HandleUtility.Repaint();
            }
        }

        if (guiEvent.type == EventType.MouseMove)
        {
            float minDstToSegment = segmentSelectDistanceThreshold;
            int newSelectedSegmentIndex = -1;

            for (int i = 0; i < creator.numPoints; i++)
            {
                float dst = Vector3.Distance(mousePos, creator[i]);
                if (dst < minDstToSegment)
                {
                    minDstToSegment = dst;
                    newSelectedSegmentIndex = i;
                }
            }

            if (newSelectedSegmentIndex != selectedSegmentIndex)
            {
                selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }
        }

        HandleUtility.AddDefaultControl(0);
    }

    void Draw()
    {
        for (int i = 0; i < creator.numPoints - 1; i++)
        {
            Handles.color = (i == selectedSegmentIndex && Event.current.shift) ? Color.green : Color.white;
            Handles.DrawLine(creator[i], creator[i + 1]);
        }

        if (creator.isClosed)
            Handles.DrawLine(creator[creator.numPoints - 1], creator[0]);

        for (int i = 0; i < creator.numPoints; i++)
        {
            Handles.color = Color.red;
            float handleSize = 0.8f;
            Vector3 newPos = Handles.FreeMoveHandle(creator[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
            if (creator[i] != newPos)
            {
                creator.MovePoint(i, newPos);
            }
        }
    }


    void OnEnable()
    {
        creator = (LocalPointsMove)target;
    }
}

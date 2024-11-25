using UnityEditor;
using UnityEngine;

namespace PunkPlatformerGame
{

    [CustomEditor(typeof(GameCamera))]
    public class CameraFrustumIntersectionEditor : Editor
    {
        private void OnEnable()
        {
            Debug.Log("CameraFrustumIntersectionEditor enabled!");
        }

        private void OnSceneGUI()
        {
            GameCamera gameCamera = (GameCamera)target;
            Camera camera = gameCamera.GetComponent<Camera>();
            Debug.Log(camera.name);
            // Ensure we're working with a perspective camera
            if (camera == null || camera.orthographic)
                return;

            // Get the frustum corners at near and far planes
            Vector3[] nearCorners = new Vector3[4];
            Vector3[] farCorners = new Vector3[4];

            Transform camTransform = camera.transform;
            GetFrustumCorners(camera, camera.nearClipPlane, nearCorners, camTransform);
            GetFrustumCorners(camera, camera.farClipPlane, farCorners, camTransform);

            // Compute the intersection points with the XY plane
            Vector3[] intersectionPoints = new Vector3[4];
            int validPoints = 0;

            for (int i = 0; i < 4; i++)
            {
                if (RayIntersectsXYPlane(nearCorners[i], farCorners[i], out Vector3 intersection))
                {
                    intersectionPoints[validPoints] = intersection;
                    validPoints++;
                }
            }

            // Draw the intersection if valid
            if (validPoints >= 3)
            {
                Handles.color = Color.green;
                Handles.DrawAAPolyLine(3, intersectionPoints);
                Handles.DrawAAPolyLine(3, intersectionPoints[validPoints - 1], intersectionPoints[0]); // Close the loop
                // WidthLabel
                Vector3 width = intersectionPoints[0] - intersectionPoints[1];
                Handles.Label(Vector3.Lerp(intersectionPoints[0], intersectionPoints[1], .5f), $"width: {width.magnitude}");
                // HeightLabel
                Vector3 height = intersectionPoints[1] - intersectionPoints[2];
                Handles.Label(Vector3.Lerp(intersectionPoints[1], intersectionPoints[2], .5f), $"height: {height.magnitude}");
            }
        }

        private void GetFrustumCorners(Camera camera, float planeDistance, Vector3[] corners, Transform camTransform)
        {
            float fov = camera.fieldOfView;
            float aspect = camera.aspect;

            float height = 2.0f * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * planeDistance;
            float width = height * aspect;

            Vector3 center = camTransform.position + camTransform.forward * planeDistance;
            Vector3 right = camTransform.right * (width / 2);
            Vector3 up = camTransform.up * (height / 2);

            corners[0] = center - right - up; // Bottom left
            corners[1] = center + right - up; // Bottom right
            corners[2] = center + right + up; // Top right
            corners[3] = center - right + up; // Top left
        }

        private bool RayIntersectsXYPlane(Vector3 start, Vector3 end, out Vector3 intersection)
        {
            intersection = Vector3.zero;

            Vector3 direction = end - start;
            if (Mathf.Approximately(direction.z, 0))
            {
                return false; // No intersection
            }

            float t = -start.z / direction.z;
            if (t >= 0 && t <= 1)
            {
                intersection = start + t * direction;
                return true;
            }

            return false; // Intersection is outside the segment
        }
    }
}

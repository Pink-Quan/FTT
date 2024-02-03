using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QuanUtilities
{
    public static class Utilities
    {
        public static Coroutine DelayFunction(this MonoBehaviour mono, float delay, System.Action onDone)
        {
            return mono.StartCoroutine(StartDelayFuction(delay, onDone));
        }

        private static IEnumerator StartDelayFuction(float delay, System.Action onDone)
        {
            yield return new WaitForSeconds(delay);
            onDone?.Invoke();
        }

        public static void SaveData(string data, string fileName)
        {
            string dataPath = $"{Application.persistentDataPath}/{fileName}.txt";

            File.WriteAllText(dataPath, data);
        }
        public static string LoadData(string fileName)
        {
            string dataPath = $"{Application.persistentDataPath}/{fileName}.txt";

            if (File.Exists(dataPath))
                return File.ReadAllText(dataPath);
            else
                return "";
        }

        /// <summary>
        /// Use this if EventSystem.current.IsPointerOverGameObject() not work, this often occur in mobile
        /// </summary>
        /// <returns></returns>

        public static bool IsPointerOverUIObject(this MonoBehaviour mono)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public static bool IsOverlapBox2D(this BoxCollider2D box1, BoxCollider2D box2)
        {
            return box1.bounds.Intersects(box2.bounds);
        }

        public static float BottomBorder(this Camera cam)
        {
            return cam.transform.position.y - cam.orthographicSize;
        }
        public static float TopBorder(this Camera cam)
        {
            return cam.transform.position.y + cam.orthographicSize;
        }
        public static float RightBorder(this Camera cam)
        {
            return cam.transform.position.x + cam.orthographicSize * cam.aspect;
        }
        public static float LeftBorder(this Camera cam)
        {
            return cam.transform.position.x - cam.orthographicSize * cam.aspect;
        }

        public static Vector2 UpRightCorner(this Camera cam)
        {
            return new Vector2(RightBorder(cam), TopBorder(cam));
        }
        public static Vector2 UpLeftCorner(this Camera cam)
        {
            return new Vector2(LeftBorder(cam), TopBorder(cam));
        }
        public static Vector2 DownLeftCorner(this Camera cam)
        {
            return new Vector2(LeftBorder(cam), BottomBorder(cam));
        }
        public static Vector2 DownRightCorner(this Camera cam)
        {
            return new Vector2(RightBorder(cam), BottomBorder(cam));
        }
    }

}


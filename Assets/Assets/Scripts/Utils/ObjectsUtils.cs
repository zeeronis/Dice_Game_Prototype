using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class ObjectsUtils
    {

        #region By T type

        /// <summary>
        /// Clear input collection and Collect all children items with T type
        /// </summary>
        public static void ClearAndCollectObjects<T>(Transform root, ref List<T> list, bool includeSelf = true) where T : Component
        {
            list.Clear();
            CollectObjects(root, ref list);

            if (includeSelf == false && list.Count > 0)
            {
                if (root.TryGetComponent(out T obj))
                    list.RemoveAt(0);
            }
        }

        /// <summary>
        /// Collect all children items with T type
        /// </summary>
        public static void CollectObjects<T>(Transform root, ref List<T> list) where T : Component
        {
            if (root.TryGetComponent(out T obj))
            {
                list.Add(obj);
            }

            foreach (Transform item in root.GetComponentInChildren<Transform>())
            {
                CollectObjects(item, ref list);
            }
        }

        #endregion


        #region By T type and Name

        /// <summary>
        /// Clear input collection and Collect all children items with T type
        /// </summary>
        public static void ClearAndCollectObjectsWithName<T>(Transform root, ref List<T> list, string name, bool includeSelf = true) where T : Component
        {
            list.Clear();
            CollectObjectsWithName(root, ref list, name);

            if (includeSelf == false && list.Count > 0 && root.name.Contains(name))
            {
                if (root.TryGetComponent(out T obj))
                    list.RemoveAt(0);
            }
        }

        /// <summary>
        /// Collect all children items with T type and contains name
        /// </summary>
        public static void CollectObjectsWithName<T>(Transform root, ref List<T> list, string name) where T : Component
        {
            if (root.TryGetComponent(out T obj) && root.name.Contains(name) == true)
            {
                list.Add(obj);
            }

            foreach (Transform item in root.GetComponentInChildren<Transform>())
            {
                CollectObjectsWithName(item, ref list, name);
            }
        }

        #endregion


        #region By T type and Tag

        /// <summary>
        /// Clear input collection and Collect all children items with T type and with tag
        /// </summary>
        public static void ClearAndCollectObjectsWithTag<T>(Transform root, ref List<T> list, string tag, bool includeSelf = true) where T : Component
        {
            list.Clear();
            CollectObjectsWithTag(root, ref list, tag);

            if (includeSelf == false && list.Count > 0)
            {
                if (root.TryGetComponent(out T obj) == true && root.CompareTag(tag) == true)
                    list.RemoveAt(0);
            }
        }

        /// <summary>
        /// Collect all children items with T type and with tag
        /// </summary>
        public static void CollectObjectsWithTag<T>(Transform root, ref List<T> list, string tag) where T : Component
        {
            if (root.TryGetComponent(out T obj) == true && root.CompareTag(tag) == true)
            {
                list.Add(obj);
            }

            foreach (Transform item in root.GetComponentInChildren<Transform>())
            {
                CollectObjectsWithTag(item, ref list, tag);
            }
        }

        #endregion

    }
}

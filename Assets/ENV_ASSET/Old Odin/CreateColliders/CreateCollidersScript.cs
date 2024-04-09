using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OldOdin
{

    public class CreateCollidersScript : MonoBehaviour
    {
        public float minArea = 0;

        public void Add()
        {
            var meshs = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer m in meshs)
            {
                if (m.enabled && m.GetComponent<Collider>() == null)
                {
                    var s = m.bounds.size;
                    float area = s.x * s.y * s.z;
                    if (area >= minArea || s.x * s.z >= minArea || s.x * s.y >= minArea || s.y * s.z >= minArea)
                    {
                        if (m.gameObject.GetComponent<MeshFilter>() != null
                            && m.gameObject.GetComponent<MeshFilter>().sharedMesh != null
                          )
                        {
                            try
                            {
                                m.gameObject.AddComponent<MeshCollider>();
                            }
                            catch(Exception e)
                            {
                                Debug.LogError(e.ToString());
                            }
                        }
                    }
                }
            }
            Debug.Log("Successfully created colliders");
        }

    }
}

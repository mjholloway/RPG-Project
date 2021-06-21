using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class HideObjects : MonoBehaviour
    {
        [SerializeField] GameObject player = null;
        [SerializeField] float raycastRadius = 3f;

        List<MeshRenderer> hiddenMeshes = new List<MeshRenderer>();

        private void Update()
        {
            ShowHiddenObjects();
            FindObjectsToHide();
        }

        private void ShowHiddenObjects()
        {
            if (hiddenMeshes.Count > 0)
            {
                for (int i = 0; i < hiddenMeshes.Count; i++)
                {
                    if (hiddenMeshes[i].materials.Length > 1)
                    {
                        hiddenMeshes[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    }

                    hiddenMeshes[i].material.ToOpaqueMode();
                    hiddenMeshes.Remove(hiddenMeshes[i]);
                }
            }
        }

        private void FindObjectsToHide()
        {
            Vector3 playerPosition = player.transform.position;
            RaycastHit[] hits = Physics.SphereCastAll(Camera.main.transform.position,
                raycastRadius, playerPosition - Camera.main.transform.position, 1000f, 1 << 8);

            foreach (RaycastHit hit in hits)
            {
                MeshRenderer mesh = hit.collider.GetComponent<MeshRenderer>();
                if (Vector3.Distance(playerPosition, Camera.main.transform.position) > Vector3.Distance(hit.point, Camera.main.transform.position))
                {
                    if (mesh.materials.Length > 1) { mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly; }
                    mesh.material.ToFadeMode();
                    hiddenMeshes.Add(mesh);
                }
            }
        }
    }
}

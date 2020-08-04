using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EZSlice : MonoBehaviour
{
    public GameObject objectToSlice,BladeObject; // non-null
    Animator anim;
    public LayerMask layer;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    Vector3 firstMousePos;
    Vector3 secMousePos;
    Vector3 bladeRota;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1")) {
            bladeRota = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
            transform.Rotate(bladeRota,Space.World);
        }
        //注意 因ezySlice所限，被切割物体不得有缩放。
        if (Input.GetKeyDown(KeyCode.Space)) {
            //BSlice(planeObject.transform.position, planeObject.transform.up);
            //Instantiate(gm[0]);
            //anim.SetTrigger("cut");

            Collider[] targets = Physics.OverlapBox(BladeObject.transform.position, new Vector3(7f,0.1f, 10f), BladeObject.transform.rotation, layer);
            if (targets.Length <= 0) return;
            foreach (Collider target in targets) {
                SlicedHull result = target.gameObject.Slice(BladeObject.transform.position, BladeObject.transform.up);
                if (result != null)
                {
                    GameObject up = result.CreateUpperHull(target.gameObject, mat);
                    GameObject down = result.CreateLowerHull(target.gameObject, mat);
                    up.layer = 8;
                    down.layer = 8;
                    up.AddComponent<MeshCollider>().convex=true;
                    down.AddComponent<MeshCollider>().convex = true;
                    Rigidbody rigU = up.AddComponent<Rigidbody>();
                    Rigidbody rigD = down.AddComponent<Rigidbody>();
                    rigU.AddExplosionForce(100, down.transform.position, 10);
                    rigD.AddExplosionForce(100, down.transform.position, 10);
                    Destroy(target.gameObject);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(BladeObject.transform.position, new Vector3(7f, 5f, 0.1f));
    }

    /**
     * Example on how to slice a GameObject in world coordinates.
     */
    public SlicedHull ASlice(Vector3 planeWorldPosition, Vector3 planeWorldDirection)
    {
        return objectToSlice.Slice(planeWorldPosition, planeWorldDirection);
    }
    /**
    * Example on how to slice a GameObject in world coordinates.
    */
    public GameObject[] BSlice(Vector3 planeWorldPosition, Vector3 planeWorldDirection)
    {
        return objectToSlice.SliceInstantiate(planeWorldPosition, planeWorldDirection);
    }


    public SlicedHull CSlice(Vector3 planeWorldPosition, Vector3 planeWorldDirection, TextureRegion region)
    {
        return objectToSlice.Slice(planeWorldPosition, planeWorldDirection, region);
    }


    public GameObject[] DSlice(Vector3 planeWorldPosition, Vector3 planeWorldDirection, TextureRegion region)
    {
        return objectToSlice.SliceInstantiate(planeWorldPosition, planeWorldDirection, region);
    }
}

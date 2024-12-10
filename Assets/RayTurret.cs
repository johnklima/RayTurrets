using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RayTurret : MonoBehaviour
{


    public float probeDepth = 3.0f;
    public Transform probeResult;
    public Transform[] ImHit;         //cheating, will preallocate

    // Start is called before the first frame update
    void Start()
    {
        
    }

 
    // Update is called once per frame
    void Update()
    {

        

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, probeDepth))
        {

            Debug.Log(transform.name + " probing " + hitInfo.collider.name);
            probeResult = hitInfo.collider.transform;

            Debug.DrawRay(transform.position, transform.forward * probeDepth, Color.red);

            RayTurret targ = probeResult.GetComponent<RayTurret>();
            bool exists = false;
            int next = 0;
            foreach (Transform hit in targ.ImHit)
            { 

                //am I in the list already?
                if(hit == transform)
                {
                    exists = true;
                }
                if (hit != null)
                {
                    next++;
                }

     
            }

            if (!exists)
            {
                targ.ImHit[next] = transform;
            }
        }
        else  //Im not hitting anything
        {
            probeResult = null;
        }

        //now if nothing is hitting me, drop the old from my list
        //this is wrong, need to think about it
        
        
        for(int i = 0; i < ImHit.Length; i++)
        {
            if (ImHit[i] != null)
            {
                RayTurret turret = ImHit[i].GetComponent<RayTurret>();
                //if what it is looking at is not me, rermove from my list
                if (turret.probeResult != transform)
                {
                    ImHit[i] = null;
                }

            }
        }
        

        //now that I have checked if I am hitting something, check
        //if something is hitting me, add the vectors

        Vector3 dir = Vector3.zero;
        bool doit = false;
        foreach (Transform hit in ImHit)
        {
            if (hit != null)
            {
                doit = true;
                dir += hit.forward;
            }
        }

        if (doit)
        {
            dir.Normalize();           
            Debug.DrawRay(transform.position, dir * probeDepth, Color.red);
            transform.LookAt(transform.position + dir);

        }

        //trash the array of hits, we try again on reentry. So much cleaner than hunt and peck
        /*for (int i = 0; i < ImHit.Length; i++)
        {
            ImHit[i] = null;
        }*/
    }


}

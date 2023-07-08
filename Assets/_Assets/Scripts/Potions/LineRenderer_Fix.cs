using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineRenderer_Fix : MonoBehaviour {

    public bool loop = false;

    
    private bool changed = false;
    private LineRenderer lr;


    private void OnEnable()
    {
        lr = this.GetComponent<LineRenderer>();
    }


	// Update is called once per frame
	void Update () {
        
        // this is for make the loop
        if (loop&&!changed)
        {

            
            lr.loop = false;
            lr.numCapVertices = 0;

            Vector3[] pos = new Vector3[this.GetComponent<LineRenderer>().positionCount + 2];
            this.GetComponent<LineRenderer>().GetPositions(pos);
            
            /* if (pos[pos.Length - 3 ] == pos[0])
            {
                Debug.Log("This LineRenderer is alrady a loop ");
                System.Array.Clear(pos,0,pos.Length);
            }
            else
            { */

            for (int i = 0; i < pos.Length - 2; i++)
            {
                    pos[pos.Length - 2 - i ] = pos[pos.Length - 3 - i];
            }

            pos[pos.Length - 1] = new Vector3((pos[1].x + pos[pos.Length - 2].x) / 2, (pos[1].y + pos[pos.Length - 2].y) / 2, (pos[1].z + pos[pos.Length - 2].z) / 2);
            pos[0] = pos[pos.Length - 1];

            this.GetComponent<LineRenderer>().positionCount = pos.Length;
            this.GetComponent<LineRenderer>().SetPositions(pos);
            
            // }

            Debug.Log("LineRenderer loop = true ");
            changed = true;
        }

        //this is for open the loop 
        if (!loop && changed)
        {

            
            Vector3[] pos = new Vector3[this.GetComponent<LineRenderer>().positionCount];
            this.GetComponent<LineRenderer>().GetPositions(pos);

            for (int i = 0; i < pos.Length - 1; i++)
            {
                pos[i] = pos[i+1];
            }

            this.GetComponent<LineRenderer>().positionCount = pos.Length-2;
            this.GetComponent<LineRenderer>().SetPositions(pos);

            Debug.Log("LineRenderer loop = false ");

            changed = false;

        }

    }
}

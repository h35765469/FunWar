using UnityEngine;
using UnityEngine.Networking;

public class Interactable : NetworkBehaviour {
    public float radius = 3f;
    public Transform interactionTransform;

    bool isFocus = false;
    Transform player;

    bool hasInteracted = false; //Have we interacted with the object

    public virtual void OnCollisionEnter(Collision collision)
    {
        //switch (collision.gameObject.tag){
        //    case "Point":
        //        Debug.Log("You get point");
        //        Destroy(collision.gameObject);
        //        points += 1;
        //        break;
        //    case "QuestionMark":
        //        Debug.Log("You get QuestionMark");
        //        Destroy(collision.gameObject);
        //        GetRandomWeapon();
        //        break;
        //}

        //Debug.Log("Interacting with " + transform.name);

    }

	private void Update()
	{
		
        if(isFocus && !hasInteracted){

            //if we are close enough
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if(distance <= radius){
                hasInteracted = true;
            }
        }
	}

    public void OnFocused(Transform playerTransform){
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    } 

    public void OnDefocused(){
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

	private void OnDrawGizmosSelected()
	{
        if(interactionTransform == null){
            interactionTransform = transform;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}
}

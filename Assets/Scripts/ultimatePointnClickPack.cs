using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ultimatePointnClickPack : MonoBehaviour
{

    public GameObject objectThatActivatesOrDeactivates;
    public DialogueManager dialogueManager;
    public Dialogue dialogue;
    public Dialogue inactiveDialogue;
    public GameObject nametag;
    public ParameterHolder parameterHolder;

    public void OnPointerEnter(PointerEventData eventData){
        nametag.SetActive(true);
    }
      public void OnPointerExit(PointerEventData eventData){
        nametag.SetActive(false);
    }

  public void activateDialogue(){
    dialogueManager.StartDialogue(dialogue);
  }

  public void revealItem(bool isActive){
    objectThatActivatesOrDeactivates.SetActive(isActive);
  }

 
    public void getOut(){

       Debug.Log("Here");
        if (parameterHolder.Key1 && parameterHolder.Key2 && parameterHolder.Key3){
          revealItem(true);
            dialogueManager.StartDialogue(dialogue);
            Debug.Log("Bark");

        } else {dialogueManager.StartDialogue(inactiveDialogue);
        Debug.Log("Meow");
        }
    }


}

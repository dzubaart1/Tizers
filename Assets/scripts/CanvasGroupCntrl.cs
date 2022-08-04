using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CanvasGroupCntrl 
{
   public static void ChangeStateCanvas(CanvasGroup @group, bool state)
   {
      if (state)
      {
         group.interactable = true;
         group.alpha = 1;
         group.blocksRaycasts = true;
      }
      else
      {
         group.interactable = false;
         group.alpha = 0;
         group.blocksRaycasts = false;
      }
   }
}

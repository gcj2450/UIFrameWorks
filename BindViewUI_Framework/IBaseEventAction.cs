using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IBaseEventAction 
{
	 void HandleNotification(object sender,ChangeUIEventArgs args);

	 List<string> ListNotificationString();
}

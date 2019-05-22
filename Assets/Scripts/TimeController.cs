using UnityEngine;
using UnityEngine.Events;

public class TimeController : MonoBehaviour {
	public bool isOn;
	public UnityEvent OnAccelerate,OnDeaccelerate,OnBackToNormal;
	float lastSpeed = 1;
	
	public void ChangeTime(int newSpeed) {
		if(isOn){
			Time.timeScale = newSpeed;
			if(lastSpeed != newSpeed){
				if(lastSpeed < newSpeed){
					OnAccelerate.Invoke();
					lastSpeed = newSpeed;
				} else if(lastSpeed > newSpeed){
					OnDeaccelerate.Invoke();
					lastSpeed = newSpeed;
				} else if(lastSpeed != newSpeed && newSpeed == 1){
					OnBackToNormal.Invoke();
					lastSpeed = newSpeed;
				}
			}
		}
	}

	public void PauseTime() {
		Time.timeScale = 0;
	}
	
}

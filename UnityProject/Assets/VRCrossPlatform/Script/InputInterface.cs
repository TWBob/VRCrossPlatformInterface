using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Unity 2019.1.1f1
	SteamVR plugin for Unity - v1.2.3
	WaveVR 3.1.1
 */


//Vive
using Valve.VR;
//Focus
using wvr;
using WaveVR_Log;

namespace VRCrossPlatform {
	public class InputInterface : MonoBehaviour {
		public static InputInterface Instance;
		public PlatformEnum Platform;
		public SteamVR_ControllerManager SteamVRCtlrMgr;
		public Controller LeftController { get; private set; }
		public Controller RightController { get; private set; }
		private void Awake () {
			if (Instance != null) {
				Destroy (this.gameObject);
				return;
			}
			Instance = this;
		}
		private void OnEnable () {
#if UNITY_STANDALONE
			Platform = PlatformEnum.SteamVR;
			if (SteamVRCtlrMgr == null) {
				Debug.LogWarning ("SteamVR Controller Manager NOT set!");
			}
			LeftController = new Controller (this, Platform, Controller.HandEnum.Left, SteamVRCtlrMgr);
			RightController = new Controller (this, Platform, Controller.HandEnum.Right, SteamVRCtlrMgr);
#elif UNITY_ANDROID
			Platform = PlatformEnum.WaveVR;
			LeftController = new Controller (this, Platform, Controller.HandEnum.Left);
			RightController = new Controller (this, Platform, Controller.HandEnum.Right);
#else
			Platform = PlatformEnum.none;
			Debug.LogWarning ("WTF Platform you set?");
#endif
		}
		private void Reset () {
			Platform = PlatformEnum.none;
		}
	}
	public class Controller {
		private InputInterface _inputInterface;
		public HandEnum Hand { get; private set; }
		public PlatformEnum Platform { get; private set; }
		public SteamVR_ControllerManager CtlrMgr { get; private set; }

		public Controller (InputInterface inputInterface, PlatformEnum platform, HandEnum hand) {
			_inputInterface = inputInterface;
			Platform = platform;
			Hand = hand;
		}
		public Controller (InputInterface inputInterface, PlatformEnum platform, HandEnum hand, SteamVR_ControllerManager ctlrMgr) {
			_inputInterface = inputInterface;
			Platform = platform;
			Hand = hand;
			CtlrMgr = ctlrMgr;
		}
		public void SetPlatform (PlatformEnum platform, SteamVR_ControllerManager ctlrMgr) {
			Platform = platform;
			if (Platform == PlatformEnum.SteamVR) {
				CtlrMgr = ctlrMgr;
			}
		}
		#region ButtonPress
		public bool GetButtonPressDown (ButtonEnum button) {
			bool buttonPressDown = false;
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					EVRButtonId buttonID = GetSteamVRButtonID (button);
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return false;
					}
					buttonPressDown = SteamVR_Controller.Input (deviceIndex).GetPressDown (buttonID);
					break;
				case PlatformEnum.WaveVR:
					WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					WVR_InputId id = GetWaveVRButtonID (button);
					if (Hand == HandEnum.Left) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Left;
					} else if (Hand == HandEnum.Right) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					}
					buttonPressDown = WaveVR_Controller.Input (device).GetPressDown (id);
					break;
			}
			return buttonPressDown;
		}
		public bool GetButtonPressUp (ButtonEnum button) {
			bool buttonPressUp = false;
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					EVRButtonId buttonID = GetSteamVRButtonID (button);
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return false;
					}
					buttonPressUp = SteamVR_Controller.Input (deviceIndex).GetPressUp (buttonID);
					break;
				case PlatformEnum.WaveVR:
					WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					WVR_InputId id = GetWaveVRButtonID (button);
					if (Hand == HandEnum.Left) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Left;
					} else if (Hand == HandEnum.Right) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					}
					buttonPressUp = WaveVR_Controller.Input (device).GetPressUp (id);
					break;
			}
			return buttonPressUp;
		}
		public bool GetButtonPress (ButtonEnum button) {
			bool buttonPress = false;
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					EVRButtonId buttonID = GetSteamVRButtonID (button);
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return false;
					}
					buttonPress = SteamVR_Controller.Input (deviceIndex).GetPress (buttonID);
					break;
				case PlatformEnum.WaveVR:
					WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					WVR_InputId id = GetWaveVRButtonID (button);
					if (Hand == HandEnum.Left) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Left;
					} else if (Hand == HandEnum.Right) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					}
					buttonPress = WaveVR_Controller.Input (device).GetPress (id);
					break;
			}
			return buttonPress;
		}
		#endregion
		#region ButtonTouch
		public bool GetButtonTouch (ButtonEnum button) {
			bool buttonTouch = false;
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					EVRButtonId buttonID = GetSteamVRButtonID (button);
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return false;
					}
					buttonTouch = SteamVR_Controller.Input (deviceIndex).GetTouch (buttonID);
					break;
				case PlatformEnum.WaveVR:
					WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					WVR_InputId id = GetWaveVRButtonID (button);
					if (Hand == HandEnum.Left) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Left;
					} else if (Hand == HandEnum.Right) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					}
					buttonTouch = WaveVR_Controller.Input (device).GetTouch (id);
					break;
			}
			return buttonTouch;
		}
		public bool GetButtonTouchUp (ButtonEnum button) {
			bool buttonTouchUp = false;
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					EVRButtonId buttonID = GetSteamVRButtonID (button);
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return false;
					}
					buttonTouchUp = SteamVR_Controller.Input (deviceIndex).GetTouchUp (buttonID);
					break;
				case PlatformEnum.WaveVR:
					WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					WVR_InputId id = GetWaveVRButtonID (button);
					if (Hand == HandEnum.Left) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Left;
					} else if (Hand == HandEnum.Right) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					}
					buttonTouchUp = WaveVR_Controller.Input (device).GetTouchUp (id);
					break;
			}
			return buttonTouchUp;
		}
		public bool GetButtonTouchDown (ButtonEnum button) {
			bool buttonTouch = false;
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					EVRButtonId buttonID = GetSteamVRButtonID (button);
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return false;
					}
					buttonTouch = SteamVR_Controller.Input (deviceIndex).GetTouchDown (buttonID);
					break;
				case PlatformEnum.WaveVR:
					WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					WVR_InputId id = GetWaveVRButtonID (button);
					if (Hand == HandEnum.Left) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Left;
					} else if (Hand == HandEnum.Right) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					}
					buttonTouch = WaveVR_Controller.Input (device).GetTouchDown (id);
					break;
			}
			return buttonTouch;
		}
		#endregion
		public Vector2 GetAxis (ButtonEnum button) {
			bool isPadOrTrigger = button == ButtonEnum.TouchPad || button == ButtonEnum.Trigger;
			if (!isPadOrTrigger) {
				Debug.LogWarning ("Only be able to get axis of TouchPad or Trigger!");
				return Vector2.zero;
			}
			Vector2 axis = Vector2.zero;
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					EVRButtonId buttonID = GetSteamVRButtonID (button);
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return axis;
					}
					axis = SteamVR_Controller.Input (deviceIndex).GetAxis (buttonID);
					break;
				case PlatformEnum.WaveVR:
					WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					WVR_InputId id = GetWaveVRButtonID (button);
					if (Hand == HandEnum.Left) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Left;
					} else if (Hand == HandEnum.Right) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					}
					axis = WaveVR_Controller.Input (device).GetAxis (id);
					break;
			}
			return axis;
		}
		#region HairTrigger
		public bool GetHairTrigger () {
			bool hairTrigger = false;
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return false;
					}
					hairTrigger = SteamVR_Controller.Input (deviceIndex).GetHairTrigger ();
					break;
				case PlatformEnum.WaveVR:
					Debug.LogWarning ("HairTrigger not works at WaveVR");
					break;
			}
			return hairTrigger;
		}
		public bool GetHairTriggerDown () {
			bool hairTriggerDown = false;
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return false;
					}
					hairTriggerDown = SteamVR_Controller.Input (deviceIndex).GetHairTrigger ();
					break;
				case PlatformEnum.WaveVR:
					Debug.LogWarning ("HairTrigger not works at WaveVR");
					break;
			}
			return hairTriggerDown;
		}
		public bool GetHairTriggerUp () {
			bool hairTriggerUp = false;
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return false;
					}
					hairTriggerUp = SteamVR_Controller.Input (deviceIndex).GetHairTrigger ();
					break;
				case PlatformEnum.WaveVR:
					Debug.LogWarning ("HairTrigger not works at WaveVR");
					break;
			}
			return hairTriggerUp;
		}
		#endregion
		public Vector3 GetVelocity () {
			Vector3 velocity = Vector3.zero;
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return velocity;
					}
					velocity = SteamVR_Controller.Input (deviceIndex).velocity;
					break;
				case PlatformEnum.WaveVR:
					WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					if (Hand == HandEnum.Left) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Left;
					} else if (Hand == HandEnum.Right) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					}
					WVR_Vector3f_t v = WaveVR_Controller.Input (device).velocity;
					velocity.x = v.v0;
					velocity.y = v.v1;
					velocity.z = v.v2;
					break;
			}
			return velocity;
		}
		public void Vibrate (float duration, float strength = 1.0f) {
			switch (Platform) {
				case PlatformEnum.SteamVR:
					int deviceIndex = 0;
					if (Hand == HandEnum.Left) {
						deviceIndex = (int) CtlrMgr.left.GetComponent<SteamVR_TrackedObject> ().index;
					} else if (Hand == HandEnum.Right) {
						deviceIndex = (int) CtlrMgr.right.GetComponent<SteamVR_TrackedObject> ().index;
					}
					if (deviceIndex == (int) SteamVR_TrackedObject.EIndex.None) {
						Debug.LogWarning ("Cannot get Device Index.");
						return;
					}
					StartHapticVibration (SteamVR_Controller.Input (deviceIndex), duration, strength);
					break;
				case PlatformEnum.WaveVR:
					WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					if (Hand == HandEnum.Left) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Left;
					} else if (Hand == HandEnum.Right) {
						device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
					}
					StartHapticVibration (WaveVR_Controller.Input (device), duration, strength);
					break;
			}
		}
		protected Dictionary<SteamVR_Controller.Device, Coroutine> _activeHapticCoroutines_SteamVR = new Dictionary<SteamVR_Controller.Device, Coroutine> ();
		protected Dictionary<WaveVR_Controller.Device, Coroutine> _activeHapticCoroutines_WaveVR = new Dictionary<WaveVR_Controller.Device, Coroutine> ();
		protected void StartHapticVibration (SteamVR_Controller.Device device, float duration, float strength) { //Start Vibrate SteamVR
			if (_activeHapticCoroutines_SteamVR.ContainsKey (device)) {
				Debug.Log ("This device(" + device.ToString () + ") is already vibrating");
				return;
			}
			Coroutine coroutine = _inputInterface.StartCoroutine (StartHapticVibrationCoroutine (device, duration, strength));
			_activeHapticCoroutines_SteamVR.Add (device, coroutine);
		}
		protected void StartHapticVibration (WaveVR_Controller.Device device, float duration, float strength) { //Start Vibrate WaveVR
			if (_activeHapticCoroutines_WaveVR.ContainsKey (device)) {
				Debug.Log ("This device(" + device.ToString () + ") is already vibrating");
				return;
			}
			Coroutine coroutine = _inputInterface.StartCoroutine (StartHapticVibrationCoroutine (device, duration, strength));
			_activeHapticCoroutines_WaveVR.Add (device, coroutine);
		}
		protected void StopHapticVibration (SteamVR_Controller.Device device) { //Stop Vibrate SteamVR
			if (!_activeHapticCoroutines_SteamVR.ContainsKey (device)) {
				Debug.Log ("Could not find this device");
				return;
			}
			_inputInterface.StopCoroutine (_activeHapticCoroutines_SteamVR[device]);
			_activeHapticCoroutines_SteamVR.Remove (device);
		}
		protected void StopHapticVibration (WaveVR_Controller.Device device) { //Stop Vibrate WaveVR
			if (!_activeHapticCoroutines_WaveVR.ContainsKey (device)) {
				Debug.Log ("Could not find this device");
				return;
			}
			_inputInterface.StopCoroutine (_activeHapticCoroutines_WaveVR[device]);
			_activeHapticCoroutines_WaveVR.Remove (device);
		}
		protected IEnumerator StartHapticVibrationCoroutine (SteamVR_Controller.Device device, float length, float strength) { //Vibration Coroutine SteamVR
			for (float i = 0; i < length; i += Time.deltaTime) {
				device.TriggerHapticPulse ((ushort) Mathf.Lerp (0, ushort.MaxValue, strength));
				yield return null;
			}
			_activeHapticCoroutines_SteamVR.Remove (device);
		}
		protected IEnumerator StartHapticVibrationCoroutine (WaveVR_Controller.Device device, float length, float strength) { //Vibration Coroutine WaveVR
			for (float i = 0; i < length; i += Time.deltaTime) {
				//argument must be multiple of 1000
				ushort maxValue = 65000;
				ushort baseValue = 1000;
				float lerpValue = Mathf.Lerp (0, maxValue, strength);
				ushort q = (ushort)((ushort)lerpValue / baseValue);
				ushort r = (ushort)((ushort)lerpValue % baseValue);
				bool isNearRightValue = r>=500;
				if(isNearRightValue){
					q++;
				}
				ushort pulseValue = (ushort)(q * baseValue);
				device.TriggerHapticPulse (pulseValue);
				yield return null;
			}
			_activeHapticCoroutines_WaveVR.Remove (device);
		}
		private WVR_InputId GetWaveVRButtonID (ButtonEnum button) {
			switch (button) {
				case ButtonEnum.Trigger:
					return WVR_InputId.WVR_InputId_Alias1_Digital_Trigger;
					// return WVR_InputId.WVR_InputId_Alias1_Trigger;
				case ButtonEnum.Grip:
					return WVR_InputId.WVR_InputId_Alias1_Grip;
				case ButtonEnum.TouchPad:
					return WVR_InputId.WVR_InputId_Alias1_Touchpad;
				case ButtonEnum.Menu:
					return WVR_InputId.WVR_InputId_Alias1_Menu;
				case ButtonEnum.System:
					return WVR_InputId.WVR_InputId_Alias1_System;
				case ButtonEnum.PadUp:
					return WVR_InputId.WVR_InputId_Alias1_DPad_Up;
				case ButtonEnum.PadDown:
					return WVR_InputId.WVR_InputId_Alias1_DPad_Down;
				case ButtonEnum.PadLeft:
					return WVR_InputId.WVR_InputId_Alias1_DPad_Left;
				case ButtonEnum.PadRight:
					return WVR_InputId.WVR_InputId_Alias1_DPad_Right;
			}
			return WVR_InputId.WVR_InputId_Max;
		}
		private EVRButtonId GetSteamVRButtonID (ButtonEnum button) {
			switch (button) {
				case ButtonEnum.Trigger:
					// return WVR_InputId.WVR_InputId_Alias1_Digital_Trigger;
					// return SteamVR_Controller.ButtonMask.Trigger;
					return EVRButtonId.k_EButton_SteamVR_Trigger;
				case ButtonEnum.Grip:
					// return SteamVR_Controller.ButtonMask.Grip;
					return EVRButtonId.k_EButton_Grip;
				case ButtonEnum.TouchPad:
					// return SteamVR_Controller.ButtonMask.Touchpad;
					return EVRButtonId.k_EButton_SteamVR_Touchpad;
				case ButtonEnum.Menu:
					// return SteamVR_Controller.ButtonMask.ApplicationMenu;
					return EVRButtonId.k_EButton_ApplicationMenu;
				case ButtonEnum.System:
					// return SteamVR_Controller.ButtonMask.System;
					return EVRButtonId.k_EButton_System;
				case ButtonEnum.PadUp:
					return EVRButtonId.k_EButton_DPad_Up;
				case ButtonEnum.PadDown:
					return EVRButtonId.k_EButton_DPad_Down;
				case ButtonEnum.PadLeft:
					return EVRButtonId.k_EButton_DPad_Left;
				case ButtonEnum.PadRight:
					return EVRButtonId.k_EButton_DPad_Right;

			}
			return 0;
		}
		public enum HandEnum {
			Left = 0,
			Right
		}
		public enum ButtonEnum {
			Trigger = 0,
			Grip,
			TouchPad,
			Menu,
			System,
			PadUp,
			PadDown,
			PadLeft,
			PadRight
		}
	}
	public enum PlatformEnum {
		SteamVR = 0,
		WaveVR,
		none
	}
}
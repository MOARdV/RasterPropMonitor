using UnityEngine;
using System;
using System.Collections.Generic;

namespace JSI
{
	public class SmarterButton: MonoBehaviour
	{
		private Action<int> handlerID;
		private Action<MonitorPage> pageSelectionHandlerFunction;
		private Action handler;
		private int id;
		private readonly List<MonitorPage> pageReferences = new List<MonitorPage>();
		private int listCounter;

		public void OnMouseDown()
		{
			if (pageReferences.Count > 0) {
				pageSelectionHandlerFunction(pageReferences[listCounter]);
				listCounter++;
				if (listCounter >= pageReferences.Count)
					listCounter = 0;
			}
			if (handlerID != null)
				handlerID(id);
			if (handler != null)
				handler();
		}

		private static SmarterButton AttachBehaviour(InternalProp thatProp, string buttonName)
		{
			try {
				GameObject buttonObject = thatProp.FindModelTransform(buttonName).gameObject;
				SmarterButton thatComponent = buttonObject.GetComponent<SmarterButton>() ?? buttonObject.AddComponent<SmarterButton>();
				return thatComponent;
			} catch {
				Debug.LogError(string.Format(
					"ERROR: Could not register a button on transform named '{0}' in prop named '{1}'. Check your configuration.",
					buttonName, thatProp.propName));
			}
			return null;
		}

		public static void CreateButton(InternalProp thatProp, string buttonName, MonitorPage thatPage, Action<MonitorPage> handlerFunction)
		{
			SmarterButton buttonBehaviour;
			if ((buttonBehaviour = AttachBehaviour(thatProp, buttonName)) == null)
				return;
			buttonBehaviour.pageSelectionHandlerFunction = handlerFunction;
			buttonBehaviour.pageReferences.Add(thatPage);
		}

		public static void CreateButton(InternalProp thatProp, string buttonName, int id, Action<int> handlerFunction)
		{
			SmarterButton buttonBehaviour;
			if ((buttonBehaviour = AttachBehaviour(thatProp, buttonName)) == null)
				return;
			buttonBehaviour.id = id;
			buttonBehaviour.handlerID = handlerFunction;
		}

		public static void CreateButton(InternalProp thatProp, string buttonName, Action handlerFunction)
		{
			SmarterButton buttonBehaviour;
			if ((buttonBehaviour = AttachBehaviour(thatProp, buttonName)) == null)
				return;
			buttonBehaviour.handler = handlerFunction;
		}
	}
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CatStudio.Character
{
	namespace Viewer
	{
		public class MotionViewer : MonoBehaviour
		{



			[System.Serializable]
			public class MotionInfo
			{
				public string Title;

				public int Mode;

				public string AttachSceneSettingId;

			}


			[SerializeField]
			MotionInfo[] MotionInfos;


			[SerializeField]
			protected Animator RefAnimator;


			[SerializeField]
			Object PrefabCell;


			[SerializeField]
			ScrollRect MotionScrollRect;



			AttachPairingManagement AttachObjectManager;

			AttachEnvironmentManagement _AttachEnvironmentManagement;


			protected void Setup()
			{

				for (int i = 0; i < MotionInfos.Length; i++)
				{
					var item = MotionInfos[i];
					var newObj = GameObject.Instantiate(this.PrefabCell) as GameObject;

					newObj.transform.SetParent(this.MotionScrollRect.content, false);

					var spt = newObj.GetComponentInChildren<MotionViewerCell>();
					spt.SetMotionInfo(item);

					spt.DidClicked += ((MotionViewerCell sender, MotionInfo info) =>
					{
						//Debug.Log("Clicked : " + info.Title);
						RefAnimator.SetInteger("Mode", info.Mode);
						RefAnimator.SetTrigger("Trigger");



						_AttachEnvironmentManagement.Execute(info.AttachSceneSettingId);


					});

				}
				MotionScrollRect.content.sizeDelta = new Vector2(MotionScrollRect.content.sizeDelta.x, MotionInfos.Length * 40 + (MotionInfos.Length - 1) * 10);

				if (_AttachEnvironmentManagement == null)
				{
					_AttachEnvironmentManagement = this.GetComponentInChildren<AttachEnvironmentManagement>(true);
				}






			}





			void ClearMotionInfo()
			{
				MotionInfos = new MotionInfo[0];
			}

			void RegistMotionInfo(string name, int val)
			{
				var list = MotionInfos.ToList();

				if (list.Any(obj => obj.Title.CompareTo(name) == 0))
				{
					var elem = list.Find(obj2 => obj2.Mode == val);
					if (elem != null)
					{
						elem.Mode = val;
					}
				}
				else
				{
					list.Add(new MotionInfo() { Title = name, Mode = val, });
				}

				list.Sort((x, y) => x.Mode.CompareTo(y.Mode));

				MotionInfos = list.ToArray();
			}






#if UNITY_EDITOR


			static void ScanMotionInformations(MotionViewer viewer)
			{

				var animCon = viewer.RefAnimator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;


				var ac = animCon;

				viewer.ClearMotionInfo();



				int layer_num = 0;
				foreach (UnityEditor.Animations.AnimatorControllerLayer layer in ac.layers)
				{

					var anyStats = layer.stateMachine.anyStateTransitions;
					//Debug.Log("entrys:" + anyStats.Length);
					foreach (var stat in anyStats)
					{
						//Debug.Log("dest name:" + stat.destinationState.name );
						int mode = -1;
						foreach (var condition in stat.conditions)
						{

							//Debug.Log("mode:" + condition.mode + "\t params:" + condition.parameter + "\t val:" + condition.threshold );

							if (condition.mode == UnityEditor.Animations.AnimatorConditionMode.Equals)
							{
								mode = (int)condition.threshold;
							}
						}

						if (0 <= mode)
						{
							viewer.RegistMotionInfo(stat.destinationState.name, mode);
						}

					}


#if false
				
					foreach(UnityEditor.Animations.ChildAnimatorState childAS in ac.layers[layer_num].stateMachine.states) {
						Debug.Log("レイヤー：" + layer.name + "\tステート：" + childAS.state.name + "\tTrasitions:" + childAS.state.transitions.Length);
						foreach (var trasition in childAS.state.transitions) {
							
						}
		 			}
#endif

					layer_num++;
				}

			}



			[ContextMenu("Scan Motion")]
			public void ScanMotionInformations()
			{

				var objs = GameObject.FindObjectsOfType(typeof(MonoBehaviour)); //returns Object[]

				foreach (var obj in objs)
				{
					if (obj is MotionViewer)
					{
						var viewerspt = obj as MotionViewer;
						ScanMotionInformations(viewerspt);
						break;
					}
				}

			}




			#region MonoBehaviour

			private void Reset()
			{
				PrefabCell = AssetDatabase.LoadAssetAtPath<Object>("Assets/CatStudio/Characters/Viewer/Prefabs/PanelMotionInfo.prefab");

				this.RefAnimator = GameObject.FindObjectOfType<Animator>();
				this.MotionScrollRect = GameObject.Find("Canvas-Motions").GetComponentInChildren<ScrollRect>(true);

			}


			#endregion

#endif

		}

	}
}
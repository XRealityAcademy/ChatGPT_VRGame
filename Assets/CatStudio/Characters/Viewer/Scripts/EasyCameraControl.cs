using UnityEngine;
using System.Collections;

namespace CatStudio.Character
{
	namespace Viewer
	{
		public class EasyCameraControl : MonoBehaviour
		{

			[SerializeField]
			Transform ControlTransformH;


			[SerializeField]
			Transform ControlTransformV;



			[SerializeField]
			float SpeedRate = 1.0f;



			#region	MonoBehaviour

			void Awake()
			{

			}

			// Use this for initialization
			void Start()
			{

			}

			// Update is called once per frame
			void Update()
			{

			}

			void LateUpdate()
			{

				var vValue = Input.GetAxis("Vertical");
				var hValue = Input.GetAxis("Horizontal");

				vValue *= SpeedRate;
				hValue *= SpeedRate;

				ControlTransformH.Rotate(new Vector3(0.0f, hValue, 0.0f));
				ControlTransformV.Rotate(new Vector3(vValue, 0.0f, 0.0f));

			}

			#endregion

		}

	}
}
using UnityEngine;
using System.Collections;
using System.Numerics;
using UnityEngine.Rendering.Universal.Internal;
using Vector3 = UnityEngine.Vector3;

namespace AstronautPlayer
{

	public class AstronautPlayer : MonoBehaviour {

		private Animator anim;
		private CharacterController controller;

		public float speed = 600.0f;
		public float turnSpeed = 400.0f;
		private Vector3 moveDirection;
		public float gravity = 20.0f;

		void Start () {
			controller = GetComponent <CharacterController>();
			anim = gameObject.GetComponentInChildren<Animator>();
		}

		void Update (){
			if (Input.GetKey ("w")) {
				anim.SetInteger ("AnimationPar", 1);
				controller.Move(moveDirection * Time.deltaTime);
				
			}  else {
				anim.SetInteger ("AnimationPar", 0);
			}

			if(controller.isGrounded){
				moveDirection = Vector3.forward * Input.GetAxis("Vertical") * speed;
			}

			float turn = Input.GetAxis("Horizontal");
			transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
			moveDirection.y -= gravity * Time.deltaTime;
		}
	}
}

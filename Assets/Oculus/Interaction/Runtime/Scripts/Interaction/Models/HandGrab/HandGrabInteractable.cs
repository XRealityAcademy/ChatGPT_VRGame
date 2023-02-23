/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.Input;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oculus.Interaction.HandGrab
{
    /// <summary>
    /// A HandGrabInteractable indicates the properties about how a hand can HandGrab an object.
    /// It specifies the fingers that must perform the grab and the release and generates the events
    /// for the Pointable to move the object.
    /// Optionally it can  reference a list of differently scaled HandGrabPoses to inform the
    /// interactor about the best pose the hand should adopt when grabbing the object with different
    /// sized hands.
    /// </summary>
    [Serializable]
    public partial class HandGrabInteractable : PointerInteractable<HandGrabInteractor, HandGrabInteractable>,
        IRigidbodyRef, IHandGrabbable, ICollidersRef, IRelativeToRef
    {
        [Header("Grab")]
        [SerializeField]
        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody;

        [SerializeField]
        private bool _resetGrabOnGrabsUpdated = true;
        public bool ResetGrabOnGrabsUpdated
        {
            get
            {
                return _resetGrabOnGrabsUpdated;
            }
            set
            {
                _resetGrabOnGrabsUpdated = value;
            }
        }

        [SerializeField, Optional]
        private PhysicsGrabbable _physicsGrabbable = null;

        [SerializeField]
        private PoseMeasureParameters _scoringModifier = new PoseMeasureParameters(0.8f);

        [Space]
        [SerializeField]
        private GrabTypeFlags _supportedGrabTypes = GrabTypeFlags.All;
        [SerializeField]
        private GrabbingRule _pinchGrabRules = GrabbingRule.DefaultPinchRule;
        [SerializeField]
        private GrabbingRule _palmGrabRules = GrabbingRule.DefaultPalmRule;

        [Header("Movement")]
        [SerializeField, Optional, Interface(typeof(IMovementProvider))]
        private MonoBehaviour _movementProvider;
        private IMovementProvider MovementProvider { get; set; }

        [SerializeField]
        private HandAlignType _handAligment = HandAlignType.AlignOnGrab;
        public HandAlignType HandAlignment
        {
            get
            {
                return _handAligment;
            }
            set
            {
                _handAligment = value;
            }
        }

        [SerializeField, Optional]
        [UnityEngine.Serialization.FormerlySerializedAs("_handGrabPoints")]
        private List<HandGrabPose> _handGrabPoses = new List<HandGrabPose>();
        public List<HandGrabPose> HandGrabPoses => _handGrabPoses;

        /// <summary>
        /// General getter for the transform of the object this interactable refers to.
        /// </summary>
        public Transform RelativeTo => _rigidbody.transform;

        public PoseMeasureParameters ScoreModifier => _scoringModifier;

        public GrabTypeFlags SupportedGrabTypes => _supportedGrabTypes;
        public GrabbingRule PinchGrabRules => _pinchGrabRules;
        public GrabbingRule PalmGrabRules => _palmGrabRules;

        public Collider[] Colliders { get; private set; }

        private GrabPoseFinder _grabPoseFinder;

        private static CollisionInteractionRegistry<HandGrabInteractor, HandGrabInteractable> _registry = null;

        #region editor events
        protected virtual void Reset()
        {
            _rigidbody = this.GetComponentInParent<Rigidbody>();

            Grabbable grabbable = this.GetComponentInParent<Grabbable>();
            if (grabbable != null)
            {
                InjectOptionalPointableElement(grabbable);
            }
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            if (_registry == null)
            {
                _registry = new CollisionInteractionRegistry<HandGrabInteractor, HandGrabInteractable>();
                SetRegistry(_registry);
            }
            MovementProvider = _movementProvider as IMovementProvider;
        }

        protected override void Start()
        {
            this.BeginStart(ref _started, () => base.Start());
            this.AssertField(Rigidbody, nameof(Rigidbody));
            Colliders = Rigidbody.GetComponentsInChildren<Collider>();
            this.AssertCollectionField(Colliders, nameof(Colliders),
                whyItFailed: $"The associated {nameof(Rigidbody)} must have at least one collider.");

            if (MovementProvider == null)
            {
                IMovementProvider movementProvider;
                movementProvider = this.gameObject.AddComponent<MoveTowardsTargetProvider>();
                InjectOptionalMovementProvider(movementProvider);
            }
            _grabPoseFinder = new GrabPoseFinder(_handGrabPoses);
            this.EndStart(ref _started);
        }

        #region pose moving

        public IMovement GenerateMovement(in Pose from, in Pose to)
        {
            IMovement movement = MovementProvider.CreateMovement();
            movement.StopAndSetPose(from);
            movement.MoveTo(to);
            return movement;
        }

        public void ApplyVelocities(Vector3 linearVelocity, Vector3 angularVelocity)
        {
            if (_physicsGrabbable == null)
            {
                return;
            }
            _physicsGrabbable.ApplyVelocities(linearVelocity, angularVelocity);
        }

        #endregion

        public bool CalculateBestPose(Pose userPose, float handScale, Handedness handedness,
            ref HandGrabResult result)
        {
            GrabPoseFinder.FindResult findResult = _grabPoseFinder.FindBestPose(userPose,
                handScale, handedness, _scoringModifier, ref result);
            if (findResult == GrabPoseFinder.FindResult.NotFound)
            {
                result.HasHandPose = false;
                result.Score = GrabPoseHelper.CollidersScore(userPose.position, Colliders, out Vector3 hit);
                Pose worldSnap = new Pose(hit, userPose.rotation);
                result.SnapPose = RelativeTo.Delta(worldSnap);
            }

            return findResult != GrabPoseFinder.FindResult.NotCompatible;
        }

        public bool UsesHandPose()
        {
            return _grabPoseFinder.UsesHandPose();
        }

        public bool SupportsHandedness(Handedness handedness)
        {
            return _grabPoseFinder.SupportsHandedness(handedness);
        }

        #region Inject

        public void InjectAllHandGrabInteractable(GrabTypeFlags supportedGrabTypes,
            Rigidbody rigidbody, PoseMeasureParameters scoreModifier,
            GrabbingRule pinchGrabRules, GrabbingRule palmGrabRules)
        {
            InjectSupportedGrabTypes(supportedGrabTypes);
            InjectRigidbody(rigidbody);
            InjectScoreModifier(scoreModifier);
            InjectPinchGrabRules(pinchGrabRules);
            InjectPalmGrabRules(palmGrabRules);
        }

        public void InjectSupportedGrabTypes(GrabTypeFlags supportedGrabTypes)
        {
            _supportedGrabTypes = supportedGrabTypes;
        }

        public void InjectPinchGrabRules(GrabbingRule pinchGrabRules)
        {
            _pinchGrabRules = pinchGrabRules;
        }

        public void InjectPalmGrabRules(GrabbingRule palmGrabRules)
        {
            _palmGrabRules = palmGrabRules;
        }

        public void InjectScoreModifier(PoseMeasureParameters scoreModifier)
        {
            _scoringModifier = scoreModifier;
        }

        public void InjectRigidbody(Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;
        }

        public void InjectOptionalPhysicsGrabbable(PhysicsGrabbable physicsGrabbable)
        {
            _physicsGrabbable = physicsGrabbable;
        }

        public void InjectOptionalHandGrabPoses(List<HandGrabPose> handGrabPoses)
        {
            _handGrabPoses = handGrabPoses;
        }

        public void InjectOptionalMovementProvider(IMovementProvider provider)
        {
            _movementProvider = provider as MonoBehaviour;
            MovementProvider = provider;
        }
        #endregion

    }
}

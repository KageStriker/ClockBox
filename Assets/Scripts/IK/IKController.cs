using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour
{
    private Animator anim;

    private Vector3 _lookAtPosition;
    
    private Vector3 _RightHand_Pos;
    private Vector3 _RightFoot_Pos;
    private Vector3 _LeftHand_Pos;
    private Vector3 _LeftFoot_Pos;

    private Quaternion _RightHand_Rot;
    private Quaternion _RightFoot_Rot;
    private Quaternion _LeftHand_Rot;
    private Quaternion _LeftFoot_Rot;

    private Vector3 _RightElbow;
    private Vector3 _LeftElbow;
    private Vector3 _RightKnee;
    private Vector3 _LeftKnee;

    private  float _headWeight = 1;

    private float _RightHandWeight = 0;
    private float _LeftHandWeight = 0;
    private float _RightFootWeight = 0;
    private float _LeftFootWeight = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _lookAtPosition = GameObject.Find("LookAtPosition").transform.position;
    }

    private void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        _lookAtPosition = new Vector3(ray.GetPoint(20).x, transform.position.y + 1.9f, ray.GetPoint(20).z);

        if (_RightHandWeight == 0 && _LeftHandWeight == 0 && _RightFootWeight == 0 && _LeftFootWeight == 0) {
            SetInitialIKPositions(null, null, null, null);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (layerIndex == 0)
        {
            _RightElbow = transform.position + transform.up + transform.right * 2.0f;
            _LeftElbow = transform.position + transform.up - transform.right * 2.0f;
            _RightKnee = transform.position + transform.up * 2f + transform.forward * 2f + transform.right;
            _LeftKnee = transform.position + transform.up * 2f + transform.forward * 2f - transform.right;
            
            anim.SetLookAtWeight(_headWeight, 0.5f, 1.0f, 1.0f, 1.0f);
            anim.SetLookAtPosition(_lookAtPosition);


            //  --Inverse Kinimatics for Arms--
            //RightHand
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, _RightHandWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, _RightHandWeight);
            anim.SetIKPosition(AvatarIKGoal.RightHand, _RightHand_Pos);
            anim.SetIKRotation(AvatarIKGoal.RightHand, _RightHand_Rot);

            //LeftHand
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, _LeftHandWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, _LeftHandWeight);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, _LeftHand_Pos);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, _LeftHand_Rot);

            //RightElbow
            anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, RightHandWeight);
            anim.SetIKHintPosition(AvatarIKHint.RightElbow, _RightElbow);

            //LeftElbow
            anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, _LeftHandWeight);
            anim.SetIKHintPosition(AvatarIKHint.LeftElbow, _LeftElbow);

            //  --Inverse Kinimatics for Legs--
            //RightFoot
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, _RightFootWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, _RightFootWeight);
            anim.SetIKPosition(AvatarIKGoal.RightFoot, _RightFoot_Pos);
            anim.SetIKRotation(AvatarIKGoal.RightFoot, _RightFoot_Rot);
            

            //LeftFoot
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _LeftFootWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _LeftFootWeight);
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, _LeftFoot_Pos);
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, _LeftFoot_Rot);

            //RightKnee
            anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, _RightFootWeight);
            anim.SetIKHintPosition(AvatarIKHint.RightKnee, _RightKnee);

            //LeftKnee
            anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, _LeftFootWeight);
            anim.SetIKHintPosition(AvatarIKHint.LeftKnee, _LeftKnee);
        }
    }

    public void SetInitialIKPositions(Transform rightH, Transform leftH, Transform rightF, Transform leftF)
    {
        if (rightH && RightHandWeight > 0)
        {
            _RightHand_Pos = rightH.position;
            _RightHand_Rot = rightH.rotation;
        }
        else
        {
            Transform righthand = anim.GetBoneTransform(HumanBodyBones.RightHand);
            _RightHand_Pos = righthand.position;
            _RightHand_Rot = righthand.rotation;
        }

        if (leftH && LeftHandWeight > 0)
        {
            _LeftHand_Pos = leftH.position;
            _LeftHand_Rot = leftH.rotation;
        }
        else
        {
            Transform lefthand = anim.GetBoneTransform(HumanBodyBones.LeftHand);
            _LeftHand_Pos = lefthand.position;
            _LeftHand_Rot = lefthand.rotation;
        }

        if (rightF && RightFootWeight > 0)
        {
            _RightFoot_Pos = rightF.position;
            _RightFoot_Rot = rightF.rotation;
        }
        else
        {
            Transform rightfoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);
            _RightFoot_Pos = rightfoot.position;
            _RightFoot_Rot = rightfoot.rotation;
        }

        if (leftF && LeftFootWeight > 0)
        {
            _LeftFoot_Pos = leftF.position;
            _LeftFoot_Rot = leftF.rotation;
        }
        else
        {
            Transform leftfoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
            _LeftFoot_Pos = leftfoot.position;
            _LeftFoot_Rot = leftfoot.rotation;
        }
    }

    public bool MoveIKTarget(Transform IKTarget, Transform NewTarget, float duration)
    {
        if (IKTarget.position != NewTarget.position) {
            //move IKtransform toward Target
            IKTarget.position = Vector3.MoveTowards(IKTarget.position, NewTarget.position, duration);
            IKTarget.rotation = Quaternion.Lerp(IKTarget.rotation, NewTarget.rotation, duration * 4);
            return false;
        }
        else return true;
    }

    public bool MoveRightHand(Transform NewTarget, float duration)
    {
        if (_RightHand_Pos != NewTarget.position)
        {
            //move IKtransform toward Target
            _RightHand_Pos = Vector3.MoveTowards(_RightHand_Pos, NewTarget.position, duration);
            _RightHand_Rot = Quaternion.Lerp(_RightHand_Rot, NewTarget.rotation, duration * 4);
            return false;
        }
        else return true;
    }

    public bool MoveRightFoot(Transform NewTarget, float duration)
    {
        if (_RightFoot_Pos != NewTarget.position)
        {
            //move IKtransform toward Target
            _RightFoot_Pos = Vector3.MoveTowards(_RightFoot_Pos, NewTarget.position, duration);
            _RightFoot_Rot = Quaternion.Lerp(_RightFoot_Rot, NewTarget.rotation, duration * 4);
            return false;
        }
        else return true;
    }
    public bool MoveLeftHand(Transform NewTarget, float duration)
    {
        if (_LeftHand_Pos != NewTarget.position)
        {
            //move IKtransform toward Target
            _LeftHand_Pos = Vector3.MoveTowards(_LeftHand_Pos, NewTarget.position, duration);
            _LeftHand_Rot = Quaternion.Lerp(_LeftHand_Rot, NewTarget.rotation, duration * 4);
            return false;
        }
        else return true;
    }
    public bool MoveLeftFoot(Transform NewTarget, float duration)
    {
        if (_LeftFoot_Pos != NewTarget.position)
        {
            //move IKtransform toward Target
            _LeftFoot_Pos = Vector3.MoveTowards(_LeftFoot_Pos, NewTarget.position, duration);
            _LeftFoot_Rot = Quaternion.Lerp(_LeftFoot_Rot, NewTarget.rotation, duration * 4);
            return false;
        }
        else return true;
    }

    public bool MoveIKTargets(Vector3 IKTarget1_Pos, Quaternion IKTarget1_Rot, Vector3 IKTarget2_Pos, Quaternion IKTarget2_Rot, Transform NewTarget1, Transform NewTarget2, float duration1,float duration2)
    {
        //move IKtransform1 toward Target1
        IKTarget1_Pos = Vector3.MoveTowards(IKTarget1_Pos, NewTarget1.position, duration1);
        IKTarget1_Rot = Quaternion.Lerp(IKTarget1_Rot, NewTarget1.rotation, duration1);

        //move IKtransform2 toward Target2
        IKTarget2_Pos = Vector3.MoveTowards(IKTarget2_Pos, NewTarget2.position, duration2);
        IKTarget2_Rot = Quaternion.Lerp(IKTarget2_Rot, NewTarget2.rotation, duration2);

        Debug.Log(IKTarget1_Pos +"  :  "+NewTarget1.position +"  :  "+ duration1);

        if (IKTarget1_Pos == NewTarget1.position && IKTarget2_Pos == NewTarget2.position)
        {
            Debug.Log("Right True");
            return true;
        }
        else return false;
    }

    public bool MoveRoot(Transform NewTarget, float duration, Vector3 Offset)
    {
        //move IKtransform toward Target
        transform.position = Vector3.MoveTowards(transform.position, NewTarget.position + Offset, duration);
        transform.rotation = Quaternion.Lerp(transform.rotation, NewTarget.rotation, duration);

        if (transform.position == NewTarget.position)
            return true;
        else return false;
    }

    //Get Set lookAtPosition
    public Vector3 lookAtPosition
    {
        set { _lookAtPosition = value; }
        get { return _lookAtPosition; }
    }

    //  ---Get/set IKTargets
    //Get Set Hand & Foot
    //Positions
    public Vector3 RightHandPosition
    {
        set { _RightHand_Pos = value; }
        get { return _RightHand_Pos; }
    }
    public Vector3 LeftHandPosition
    {
        set { _LeftHand_Pos = value; }
        get { return _LeftHand_Pos; }
    }
    public Vector3 RightFootPosition
    {
        set { _RightFoot_Pos = value; }
        get { return _RightFoot_Pos; }
    }
    public Vector3 LeftFootPosition
    {
        set { _LeftFoot_Pos = value; }
        get { return _LeftFoot_Pos; }
    }

    //Rotations
    public Quaternion RightHandRotation
    {
        set { _RightHand_Rot = value; }
        get { return _RightHand_Rot; }
    }
    public Quaternion LeftHandRotation
    {
        set { _LeftHand_Rot = value; }
        get { return _LeftHand_Rot; }
    }
    public Quaternion RightFootRotation
    {
        set { _RightFoot_Rot = value; }
        get { return _RightFoot_Rot; }
    }
    public Quaternion LeftFootRotation
    {
        set { _LeftFoot_Rot = value; }
        get { return _LeftFoot_Rot; }
    }

    //Get/Set Elbow & Knee
    public Vector3 RightElbow
    {
        set { _RightElbow = value; }
        get { return _RightElbow; }
    }
    public Vector3 LeftElbow
    {
        set { _LeftElbow = value; }
        get { return _LeftElbow; }
    }
    public Vector3 RightKnee
    {
        set { _RightKnee = value; }
        get { return _RightKnee; }
    }
    public Vector3 LeftKnee
    {
        set { _LeftKnee = value; }
        get { return _LeftKnee; }
    }

    //  ---Get/set IKWieghts
    //Get/Set Elbow & Knee
    public float GlobalWeight
    {
        set
        {
            _RightHandWeight = value;
            _LeftHandWeight = value;
            _RightFootWeight = value;
            _LeftFootWeight = value;
        }
    }

    public float headWeight
    {
        set { _headWeight = value; }
        get { return _headWeight; }
    }
    public float RightHandWeight
    {
        set { _RightHandWeight = value; }
        get { return _RightHandWeight; }
    }
    public float LeftHandWeight
    {
        set { _LeftHandWeight = value; }
        get { return _LeftHandWeight; }
    }
    public float RightFootWeight
    {
        set { _RightFootWeight = value; }
        get { return _RightFootWeight; }
    }
    public float LeftFootWeight
    {
        set { _LeftFootWeight = value; }
        get { return _LeftFootWeight; }
    }
}

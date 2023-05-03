using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimationManager : MonoBehaviourPun
{
    // -----------------------------------------
    #region Private Fields

    [SerializeField]
    private float directionDampTime = 0.25f;

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region MonoBehaviour Callbacks

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        if(!anim)
        {
            Debug.LogError("PlayerAnimationManager is missing Animator component", this);
        }
    }

    void Update()
    {
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        if(!anim)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (v < 0f) v = 0f;

        anim.SetFloat("Speed", h * h + v * v);
        anim.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
    }

    #endregion
    // -----------------------------------------

}

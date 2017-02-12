/*
using UnityEngine;
using System.Collections;
using SplatterSystem.Platformer;
using SplatterSystem;

public class PlayerSplatter : MonoBehaviour
{
    public SplatterSettings         m_SplatterSettings;
    public MeshSplatterManager      m_SplatterMeshManager;
    public bool                     m_UsePaint = true;
    public float                    m_PaintTimeout = 0.05f;
    public float                    m_PaintPositionOffset = 0;
    private float                   m_LastSplatterTime;

    private GroundState             _groundState;
    //private Player                  _player;

    void Awake()
    {
      //  _groundState = new GroundState(GetComponent<Player>().m_PlayerTransform.gameObject);
       // _player = GetComponent<Player>();
        m_LastSplatterTime = Time.time;
    }

    int i = 0;

    void _FixedUpdate()
    {
        if ( i++ < 5 )
            return;

        i = 0;
        
        // Paint.
        if (m_UsePaint && (Time.time - m_LastSplatterTime > m_PaintTimeout) && _groundState.IsTouching())// && _player.m_PositionState == Player.PositionState.ON_INNER_CIRCLE)
        {
            m_LastSplatterTime = Time.time;

         //   m_SplatterMeshManager.Spawn(m_SplatterSettings, _player.m_PlayerTransform.position, Vector3.zero - _player.m_PlayerTransform.position, m_SplatterSettings.startColor);

        }
    }
}
*/
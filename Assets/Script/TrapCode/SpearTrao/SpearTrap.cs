using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
  private CapsuleCollider2D _SpearTrapCollider2d;
    void Start()
    {
        // Gán CapsuleCollider2D của SpearTrap khi bắt đầu
        _SpearTrapCollider2d = GetComponent<CapsuleCollider2D>();

    }
    public void DisableColider()
    {
        _SpearTrapCollider2d.enabled = false;
    }

    public void EnableColider()
    {
        _SpearTrapCollider2d.enabled = true;
    }


}

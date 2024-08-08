using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAnimationsNames
{
   public static readonly int Iddle = Animator.StringToHash("Iddle");
   public static readonly int Run = Animator.StringToHash("Move");
   public static readonly int Attack = Animator.StringToHash("Attack");
   public static readonly int Landing = Animator.StringToHash("Landing");
   public static readonly int ChargingAttack = Animator.StringToHash("ChargingAttack");
   //public static readonly int Mount = Animator.StringToHash("Mounting");
   public static readonly int BackDash = Animator.StringToHash("BackDash");
   public static readonly int ForwardDash = Animator.StringToHash("ForwardDash");


}

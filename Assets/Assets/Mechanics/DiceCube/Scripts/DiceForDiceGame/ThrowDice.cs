using System.Collections;
using UnityEngine;

public class ThrowDice : Dice
{
    [SerializeField] float angularVelocityPower;

    public PlayerType Owner { get; private set; }


    public void SetOwner(PlayerType playerType)
    {
        Owner = playerType;
    }

    public void AddRadomTorque() 
    {
        rb.angularVelocity = Random.insideUnitSphere * angularVelocityPower;
    }
}
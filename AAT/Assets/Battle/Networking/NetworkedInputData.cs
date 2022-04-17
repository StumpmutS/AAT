using Fusion;
using UnityEngine;

public struct NetworkedInputData : INetworkInput
{
    public int Buttons;
    public Vector2 MousePosition;
}

public static class NetworkedInputMapping
{
    public const int MOUSEBUTTON0_DOWN = 1 << 0;
    public const int MOUSEBUTTON0_UP = 1 << 1;
    public const int MOUSEBUTTON1_DOWN = 1 << 2;
    public const int MOUSEBUTTON1_UP = 1 << 3;
    public const int MOUSEBUTTON2_DOWN = 1 << 4;
    public const int MOUSEBUTTON2_UP = 1 << 5;

    public const int W_DOWN = 1 << 6;
    public const int A_DOWN = 1 << 7;
    public const int S_DOWN = 1 << 8;
    public const int D_DOWN = 1 << 9;
    public const int SPACE_DOWN = 1 << 10;
    public const int SHIFT_DOWN = 1 << 11;

    public const int ALPHA0_DOWN = 1 << 12;
    public const int ALPHA1_DOWN = 1 << 13;
    public const int ALPHA2_DOWN = 1 << 14;
    public const int ALPHA3_DOWN = 1 << 15;
    public const int ALPHA4_DOWN = 1 << 16;
    public const int ALPHA5_DOWN = 1 << 17;
    public const int ALPHA6_DOWN = 1 << 18;
    public const int ALPHA7_DOWN = 1 << 19;
    public const int ALPHA8_DOWN = 1 << 20;
    public const int ALPHA9_DOWN = 1 << 21;
}
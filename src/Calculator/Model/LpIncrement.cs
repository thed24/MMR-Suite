﻿namespace Calculator.Model
{
    public class LpIncrement
    {
        public bool IsLpGain { get; }
        public int Value { get; }
        public LpIncrement(bool isLpGain, int value)
        {
            IsLpGain = isLpGain;
            Value = value;
        }
    }
}
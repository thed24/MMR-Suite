namespace Calculator.Model
{
    public class LpIncrement
    {
        public LpIncrement(bool isLpGain, int value)
        {
            IsLpGain = isLpGain;
            Value = value;
        }

        public bool IsLpGain { get; }
        public int Value { get; }
    }
}
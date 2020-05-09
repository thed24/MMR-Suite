namespace Calculator.Model
{
    public class LpIncrement
    {
        public bool IsLpGain { get; }
        public int Value { get; }

        public LpIncrement(bool isLpGain, int value)
        {
            this.IsLpGain = isLpGain;
            this.Value = value;
        }
    }
}
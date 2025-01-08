using Game.UnityFFB.NativeEffects;

namespace Game.UnityFFB.Structures
{
    public static class EffectBuilder
    {
        public static HapticCondition SetDirection(this HapticCondition condition, HapticDirection direction)
        {
            condition.direction = direction;
            return condition;
        }

        public static HapticCondition SetDeadband0(this HapticCondition condition, ushort deadband)
        {
            const ushort zero = 0;
            condition.deadband = new [] { deadband, zero, zero };
            return condition;
        }

        public static HapticCondition SetLeftCoeff0(this HapticCondition condition, short leftCoeff)
        {
            const short zero = 0;
            condition.left_coeff = new[] { leftCoeff, zero, zero };
            return condition;
        }

        public static HapticCondition SetRightCoeff0(this HapticCondition condition, short rightCoeff)
        {
            const short zero = 0;
            condition.right_coeff = new[] { rightCoeff, zero, zero };
            return condition;
        }

        public static HapticCondition SetLeftSat0(this HapticCondition condition, ushort leftSat)
        {
            const ushort zero = 0;
            condition.left_sat = new[] { leftSat, zero, zero };
            return condition;
        }

        public static HapticCondition SetRightSat0(this HapticCondition condition, ushort rightSat)
        {
            const ushort zero = 0;
            condition.right_sat = new[] { rightSat, zero, zero };
            return condition;
        }
    }
}
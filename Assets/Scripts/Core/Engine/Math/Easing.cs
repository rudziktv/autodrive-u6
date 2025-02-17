namespace Core.Engine.Math
{
    public static class Easing
    {
        public static float Linear(float t)
            => t;
        public static float EaseInOut(float t)
            => t * t * (3f - 2f * t);
    }
}
namespace PcRemoteControl.Animations
{
    internal static class LoadingStatusBackgroundAnimation
    {
        private static string AnimationName = "GradientMove";
        private static Color BackgroundColor = Color.FromRgba("#5CDEFF");
        private static Color HighlightColor = Color.FromRgba("#FFFFFF");

        public static Task<bool> MoveGradientStop(this VisualElement self, Action<LinearGradientBrush> callback, uint rate = 16, uint length = 2500, Easing? easing = null)
        {
            Func<double, LinearGradientBrush> transform = (t) =>
                new LinearGradientBrush(new GradientStopCollection()
                {
                    new GradientStop(BackgroundColor, 0.0f),
                    new GradientStop(HighlightColor, (float)t),
                    new GradientStop(BackgroundColor, (float)t + 0.05f),
                    new GradientStop(BackgroundColor, 1.0f),
                });
            return GradientAnimation(self, AnimationName, transform, callback, rate, length, easing);
        }

        public static void CancelAnimation(this VisualElement self)
        {
            self.AbortAnimation(AnimationName);
        }

        private static Task<bool> GradientAnimation(VisualElement element, string name, Func<double, LinearGradientBrush> transform, Action<LinearGradientBrush> callback, uint rate, uint length, Easing? easing)
        {
            easing = easing ?? Easing.CubicInOut;
            bool cancelled = false;

            element.Animate<LinearGradientBrush>(name, transform, callback, rate, length, easing, (v, c) => cancelled = c, () => true);

            var taskCompletionSource = new TaskCompletionSource<bool>();
            taskCompletionSource.SetResult(cancelled);
            return taskCompletionSource.Task;
        }
    }
}

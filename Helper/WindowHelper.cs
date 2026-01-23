namespace EShopNative.Helper
{
    public static class WindowHelper
    {
        public static Window GetCurrentWindow()
        {
            // Safely get the first window
            var window = Application.Current?.Windows.FirstOrDefault();

            if (window is null)
                throw new InvalidOperationException("No active window found.");

            return window;
        }

        public static Page GetCurrentPage()
        {
            var window = GetCurrentWindow();

            if (window.Page is null)
                throw new InvalidOperationException("The current window has no root page.");

            return window.Page;
        }
    }
}


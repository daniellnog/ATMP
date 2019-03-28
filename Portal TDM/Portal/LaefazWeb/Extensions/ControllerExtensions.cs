using System.Web.Mvc;

namespace TDMWeb.Extensions
{
    public static class ControllerExtensions
    {
        #region FlashMessenger

        public static void FlashSuccess(this Controller controller, string message)
        {
            controller.TempData["success"] = message;
        }
        public static void FlashWarning(this Controller controller, string message)
        {
            controller.TempData["warning"] = message;
        }
        public static void FlashError(this Controller controller, string message)
        {
            controller.TempData["error"] = message;
        }

        #endregion
    }
}
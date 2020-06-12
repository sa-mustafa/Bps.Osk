namespace Bps.Osk
{
    using System.Windows;

    public class Bootstrapper : Stylet.Bootstrapper<object>
    {
        public static Bootstrapper Instance
        {
            get
            {
                if (bootstrapper != null)
                    return bootstrapper;

                bootstrapper = new Bootstrapper();
                bootstrapper.Setup(Application.Current);
                bootstrapper.Configure();
                return bootstrapper;
            }
        }
        private static Bootstrapper bootstrapper;

        protected static Stylet.IViewManager ViewManager
        {
            get
            {
                if (viewManager != null)
                    return viewManager;

                viewManager = Instance.GetInstance(typeof(Stylet.IViewManager)) as Stylet.IViewManager;
                return viewManager;
            }
        }
        private static Stylet.IViewManager viewManager;

        protected static Stylet.IWindowManager WindowManager
        {
            get
            {
                if (windowManager != null)
                    return windowManager;

                windowManager = Instance.GetInstance(typeof(Stylet.IWindowManager)) as Stylet.IWindowManager;
                return windowManager;
            }
        }
        private static Stylet.IWindowManager windowManager;

        public new void Configure()
        {
            ConfigureBootstrapper();
        }

        public Window CreateWindow(object viewModel)
        {
            var view = ViewManager.CreateAndBindViewForModelIfNecessary(viewModel);
            return view as Window;
        }
    }
}
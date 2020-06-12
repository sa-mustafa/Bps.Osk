namespace Bps.Osk.Sample.ViewModels
{
    using System;
    using System.Configuration;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows;

    public class LoginViewModel : Stylet.Screen
    {
        //private Stylet.IWindowManager windowManager;

        //public LoginViewModel(Stylet.IWindowManager windowManager)
        //{
        //    this.windowManager = windowManager;
        //}

        #region Public Properties

        public string Account
        {
            get { return account; }
            set
            {
                //SetAndNotify(ref account, value);
                if (string.IsNullOrEmpty(Account))
                    throw new ArgumentException("Account is empty!");
            }
        }
        private string account;

        public bool CheckLogin { get; set; } = true;

        public bool HighestAuthority { get; set; }

        public int Level { get; private set; } = -1;

        public string Password
        {
            get { return password; }
            set
            {
                //SetAndNotify(ref password, value);
                if (string.IsNullOrEmpty(Password))
                    throw new ArgumentException("Password is empty!");
            }
        }
        private string password;

        public int RequiredLevel { get; set; } = 1;

        #endregion

        #region Methods

        //protected override Task<bool> ValidatePropertyAsync([CallerMemberName] string propertyName = null)
        //{
        //    switch (propertyName)
        //    {
        //        case nameof(Account):
        //            if (string.IsNullOrEmpty(Account))
        //                return Task.FromResult(false);
        //            break;
        //        case nameof(Password):
        //            if (string.IsNullOrEmpty(Password))
        //                return Task.FromResult(false);
        //            break;
        //        default:
        //            break;
        //    }

        //    return base.ValidatePropertyAsync(propertyName);
        //}

        public void OkClick()
        {
            try
            {
                if ((!HighestAuthority && string.IsNullOrEmpty(Account)) || string.IsNullOrEmpty(Password))
                {
                    //windowManager.ShowMessageBox("Please enter account & password.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!CheckLogin)
                {
                    //RequestClose(true);
                    return;
                }

                string entry = ConfigurationManager.AppSettings[HighestAuthority ? "_super_" : Account];
                if (string.IsNullOrEmpty(entry))
                {
                    //windowManager.ShowMessageBox("Account not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                var parts = entry.Split(',');
                if (parts?.Length != 2)
                {
                    //windowManager.ShowMessageBox("App.config file is corrupt!", "Fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int level = int.Parse(parts[0]);
                var password = parts[1];
                if (!Password.Equals(password))
                {
                    //windowManager.ShowMessageBox("Wrong password!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (level < RequiredLevel)
                {
                    //windowManager.ShowMessageBox("Low security level!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                Level = level;
                //RequestClose(true);
            }
            catch (Exception ex)
            {
                //Logger.Instance.AddError(nameof(LoginViewModel), "OkClick", ex.Message);
            }
        }

        #endregion

    }
}

using System.Collections;
using System.Globalization;

using UnityEngine;

using CFM.Framework.Example;
using CFM.Framework.Views;
using CFM.Framework.Contexts;
using CFM.Framework.Messaging;
using CFM.Framework.Services;
using CFM.Framework.Binding;
using CFM.Framework.Localizations;

public class Launcher : MonoBehaviour
{
    private ApplicationContext context;

    ISubscription<WindowStateEventArgs> subscription;

    void Awake()
    {
        GlobalWindowManager windowManager = FindObjectOfType<GlobalWindowManager>();
        if (windowManager == null)
            throw new NotFoundException("");

        context = ContextManager.GetApplicationContext();

        IServiceContainer container = context.GetContainer();

        BindingServiceBundle bundle = new BindingServiceBundle(container);
        bundle.Start();

        container.Register<IUIViewLocator>(new ResourcesViewLocator());

        CultureInfo cultureInfo = Locale.GetCultureInfo();
        var localization = Localization.Current;
        localization.CultureInfo = cultureInfo;
        localization.AddDataProvider(new ResourcesDataProvider("", new XmlDocumentParser()));

        container.Register<Localization>(localization);

        IAccountRepository accountRepository = new AccountRepository();
        container.Register<IAccountService>(new AccountService(accountRepository));

        GlobalSetting.enableWindowStateBroadcast = true;

        GlobalSetting.useBlockRaycastsInsteadOfInteractable = true;

        subscription = Window.Messenger.Subscribe<WindowStateEventArgs>(e =>
        {
            Debug.LogFormat("{0}{1}{2}", e.Window.Name, e.OldState, e.State);
        });
    }

    IEnumerator Start() {
        WindowContainer windowContainer = WindowContainer.Create("MAIN");

        yield return null;

        IUIViewLocator locator = context.GetService<IUIViewLocator>();
        StartupWindow window = locator.LoadWindow<StartupWindow>(windowContainer, "");
        window.Create();
        ITransition transition = window.Show().OnStateChanged((w, state) => {
            Debug.LogFormat("{0}{1}", w.Name, state);
        });

        yield return transition.WaitForDone();
    }
}

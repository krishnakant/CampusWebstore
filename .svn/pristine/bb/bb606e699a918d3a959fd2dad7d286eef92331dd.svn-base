// ReSharper disable AssignNullToNotNullAttribute

namespace CampusWebSotre.Utils
{
    using System;
    using System.Web;

    using Microsoft.Practices.Unity;

    public class HttpContextLifetimeManager<T> : LifetimeManager, IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {
            RemoveValue();
        }

        #endregion

        public override object GetValue()
        {
            return HttpContext.Current.Items[typeof (T).AssemblyQualifiedName];
        }

        public override void RemoveValue()
        {
            HttpContext.Current.Items.Remove(typeof (T).AssemblyQualifiedName);
        }

        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[typeof (T).AssemblyQualifiedName] = newValue;
        }
    }
}
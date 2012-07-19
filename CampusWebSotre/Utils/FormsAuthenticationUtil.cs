namespace CampusWebSotre.Utils
{
    using System;
    using System.Web.Security;

   

    public interface IFormsAuthenticationUtil
    {
        #region Public Methods

        
        void SignOut();

        #endregion
    }

    public class FormsAuthenticationUtil : IFormsAuthenticationUtil
    {
        #region Implemented Interfaces

        #region IFormsAuthenticationUtil

        //public void SignIn(GuidId<UserProfile> profileId, bool createPersistentCookie)
        //{
        //    if (profileId == null)
        //    {
        //        throw new ArgumentNullException("profileId");
        //    }

        //    // TODO record analytics that use signed in
        //    FormsAuthentication.SetAuthCookie(profileId.ToString(), createPersistentCookie);
        //}

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        #endregion

        #endregion
    }
}
using System.Net.Mail;
using System.Collections;
using System.IO;
using System.Linq;
using CampusWebStore.Shared.Models;

namespace CampusWebStore.Data.Entities
{
    public interface IWebStoreEntityModels
    {

        IQueryable<SectionModel> UserSpecialityProfiles { get; }

        TextWriter Log { get; set; }

        void SaveChanges();

        void ClearChanges();

      
        void Delete(IEnumerable entities);

        void Delete(object entity);

        void Add(IEnumerable entities);

        void Add(object entity);

        void MakeDirty(object entity, string fieldName);

        void Dispose();
    }
}

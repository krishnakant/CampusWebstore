namespace CampusWebSotre.Models
{
    public class Pager
    {
        #region Properties for Pagging

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public Pager()
        {
            Page = 0;

            PageSize = 10;
        }
        public int ToatalPages()
        {
            var pagecount = TotalRecords / PageSize;

            if (TotalRecords % PageSize > 0)

                pagecount++;

            return pagecount;
        }

        #endregion
    }


}
namespace CampusWebSotre.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;



    public static class DateUtil
    {

        public static List<SelectListItem> GetDayList(string selectedItem)
        {
            var daysList = new List<SelectListItem>();

            for (int i = 1; i < 32; i++)
            {

                daysList.Add(
                    new SelectListItem
                    {
                        Selected = i.ToString() == selectedItem,
                        Text = i.ToString(),
                        Value = i.ToString()
                    });

            }

            return daysList;
        }

        public static List<SelectListItem> GetMonthsList(string selectedItem)
        {
            var monthsList = new List<SelectListItem>();
            var sDate = new DateTime(2010, 1, 1);
            for (int i = 0; i < 12; i++)
            {
                monthsList.Add(
                    new SelectListItem
                    {
                        Selected = (i + 1).ToString() == selectedItem,
                        Text = sDate.ToString("MMMM"),
                        Value = (i + 1).ToString()
                    });
                sDate = sDate.AddMonths(1);
            }

            return monthsList;
        }

        public static List<SelectListItem> GetYearsList(string selectedItem)
        {
            var yearsList = new List<SelectListItem>();
            var sDate = DateTime.Now;
            sDate = sDate.AddYears(-13);
            for (int i = 1; i <= 80; i++)
            {
                yearsList.Add(
                    new SelectListItem
                    {
                        Selected = sDate.Year.ToString() == selectedItem,
                        Text = sDate.Year.ToString(),
                        Value = sDate.Year.ToString()
                    });
                sDate = sDate.AddYears(-1);
            }

            return yearsList;
        }

        public static string FormatPeriod(DateTime startDate, DateTime endDate)
        {
            return FormatDate(startDate) + " to " + FormatDate(endDate);
        }

        public static DateTime GetFirstDayOfYear(DateTime now)
        {
            return new DateTime(now.Year, 1, 1);
        }

        public static DateTime GetLastDayOfYear(DateTime now)
        {
            return new DateTime(now.Year, 12, 31);
        }

        public static DateTime GetLastDayOfQuarter(DateTime now)
        {
            if (now.Month <= 3)
            {
                return new DateTime(now.Year, 3, 31);
            }
            if (now.Month <= 6)
            {
                return new DateTime(now.Year, 6, 30);
            }
            return now.Month <= 9 ? new DateTime(now.Year, 9, 30) : new DateTime(now.Year, 12, 31);
        }

        public static DateTime GetLastDayOfHalfYear(DateTime now)
        {
            if (now.Month <= 6)
            {
                return new DateTime(now.Year, 6, 30);
            }
            return new DateTime(now.Year, 12, 31);
        }

        public static DateTime GetFirstDayOfHalfYear(DateTime now)
        {
            if (now.Month <= 6)
            {
                return new DateTime(now.Year, 6, 1);
            }
            return new DateTime(now.Year, 12, 1);
        }
        //public static DateTime GetDefaultDate(EFrequency eFrequency)
        //{
        //    DateTime date;
        //    var now = DateTime.Now;
        //    switch (eFrequency)
        //    {
        //        case EFrequency.Annually:
        //            date = GetLastDayOfYear(now);
        //            break;
        //        case EFrequency.BiAnnually:
        //            date = GetLastDayOfHalfYear(now);
        //            break;
        //        case EFrequency.Monthly:
        //            date = GetLastDayOfMonth(now);
        //            break;
        //        case EFrequency.Quarterly:
        //            date = GetLastDayOfQuarter(now);
        //            break;

        //        case EFrequency.Once:
        //            date = GetMaxDate();
        //            break;
        //        case EFrequency.Daily:
        //            date = DateTime.Now;
        //            break;
        //        default:
        //            date = DateTime.Now;
        //            break;
        //    }

        //    return date;
        //}

        public static DateTime GetFirstDayOfQuarter(DateTime now)
        {
            if (now.Month <= 3)
            {
                return new DateTime(now.Year - 1, 1, 1);
            }
            if (now.Month <= 6)
            {
                return new DateTime(now.Year, 4, 1);
            }
            return now.Month <= 9 ? new DateTime(now.Year, 7, 1) : new DateTime(now.Year, 10, 1);
        }

        public static DateTime GetFirstDayOfMonth(DateTime now)
        {
            return new DateTime(now.Year, now.Month, 1);
        }
        public static DateTime GetLastDayOfMonth(DateTime now)
        {
            if (now.Month == 2)
            {
                return new DateTime(now.Year, now.Month, 28);
            }
            else
            {
                DateTime tempDate = new DateTime(now.Year, now.Month, 1);
                tempDate = tempDate.AddMonths(1);
                tempDate = tempDate.AddDays(-1);
                return tempDate;
            }
        }

        public static string RelativeDate(DateTime date)
        {
            var timespan = DateTime.Now.Subtract(date);
            if (timespan.Days > 1)
                return date.ToString();
            if (timespan.Hours > 1)
                return string.Format("Over {0} hours ago.", timespan.Hours);
            if (timespan.Minutes > 1)
                return string.Format("{0} minutes ago.", timespan.Minutes);
            return string.Format("{0} seconds ago.", timespan.Seconds);
        }

        public static int CalculateAge(DateTime birthdate)
        {
            int years = DateTime.Now.Year - birthdate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (DateTime.Now.Month < birthdate.Month || (DateTime.Now.Month == birthdate.Month && DateTime.Now.Day < birthdate.Day))
                years--;
            return years;
        }

        public static IDictionary<string, DateTime> GetQuarters(int startYear, int endYear, bool descending)
        {
            bool greaterYear = startYear <= endYear;
            if (!greaterYear) throw new ArgumentException("End year must be greater than or equal to start year.");

            var quarters = new Dictionary<string, DateTime>();
            if (descending)
            {
                for (int year = endYear; year >= startYear; year--)
                {
                    var q4 = new DateTime(year, 12, 31);
                    quarters.Add(year + " - Quarter 4", q4);

                    var q3 = new DateTime(year, 9, 30);
                    quarters.Add(year + " - Quarter 3", q3);

                    var q2 = new DateTime(year, 6, 30);
                    quarters.Add(year + " - Quarter 2", q2);

                    var q1 = new DateTime(year, 3, 31);
                    quarters.Add(year + " - Quarter 1", q1);
                }
            }
            else
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    var q1 = new DateTime(year, 3, 31);
                    quarters.Add(year + " - Quarter 1", q1);

                    var q2 = new DateTime(year, 6, 30);
                    quarters.Add(year + " - Quarter 2", q2);

                    var q3 = new DateTime(year, 9, 30);
                    quarters.Add(year + " - Quarter 3", q3);

                    var q4 = new DateTime(year, 12, 31);
                    quarters.Add(year + " - Quarter 4", q4);
                }
            }

            return quarters;
        }

        public static IDictionary<string, DateTime> GetYears(int startYear, int endYear, bool descending)
        {
            bool greaterYear = startYear <= endYear;
            if (!greaterYear) throw new ArgumentException("End year must be greater than or equal to start year.");
            var years = new Dictionary<string, DateTime>();
            if (descending)
            {
                for (int year = endYear; year >= startYear; year--)
                {
                    var y1 = new DateTime(year, 12, 31);
                    years.Add(year.ToString(), y1);
                }
            }
            else
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    var y1 = new DateTime(year, 12, 31);
                    years.Add(year.ToString(), y1);
                }
            }
            return years;
        }

        public static IDictionary<string, DateTime> GetHalfYears(int startYear, int endYear, bool descending)
        {
            if (startYear > endYear) throw new ArgumentException("End year must be greater than start year.");
            var years = new Dictionary<string, DateTime>();
            if (descending)
            {
                for (int year = endYear; year >= startYear; year--)
                {
                    var y2 = new DateTime(year, 12, 31);
                    years.Add(year + " - second half", y2);

                    var y1 = new DateTime(year, 6, 30);
                    years.Add(year + " - first half", y1);

                }
            }
            else
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    var y1 = new DateTime(year, 6, 30);
                    years.Add(year + " - first half", y1);

                    var y2 = new DateTime(year, 12, 31);
                    years.Add(year + " - second half", y2);
                }
            }
            return years;
        }

        public static string FormatDate(DateTime? date)
        {
            return String.Format("{0:MMMM dd, yyyy}", date);
        }

        public static string FormatJsonDate(DateTime? date)
        {
            return String.Format("{0:yyyy-MM-dd}", date);
        }

        public static DateTime ParseJavascriptDate(string sDate, bool isStartDate)
        {
            if (sDate == null)
            {
                throw new ArgumentNullException("sDate");
            }
            sDate = sDate.Replace("%2F", "-");
            string[] parts = sDate.Split('-');
            int sYear = Convert.ToInt32(parts[0]);
            int sMonth = Convert.ToInt32(parts[1]);
            int sDay = Convert.ToInt32(parts[2]);
            if (isStartDate) return new DateTime(sYear, sMonth, sDay, 0, 0, 0, 0);
            return new DateTime(sYear, sMonth, sDay, 23, 59, 59, 59);
        }

        public static string FormatDateTime(DateTime? date)
        {
            return date.ToString();
        }

        public static long FormatTimeMillis(DateTime? date)
        {
            if (date.HasValue)
            {
                var dateTime = (DateTime)date;
                return dateTime.ToFileTimeUtc();
            }
            return 0;
        }

        public static double CurrentTimeMills() { return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds; }

        public static DateTime GetMaxDate() { return new DateTime(9999, 12, 31); }

        public static bool AreEqual(DateTime date, DateTime date2)
        {
            bool areEqual = date.Equals(date2);
            return areEqual;
        }

        //public static string GetFormatDate(DateTime date, EFrequency eFrequency)
        //{
        //    string formatdate = "";
        //    switch (eFrequency)
        //    {
        //        case EFrequency.Annually:
        //            formatdate = date.Year.ToString();
        //            break;
        //        case EFrequency.BiAnnually:
        //            formatdate = GetHalfYear(date);
        //            break;
        //        case EFrequency.Monthly:
        //            formatdate = String.Format("{0:MMMM yyyy}", date);
        //            break;
        //        case EFrequency.Quarterly:
        //            formatdate = GetQuarterOfYear(date);
        //            break;
        //        case EFrequency.Daily:
        //            formatdate = String.Format("{0:MMMM dd, yyyy}", date);
        //            break;
        //        default:
        //            formatdate = String.Format("{0:MMMM dd, yyyy}", date);
        //            break;
        //    }

        //    return formatdate;
        //}

        public static string GetHalfYear(DateTime date)
        {
            string txtyear = "";
            if (date.Month <= 6)
            {
                txtyear = "1st Half, " + date.Year.ToString();
            }
            else
            {
                txtyear = "2st Half, " + date.Year.ToString();
            }
            return txtyear;
        }
        public static string GetQuarterOfYear(DateTime date)
        {
            string txtQuater = "";
            int quater = date.Month / 3;
            txtQuater = "Quarter " + quater.ToString() + ", " + date.Year.ToString();
            return txtQuater;
        }

        public static IDictionary<string, DateTime> GetMonths(DateTime startDate, DateTime endDate, bool descending)
        {
            int startYear = startDate.Year;
            int endYear = endDate.Year;
            bool greaterYear = startYear <= endYear;
            if (!greaterYear) throw new ArgumentException("End year must be greater than or equal to start year.");
            var months = new Dictionary<string, DateTime>();
            startDate = GetLastDayOfMonth(startDate);
            endDate = GetLastDayOfMonth(endDate);
            if (descending)
            {
                while (endDate >= startDate)
                {
                    months.Add(endDate.Year + " - " + endDate.ToString("MMMM"), endDate);
                    endDate = endDate.AddMonths(-1);
                    endDate = GetLastDayOfMonth(endDate);
                }


            }
            else
            {
                while (startDate <= endDate)
                {
                    months.Add(startDate.Year + " - " + startDate.ToString("MMMM"), startDate);
                    startDate = startDate.AddMonths(1);
                    startDate = GetLastDayOfMonth(startDate);
                }

            }
            return months;
        }

        public static IDictionary<string, DateTime> GetHalfYearsbyDate(DateTime startDate, DateTime endDate, bool descending)
        {
            int startYear = startDate.Year;
            int endYear = endDate.Year;
            if (startYear > endYear) throw new ArgumentException("End year must be greater than start year.");
            var years = new Dictionary<string, DateTime>();
            startDate = GetLastDayOfHalfYear(startDate);
            endDate = GetLastDayOfHalfYear(endDate);
            if (descending)
            {
                while (endDate >= startDate)
                {
                    if (endDate.Month <= 6)
                    {
                        var y1 = new DateTime(endDate.Year, 6, 30);
                        years.Add(endDate.Year + " - first half", y1);
                    }
                    else
                    {
                        var y2 = new DateTime(endDate.Year, 12, 31);
                        years.Add(endDate.Year + " - second half", y2);
                    }
                    endDate = endDate.AddMonths(-6);
                    endDate = GetLastDayOfHalfYear(endDate);
                }

            }
            else
            {
                while (startDate <= endDate)
                {
                    if (startDate.Month <= 6)
                    {
                        var y1 = new DateTime(startDate.Year, 6, 30);
                        years.Add(startDate.Year + " - first half", y1);
                    }
                    else
                    {
                        var y2 = new DateTime(startDate.Year, 12, 31);
                        years.Add(startDate.Year + " - second half", y2);
                    }
                    startDate = startDate.AddMonths(6);
                    startDate = GetLastDayOfHalfYear(startDate);
                }

            }
            return years;
        }

        public static IDictionary<string, DateTime> GetQuartersbyDate(DateTime startDate, DateTime endDate, bool descending)
        {
            int startYear = startDate.Year;
            int endYear = endDate.Year;
            bool greaterYear = startYear <= endYear;
            if (!greaterYear) throw new ArgumentException("End year must be greater than or equal to start year.");
            startDate = GetLastDayOfQuarter(startDate);
            endDate = GetLastDayOfQuarter(endDate);
            var quarters = new Dictionary<string, DateTime>();
            if (descending)
            {

                while (endDate >= startDate)
                {
                    int quater = endDate.Month / 3;
                    quarters.Add(endDate.Year + " - Quarter " + quater, endDate);
                    endDate = endDate.AddMonths(-3);
                    endDate = GetLastDayOfQuarter(endDate);
                }

            }
            else
            {
                while (startDate <= endDate)
                {
                    int quater = startDate.Month / 3;
                    quarters.Add(startDate.Year + " - Quarter " + quater, startDate);
                    startDate = startDate.AddMonths(3);
                    startDate = GetLastDayOfQuarter(endDate);
                }
            }

            return quarters;
        }

        public static IDictionary<string, DateTime> GetDays(DateTime startDate, DateTime endDate, bool descending)
        {
            int startYear = startDate.Year;
            int endYear = endDate.Year;
            bool greaterYear = startYear <= endYear;
            if (!greaterYear) throw new ArgumentException("End year must be greater than or equal to start year.");
            var days = new Dictionary<string, DateTime>();
            //startDate = GetLastDayOfMonth(startDate);
            //endDate = GetLastDayOfMonth(endDate);
            if (descending)
            {
                while (endDate >= startDate)
                {
                    days.Add(String.Format("{0:MMMM dd, yyyy}", endDate), Convert.ToDateTime(endDate.ToShortDateString()));
                    endDate = endDate.AddDays(-1);

                }


            }
            else
            {
                while (startDate <= endDate)
                {
                    days.Add(String.Format("{0:MMMM dd, yyyy}", startDate), Convert.ToDateTime(startDate.ToShortDateString()));
                    startDate = startDate.AddDays(1);
                }

            }
            return days;
        }
        public static string GetDateDiffInWord(DateTime now, DateTime comparedate)
        {
            string dateinWord = "";
            TimeSpan tspDif;
            tspDif = now - comparedate;
            int minutes = (int)tspDif.TotalMinutes;
          
            if (minutes <= 0)
            {
                dateinWord = "0 minutes.";
            }
            else
            {
                if (minutes >= 24 * 60 * 465)
                {
                    dateinWord = String.Format("{0:MMMM yyyy}", comparedate);
                }
                else
                {
                    if (minutes >= 24 * 60 * 365)
                    {
                        dateinWord = "1 year ago";
                    }
                    else
                    {
                        if (minutes >= 24 * 60 * 30)
                        {
                            int month = minutes / (24 * 60 * 30);
                            if (month == 12)
                            {
                                dateinWord = "1 year ago";
                            }
                            else
                            {
                                dateinWord = month + " month(s) ago";
                            }
                        }
                        else
                        {
                            if (minutes >= 24 * 60 * 7)
                            {
                                int week = minutes / (24 * 60 * 7);
                                dateinWord = week + " week(s) ago";
                            }
                            else
                            {
                                if (minutes >= 24 * 60)
                                {
                                    dateinWord = minutes / (24 * 60) + " day(s) ago";
                                }
                                else
                                {
                                    if (minutes >= 60)
                                    {
                                        dateinWord = minutes / (60) + " hour(s) ago";
                                    }
                                    else
                                    {
                                        dateinWord = minutes + " minute(s) ago";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dateinWord;
        }
        public static bool IsVailid(string date)
        {
            try
            {
                DateTime sdate = Convert.ToDateTime(date);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
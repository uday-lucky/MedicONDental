using MedCon.LocalDB;
using MedCon.LocalDB.Tables;
using MedCon.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.Services
{
    public class DoseRemainderService
    {
        List<DoseRemainder> doses;
        double _interval;
        public DoseRemainderService()
        {
            doses = new List<DoseRemainder>();
        }
        //public void StartDoseRemainderService(double interval, int trialId)
        //{
        //    string todayDate = DateTime.Now.Date.ToString("dd/MM/yyyy");

        //    var morningList = SqliteService.GetMorningMedicines(trialId);
        //    var afternoonList = SqliteService.GetAfternoonMedicines(trialId);
        //    var eveningList = SqliteService.GetEveningMedicines(trialId);
        //    var bedtimeList = SqliteService.GetBedtimeMedicines(trialId);
        //    foreach (var item in morningList)
        //    {
        //        string truncatedString = item.dose_time.Replace(" ", "");
        //        string newString = truncatedString;

        //        doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + item.dose_time, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });
        //    }
        //    foreach (var item in afternoonList)
        //    {
        //        string truncatedString = item.dose_time.Replace(" ", "");
        //        string newString = truncatedString;
        //        doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + item.dose_time, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });
        //    }
        //    foreach (var item in eveningList)
        //    {
        //        string truncatedString = item.dose_time.Replace(" ", "");
        //        string newString = truncatedString;
        //        doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + item.dose_time, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });
        //    }
        //    foreach (var item in bedtimeList)
        //    {
        //        string truncatedString = item.dose_time.Replace(" ", "");
        //        string newString = truncatedString;
        //        doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + item.dose_time, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });
        //    }
        //    //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 09:30:00 AM" });
        //    //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 01:00:00 PM" });
        //    //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 01:30:00 PM" });
        //    //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 08:00:00 PM" });
        //    //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 08:30:00 PM" });
        //    _interval = interval;
        //    try
        //    {

        //        doses.Sort((ps1, ps2) => DateTime.Compare(DateTime.ParseExact(ps1.Time, "dd/MM/yyyy HH:mm tt", null), DateTime.ParseExact(ps2.Time, "dd/MM/yyyy HH:mm tt", null)));

        //    }
        //    catch (Exception ex)
        //    {


        //    }

        //    Constants.doseRemainder = new DoseRemainder();
        //    int _index = InitializeTimer();
        //    if (_index == -1) return;

        //    Device.StartTimer(TimeSpan.FromSeconds(interval + DateTime.Now.Second), () =>
        //    {
        //        if (!Constants.doseRemainder.IsRemainderVisible)
        //            Constants.doseRemainder.IsRemainderVisible = true;
        //        InitializeTimer();
        //        return true;
        //    });
        //}
        public void StartDoseRemainderService(double interval,ObservableCollection<RegimenRoot> regimenRoots)
        {
            string todayDate = DateTime.Now.Date.ToString("dd/MM/yyyy");

            foreach (var item in regimenRoots)
            {
                if (item.regimen.morning_schedule != "undefined"&& item.regimen.morning_schedule != "")
                {
                    string[] morningTimes = item.regimen.morning_schedule.Split(',');
                    foreach (var time in morningTimes)
                    {
                        string[] times = time.Split(':');
                        if (times[0].Length == 1)
                            times[0] = times[0].Insert(0, "0");
                        if (times[1].Length == 1)
                            times[1] = times[1].Insert(0, "0");
                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                        string truncatedString = time.Replace(" ", "");
                        string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
                        doses.Add(new DoseRemainder { MedicineImage =Constants.MedicineImageBase+item.drug.image, DoseName = item.regimen.name, Time = todayDate + " " + exactString, drug_name = item.drug.drugname, dose_amount = item.drug.amount, dose_unit = item.drug.unit, total_dose = item.regimen.totalDoses.ToString() });

                    }
                }
                if (item.regimen.afternoon_schedule != "undefined" && item.regimen.afternoon_schedule != "")
                {
                    string[] morningTimes = item.regimen.afternoon_schedule.Split(',');
                    foreach (var time in morningTimes)
                    {
                        string[] times = time.Split(':');
                        if (times[0].Length == 1)
                            times[0] = times[0].Insert(0, "0");
                        if (times[1].Length == 1)
                            times[1] = times[1].Insert(0, "0");
                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                        string truncatedString = time.Replace(" ", "");
                        string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
                        doses.Add(new DoseRemainder { MedicineImage = Constants.MedicineImageBase + item.drug.image, DoseName = item.regimen.name, Time = todayDate + " " + exactString, drug_name = item.drug.drugname, dose_amount = item.drug.amount, dose_unit = item.drug.unit, total_dose = item.regimen.totalDoses.ToString() });

                    }
                }
                if (item.regimen.evening_schedule != "undefined" && item.regimen.evening_schedule != "")
                {
                    string[] morningTimes = item.regimen.evening_schedule.Split(',');
                    foreach (var time in morningTimes)
                    {
                        string[] times = time.Split(':');
                        if (times[0].Length == 1)
                            times[0] = times[0].Insert(0, "0");
                        if (times[1].Length == 1)
                            times[1] = times[1].Insert(0, "0");
                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                        string truncatedString = time.Replace(" ", "");
                        string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
                        doses.Add(new DoseRemainder { MedicineImage = Constants.MedicineImageBase + item.drug.image, DoseName = item.regimen.name, Time = todayDate + " " + exactString, drug_name = item.drug.drugname, dose_amount = item.drug.amount, dose_unit = item.drug.unit, total_dose = item.regimen.totalDoses.ToString() });

                    }
                }
                if (item.regimen.bedtime_schedule != "undefined" && item.regimen.bedtime_schedule != "")
                {
                    string[] morningTimes = item.regimen.bedtime_schedule.Split(',');
                    foreach (var time in morningTimes)
                    {
                        string[] times = time.Split(':');
                        if (times[0].Length == 1)
                            times[0] = times[0].Insert(0, "0");
                        if (times[1].Length == 1)
                            times[1] = times[1].Insert(0, "0");
                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                        string truncatedString = time.Replace(" ", "");
                        string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
                        doses.Add(new DoseRemainder { MedicineImage = Constants.MedicineImageBase + item.drug.image, DoseName = item.regimen.name, Time = todayDate + " " + exactString, drug_name = item.drug.drugname, dose_amount = item.drug.amount, dose_unit = item.drug.unit, total_dose = item.regimen.totalDoses.ToString() });

                    }
                }
            }

            //var morningList = SqliteService.GetMorningMedicines(trialId);
            //var afternoonList = SqliteService.GetAfternoonMedicines(trialId);
            //var eveningList = SqliteService.GetEveningMedicines(trialId);
            //var bedtimeList = SqliteService.GetBedtimeMedicines(trialId);
            //foreach (var item in morningList)
            //{
            //    if (item.dose_time != "undefined")
            //    {
            //        string[] times = item.dose_time.Split(':');
            //        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
            //        string truncatedString = item.dose_time.Replace(" ", "");
            //        string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
            //        doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + exactString, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });
            //    }
            //}
            //foreach (var item in afternoonList)
            //{
            //    if (item.dose_time != "undefined")
            //    {


            //        string[] times = item.dose_time.Split(':');
            //        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
            //        string truncatedString = item.dose_time.Replace(" ", "");
            //        string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
            //        doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + exactString, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });
            //    }
            //}
            //foreach (var item in eveningList)
            //{
            //    if (item.dose_time != "undefined")
            //    {
            //        string[] times = item.dose_time.Split(':');
            //        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
            //        string truncatedString = item.dose_time.Replace(" ", "");
            //        string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
            //        doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + exactString, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });
            //    }
            //}
            //foreach (var item in bedtimeList)
            //{
            //    if (item.dose_time != "undefined")
            //    {
            //        string[] times = item.dose_time.Split(':');
            //        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
            //        string truncatedString = item.dose_time.Replace(" ", "");
            //        string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
            //        doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + exactString, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });

            //    }
            //}



            //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 09:30:00 AM" });
            //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 01:00:00 PM" });
            //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 01:30:00 PM" });
            //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 08:00:00 PM" });
            //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 08:30:00 PM" });
            _interval = interval;
            try
            {
                // var dssdsd = DateTime.Now.AddHours(-9).ToString("dd/MM/yyyy hh:mm tt");
                //var dsd = DateTime.ParseExact(doses[1].Time, "dd/MM/yyyy hh:mm tt",null);
                var ddd = DateTime.ParseExact(doses[0].Time, "dd/MM/yyyy hh:mm tt", null);
                doses.Sort((ps1, ps2) => DateTime.Compare(DateTime.ParseExact(ps1.Time, "dd/MM/yyyy hh:mm tt", null), DateTime.ParseExact(ps2.Time, "dd/MM/yyyy hh:mm tt", null)));

            }
            catch (Exception ex)
            {

            }

            Constants.doseRemainder = new DoseRemainder();
            int _index = InitializeTimer();
            if (_index == -1) return;

            Device.StartTimer(TimeSpan.FromSeconds(interval + DateTime.Now.Second), () =>
            {
                if (!Constants.doseRemainder.IsRemainderVisible)
                    Constants.doseRemainder.IsRemainderVisible = true;
                InitializeTimer();
                return true;
            });
        }
        public void StartDoseRemainderService(double interval, int trialId)
        {
            string todayDate = DateTime.Now.Date.ToString("dd/MM/yyyy");

            var morningList = SqliteService.GetMorningMedicines(trialId);
            var afternoonList = SqliteService.GetAfternoonMedicines(trialId);
            var eveningList = SqliteService.GetEveningMedicines(trialId);
            var bedtimeList = SqliteService.GetBedtimeMedicines(trialId);
            foreach (var item in morningList)
            {
                if (item.dose_time != "undefined")
                {
                    string[] times = item.dose_time.Split(':');
                    string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                    string truncatedString = item.dose_time.Replace(" ", "");
                    string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
                    doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + exactString, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });
                }
            }
            foreach (var item in afternoonList)
            {
                if (item.dose_time != "undefined")
                {


                    string[] times = item.dose_time.Split(':');
                    string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                    string truncatedString = item.dose_time.Replace(" ", "");
                    string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
                    doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + exactString, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });
                }
            }
            foreach (var item in eveningList)
            {
                if (item.dose_time != "undefined")
                {
                    string[] times = item.dose_time.Split(':');
                    string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                    string truncatedString = item.dose_time.Replace(" ", "");
                    string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
                    doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + exactString, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });
                }
            }
            foreach (var item in bedtimeList)
            {
                if (item.dose_time != "undefined")
                {
                    string[] times = item.dose_time.Split(':');
                    string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                    string truncatedString = item.dose_time.Replace(" ", "");
                    string newString = truncatedString.Insert(truncatedString.Length - 2, "00 ");
                    doses.Add(new DoseRemainder { MedicineImage = item.image, DoseName = item.drugname, Time = todayDate + " " + exactString, drug_name = item.drugname, dose_amount = item.amount, dose_unit = item.unit, total_dose = item.total_doses.ToString() });

                }
            }
            //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 09:30:00 AM" });
            //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 01:00:00 PM" });
            //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 01:30:00 PM" });
            //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 08:00:00 PM" });
            //doses.Add(new DoseRemainder { MedicineImage = "capsule.png", DoseName = "Dose Name", Time = todayDate + " 08:30:00 PM" });
            _interval = interval;
            try
            {
                // var dssdsd = DateTime.Now.AddHours(-9).ToString("dd/MM/yyyy hh:mm tt");
                //var dsd = DateTime.ParseExact(doses[1].Time, "dd/MM/yyyy hh:mm tt",null);
                doses.Sort((ps1, ps2) => DateTime.Compare(DateTime.ParseExact(ps1.Time, "dd/MM/yyyy hh:mm tt", null), DateTime.ParseExact(ps2.Time, "dd/MM/yyyy hh:mm tt", null)));

            }
            catch (Exception ex)
            {

            }

            Constants.doseRemainder = new DoseRemainder();
            int _index = InitializeTimer();
            if (_index == -1) return;

            Device.StartTimer(TimeSpan.FromSeconds(interval + DateTime.Now.Second), () =>
            {
                if (!Constants.doseRemainder.IsRemainderVisible)
                    Constants.doseRemainder.IsRemainderVisible = true;
                InitializeTimer();
                return true;
            });
        }
        public static void ParseDate(string value, string[] masks, out DateTime result)
        {
            DateTime.TryParseExact(value, masks,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal
                | System.Globalization.DateTimeStyles.AdjustToUniversal
                | System.Globalization.DateTimeStyles.AllowInnerWhite
                | System.Globalization.DateTimeStyles.AllowLeadingWhite
                | System.Globalization.DateTimeStyles.AllowTrailingWhite,
                    out result);
        }
        int InitializeTimer()
        {
            var greaterDate = doses.Where(x => DateTime.ParseExact(x.Time, "dd/MM/yyyy hh:mm tt", null) > DateTime.Now).FirstOrDefault();
            if (greaterDate == null) return -1;
            int indexOfLessTime = doses.IndexOf(greaterDate);
            var nextTime1 = DateTime.ParseExact(doses[indexOfLessTime].Time, "dd/MM/yyyy hh:mm tt", null)  - DateTime.Now;
            Constants.doseRemainder.DoseInTime = doses[indexOfLessTime].DoseInTime = string.Format("{0}hrs {1}min", nextTime1.Hours, nextTime1.Minutes);
            Constants.doseRemainder.Time = DateTime.ParseExact(doses[indexOfLessTime].Time, "dd/MM/yyyy hh:mm tt", null).ToString("hh:mm tt", CultureInfo.InvariantCulture);
            Constants.doseRemainder.DoseName = doses[indexOfLessTime].DoseName;
            Constants.doseRemainder.MedicineImage = doses[indexOfLessTime].MedicineImage;
            if (nextTime1.Hours == 0 && nextTime1.Minutes <= 15)
            {
                Constants.doseRemainder.ViewColor = Color.FromHex("#e53935");
                DisplayAlerts(0, doses[indexOfLessTime]);
            }
            else
                Constants.doseRemainder.ViewColor = Color.FromHex("#5C6BC0");
            return indexOfLessTime;
        }
        void DisplayAlerts(int status, DoseRemainder doseRemainder)
        {
          //  AlertTable alertTable = SqliteService.GetAlerts();
            if (status == 0)
                new DialogServices().DisplayNativeAlert(string.Format("Your need to take {0} of size {1} {2} {3}", doseRemainder.drug_name, doseRemainder.dose_amount, doseRemainder.dose_unit, doseRemainder.total_dose),"OK");
            doses.Remove(doseRemainder);
        }
    }
}

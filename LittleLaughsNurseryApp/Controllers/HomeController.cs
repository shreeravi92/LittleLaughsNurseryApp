using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LittleLaughsNurseryApp.Models;
using System.Net;
using System.Net.Mail;

namespace LittleLaughsNurseryApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginDetails objloginDetail, Rating rateList)
        {
            if (ModelState.IsValid)
            {
                using (LoginEntities le = new LoginEntities())
                {
                    var obj = le.loginDetails.Where(a => a.UserName.Equals(objloginDetail.UserName) && a.Pwd.Equals(objloginDetail.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserName"] = obj.UserName;
                        ViewBag.Message = "Success";
                        ViewBag.userName = obj.UserName;
                    }
                    else
                    {
                        ViewBag.Message = "Failure";
                    }
                }
            }
            return View("Index");
        }

        public ActionResult CardBoard(RatesAndTimings objTiming)
        {
            if (ModelState.IsValid)
            {
                if (objTiming.rateDetails == null)
                {
                    objTiming.rateDetails = new List<Rating>();
                }

                try
                {
                    using (LoginEntities le = new LoginEntities())
                    {
                        var values = (from p in le.Rates
                                      orderby p.Rate_ID
                                      select p).ToList();
                        foreach (var i in values)
                        {
                            Rating objRating = new Rating();
                            objRating.timings = i.Timings;
                            objRating.rate_Monthly = Convert.ToInt32(i.Rate_Monthly);
                            objRating.rate_Weekly = Convert.ToInt32(i.Rate_weekly);
                            objRating.threeDaysPerWeek = Convert.ToInt32(i.Rate_3DaysPerWeek);
                            objTiming.rateDetails.Add(objRating);
                        }

                        ViewBag.Message = "rate fetched";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "rate failed" + ex;
                }
            }
            return PartialView("CardBoard", objTiming);
        }

        public ActionResult MealChart(RatesAndTimings objTiming)
        {
            if (ModelState.IsValid)
            {
                if (objTiming.mealDetails == null)
                {
                    objTiming.mealDetails = new List<Meal_Chart>();
                }

                try
                {
                    using (LoginEntities le = new LoginEntities())
                    {
                        var values = (from p in le.MealCharts
                                      orderby p.MealID
                                      select p).ToList();
                        foreach (var i in values)
                        {
                            Meal_Chart objMeal = new Meal_Chart();
                            objMeal.Days = i.Days;
                            objMeal.Breakfast = i.Breakfast;
                            objMeal.Lunch = i.Lunch;
                            objMeal.Snacks = i.Snacks;
                            objTiming.mealDetails.Add(objMeal);
                        }

                        ViewBag.Message = "rate fetched";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "rate failed" + ex;
                }
            }
            return PartialView("MealChart", objTiming);
        }

        [HttpPost]
        public ActionResult SignUp(loginDetail login_Detail)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    using (LoginEntities le = new LoginEntities())
                    {
                        var obj = le.loginDetails.Where(a => a.UserName.Equals(login_Detail.UserName) && a.Pwd.Equals(login_Detail.Pwd)).FirstOrDefault();
                        if (obj == null)
                        {
                            var email=login_Detail.UserName;
                            le.loginDetails.Add(login_Detail);
                            le.SaveChanges();
                            ViewBag.SuccessMessage = "Success";
                            Email("Click on the link to verify your email:http://localhost:2486/", email);
                        }
                        else
                        {
                            ViewBag.FailureMessage = "Failure";
                            ViewBag.userName = obj.UserName;
                            return View("SignUp");
                        }
                    }
                }
                catch
                {
                }
                
            }

            return View("Index");
        }

        public static void Email(string htmlString,string emailID)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("LittleLaughs@gmail.com");
                mail.To.Add(emailID);
                mail.Subject = "Verification Link";
                mail.Body = htmlString;                

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("shreeravi92@gmail.com", "Anirudh18");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception) { }
        }  
    }
}
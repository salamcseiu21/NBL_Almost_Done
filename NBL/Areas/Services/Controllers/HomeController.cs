﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Employees;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Securities;
using NBL.Models.Logs;
using NBL.Models.ViewModels;

namespace NBL.Areas.Services.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager _userManager=new UserManager();

        private readonly IServiceManager _iServiceManager;
        private IEmployeeManager _iEmployeeManager;

        public HomeController(IServiceManager iServiceManager,IEmployeeManager iEmployeeManager)
        {
            _iServiceManager = iServiceManager;
            _iEmployeeManager = iEmployeeManager;
        }
        // GET: Services/Home
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult FolioList()
        {
            try
            {
                var products = _iServiceManager.GetAllSoldProducts();
                return View(products);
            }
            catch (Exception exception)
            {
               Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
           
        }

        //--------------------------Update User Profile----------------
        [HttpGet]
        public ActionResult UpdateBasicInfo(int id)
        {

            try
            {
                Employee employee = _iEmployeeManager.GetById(id);
                return View(employee);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }

        }

        [HttpPost]
        public ActionResult UpdateBasicInfo(int id, Employee emp, HttpPostedFileBase EmployeeImage, HttpPostedFileBase EmployeeSignature)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                var anEmployee = _iEmployeeManager.GetById(id);
                anEmployee.EmployeeName = emp.EmployeeName;
                anEmployee.PresentAddress = emp.PresentAddress;
                anEmployee.Phone = emp.Phone;
                anEmployee.AlternatePhone = emp.AlternatePhone;
                anEmployee.Email = emp.Email;
                anEmployee.Gender = emp.Gender;
                anEmployee.Email = emp.Email;
                anEmployee.NationalIdNo = emp.NationalIdNo;
                anEmployee.UserId = user.UserId;
                anEmployee.DoB = emp.DoB;

                if (EmployeeImage != null)
                {
                    string ext = Path.GetExtension(EmployeeImage.FileName);
                    string image = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Employees"), image);
                    // file is uploaded
                    EmployeeImage.SaveAs(path);
                    anEmployee.EmployeeImage = "Images/Employees/" + image;
                }
                if (EmployeeSignature != null)
                {
                    string ext = Path.GetExtension(EmployeeSignature.FileName);
                    string sign = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Signatures"), sign);
                    // file is uploaded
                    EmployeeSignature.SaveAs(path);
                    anEmployee.EmployeeSignature = "Images/Signatures/" + sign;
                }

                var result = _iEmployeeManager.Update(anEmployee);

                if (result)
                {
                    //TempData["Message"] = "Saved Successfully!";
                    return RedirectToAction("MyProfile", "Home", new { id = emp.EmployeeId });
                }

                return View();

            }
            catch (Exception exception)
            {
                Employee employee = _iEmployeeManager.GetById(id);
                TempData["Error"] = exception.Message;
                return View(employee);
            }
        }

        public ActionResult UpdateEducationalInfo(int id)
        {
            try
            {
                EducationalInfo educational = new EducationalInfo { EmployeeId = id };
                return View(educational);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }
        }
        [HttpPost]
        public ActionResult UpdateEducationalInfo(int id, EducationalInfo model)
        {
            try
            {
                bool result = _iEmployeeManager.UpdateEducationalInfo(model);
                if (result)
                {
                    return RedirectToAction("MyProfile", "Home", new { id = model.EmployeeId });
                }
                return View(model);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }
        }
        public ActionResult MyProfile(int id)
        {
            try
            {
                List<EducationalInfo> educationalInfos = _iEmployeeManager.GetEducationalInfoByEmpId(id);
                var employee = _iEmployeeManager.GetEmployeeById(id);
                employee.EducationalInfos = educationalInfos;
                return View(employee);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}
using Dose.Models;
using Dose.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace Dose.Controllers
{
    public class PatientMedicationController : Controller
    {
        private readonly IPatientMedicationRepository _patientMedicationRepo;

        public PatientMedicationController(IPatientMedicationRepository patientMedicationRepository)
        {
            _patientMedicationRepo = patientMedicationRepository;
        }
        // GET: PatientMedicationController
        public ActionResult Index()
        {
            int patientId = GetCurrentPatientId();
            List<PatientMedication> patientMedications = _patientMedicationRepo.GetAllPatientMedicationsByPatientId(patientId);
            return View(patientMedications);
        }

        // GET: PatientMedicationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PatientMedicationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientMedicationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatientMedicationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PatientMedicationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatientMedicationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatientMedicationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private int GetCurrentPatientId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}

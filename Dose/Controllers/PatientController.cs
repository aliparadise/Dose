using Dose.Models;
using Dose.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Dose.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientRepository _patientRepo;
        private readonly IPatientMedicationRepository _patientMedicationRepo;
        private readonly IMedicationRepository _medicationRepo;

        public PatientController(
            IPatientRepository patientRepository,
            IPatientMedicationRepository patientMedicationRepository,
            IMedicationRepository medicationRepository)
        {
            _patientRepo = patientRepository;
            _patientMedicationRepo = patientMedicationRepository;
            _medicationRepo = medicationRepository;
        }

        // GET: PatientController
        public ActionResult Index()
        {
            int userProfileId = GetCurrentUserId();

            List<Patient> patients = _patientRepo.GetAllPatientsByUserId(userProfileId);
            return View(patients);
        }

        // GET: PatientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PatientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient)
        {
            try
            {
                patient.UserProfileId = GetCurrentUserId();

                _patientRepo.AddPatient(patient);

                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                return View(patient);
            }
        }

        // GET: PatientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PatientController/Edit/5
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

        // GET: PatientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatientController/Delete/5
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
        public ActionResult MedicationDetails(int id)
        {

            List<PatientMedication> patientMedications = _patientMedicationRepo.GetAllPatientMedicationsByPatientId(id);
            return View(patientMedications);
        }

        public ActionResult CreatePatientMedication()
        {
            List<Medication> medications = _medicationRepo.GetAllMedications();

            CreatePatientMedicationFormViewModel vm = new CreatePatientMedicationFormViewModel()
            {
                PatientMedication = new PatientMedication(),
                Medications = medications,
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePatientMedication(PatientMedication patientMedication)
        {
            try
            {
                _patientMedicationRepo.AddPatientMedication(patientMedication);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                CreatePatientMedicationFormViewModel vm = new CreatePatientMedicationFormViewModel()
                {
                    PatientMedication = patientMedication,
                    Medications = _medicationRepo.GetAllMedications()
                };

                return View(vm);

            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}

using Dose.Models;
using Dose.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public ActionResult Index()
        {
            int userProfileId = GetCurrentUserId();

            List<Patient> patients = _patientRepo.GetAllPatientsByUserId(userProfileId);
            return View(patients);
        }

        // GET: PatientController/Details/5
        public ActionResult Details(int id)
        {
            Patient patient = _patientRepo.GetPatientById(id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: PatientController/Create
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public ActionResult MedicationDetails(int id)
        {
            int userProfileId = GetCurrentUserId();
            List<PatientMedication> patientMedications = _patientMedicationRepo.GetAllPatientMedicationsByPatientId(id);

            if (userProfileId != GetCurrentUserId())
            {
                return NotFound();
            }

            return View(patientMedications);
        }

        [Authorize]
        public ActionResult CreatePatientMedication()
        {
            List<Medication> medications = _medicationRepo.GetAllMedications();

            PatientMedicationFormViewModel vm = new PatientMedicationFormViewModel()
            {   
               
                PatientMedication = new PatientMedication(),
                Medications = medications,
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePatientMedication(int id, PatientMedication patientMedication)
        {
            try
            {
                patientMedication.PatientId = id;
                _patientMedicationRepo.AddPatientMedication(patientMedication);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                PatientMedicationFormViewModel vm = new PatientMedicationFormViewModel()
                {
                    PatientMedication = patientMedication,
                    Medications = _medicationRepo.GetAllMedications()
                };

                return View(vm);

            }
        }

        [Authorize]
        public ActionResult EditPatientMedication(int id)
        {

            PatientMedicationFormViewModel vm = new PatientMedicationFormViewModel()
            {

                PatientMedication = _patientMedicationRepo.GetPatientMedicationsById(id),
                Medications = _medicationRepo.GetAllMedications()
            };

            if (vm.PatientMedication == null)
            {
                return NotFound();
            }
            else
            {
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPatientMedication(PatientMedicationFormViewModel vm)
        {
            try
            {
                
                _patientMedicationRepo.UpdatePatientMedication(vm.PatientMedication);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                
                return View(vm);

            }
        }

        [Authorize]
        public ActionResult DeletePatientMedication(int id)
        {
            PatientMedicationFormViewModel vm = new PatientMedicationFormViewModel()
            {

                PatientMedication = _patientMedicationRepo.GetPatientMedicationsById(id),
                Medications = _medicationRepo.GetAllMedications()
            };

            if (vm.PatientMedication == null)
            {
                return NotFound();
            }
            else
            {
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePatientMedication(int id, PatientMedicationFormViewModel vm)
        {
            try
            {
                vm.PatientMedication = _patientMedicationRepo.GetPatientMedicationsById(id);
                _patientMedicationRepo.DeletePatientMedication(vm.PatientMedication);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

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

namespace VaccOps
{
    using Models;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VaccDb : IVaccOps
    {

        Dictionary<string, Doctor> doctors = new Dictionary<string, Doctor>();
        Dictionary<string, Patient> patients = new Dictionary<string, Patient>();

        public void AddDoctor(Doctor doctor)
        {
            if (doctors.ContainsKey(doctor.Name))
            {
                throw new ArgumentException();
            }

            doctors[doctor.Name] = doctor;
        }

        public void AddPatient(Doctor doctor, Patient patient)
        {
            if (!doctors.ContainsKey(doctor.Name))
            {
                throw new ArgumentException();
            }

            if (patients.ContainsValue(patient))
            {
                throw new ArgumentException();
            }

            doctors[doctor.Name].Patients.Add(patient);

            patients.Add(patient.Name, patient);
            patient.Doctor = doctor;
            
        } //try other way to check if the pation exist

        public void ChangeDoctor(Doctor oldDoctor, Doctor newDoctor, Patient patient)
        {
            if (!doctors.ContainsValue(oldDoctor) ||
                !doctors.ContainsValue(newDoctor) ||
                !patients.ContainsValue(patient))
            {
                throw new ArgumentException();
            }
            oldDoctor.Patients.Remove(patient);
            newDoctor.Patients.Add(patient);

            if (oldDoctor.Patients.Count() == 0)
            {
                RemoveDoctor(oldDoctor.Name);
            }

            doctors[oldDoctor.Name] = oldDoctor;
            doctors[newDoctor.Name] = newDoctor;

            //doctors[oldDoctor.Name].Patients.Remove(patient);
            //doctors[newDoctor.Name].Patients.Add(patient);
        }

        public bool Exist(Doctor doctor)
        {
            return doctors.ContainsKey(doctor.Name);
        }

        public bool Exist(Patient patient)
        {
            return patients.ContainsKey(patient.Name);
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            return doctors.Values.ToList();
        }

        public IEnumerable<Doctor> GetDoctorsByPopularity(int populariry)
        {
            return doctors.Values.Where(d => d.Popularity == populariry).ToList();
        }

        public IEnumerable<Doctor> GetDoctorsSortedByPatientsCountDescAndNameAsc()
        {
            return doctors.Values.OrderByDescending(p => p.Patients.Count).ThenBy(pn => pn.Name);
        }

        public IEnumerable<Patient> GetPatients()
        {
            return patients.Values.ToList();
        }

        public IEnumerable<Patient> GetPatientsByTown(string town)
        {
            return patients.Values.Where(p => p.Town == town).ToList();
        }

        public IEnumerable<Patient> GetPatientsInAgeRange(int lo, int hi)
        {
            return patients.Values.Where(pa => pa.Age >= lo && pa.Age <= hi);
        }

        public IEnumerable<Patient> GetPatientsSortedByDoctorsPopularityAscThenByHeightDescThenByAge()
        {
            return
                    patients.Values.OrderBy(p => p.Doctor.Popularity).ThenByDescending(h => h.Height).ThenBy(a =>a.Age);
        }

        public Doctor RemoveDoctor(string name)
        {
            if (!doctors.ContainsKey(name))
            {
                throw new ArgumentException();
            }
            var doctor = doctors[name];
            int doctorPatientsCount = doctor.Patients.Count();

            for (int i = 0; i < doctor.Patients.Count(); i++)
            {
                if (patients.ContainsKey(doctor.Patients[i].Name))
                {
                    patients.Remove(doctor.Patients[i].Name);
                }
            }

            while (doctor.Patients.Count != 0)
            {
                doctor.Patients.Remove(doctor.Patients.First());
            }

            doctors[name] = null;
            doctors.Remove(name);

            return doctor;
        }
    }
}

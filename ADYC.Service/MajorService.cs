﻿using ADYC.IService;
using System;
using System.Collections.Generic;
using ADYC.Model;
using ADYC.IRepository;
using System.Linq;
using ADYC.Util.Exceptions;

namespace ADYC.Service
{
    public class MajorService : IMajorService
    {
        private IMajorRepository _majorRepository;

        public MajorService(IMajorRepository majorRepository)
        {
            _majorRepository = majorRepository;
        }

        public void Add(Major major)
        {
            ValidateMajor(major);
            ValidateDuplicatedMajor(major);

            _majorRepository.Add(major);
        }

        public IEnumerable<Major> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return _majorRepository.Find(m => m.Name.Contains(name));
        }

        public Major Get(int id)
        {
            return _majorRepository.Get(id);
        }

        public IEnumerable<Major> GetAll()
        {
            return _majorRepository.GetAll();
        }

        public void Remove(Major major)
        {
            if (major == null)
            {
                throw new ArgumentNullException("major");
            }

            if (major.Students.Count > 0)
            {
                throw new ForeignKeyEntityException("This major could not be removed. It has one or more students associated with it.");
            }

            _majorRepository.Remove(major);
        }

        public void RemoveRange(IEnumerable<Major> majors)
        {
            if (majors.Count() == 0)
            {
                throw new ArgumentNullException("majors");
            }

            var hasStudents = majors.Count(g => g.Students.Count > 0);

            if (hasStudents > 0)
            {
                throw new ForeignKeyEntityException("This major could not be removed. It has one or more students associated with it.");
            }

            _majorRepository.RemoveRange(majors);
        }

        public void Update(Major major)
        {
            ValidateMajor(major);

            _majorRepository.Update(major);
        }

        private void ValidateDuplicatedMajor(Major major)
        {
            if (_majorRepository.Find(c => c.Name.Equals(major.Name)).Count() > 0)
            {
                throw new PreexistingEntityException("A majort with the same name already exists.", null);
            }
        }

        private void ValidateMajor(Major major)
        {
            if (major == null)
            {
                throw new ArgumentNullException("major");
            }
        }
    }
}

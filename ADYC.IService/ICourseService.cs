﻿using ADYC.Model;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface ICourseService
    {
        Course Get(int id);

        IEnumerable<Course> GetAll();
        IEnumerable<Course> FindByName(string name);
        IEnumerable<Course> FindByCourseTypeId(int courseTypeId);
        IEnumerable<Course> FindNotTrashedCourses();
        IEnumerable<Course> FindTrashedCourses();

        void Add(Course course);
        void AddRange(IEnumerable<Course> courses);

        void Update(Course course);

        void Remove(Course course);
        void RemoveRange(IEnumerable<Course> courses);

        void Trash(Course course);
        void TrashRange(IEnumerable<Course> courses);
        void Restore(Course course);
        void RestoreRange(IEnumerable<Course> courses);
    }
}

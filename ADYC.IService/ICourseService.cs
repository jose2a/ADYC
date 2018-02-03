﻿using ADYC.Model;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface ICourseService
    {
        Course Get(int id);
        IEnumerable<Course> GetAll();
        IEnumerable<Course> FindNotSoftDeletedCourses();
        IEnumerable<Course> FindByName(string name);
        IEnumerable<Course> FindByCourseType(CourseType courseType);
        IEnumerable<Course> FindSoftDeletedCourses();

        void Add(Course course);
        void AddRange(IEnumerable<Course> courses);

        void Update(Course course);

        void Remove(Course course);
        void RemoveRange(IEnumerable<Course> courses);
        void SoftDelete(Course course);
        void SoftDeleteRange(IEnumerable<Course> courses);
    }
}

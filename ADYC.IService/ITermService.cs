using ADYC.Model;
using System;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface ITermService
    {
        Term Get(int id);
        Term GetCurrentTerm();
        int GetCurrentTermId();

        IEnumerable<Term> GetAll();
        IEnumerable<Term> FindByName(string name);
        IEnumerable<Term> FindByStarDate(DateTime startDate);
        IEnumerable<Term> FindByEndDate(DateTime endDate);
        IEnumerable<Term> FindByBetweenDates(DateTime startDate, DateTime endDate);

        IEnumerable<PeriodDate> GetTermPeriodDates(int termId);
        IEnumerable<PeriodDate> GetCurrentTermPeriodDates();

        void Add(Term term);
        void Update(Term term);

        void Remove(Term term);
    }
}

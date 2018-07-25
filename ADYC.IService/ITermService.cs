using ADYC.Model;
using System;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface ITermService
    {
        Term Get(int id);
        Term GetCurrentTerm();

        IEnumerable<Term> GetAll();
        IEnumerable<Term> FindByName(string name);
        IEnumerable<Term> FindByBetweenDates(DateTime startDate, DateTime endDate);

        void Add(Term term);
        void Update(Term term);

        void Remove(Term term);
    }
}

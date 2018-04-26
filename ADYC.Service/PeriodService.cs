using System;
using System.Collections.Generic;
using ADYC.IService;
using ADYC.Model;
using ADYC.IRepository;
using ADYC.Util.Exceptions;
using System.Linq;

namespace ADYC.Service
{
    public class PeriodService : IPeriodService
    {
        private IPeriodRepository _periodRepository;

        public PeriodService(IPeriodRepository periodRepository)
        {
            _periodRepository = periodRepository;
        }

        public IEnumerable<Period> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return _periodRepository.Find(m => m.Name.Contains(name));
        }

        public Period Get(int id)
        {
            return _periodRepository.Get(id);
        }

        public IEnumerable<Period> GetAll()
        {
            return _periodRepository.GetAll(p => p.OrderBy(period => period.Id));
        }
    }
}

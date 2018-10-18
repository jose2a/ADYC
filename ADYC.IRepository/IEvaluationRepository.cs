using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.IRepository
{
    public interface IEvaluationRepository : IReadOnlyRepository<Evaluation>, IWriteOnlyRepository<Evaluation>
    {
        Evaluation Get(int enrollmentId, int periodId);

        //void UpdateRange(IEnumerable<Evaluation> evaluations);

        //void RemoveRange(IEnumerable<Evaluation> evaluations);
    }
}

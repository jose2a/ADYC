using ADYC.IRepository;
using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ADYC.Repository
{
    public class EvaluationRepository : Repository<Evaluation>, IEvaluationRepository
    {
        public EvaluationRepository(DbContext context) : base(context)
        {
        }

        public Evaluation Get(int enrollmentId, int periodId)
        {
            return Entity.Find(new object[] { enrollmentId, periodId});
        }

        //public void UpdateRange(IEnumerable<Evaluation> evaluations)
        //{
        //    foreach (var evaluation in evaluations.ToList())
        //    {
        //        Update(evaluation);
        //    }
        //}
    }
}

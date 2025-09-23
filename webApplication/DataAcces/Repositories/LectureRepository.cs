using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace DataAcces.Repositories
    {
    public class LectureRepository : RepositoryBase<lecture>, ILectureRepository
        {
        public LectureRepository(LDBContext repositoryContext) : base(repositoryContext)
            {
            }
        }
    }
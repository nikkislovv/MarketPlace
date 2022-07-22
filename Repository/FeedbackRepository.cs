using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FeedbackRepository : RepositoryBase<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void CreateFeedbackForProduct(Guid productId,Feedback feedback)
        {
            feedback.ProductId = productId;
            Create(feedback);
        }
        public void DeleteFeedback(Feedback feedback)
        {
            Delete(feedback);
        }
        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
        }
        //public async Task<IEnumerable<Feedback>> GetFeedbacksByAccountAsync(Guid userId, bool trackChanges)
        //{
        //    return await FindByCondition(e => e.UserId.Equals(userId), trackChanges).ToListAsync();
        //}
    }
}

using Contracts;
using Entities.Models;
using Entities.RequestFeatures;
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
        public async Task<PagedList<Feedback>> GetFeedbacksByProductAsync(Guid productId, FeedbackParameters feedbackParameters, bool trackChanges)
        {
            var feedbacks = await FindByCondition(e => e.ProductId.Equals(productId), trackChanges)
                .OrderByDescending(e => e.Rating)
                .Skip((feedbackParameters.PageNumber - 1) * feedbackParameters.PageSize)
                .Take(feedbackParameters.PageSize)
                .ToListAsync();
            var count = await FindByCondition(e => e.ProductId.Equals(productId), trackChanges).CountAsync();
            return new PagedList<Feedback>(feedbacks, count, feedbackParameters.PageNumber, feedbackParameters.PageSize);
        }
    }
}

using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IFeedbackRepository
    {
        void CreateFeedbackForProduct(Guid productId,Feedback feedback);
        void DeleteFeedback(Feedback feedback);
        Task<PagedList<Feedback>> GetFeedbacksByProductAsync(Guid productId, FeedbackParameters feedbackParameters, bool trackChanges);

    }
}
